using GorbInuch.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorbInuch.Classes
{
    public struct Apple
    {
        public static char Apple_Char { get; private set; } = '@';
        public static short AppleX { get; private set; }
        public static short AppleY { get; private set; }
        public static void SpawnApple(ref Snake[] body)
        {
            Random rnd = new Random();
            //корды яблока(Спавн)
            AppleX = (short)rnd.Next(1, Game.Width - 1);
            AppleY = (short)rnd.Next(1, Game.Height - 1);

            //проверка спавна яблока
            while (ConsoleOut.CheckConsoleChar(AppleX, AppleY) || AppleY == Snake.TailY + body[Snake.Body_Length - 1].VectY || AppleX == Snake.TailX + body[Snake.Body_Length - 1].VectX)
            {
                AppleX = (short)rnd.Next(1, Game.Width - 1);
                AppleY = (short)rnd.Next(1, Game.Height - 1);
            }
            Console.SetCursorPosition(AppleX, AppleY);
            Console.Write(Apple_Char);

        }
    }
}
