using System;
using System.Drawing;

namespace KSInterface
{
    static class ImageHelper
    {
        public static Bitmap GetBitmap()
        {
            KSDllWrapper.Rectangle r = KSDllWrapper.GetHearthstoneWindow();
            Bitmap b = new Bitmap(r.width, r.height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.CopyFromScreen(new System.Drawing.Point(r.x, r.y), new System.Drawing.Point(0, 0), new Size(r.width, r.height));
            }
            return b;
        }
    }
}
