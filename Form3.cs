using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebCamLib;

namespace ImageProcessingApp
{
    public partial class Form3 : Form
    {
        WebCamLib.Device device;

        public Form3()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void loadCameraButton_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                device = new Device();
                this.Invoke((Action)(() => device.ShowWindow(pictureBox1)));
            });
            timer1.Enabled = true;
        }

        private void loadBackgroundButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string file = openFileDialog1.FileName;
            pictureBox2.Image = Image.FromFile(file);
        }

        private void subtractButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null || pictureBox2.Image == null)
            {
                MessageBox.Show("Please enable camera and insert a background.");
                return;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
