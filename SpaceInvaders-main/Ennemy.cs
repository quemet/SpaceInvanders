using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    public class Ennemy
    {
        private int Id;
        private int X;
        private int BulletX;
        private int Y;
        private int BulletY;
        private int Life;
        private string Skin;
        private string Name;
        private string Type;
        private ConsoleColor Color;
        private bool IsBulletActiveE;

        public int id { get => Id; set => Id = value; }
        public int x { get => X; set => X = value; }
        public int y { get => Y; set => Y = value; }
        public int bulletX { get => BulletX; set => BulletX = value; }
        public int bulletY { get => BulletY; set => BulletY = value; }
        public int life { get => Life; set => Life = value; }
        public string skin { get => Skin; set => Skin = value; }
        public string name { get => Name; set => Name = value; }
        public string type { get => Type; set => Type = value; }
        public ConsoleColor color { get => Color; set => Color = value; }
        public bool isBulletActiveE { get => IsBulletActiveE; set => IsBulletActiveE = value; }

        /// <summary>
        /// Only constructor for the class Ennemy
        /// </summary>
        /// <param name="id">The argument id is the id of the ennemy</param>
        /// <param name="x">The argument x is the value x of the ennemy</param>
        /// <param name="y">The argument y is the value y of the ennemy</param>
        /// <param name="life">The argument life is the count of life left to the ennemy</param>
        /// <param name="skin">The argument skin is the skin of the ennemy</param>
        /// <param name="name">The argument name is the name of the ennemy</param>
        /// <param name="type">The argument type is the type of the ennemy like simple alien, general alien or King alien</param>
        /// <param name="color">The argument color is the color of the ennemy</param>
        /// <param name="isBulletActiveE">The argument isBulletActiveE is the boolean to defined if the ennemy shoot a missil or not</param>
        public Ennemy(int id, int x, int y, int life, string skin, string name, string type, ConsoleColor color, bool isBulletActiveE)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.life = life;
            this.skin = skin;
            this.name = name;
            this.type = type;
            this.color = color;
            this.isBulletActiveE = isBulletActiveE;
        }

        /// <summary>
        /// This method control if the ennmy is in the two side otf the console if not she move the ennemy to one to the right
        /// </summary>
        public void MoveEnnemy()
        {
            Console.ForegroundColor = color;
            if (x >= Console.WindowWidth - 3)
            {
                for(int i = 0; i < 3; i++)
                {
                    Console.SetCursorPosition(x + i, y);
                    Console.WriteLine(" ");
                }

                x = 0;
                y = y + 1;

                Console.SetCursorPosition(x, y);
                Console.Write(skin);
            }
            else
            {
                Console.SetCursorPosition(x, y);
                Console.Write(skin);

                Console.SetCursorPosition(x - 1, y);
                Console.Write(" ");
            }

            x++;
        }

        /// <summary>
        /// This method is testing if the bullet of the player is touching a ennemy if it's true the ennemy change his colour or die and implement score
        /// </summary>
        /// <param name="ship">The argument with the name ship is the player ship</param>
        public void TouchEnnemy(Ship ship)
        {
            if ((ship.bulletX >= x - 1 && ship.bulletX <= x + skin.Length - 1) && y == ship.bulletY)
            {
                life--;
                ship.score += 300 * ship.life;
                switch (life)
                {
                    case 2:
                        color = ConsoleColor.Blue;
                        break;
                    case 1:
                        color = ConsoleColor.Red;
                        break;
                    case 0:
                        for (int i = 0; i < 3; i++)
                        {
                            Console.SetCursorPosition(x + i, y);
                            Console.WriteLine(" ");
                        }
                        Console.SetCursorPosition(ship.bulletX, ship.bulletY);
                        Console.WriteLine(" ");
                        break;
                }

                if(life != 0)
                {
                    Console.ForegroundColor = color;
                    Console.SetCursorPosition(x, y);
                    Console.WriteLine(skin);

                    Console.SetCursorPosition(x - 1, y);
                    Console.WriteLine(" ");
                }
            }
        }

        /// <summary>
        /// This methid handle the shoot of the ennemies
        /// </summary>
        /// <param name="frame">it's the frame of the program</param>
        public void EnnemyShoot(long frame)
        {
            Console.ForegroundColor = color;
            if(frame % 100 == 0)
            {
                if (isBulletActiveE)
                {
                    if (bulletY >= 0)
                    {
                        Console.SetCursorPosition(bulletX, bulletY);
                        Console.Write("|");
                        Console.SetCursorPosition(bulletX, bulletY - 1);
                        Console.Write(" ");
                    }

                    bulletY = bulletY + 1;
                }

                if (bulletY > 26)
                {
                    isBulletActiveE = false;
                    Console.SetCursorPosition(bulletX, 26);
                    Console.WriteLine(" ");
                }
            }
        }
    }
}