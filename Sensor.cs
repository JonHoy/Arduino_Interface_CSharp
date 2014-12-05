using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ArduinoClass
{
    public class Sensor
    {
        private Arduino myArduino; // the arduino that is hooked up to the sensor
        public int analogPin {get; private set;} // the analog pin number that the sensor is using
        protected double bitCount = 10; // A/D resolution of the arduino
        protected double minVoltage = 0;
        protected double maxVoltage = 5;


        public Sensor(Arduino Board, int Pin)
        {
            this.myArduino = Board;
            this.analogPin = Pin;
        }
        public double getSensorReading()
        {
            var val = (double) myArduino.AnalogRead(analogPin);
            var voltage = val / (Math.Pow(2, bitCount) - 1.0) * (maxVoltage - minVoltage) + minVoltage;
            return voltage;
        }

        // computes the resistance of a sensor in a voltage divider circuit given 
        public double getSensorResistance(double ConstResistanceValue) {
            var val = (double)myArduino.AnalogRead(analogPin);
            var ratio = 1 - val / (Math.Pow(2, bitCount) - 1);
            return ratio * ConstResistanceValue;
        }

    }
}
