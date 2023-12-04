using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WebCamLib;

namespace ImageProcessing
{
    public partial class Form1 : Form
    {
        private bool isInvert = false;
        private bool isSubtract = false;
        private bool isGrey = false;
        private Bitmap loadedImage, processedImage;
        private Bitmap imageA, imageB, resultImage;
        private Color pixel;
        private Device webcam = DeviceManager.GetDevice(0);
        private Device[] devices = DeviceManager.GetAllDevices();

        public Form1() { 
            InitializeComponent();
            this.Height = 400;
            this.Width = 850;      
        }

        private void OpenFileDialogFileOk(object sender, CancelEventArgs e)
        {
            loadedImage = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loadedImage;
        }

        private void CopyToolStripMenuItemClick(object sender, EventArgs e)
        {
            processedImage = new Bitmap(loadedImage.Width, loadedImage.Height);

            for(int x = 0; x < loadedImage.Width; x++)
            {
                for(int y = 0; y < loadedImage.Height; y++)
                {
                    pixel = loadedImage.GetPixel(x, y);
                    processedImage.SetPixel(x, y, pixel);
                }
            }

            pictureBox2.Image = processedImage;
        }

        private void GreyscaleToolStripMenuItemClick(object sender, EventArgs e)
        {
            processedImage = new Bitmap(loadedImage.Width, loadedImage.Height);

            for (int x = 0; x < loadedImage.Width; x++)
            {
                for (int y = 0; y < loadedImage.Height; y++)
                {
                    pixel = loadedImage.GetPixel(x, y);
                    int greyScale = ((pixel.R + pixel.G + pixel.B) / 3);
                    processedImage.SetPixel(x, y, Color.FromArgb(greyScale, greyScale, greyScale));
                }
            }

            pictureBox2.Image = processedImage;
        }

        private void NegativeToolStripMenuItemClick(object sender, EventArgs e)
        {
            processedImage = new Bitmap(loadedImage.Width, loadedImage.Height);

            for (int x = 0; x < loadedImage.Width; x++)
            {
                for (int y = 0; y < loadedImage.Height; y++)
                {
                    pixel = loadedImage.GetPixel(x, y);
                    processedImage.SetPixel(x, y, Color.FromArgb(255-pixel.R, 255 - pixel.G, 255 - pixel.B));
                }
            }

            pictureBox2.Image = processedImage;
        }

        private void HistogramToolStripMenuItemClick(object sender, EventArgs e)
        {
            processedImage = new Bitmap(loadedImage.Width, loadedImage.Height);

            for (int x = 0; x < loadedImage.Width; x++)
            {
                for (int y = 0; y < loadedImage.Height; y++)
                {
                    pixel = loadedImage.GetPixel(x, y);
                    int greyScale = ((pixel.R + pixel.G + pixel.B) / 3);
                    processedImage.SetPixel(x, y, Color.FromArgb(greyScale, greyScale, greyScale));
                }
            }

            Color sample;
            int[] histogramData = new int[256];

            for (int x = 0; x < loadedImage.Width; x++)
            {
                for (int y = 0; y < loadedImage.Height; y++)
                {
                    sample = processedImage.GetPixel(x, y);
                    histogramData[sample.R]++;
                }
            }

            Bitmap histogram = new Bitmap(256, 800);

            for (int x = 0; x < 256; x++)
                for (int y = 0; y < 800; y++)
                    histogram.SetPixel(x, y, Color.White);

            for (int x = 0; x < 256; x++)
                for (int y = 0; y < Math.Min(histogramData[x]/5,800); y++)
                    histogram.SetPixel(x, 799-y, Color.Black);

            pictureBox2.Image = histogram;
        }

        private void SepiaToolStripMenuItemClick(object sender, EventArgs e)
        {
            processedImage = new Bitmap(loadedImage.Width, loadedImage.Height);

            for (int x = 0; x < loadedImage.Width; x++)
            {
                for (int y = 0; y < loadedImage.Height; y++)
                {
                    pixel = loadedImage.GetPixel(x, y);
                    int alpha = pixel.A;
                    int r = pixel.R;
                    int g = pixel.G;
                    int b = pixel.B;

                    int sepiaRed = (int)(0.393 * r + 0.769 * g + 0.189 * b);
                    int sepiaGreen = (int)(0.349 * r + 0.686 * g + 0.168 * b);
                    int sepiaBlue = (int)(0.272 * r + 0.534 * g + 0.131 * b);

                    if (sepiaRed > 255)
                        r = 255;
                    else
                        r = sepiaRed;

                    if (sepiaGreen > 255)
                        g = 255;
                    else
                        g = sepiaGreen;

                    if (sepiaBlue > 255)
                        b = 255;
                    else
                        b = sepiaBlue;

                    processedImage.SetPixel(x, y, Color.FromArgb(alpha, r, g, b));
                }
            }

            pictureBox2.Image = processedImage;
        }

        private void SaveToolStripMenuItemClick(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }
        private void CloseToolStripMenuItemClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox
                        .Show("Do you want to exit?", 
                            "Exit Confirmation", 
                            MessageBoxButtons.YesNo, 
                            MessageBoxIcon.Question);

            if (result == DialogResult.No)
                e.Cancel = true;
        }

        private void SaveFileDialog1FileOk(object sender, CancelEventArgs e)
        {
            string fileName = saveFileDialog1.FileName;

            if (!fileName.ToLower().EndsWith(".png"))
                fileName += ".png";
            processedImage.Save(fileName);
        }

        private void OpenToolStripMenuItemClick(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void OpenFileDialog3FileOk(object sender, CancelEventArgs e)
        {
            imageB = new Bitmap(openFileDialog3.FileName);
            pictureBox2.Image = imageB;
        }
        private void OpenFileDialog2FileOk(object sender, CancelEventArgs e)
        {
            imageA = new Bitmap(openFileDialog2.FileName);
            pictureBox1.Image = imageA;
        }

        private void Button2Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void Button3Click(object sender, EventArgs e)
        {
            resultImage = new Bitmap(imageA.Width, imageA.Height);
            Color myGreen = Color.FromArgb(0, 255, 0);
            int greyGreen = (myGreen.R + myGreen.G + myGreen.B) / 3;
            int treshold = 5;

            for(int x = 0; x < imageA.Width; x++)
            {
                for (int y=0; y<imageA.Height; y++)
                {
                    Color pixel = imageA.GetPixel(x, y);
                    Color backPixel = imageB.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B)/3;
                    int subractionValue = Math.Abs(grey - greyGreen);

                    if(subractionValue > treshold)
                    {
                        resultImage.SetPixel(x, y, pixel);
                    }
                    else
                    {
                        resultImage.SetPixel(x, y, backPixel);
                    }
                }
            }

            pictureBox3.Image = resultImage;
        }


        private void OnToolStripMenuItem1Click(object sender, EventArgs e)
        {
            webcam.ShowWindow(pictureBox1);
            button1.Enabled = false;
            button3.Enabled = false;
        }

        private void OffToolStripMenuItem1Click(object sender, EventArgs e)
        {
            webcam.Stop();
            button1.Enabled = true;
            button3.Enabled = true;
        }

        private void InvertToolStripMenuItemClick(object sender, EventArgs e)
        {
            isInvert = !isInvert;
            timer3.Enabled = false;
            timer2.Enabled = false;
            timer1.Enabled = isInvert;
            button2.Enabled = false;
        }

        private void Timer1Tick(object sender, EventArgs e)
        {
            IDataObject data;
            Image bmap;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();

            if (data != null)
            {
                bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
                if (bmap != null)
                {
                    Bitmap b = new Bitmap(bmap);
                    ImageProcess2.BitmapFilter.Invert(b);
                    pictureBox3.Image = b;
                }
                else
                {
                    Console.WriteLine("Clipboard data is not a valid image.");
                }
            }
            else
            {
                Console.WriteLine("Clipboard data is not available.");
            }
        }

        private void GrayscaleToolStripMenuItemClick(object sender, EventArgs e)
        {
            isGrey = !isGrey;
            timer1.Enabled = false;
            timer3.Enabled = false;
            timer2.Enabled = isGrey;
            button2.Enabled = false;
        }

        private void Timer2Tick(object sender, EventArgs e)
        {
            IDataObject data;
            Image bmap;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();

            if (data != null)
            {
                bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));

                if (bmap != null)
                {
                    Bitmap b = new Bitmap(bmap);
                    ImageProcess2.BitmapFilter.GrayScale(b);
                    pictureBox3.Image = b;
                }
                else
                {
                    Console.WriteLine("Clipboard data is not a valid image.");
                }
            }
            else
            {
                Console.WriteLine("Clipboard data is not available.");
            }
        }

        private void Button4Click(object sender, EventArgs e)
        {
            isSubtract = !isSubtract;

            IDataObject data;
            Image bmap;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
            
            if(imageB != null &&bmap.Size == imageB.Size)
            {
                timer1.Enabled = false;
                timer2.Enabled = false;
                timer3.Enabled = isSubtract;
            }
            else
            {
                Console.WriteLine("Background is null or not the same resolution as webcam");
            }
        }


        private void Timer3Tick(object sender, EventArgs e)
        {
            IDataObject data;
            Image bmap;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();

            int threshold = 100;

            if (data != null)
            {
                bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));

                if (bmap != null)
                {
                    Bitmap b = new Bitmap(bmap);

                    ImageProcess2.BitmapFilter.Subtract(b, imageB, Color.Green, threshold);

                    pictureBox3.Image = b;
                }
                else
                {
                    Console.WriteLine("Clipboard data is not a valid image.");
                }
            }
            else
            {
                Console.WriteLine("Clipboard data is not available.");
            }
        }

    }
}
