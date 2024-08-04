using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GorbInuch.Utils;

namespace GorbInuch
{
    internal class Entrance
    {
        static void Main()
        {
            Settings.LoadSettings();
            #region Welcome Screen
            Console.WriteLine(
                ""
                );
            #endregion
            Game.Start();
            Settings.SaveSettings();
        }
    }
}
