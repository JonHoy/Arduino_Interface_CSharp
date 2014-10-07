using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArduinoClass;
using System.IO.Ports;

namespace Arduino_AnalogRead
{
    public partial class Form1 : Form
    {
        public Arduino myArduino;
        public Sensor mySensor;
        public Form1()   
        {
            InitializeComponent();
            string[] Ports = SerialPort.GetPortNames();
            foreach (var port in Ports)
            {
                try
                {
                    this.myArduino = new Arduino(port); 
                    break;
                }
                catch (Exception)
                {
                }
            }
            mySensor = new Sensor(myArduino, 0);
              
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.chart1.Series.Clear();
            button1_Click(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                string PinNumber = "Analog " + i.ToString();
                this.chart1.Series.Add(PinNumber);
                this.chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;    
            }
            
            
            this.timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            for (int i = 0; i < 1; i++)
            {
                string PinNumber = "Analog " + i.ToString();
                var Val = mySensor.getSensorReading();
                var Dist = Math.Min(12 / Val, 40);
                this.chart1.Series[PinNumber].Points.AddY((double)Dist);
            }
        }
    }
}
