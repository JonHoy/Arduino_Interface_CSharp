using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace ArduinoClass
{
    public class Arduino
    {
        private int WAIT_TIME = 1; //  (12 sec = default) amount of time (sec) required to wait before issuing commands after opening Serial port
        private SerialPort Serial; // Serial port object which controls read/write operations

        public Arduino(string COM) // eg COM = "COM4"
        { 
            try 
	        {
                Serial = new SerialPort(COM, 115200);
	        }
	        catch (Exception)
	        {
                throw new Exception("Unable to Connect to Arduino");
	        }
            Serial.Encoding = new UTF8Encoding();
            Serial.ReadTimeout = 500;
            try
            {
                Serial.Open();
            }
            catch (Exception)
            { 
                Console.WriteLine("Unable to Open {0}",COM);
                throw;  
            }
            Console.Write("Attemping Connection .");
            for (int i = 0; i < WAIT_TIME; i++) // make the user wait before trying to establish communication
            {
                Thread.Sleep(1000); // wait for 1 second then continue iterations
                Console.Write(".");
            }
            Console.Write("\n");
            // Query Script Type
            Serial.Write("99");
            try
            {
                string Check = Serial.ReadLine(); // try to get a response from the arduino board
            }
            catch (Exception)
            {
                Console.WriteLine("Connection Unsuccessful!");
                throw new Exception("Unable to Connect to the Arduino Exiting Now!");
            }
            Console.WriteLine("Arduino Sucessfully Connected!");
        }

        ~Arduino() // destructor
        {
            Serial.Close(); // close the Serial connection
        }
        public void PinMode(int pin, bool mode)
        {
            SendCommand(new int[] { 48, 97 + pin, 48 + Convert.ToInt32(mode) });
            ServoDetach(pin);
        }
        // Read the digital pin from the arduino board
        public bool DigitalRead(int pin)
        {
            SendCommand(new int[] { 49, 97 + pin });
            string val = Serial.ReadLine();
            bool pinNumber = Convert.ToBoolean(ushort.Parse(val));
            return pinNumber;
        }
        // read the analog pin from the arduino board
        public UInt16 AnalogRead(int pin)
        {
            SendCommand(new int[] { 51, 97 + pin });
            string val = Serial.ReadLine();
            UInt16 AnalogValue = UInt16.Parse(val);
            return AnalogValue;
        }
        // write PWM value
        public void AnalogWrite(int pin, int value)
        {
            CheckInputs(value, 255, 0);
            SendCommand(new int[] { 52, 97 + pin, value });
        }
        // write low or high to digital pin
        public void DigitalWrite(int pin, int value)
        {
            CheckInputs(value, 1, 0);
            SendCommand(new int[] { 50, 97 + pin, value + 48 });
        }
        // send command to attach servo to the pin
        public void ServoAttach(int pin)
        {
            SendCommand(new int[] { 54, 97 + pin, 49 });
        }
        // send command to detach servo to the pin
        public void ServoDetach(int pin)
        {
            SendCommand(new int[] { 54, 97 + pin, 48 });
        }
        // determine if servo is attached or detached for the pin
        public bool ServoStatus(int pin)
        {
            SendCommand(new int[] { 53, 97 + pin });
            string val = Serial.ReadLine();
            bool Status = Convert.ToBoolean(Convert.ToUInt16(val));
            return Status;
        }
        // write value to servo motor
        public void ServoWrite(int pin, int value)
        {
            CheckInputs(value, 180, 0);
            SendCommand(new int[] { 56, 97 + pin, value});
        }

        // read value of servo motor
        public void ServoRead(int pin, int value)
        {
            CheckInputs(value, 180, 0);
            SendCommand(new int[] { 55, 97 + pin, value });
        }

        // set speed of dc motor
        public void MotorSpeed(int num, int value)
        {
            CheckInputs(num, 4, 1);
            CheckInputs(value, 255, 0);
            SendCommand(new int[] { 65, 48 + num, value });
        }

        private void CheckInputs(int value, int UpperLimit, int LowerLimit)
        {
            if (value > UpperLimit || value < LowerLimit)
            {
                string UpperLimitString = UpperLimit.ToString();
                string LowerLimitString = LowerLimit.ToString();
                throw new Exception("Value entered must be between " + LowerLimitString + " and " + UpperLimitString);
            }
        }
        
        private void SendCommand(int[] Array) // subroutine used to write serial data commands to the arduino
        {
            byte[] ArrayNew = new byte[Array.Length];
            for (int i = 0; i < Array.Length; i++)
			{
			    ArrayNew[i] = (byte) Array[i];
			}
            Serial.Write(ArrayNew, 0, ArrayNew.Length);
            Serial.WriteLine("");
        }
    }
}
