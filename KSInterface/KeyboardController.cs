using System;
using System.Windows.Forms;

namespace KSInterface
{
    class KeyboardController : Board
    {
        private MainForm _mainForm;

        private int friendly_mobs_x = 0;
        private int enemy_mobs_x = 0;
        private bool hero = true;
        private int cards_x = 0;
        private YLevel current_level = YLevel.FRIENDLY_CARDS;
        private Point position;
        public KeyboardController (MainForm mf, int offset_x, int offset_y, int res_x, int res_y) 
            : base(offset_x, offset_y, res_x, res_y)
        {
            _mainForm = mf;
            position = middle;
            /* Find Hearthstone Window */
        }
        public void Up()
        {
            current_level--;
            if (current_level < YLevel.ENEMY_HERO)
            {
                current_level = YLevel.FRIENDLY_CARDS;
            }
        }

        public void Down()
        {
            current_level++;
            if (current_level > YLevel.FRIENDLY_CARDS)
            {
                current_level = YLevel.ENEMY_HERO;
            }
        }

        public void Right()
        {
            switch (current_level)
            {
                case YLevel.ENEMY_HERO:
                    break;
                case YLevel.ENEMY_MOBS:
                    enemy_mobs_x++;
                    if (enemy_mobs_x >= enemy_mobs)
                    {
                        enemy_mobs_x = 0;
                    }
                    break;
                case YLevel.FRIENDLY_MOBS:
                    friendly_mobs_x++;
                    if (friendly_mobs_x >= friendly_mobs)
                    {
                        friendly_mobs_x = 0;
                    }
                    break;
                case YLevel.FRIENDLY_HERO:
                    hero = !hero;
                    break;

                case YLevel.FRIENDLY_CARDS: // TODO
                    break;
            }
        }
        public void UpdatePosition()
        {
            switch (current_level)
            {
                case YLevel.ENEMY_HERO:
                    position.x = enemy_hero.x;
                    position.y = enemy_hero.y;
                    break;
                case YLevel.ENEMY_MOBS:
                    position.y = enemy_y;
                    position.x = middle.x + (enemy_mobs_x * mob_width)
                        - mob_width * (enemy_mobs / 2) - (mob_width / 2) * ((enemy_mobs % 2)-1);
                    break;
                case YLevel.FRIENDLY_MOBS:
                    position.y = friendly_y;
                    position.x = middle.x + (friendly_mobs_x * mob_width)
                        - mob_width * (friendly_mobs / 2) - (mob_width / 2) * ((friendly_mobs % 2)-1);
                    break;
                case YLevel.FRIENDLY_HERO:
                    if (hero)
                    {
                        position.x = friendly_hero.x;
                        position.y = friendly_hero.y;
                    }
                    else
                    {
                        position.x = ability.x;
                        position.y = ability.y;
                    }
                    break;
                case YLevel.FRIENDLY_CARDS:// TODO 
                    position.x = 700;
                    position.y = 1020;
                    break;
            }

            KSDllWrapper.SetMousePosition(GetNormalizedX(position.x), GetNormalizedY(position.y));
        }
        public void KeyboardLoop()
        {
            //Console.Write("Controller ready
            foreach (Keys k in KSDllWrapper.KeyInputs.GetConsumingEnumerable())
            {
                switch (k)
                {
                    case Keys.Down:
                    case Keys.J:
                        Down();
                        break;
                    case Keys.Up:
                    case Keys.K:
                        Up();
                        break;
                    case Keys.Left:
                    case Keys.H:
                       // Left();
                        break;
                    case Keys.Right:
                    case Keys.L:
                        Right();
                        break;
                }
                UpdatePosition();
            }
        }

    }
}
