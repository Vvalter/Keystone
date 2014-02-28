using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
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
    class Board
    {
        private Random random = new Random();
        protected bool randomActive = true;
        private double scale = 1.0;
        private double margin = 0.0;

        protected Point offset, resolution;
        
        protected int enemy_mobs = 4;
        protected int friendly_mobs = 3;
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
        protected Point GetMob(bool enemy, int pos)
        {
            Point p = new Point();
            int num_mobs = (enemy) ? (enemy_mobs) : (friendly_mobs);
            p.y = GetRealY(middle.y + (enemy ? (-mob.y / 2) : (mob.y / 2)) + GetRandom(50));
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
        protected KSDllWrapper.Rectangle windowRect;
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
            return (65535 * (windowRect.x + (int)Math.Round(x * scale + margin))) / Screen.PrimaryScreen.Bounds.Width;
        }
        private int GetRealY(double y)
        {
            return (65535 * (windowRect.y + (int)Math.Round(y * scale))) / Screen.PrimaryScreen.Bounds.Height;
        }
        public bool Update()
        {
            try
            {
                windowRect = KSDllWrapper.GetHearthstoneWindow();
                aspect = KSDllWrapper.GetAspectRatio(); // TODO handle Exception
            } 
            catch (Win32Exception)
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

            using (Bitmap b = ImageHelper.GetBitmap())
            {
		    b.
            }

            return true;
        }

    }
}
