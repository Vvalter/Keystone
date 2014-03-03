using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
namespace KSInterface
{
    static class ImageHelper
    {
	public static void ProcessImage(string dir)
        {
            var files = Directory.EnumerateFiles(dir);
            foreach (string filename in files)
            {
                System.Console.WriteLine("Processing: " + filename);
                using (Bitmap b = new Bitmap(filename))
                using (Bitmap b2 = new Bitmap(162, 150))
                using (Graphics g = Graphics.FromImage(b2))
                {
                    g.DrawImage(b, new System.Drawing.Rectangle(0, 0, 162, 150), new System.Drawing.Rectangle(68, 5, 162, 150), GraphicsUnit.Pixel);
                    b2.Save(Path.Combine(dir, Path.GetFileNameWithoutExtension(filename)+"_converted.bmp"), ImageFormat.Bmp);
                }

            }
        }
        public static Bitmap GetBitmap(double scale, double margin)
        {
            try
            {
                Rectangle r = KSDllWrapper.GetHearthstoneWindow();
                Bitmap result;
                using (Bitmap b = new Bitmap(r.width, r.height))
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.CopyFromScreen(new System.Drawing.Point(r.x + (int)margin, r.y), new System.Drawing.Point(0, 0), new Size(r.width-2*(int)margin, r.height));
                    result = new Bitmap(b, new Size((int)(b.Width / scale), (int)(b.Height / scale)));
                }
                return result;
            } 
            catch (Exception)
            {
                return null;
            }
        }
    }
}
