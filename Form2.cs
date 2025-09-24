using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessingApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void CreateHistogram(PictureBox pictureBox)
        {
            Bitmap bitmap = new Bitmap(pictureBox.Image);
            int[] histogram = new int[256];

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    byte grayscaleValue = Form1.ToGrayscale(bitmap, i, j);
                    histogram[grayscaleValue]++;
                }
            }

            int histMax = histogram.Max();
            int histHeight = pictureBox1.Height;
            int histWidth = pictureBox1.Width;
            float binWidth = (float)histWidth / 256f;

            Bitmap histImage = new Bitmap(histWidth, histHeight);
            Graphics g = Graphics.FromImage(histImage);

            for (int i = 0; i < 256; i++)
            {
                int barHeight = (int)((float)histogram[i] / histMax * (histHeight));
                RectangleF barRect = new RectangleF(i * binWidth, histHeight - barHeight, binWidth, barHeight);
                g.FillRectangle(Brushes.Black, barRect);
            }

            pictureBox1.Image = histImage;
        }
    }
}
