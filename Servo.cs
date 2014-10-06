using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArduinoClass;

namespace ArduinoClass
{
    public class Servo
    {
        public Arduino myArduino;

        public int Angle { get; private set; } // define the current angle of the servo (by default it is equal to 90)
        public int Digital_Pin { get; private set; } // define the pin number of the digital pin
        public int MinAngle { get; private set; } // define the min angle that the servo can be
        public int MaxAngle { get; private set; } // define the min angle that the servo can be

        public Servo(ref Arduino myArduino, int Digital_Pin, int MaxAngle = 180, int MinAngle = 0)
        {
            this.Angle = 90;
            this.myArduino = myArduino;
            this.Digital_Pin = Digital_Pin;
            this.myArduino.ServoAttach(Digital_Pin);
            this.MaxAngle = MaxAngle;
            this.MinAngle = 0;
        }

        public void DigitalPinChange(int NewPin)
        {
            myArduino.ServoDetach(Digital_Pin); // disconnect the old servo from the old pin
            Digital_Pin = NewPin; // reassign the pin
            myArduino.ServoAttach(Digital_Pin); // attach the servo to the new pin
            myArduino.ServoWrite(Digital_Pin, Angle); // write the angle that was on the old pin to the new pin
        }

        public void ServoAngleChange(int NewAngle)
        {
            if (NewAngle >= MinAngle && NewAngle <= MaxAngle)
            {
                Angle = NewAngle;
                myArduino.ServoWrite(Digital_Pin, Angle);
            }
        }

        public void Detach()
        {
            myArduino.ServoDetach(Digital_Pin);
        }

        public void Attach()
        {
            myArduino.ServoAttach(Digital_Pin);
        }
    }

}
