using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace KSInterface
{
    class Point
    {
        public int x, y;
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Point() : this(0,0) {}
    }
    public struct Rectangle
    {
        public int x, y;
        public int width, height;

        public Rectangle(long x, long y, long width, long height)
        {
            this.x = (int)x;
            this.y = (int)y;

            this.width = (int)width;
            this.height = (int)height;
        }
    }
    class Board
    {
        private string workingDir = @"C:\Keystone\";
        private System.Threading.Timer timer;
        private Random random = new Random();
        protected bool randomActive = true;
        protected bool realityActive = true;
        private double scale = 1.0;
        private double margin = 0.0;

        protected int enemy_mobs = 4;
        protected int friendly_mobs = 1;
        protected int cards = 5;
        protected Point end
        {
            get { return new Point(GetRealX(937+GetRandom(35)), GetRealY(347+GetRandom(14)));}
        }
        protected Point enemy
        {
            get { return new Point(GetRealX(520 + GetRandom(38)), GetRealY(144 + GetRandom(41))); }
        }
        protected Point hero
        {
            get { return new Point(GetRealX(520 + GetRandom(38)), GetRealY(590 + GetRandom(41))); }
        }
        protected Point ability
        {
            get { return new Point(GetRealX(641 + GetRandom(33)), GetRealY(587+ GetRandom(33))); }
        }
        protected Point middle = new Point(512, 356);
        protected Point mob = new Point(100, 120);
        protected int mobDistance = 65;
        protected Point mobMin = new Point(82, 114);
        protected Point GetMob(bool enemy, int pos)
        {
            Point p = new Point();
            int num_mobs = (enemy) ? (enemy_mobs) : (friendly_mobs);
            p.y = GetRealY(middle.y + (enemy ? (-mobDistance) : (mobDistance)) + GetRandom(50));
            p.x = middle.x;
            if (num_mobs % 2 == 0)
            {
                p.x += mob.x / 2;
            }
            p.x -= (num_mobs / 2) * mob.x; // Move to the left
            p.x += pos * mob.x;
            p.x = GetRealX(p.x + GetRandom(40));
            return p;
        }
        protected Rectangle windowRect;
        protected ASPECTRATIO aspect;
        protected enum YLevel {
            ENEMY_HERO = 0, ENEMY_MOBS = 1, FRIENDLY_MOBS = 2, FRIENDLY_HERO = 3, FRIENDLY_CARDS = 4
        };

        private double GetRandom(double length)
        {
            return 0;
            if (randomActive)
            {
                return (2 * length * random.NextDouble()) - length;
            } 
            else
            {
                return 0;
            }
        }
        private int GetRealX(double x)
        {
            if (realityActive)
            {
                return (65535 * (windowRect.x + (int)Math.Round(x * scale + margin))) / Screen.PrimaryScreen.Bounds.Width;
            }
            else
            {
                return (int)Math.Round(x);
            }
        }
        private int GetRealY(double y)
        {
            if (realityActive)
            {
                return (65535 * (windowRect.y + (int)Math.Round(y * scale))) / Screen.PrimaryScreen.Bounds.Height;
            } 
            else
            {
                return (int)Math.Round(y);
            }
        }
        public bool Update()
        {
            try
            {
                windowRect = KSDllWrapper.GetHearthstoneWindow();
                aspect = KSDllWrapper.GetAspectRatio(); // TODO handle Exception
            }
            catch (Exception)
            {
                return false;
            }

            switch (aspect)
            {
                case ASPECTRATIO.R4_3:
                    margin = 0.0;
                    break;
                case ASPECTRATIO.R16_9:
                    switch (windowRect.width)
                    {
                        case 1366:
                            margin = 170;
                            break;
                        case 1360:
                            margin = 168;
                            break;
                        case 1600:
                            margin = 200;
                            break;
                        case 1840: // ~1920
                            margin = 252;
                            break;
                        default:
                            throw new Exception("Board.Update: Invalid windowRect.width");
                    }
                    break;
                case ASPECTRATIO.R16_10:
                    switch (windowRect.width)
                    {
                        case 1280:
                            margin = 106;
                            break;
                        case 1600:
                            margin = 135;
                            break;
                        case 1680:
                            margin = 175;
                            break;
                        default:
                            throw new Exception("Board.Update: Invalid windowRect.width");
                    }
                    break;
                default:
                    throw new NotImplementedException("Board.Update: Invalid Aspectratio");
            }
            // Normed for 1024x768
            scale = windowRect.height / 768.0;

            if (timer == null)
            {
                timer = new System.Threading.Timer(FetchAndProcessImage,null,0,100);
            }
            return true;
        }
	public virtual void Stop()
        {
            timer.Dispose();
            timer = null;
        }
	private void FetchAndProcessImage(Object state)
        {
            bool saveReality = realityActive;
            bool saveRandom = randomActive;
            realityActive = false;
            randomActive = false;
            //using (Bitmap screen = ImageHelper.GetBitmap())
            Bitmap screen = ImageHelper.GetBitmap(scale, margin);
            using (Graphics g = Graphics.FromImage(screen))
            {
		foreach (bool enemy in new bool[]{true, false})
		{
		    /* Check if there's on in the middle */
		    Point midMob = new Point(middle.x, (middle.y + (enemy ? (-mobDistance) : (mobDistance))));
		    using (Bitmap m = new Bitmap(mobMin.x, (int)(mob.y*0.6)))
		    using (Graphics gm = Graphics.FromImage(m))
		    {
			gm.DrawImage(screen, 0, 0, new System.Drawing.Rectangle(midMob.x-mobMin.x/2, midMob.y-mobMin.y/2,mobMin.x, mobMin.y), GraphicsUnit.Pixel);
            string st = Path.Combine(workingDir, "mid.bmp");
            Console.WriteLine(st);
            m.Save(Path.Combine(workingDir, "mid.bmp"), ImageFormat.Bmp);
            }
		}
                /*for (int i = 0; i < friendly_mobs; i++)
                {
		    Point p = GetMob(false, i);
		    //g.DrawEllipse(Pens.HotPink, p.x-mobMin.x/2, p.y-mobMin.y/2, mobMin.x, mobMin.y);
		    using (Bitmap m = new Bitmap(mob.x, (int)(mob.y*0.6)))
		    using (Graphics gm = Graphics.FromImage(m))
		    {
			gm.DrawImage(screen, 0, 0, new System.Drawing.Rectangle(p.x-mobMin.x/2, p.y-mobMin.y/2,mobMin.x, mobMin.y), GraphicsUnit.Pixel);
			m.Save(@"C:/test/" + i.ToString() + ".bmp"); 
		    }
		    g.Flush();
                }
                for (int i = 0; i < enemy_mobs; i++)
                {
		    Point p = GetMob(true, i);
		    g.DrawRectangle(Pens.HotPink, p.x-mob.x/2, p.y-mob.y/2, mob.x, mob.y);
		    g.Flush();
                }*/
            }

            if (screen != null)
            {
                MainForm.mf.SetImage(screen);
            }
            realityActive = saveReality;
            randomActive = saveRandom;
        }
    }
}
