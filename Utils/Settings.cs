using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GorbInuch.Classes;

namespace GorbInuch.Utils
{
    public class Settings
    {
        private static string dir = Directory.GetCurrentDirectory()+@"\data\";
        private static string file = "settings.txt";
     
        public static void SaveSettings()
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            StringBuilder sb = new StringBuilder();
            sb.Append(Snake.Body_Length + ";");
            sb.Append(Snake.Head + ";");
            sb.Append(Snake.Body + ";");
            sb.Append((Game.Height-1) + ";");
            sb.Append(Game.Width + ";");
            File.WriteAllText(dir+file, sb.ToString());
        }

        public static void LoadSettings()
        {
            if (File.Exists(dir + file))
            {
                string text = File.ReadAllText(dir + file);
                string[] settings = text.Split(';');

                Snake.Settings(int.Parse(settings[0]), char.Parse(settings[1]), char.Parse(settings[2]));
                Game.Height = short.Parse(settings[3]);
                Game.Width = short.Parse(settings[4]);
            }
            else
            {
                Snake.Settings();
                Game.Height = 10;
                Game.Width = 20;
            }
        }
    }
}
