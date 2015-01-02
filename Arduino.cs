using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace ArduinoClass
{
    // INSPIRED BY: MATLAB® Support Package for Arduino® Hardware
    
    public class Arduino : IDisposable
    {
        private int WAIT_TIME = 1; //  (12 sec = default) amount of time (sec) required to wait before issuing commands after opening Serial port
        private int PinMax = 69; // define maximum value for a digital pin 
        private SerialPort Serial; // Serial port object which controls read/write operations
        private bool[] servoStatus; // connection status of each servo
        private bool disposed = false; // Flag: Has Dispose already been called? 

        // code to auto detect arduino
        public Arduino() {
            string[] ports = SerialPort.GetPortNames();
            bool Connected = false; 
            for (int i = 0; i < ports.Length; i++)
            {
                try
                {
                    Arduino_Initialize(ports[i]);
                    Connected = true;
                    break;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            if (Connected == false)
	        {
                throw new Exception("Unable to connect to any of the COM Ports");
	        }

        }

        public Arduino(string COM) {
            Arduino_Initialize(COM);
        }
        
        private void Arduino_Initialize(string COM) // eg COM = "COM4"
        { 
            try 
	        {
                Serial = new SerialPort(COM, 115200);
                Serial.Encoding = new UTF8Encoding();
                Serial.ReadTimeout = 500;

                Serial.Open();
                Thread.Sleep(1000 * WAIT_TIME);
                // Send command to the arduino, wait for a response
                Serial.Write("99");
	        }
	        catch (Exception)
	        {
                throw new Exception("Unable to Connect to Arduino");
	        }

            try
            {
                string Check = Serial.ReadLine(); // try to get a response from the arduino board
            }
            catch (Exception)
            {
                throw new Exception("Unable to Connect to the Arduino at " + COM);
            }
            servoStatus = new bool[PinMax];
        }

        public void Disconnect() // destructor
        {
            // detach servos
            for (int i = 0; i < PinMax; i++)
            {
                if (servoStatus[i]) // if connected, detach it
                    ServoDetach(i);
            }
            Serial.Close(); // close the Serial connection
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
                Disconnect(); // disconnect the arduino before being disposed by the GC

            disposed = true;
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
        public double AnalogRead(int pin)
        {
            SendCommand(new int[] { 51, 97 + pin });
            string val = Serial.ReadLine();
            double AnalogValue = double.Parse(val);
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
            servoStatus[pin] = true;
        }
        // send command to detach servo to the pin
        public void ServoDetach(int pin)
        {
            SendCommand(new int[] { 54, 97 + pin, 48 });
            servoStatus[pin] = false;
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
