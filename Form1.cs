using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessingApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static byte ToGrayscale(Bitmap bitmap, int x, int y)
        {
            Color bitmapColor = bitmap.GetPixel(x, y);

            byte red = bitmapColor.R;
            byte green = bitmapColor.G;
            byte blue = bitmapColor.B;

            byte grayscaleValue = (byte)((red + green + blue) / 3);

            return grayscaleValue;
        }

        public static Color ToSepia(Bitmap bitmap, int x, int y)
        {
            Color bitmapColor = bitmap.GetPixel(x, y);

            byte newRed = (byte)(0.393 * bitmapColor.R + 0.769 * bitmapColor.G + 0.189 * bitmapColor.B);
            byte newGreen = (byte)(0.349 * bitmapColor.R + 0.686 * bitmapColor.G + 0.168 * bitmapColor.B);
            byte newBlue = (byte)(0.272 * bitmapColor.R + 0.534 * bitmapColor.G + 0.131 * bitmapColor.B);

            newRed = Math.Min((byte)255, newRed);
            newGreen = Math.Min((byte)255, newGreen);
            newBlue = Math.Min((byte)255, newBlue);

            Color sepia = Color.FromArgb(newRed, newGreen, newBlue);

            return sepia;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string file = openFileDialog1.FileName;
            pictureBox1.Image = Image.FromFile(file);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmap1 = new Bitmap(pictureBox1.Image);
            Bitmap bitmap2 = new Bitmap(bitmap1.Width, bitmap1.Height);

            for (int i = 0; i < bitmap1.Width; i++)
            {
                for (int j = 0; j < bitmap1.Height; j++)
                {
                    Color bitmap1Color = bitmap1.GetPixel(i, j);
                    bitmap2.SetPixel(i, j, bitmap1Color);
                }
            }

            pictureBox3.Image = bitmap2;
        }

        private void grayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmap1 = new Bitmap(pictureBox1.Image);

            for (int i = 0; i < bitmap1.Width; i++)
            {
                for (int j = 0; j < bitmap1.Height; j++)
                {
                    byte grayscaleValue = ToGrayscale(bitmap1, i, j);
                    bitmap1.SetPixel(i, j, Color.FromArgb(grayscaleValue, grayscaleValue, grayscaleValue));
                }
            }

            pictureBox3.Image = bitmap1;
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmap1 = new Bitmap(pictureBox1.Image);

            for (int i = 0; i < bitmap1.Width; i++)
            {
                for (int j = 0; j < bitmap1.Height; j++)
                {
                    Color bitmap1Color = bitmap1.GetPixel(i, j);

                    byte red = bitmap1Color.R;
                    byte green = bitmap1Color.G;
                    byte blue = bitmap1Color.B;

                    bitmap1.SetPixel(i, j, Color.FromArgb(255 - red, 255 - green, 255 - blue));
                }
            }

            pictureBox3.Image = bitmap1;
        }

        private void loadForegroundButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void loadBackgroundButton_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            string file = openFileDialog2.FileName;
            pictureBox2.Image = Image.FromFile(file);
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Please insert a foreground image.");
                return;
            }

            Form2 form = Application.OpenForms.OfType<Form2>().FirstOrDefault();
            if (form != null)
            {
                form.Activate();
                return;
            }
            form = new Form2();
            form.Show();
            form.CreateHistogram(pictureBox1);
        }

        private void subtractionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null ||  pictureBox2.Image == null)
            {
                MessageBox.Show("Please insert a foreground and a background to continue");
                return;
            }

            Color controlGreen = Color.FromArgb(0, 255, 0);
            int grayscaleGreen = (controlGreen.R + controlGreen.G + controlGreen.B) / 3;
            int threshold = 5;

            Bitmap bitmap1 = new Bitmap(pictureBox1.Image);
            Bitmap bitmap2 = new Bitmap(pictureBox2.Image);
            Bitmap bitmap3 = new Bitmap(bitmap1.Width, bitmap1.Height);

            for (int i = 0; i < bitmap1.Width; i++)
            {
                for (int j = 0; j < bitmap1.Height; j++)
                {
                    Color bitmap1Color = bitmap1.GetPixel(i, j);
                    Color bitmap2Color = bitmap2.GetPixel(i, j);

                    int grayscale = (int) Form1.ToGrayscale(bitmap1, i, j);
                    int subtractValue = Math.Abs(grayscale - grayscaleGreen);

                    if (subtractValue > threshold)
                    {
                        bitmap3.SetPixel(i, j, bitmap1Color);
                    } else
                    {
                        bitmap3.SetPixel(i, j, bitmap2Color);
                    }
                }
            }
            pictureBox3.Image = bitmap3;
        }

        private void saveOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox3.Image == null)
            {
                MessageBox.Show("No output image");
                return;
            }

            saveFileDialog1.DefaultExt = "bmp";
            saveFileDialog1.FileName = "image.bmp";
            saveFileDialog1.Filter = "Bitmap Image|*.bmp";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox3.Image.Save(saveFileDialog1.FileName);
            }
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmap1 = new Bitmap(pictureBox1.Image);

            for (int i = 0; i < bitmap1.Width; i++)
            {
                for (int j = 0; j < bitmap1.Height; j++)
                {
                    Color sepiaColor = ToSepia(bitmap1, i, j);
                    bitmap1.SetPixel(i, j, sepiaColor);
                }
            }

            pictureBox3.Image = bitmap1;
        }

        private void cameraSubtractionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form = Application.OpenForms.OfType<Form3>().FirstOrDefault();
            if (form != null)
            {
                form.Activate();
                return;
            }
            form = new Form3();
            form.Show();
        }
    }
}
