using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using AForge.Imaging;
using AForge.Imaging.Filters;
namespace ImageComperator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
	    
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string[] files = openFileDialog1.FileNames;
            FileInfo info;
            FileStream stream;

            info = new FileInfo(files[0]);
            stream = info.OpenRead();
            pictureBox1.Image = System.Drawing.Image.FromStream(stream);
            stream.Close();
            if (files.Length == 2)
            {
                info = new FileInfo(files[1]);
                stream = info.OpenRead();
                pictureBox2.Image = System.Drawing.Image.FromStream(stream);
                stream.Close();
            }

            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0);
	    System.Drawing.Image i1 = pictureBox1.Image;
	    System.Drawing.Image i2 = pictureBox2.Image;
	    using (Bitmap b1 = new Bitmap(i1.Width, i1.Height, PixelFormat.Format24bppRgb))
	    using (Bitmap b2 = new Bitmap(i2.Width, i2.Height, PixelFormat.Format24bppRgb))
	    using (Graphics g1 = Graphics.FromImage(b1))
	    using (Graphics g2 = Graphics.FromImage(b2))
        {
            g1.DrawImage(i1, new Point(0,0));
            g2.DrawImage(i2, new Point(0,0));
            TemplateMatch[] matching = tm.ProcessImage(b2, b1);
            toolStripStatusLabel1.Text = matching[0].Similarity.ToString();
        }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            openFileDialog1.ShowDialog();
        }
    }
}
