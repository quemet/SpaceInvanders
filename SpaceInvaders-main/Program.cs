using System;
using System.Threading;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace SpaceInvaders
{
    public class Program
    {
        /// <summary>
        /// This function select the high score from the db and show it to the user
        /// </summary>
        /// <param name="connection">The argument connection is the mysql connection from the function ConnectToTheDB</param>
        public static void SelectHighScore(MySqlConnection connection)
        {
            string query = "SELECT * FROM t_joueur ORDER BY jouNombrePoints DESC LIMIT 5;";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                Console.WriteLine("Voici les high scores :");
                while (reader.Read())
                {
                    int idJoueur = reader.GetInt32("idJoueur");
                    string Pseudo = reader.GetString("jouPseudo");
                    string NombrePoints = reader.GetString("jouNombrePoints");
                    Console.WriteLine(idJoueur + "  " + Pseudo + "  " + NombrePoints);
                }
            }
        }

        /// <summary>
        /// This function is using to make a connection to the db_space_invaders
        /// </summary>
        public static void ConnectionToTheDB()
        {
            Console.Clear();
            string connectionString = "Server=localhost;Port=6033;Database=db_space_invaders;User=root;Password=root";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SelectHighScore(connection);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Erreur lors de la connexion à la base de données : " + ex);
                }
            }
            Console.ReadLine();
        }

        /// <summary>
        /// This function is to show the menu the user press 1, 2 or 3 and he calls the method or function corresponding at the action and handle error
        /// </summary>
        public static void Main()
        {
            int selectedMenuItem = 0;
            Games games = new Games();

            while (true)
            {
                Console.Clear();

                string[] menuItems = {
                    "1 - Jouer",
                    "2 - High Score",
                    "3 - Quitter"
                };

                int windowHeight = Console.WindowHeight;
                int menuHeight = menuItems.Length;
                int verticalOffset = (windowHeight - menuHeight) / 2;

                for (int i = 0; i < menuItems.Length; i++)
                {
                    int horizontalOffset = (Console.WindowWidth - menuItems[i].Length) / 2;
                    Console.SetCursorPosition(horizontalOffset, verticalOffset + i);

                    if (i == selectedMenuItem)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                    Console.WriteLine(menuItems[i]);

                    Console.ResetColor();
                }

                try
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (selectedMenuItem > 0)
                            {
                                selectedMenuItem--;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (selectedMenuItem < menuItems.Length - 1)
                            {
                                selectedMenuItem++;
                            }
                            break;
                        case ConsoleKey.Enter:
                            char userInput = (char)(selectedMenuItem + '1');
                            switch (userInput)
                            {
                                case '1':
                                    Console.WriteLine("Lancement du jeu...");
                                    Thread.Sleep(2000);
                                    games.Game();
                                    break;
                                case '2':
                                    Console.WriteLine("Affichage du High Score...");
                                    Thread.Sleep(2000);
                                    ConnectionToTheDB();
                                    break;
                                case '3':
                                    Environment.Exit(0);
                                    break;
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine("Erreur de saisie. Veuillez réessayer.");
                    Console.ReadLine();
                    continue;
                }
            }
        }
    }
}