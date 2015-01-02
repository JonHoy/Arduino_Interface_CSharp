using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ArduinoClass
{
    public class Sensor
    {
        protected Arduino myArduino; // the arduino that is hooked up to the sensor
        protected int analogPin;
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
            var val = myArduino.AnalogRead(analogPin);
            var voltage = val / (Math.Pow(2, bitCount) - 1.0) * (maxVoltage - minVoltage) + minVoltage;
            return voltage;
        }

        // computes the resistance of a sensor in a voltage divider circuit given 
        public double getSensorResistance(double ConstResistanceValue) {
            var val = myArduino.AnalogRead(analogPin);
            var Resistance = (Math.Pow(2, bitCount) / val - 1) * ConstResistanceValue;
            return Resistance;
        }

    }
}
