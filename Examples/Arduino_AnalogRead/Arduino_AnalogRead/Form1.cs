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
namespace Arduino_AnalogRead
{
    public partial class Form1 : Form
    {
        public Arduino myArduino;
        public Form1()
        {
            InitializeComponent();
            this.myArduino = new Arduino("COM4");   
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
            for (int i = 0; i < 6; i++)
            {
                string PinNumber = "Analog " + i.ToString();
                this.chart1.Series.Add(PinNumber);
                this.chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;    
            }
            
            
            this.timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            for (int i = 0; i < 6; i++)
            {
                ushort Val = myArduino.AnalogRead(i);
                string PinNumber = "Analog " + i.ToString();
                this.chart1.Series[PinNumber].Points.AddY((double)Val);
            }
        }
    }
}
