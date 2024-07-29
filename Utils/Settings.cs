using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorbInuch
{
    public class Settings
    {
        public static int Delay { get; private set; } = 100;
        public static short height { get; private set; } = 10 + 1;//1 for score
        public static short width { get; private set; } = 20;

        public static void SaveSettings()
        {

        }

        public static void LoadSettings()
        {

        }
    }
}
