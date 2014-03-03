using System;
using System.Windows.Forms;
using System.Threading;

namespace KSInterface
{
    class KeyboardController : Board
    {
        private MainForm _mainForm;
        
        private int friendly_mobs_x = 0;
        private int enemy_mobs_x = 0;
        private bool hero_selected = true;
        private int cards_x = 0;
        private YLevel current_level = YLevel.FRIENDLY_CARDS;
        private Point position;
        private Thread keyThread;
        public KeyboardController (MainForm mf) 
        {
            _mainForm = mf;
            position = middle;

            Update();
            keyThread = new Thread(KeyboardLoop);
            keyThread.Name = "Controller Thread";
            keyThread.IsBackground = true;
            keyThread.Start();
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
                    hero_selected = !hero_selected;
                    break;

                case YLevel.FRIENDLY_CARDS: // TODO
                    break;
            }
        }
        public void Left()
        {
            switch (current_level)
            {
                case YLevel.ENEMY_HERO:
                    break;
                case YLevel.ENEMY_MOBS:
                    enemy_mobs_x--;
                    if (enemy_mobs_x < 0)
                    {
                        enemy_mobs_x = enemy_mobs - 1;
                    }
                    break;
                case YLevel.FRIENDLY_MOBS:
                    friendly_mobs_x--;
                    if (friendly_mobs_x < 0)
                    {
                        friendly_mobs_x = friendly_mobs - 1;
                    }
                    break;
                case YLevel.FRIENDLY_HERO:
                    hero_selected = !hero_selected;
                    break;

                case YLevel.FRIENDLY_CARDS: // TODO
                    break;
            }
        }
        private void ChangeCards(int num)
        {
            switch (current_level)
            {
                case YLevel.ENEMY_MOBS:
                    enemy_mobs += num;
                    if (enemy_mobs < 0)
                    {
                        enemy_mobs = 0;
                    }
                    if (enemy_mobs > 7)
                    {
                        enemy_mobs = 7;
                    }
                    _mainForm.SetEnemyCards(enemy_mobs);
                    break;
                case YLevel.FRIENDLY_MOBS:
                    friendly_mobs += num;
                    if (friendly_mobs < 0)
                    {
                        friendly_mobs = 0;
                    }
                    if (friendly_mobs > 7)
                    {
                        friendly_mobs = 7;
                    }
                    _mainForm.SetFriendlyCards(friendly_mobs);
                    break;
                case YLevel.FRIENDLY_CARDS:
                    // TODO
                    break;
            }
        }
        public void UpdatePosition()
        {
            randomActive = true;
            Point currentPoint = new Point(0,0);;
            switch (current_level)
            {
                case YLevel.ENEMY_HERO:
                    currentPoint = enemy;
                    break;
                case YLevel.ENEMY_MOBS:
                    currentPoint = GetMob(true, enemy_mobs_x);
                    /*position.y = enemy_y;
                    position.x = middle.x + (enemy_mobs_x * mob_width)
                        - mob_width * (enemy_mobs / 2) - (mob_width / 2) * ((enemy_mobs % 2)-1);*/
                    break;
                case YLevel.FRIENDLY_MOBS:
                    currentPoint = GetMob(false, friendly_mobs_x);
                    /*position.y = friendly_y;
                    position.x = middle.x + (friendly_mobs_x * mob_width)
                        - mob_width * (friendly_mobs / 2) - (mob_width / 2) * ((friendly_mobs % 2)-1);*/
                    break;
                case YLevel.FRIENDLY_HERO:
                    if (hero_selected)
                    {
                        currentPoint = hero;
                    }
                    else
                    {
                        currentPoint = ability;
                    }
                    break;
                case YLevel.FRIENDLY_CARDS:
                    currentPoint = end;
                    break;
            }
            KSDllWrapper.SetMousePosition(currentPoint.x, currentPoint.y);
        }
        public void KeyboardLoop()
        {
            //Console.Write("Controller ready
            foreach (Keys k in KSDllWrapper.KeyInputs.GetConsumingEnumerable())
            {
                bool updateNeeded = false;
                switch (k)
                {
                    case Keys.Down:
                    case Keys.J:
                        Down();
                        updateNeeded = true;
                        break;
                    case Keys.Up:
                    case Keys.K:
                        Up();
                        updateNeeded = true;
                        break;
                    case Keys.Left:
                    case Keys.H:
                        Left();
                        updateNeeded = true;
                        break;
                    case Keys.Right:
                    case Keys.L:
                        Right();
                        updateNeeded = true;
                        break;
                    case Keys.Space:
                        KSDllWrapper.PressMouse(true);
                        KSDllWrapper.PressMouse(false);
                        break;
                    case Keys.Add:
                        ChangeCards(1);
                        break;
                    case Keys.Subtract:
                        ChangeCards(-1);
                        break;
                }
                if (updateNeeded)
                {
                    UpdatePosition();
                }
            }
        }
	public override void Stop()
	{
        keyThread.Abort();
        base.Stop();
	}
    }
}
