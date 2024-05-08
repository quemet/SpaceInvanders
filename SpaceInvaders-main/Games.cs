using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    public class Games
    {
        /// <summary>
        /// This function create a ship and returns it
        /// </summary>
        /// <returns>A Ship</returns>
        public Ship CreateShip()
        {
            var ship = new Ship(
                0,
                25,
                3,
                @"\_/",
                "Ship",
                0,
                ConsoleColor.White,
                false
            );
            return ship;
        }
        /// <summary>
        /// This function create 10 ennemies and store it in a list of ennemies and control if two ennemies had the same x
        /// </summary>
        /// <returns>A List of ennemies</returns>
        public List<Ennemy> CreationEnnemy()
        {
            Random random = new Random();
            List<Ennemy> ennemies = new List<Ennemy>();

            for (int i = 0; i < 10; i++)
            {
                var ennemy = new Ennemy(
                    i,
                    random.Next(1, 120 - 4),
                    random.Next(2, 11),
                    3,
                    "{0}",
                    $"alien{i + 1}",
                    "unknown",
                    ConsoleColor.White,
                    false
                );
                for (int j = 0; j < ennemies.Count; j++)
                {
                    while (true)
                    {
                        if (ennemy.x == ennemies[j].x || ennemy.x == ennemies[j].x + 1 || ennemy.x == ennemies[j].x + 2 || ennemy.x == ennemies[j].x + 3)
                        {
                            ennemy.x = random.Next(0, 120 - 4);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                ennemies.Add(ennemy);
            }

            return ennemies;
        }

        /// <summary>
        /// This method is handling the space invaders game
        /// </summary>
        public void Game()
        {
            List<Ennemy> ennemyToRemove = new List<Ennemy>();

            Ship ship = CreateShip();

            Console.Clear();
            Console.CursorVisible = false;

            ship.bulletX = ship.x + 1;
            ship.bulletY = ship.y - 1;

            ship.isBulletActive = false;

            List<Ennemy> ennemies = CreationEnnemy();
            foreach(var ennemy in ennemies)
            {
                ennemy.bulletX = ennemy.x + 1;
                ennemy.bulletY = ennemy.y + 1;
            }

            long frame = 0;
            bool hasShipMoved = false;
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(intercept: true);

                    if (keyInfo.Key == ConsoleKey.LeftArrow && ship.x > 0)
                    {
                        Console.SetCursorPosition(ship.x+2, ship.y);
                        Console.Write(" ");
                        ship.x--;
                        hasShipMoved = true;
                    }
                    else if (keyInfo.Key == ConsoleKey.RightArrow && ship.x < Console.WindowWidth - 1)
                    {
                        Console.SetCursorPosition(ship.x, ship.y);
                        Console.Write(" ");
                        ship.x++;
                        hasShipMoved=true;
                    }
                    else if (keyInfo.Key == ConsoleKey.Spacebar && !ship.isBulletActive)
                    {
                        ship.isBulletActive = true;
                        ship.bulletX = ship.x;
                        ship.bulletY = ship.y - 1;
                    }

                    if (hasShipMoved)
                    {
                        Console.ForegroundColor = ship.color;
                        Console.SetCursorPosition(ship.x, ship.y);
                        Console.WriteLine(ship.skin);
                        hasShipMoved = false;
                    }
                }

                if (frame % 700 == 0)
                {
                    foreach (var ennemi in ennemies)
                    {
                        ennemi.MoveEnnemy();
                    }
                    Console.ForegroundColor = ship.color;

                    Console.SetCursorPosition(ship.x, ship.y);
                    Console.WriteLine(ship.skin);

                    frame = 0;
                }
                else if(frame % 100 == 0)
                {
                    ship.Shoot();

                    ship.TouchShip(ennemies);

                    Console.ForegroundColor = ship.color;
                    Console.SetCursorPosition(ship.x, ship.y);
                    Console.WriteLine(ship.skin);

                    foreach(var ennemi in ennemies)
                    {
                        ennemi.TouchEnnemy(ship);
                        if(ennemi.life == 0)
                        {
                            ennemyToRemove.Add(ennemi);
                        }
                    }
                    foreach(var ennemis in ennemyToRemove)
                    {
                        Console.SetCursorPosition(ennemis.bulletX, ennemis.bulletY - 1);
                        Console.WriteLine(" ");

                        ennemies.Remove(ennemis);
                    }
                }

                foreach (var ennemy in ennemies)
                {
                    if (ennemy.isBulletActiveE || frame % 5000 == 0)
                    {
                        ennemy.isBulletActiveE = true;
                        ennemy.EnnemyShoot(frame);

                        Console.ForegroundColor = ennemy.color;

                        Console.SetCursorPosition(ennemy.x, ennemy.y);
                        Console.WriteLine(ennemy.skin);

                        Console.SetCursorPosition(ennemy.x - 1, ennemy.y);
                        Console.WriteLine(" ");
                    }
                    else
                    {
                        ennemy.bulletX = ennemy.x + 1;
                        ennemy.bulletY = ennemy.y + 1;
                    }
                }

                frame++;

                if(ennemies.Count == 0)
                {
                    string username = ship.AskUsername();

                    ship.name = username;
                    ship.InsertShipScore();
                }

                Console.SetCursorPosition(10, 0);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Score : {ship.score}");
            }
        }
    }
}