using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSInterface
{
    struct Point
    {
        public int x, y;
    }
    class Board
    {
        protected int enemy_y = 400, friendly_y = 600, mob_width = 140;
        protected Point offset, resolution;
        protected Point end, enemy_hero, friendly_hero, ability, middle;

        protected enum YLevel {
            ENEMY_HERO = 0, ENEMY_MOBS = 1, FRIENDLY_MOBS = 2, FRIENDLY_HERO = 3, FRIENDLY_CARDS = 4
        };

        protected int enemy_mobs = 1;
        protected int friendly_mobs = 2;
        protected int cards = 5;

        public Board(int offset_x, int offset_y, int res_x, int res_y)
        {
            offset.x = offset_x;
            offset.y = offset_y;

            resolution.x = res_x;
            resolution.y = res_y;

            end.x = 1550;
            end.y = 490;

            enemy_hero.x = 960;
            enemy_hero.y = 200;

            friendly_hero.x = 960;
            friendly_hero.y = 850;

            ability.x = 1130;
            ability.y = 830;

            middle.x = 960;
            middle.y = 500;
        }

        public int GetNormalizedX(int x)
        {
            return (65535 * x) / resolution.x;
        }
        public int GetNormalizedY(int y)
        {
            return (65535 * y) / resolution.y;
        }
    }
}
