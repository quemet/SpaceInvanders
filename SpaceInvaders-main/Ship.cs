using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    public class Ship
    {
        private int X;
        private int BulletX;
        private int Y;
        private int BulletY;
        private int Life;
        private int Score;
        private string Skin;
        private string Name;
        private ConsoleColor Color;
        private bool IsBulletActive;

        public int x { get => X; set => X = value; }
        public int bulletX { get => BulletX; set => BulletX = value; }
        public int y { get => Y; set => Y = value; }
        public int bulletY { get => BulletY; set => BulletY = value; }
        public int life { get => Life; set => Life = value; }
        public int score { get => Score; set => Score = value; }
        public string skin { get => Skin; set => Skin = value; }
        public string name { get => Name; set => Name = value; }
        public ConsoleColor color { get => Color; set => Color = value; }
        public bool isBulletActive { get => IsBulletActive; set => IsBulletActive = value; }

        /// <summary>
        /// Only constructor for the class Ship
        /// </summary>
        /// <param name="x">The argument x is the value x of the ship</param>
        /// <param name="y">The argument y is the value y of the ship</param>
        /// <param name="life">The argument life is the count of life left on the ship</param>
        /// <param name="skin">The argument skin is the skin of the ship</param>
        /// <param name="name">The argument name is the name of the ship</param>
        /// <param name="score">The argument score is the score of the ship</param>
        /// <param name="color">The argument color is the color of the ship</param>
        /// <param name="isBulletActive">The argument isBulletActive is the boolean to know if the ship is shooting a bullet</param>
        public Ship(int x, int y, int life, string skin, string name, int score, ConsoleColor color, bool isBulletActive)
        {
            this.x = x;
            this.y = y;
            this.life = life;
            this.skin = skin;
            this.name = name;
            this.score = score;
            this.color = color;
            this.isBulletActive = isBulletActive;
        }

        /// <summary>
        /// This method handle the shoot from the ship
        /// </summary>
        public void Shoot()
        {
            Console.ForegroundColor = color;

            if (isBulletActive)
            {
                if (bulletY >= 0)
                {
                    Console.SetCursorPosition(bulletX + 1, bulletY);
                    Console.Write("|");
                    Console.SetCursorPosition(bulletX + 1, bulletY + 1);
                    Console.Write(" ");
                }

                bulletY = bulletY - 1;
            }

            if (bulletY < 0)
            {
                isBulletActive = false;
                Console.SetCursorPosition(bulletX + 1, 0);
                Console.WriteLine(" ");
            }
        }

        /// <summary>
        /// This method watch if the ennemies bullet touch the ship if it's true the ship is losing a life or it's end the game
        /// </summary>
        /// <param name="ennemies">The argument ennemies is a list of all the ennemies</param>
        public void TouchShip(List<Ennemy> ennemies)
        {
            foreach (Ennemy ennemy in ennemies)
            {
                if ((ennemy.bulletX == x || ennemy.bulletX == x + 1 || ennemy.bulletX == x + 2) && (ennemy.bulletY == y + 1))
                {
                    life--;
                    switch (life)
                    {
                        case 2:
                            color = ConsoleColor.Blue;
                            score -= 200;
                            break;
                        case 1:
                            color = ConsoleColor.Red;
                            score -= 400;
                            break;
                        case 0:
                            GameOver();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// This function is asking to the user if he wants to replay if yes he call the function main
        /// </summary>
        public void GameOver()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("You lost !!!");

            Console.WriteLine("Do you want to replay ?");
            
            char userInput = Console.ReadKey().KeyChar;

            if(userInput.ToString().ToUpper() == "Y")
            {
                Program.Main();
            }
            Environment.Exit(0);
        }

        /// <summary>
        /// We ask the username of the player to store in the db
        /// </summary>
        public string AskUsername()
        {
            Console.Clear();
            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("You win !!!");

            Console.Write("Enter your username : ");
            string username = Console.ReadLine();

            if(username == "")
            {
                AskUsername();
            }

            return username;
        }

        /// <summary>
        /// This method insert the score of the player if he wins 
        /// </summary>
        public void InsertShipScore()
        {
            Console.Clear();
            string connectionString = "Server=localhost;Port=6033;Database=db_space_invaders;User=root;Password=root";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO t_joueur (jouPseudo, jouNombrePoints) VALUES (@ship.name, @ship.score)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@jouPseudo", name);
                cmd.Parameters.AddWithValue("@jouNombrePoints", score);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Score élevé enregistré avec succès !");
                }
                else
                {
                    Console.WriteLine("Échec de l'enregistrement du score élevé.");
                }
            }
        }
    }
}
