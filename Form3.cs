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
        }

        private void loadCameraButton_Click(object sender, EventArgs e)
        {
            device = new Device();
            device.ShowWindow(pictureBox1);
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
    }
}
