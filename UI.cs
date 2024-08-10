using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorbInuch
{
    public class UI
    {
        static public void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose option: ");
                Console.Write("(S)tart game\n" +
                              "(O)ptions\n" +
                              "(Q)uit\n");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.S:
                        GameMenu();
                        break;
                    case ConsoleKey.O:
                        SettingsMenu();
                        break;
                    case ConsoleKey.Q:
                        return;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            }
        }
        private static void GameMenu()
        {
            Console.WriteLine("Choose difficulty: ");
            Console.Write("(E)asy, (M)edium, (H)ard");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.E:
                    Game.Delay(1);
                    break;
                case ConsoleKey.M:
                    Game.Delay(2);
                    break;
                case ConsoleKey.H:
                    Game.Delay(3);
                    break;
            }
            Game.Start();
        }
        private static void SettingsMenu()
        {
            Console.Clear();
            Console.WriteLine("Current resolution: " + Game.Width+" x "+Game.Height);
            Console.WriteLine("Do you want to change it?\n Y/N");
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                try
                {
                    Console.Write("\nInput width: ");
                    Game.Width = short.Parse(Console.ReadLine());
                    Console.Write("\nInput height: ");
                    Game.Height = short.Parse(Console.ReadLine());
                    Console.WriteLine();
                    GorbInuch.Utils.Settings.ChangeWindowSize();
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid Input!\n");
                    Console.ResetColor();
                    GorbInuch.Utils.Settings.LoadSettings();
                }
                
            }
            return;
        }
    }
}
