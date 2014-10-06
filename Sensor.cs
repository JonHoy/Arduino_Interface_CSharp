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
        private int analogPin; // the analog pin number that the sensor is using
        private double bitCount = 10; // A/D resolution of the arduino
        private double minVoltage = 0;
        private double maxVoltage = 5;


        public Sensor(ref Arduino Board, int Pin)
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
 

    }
}
