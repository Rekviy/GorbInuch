using System.Runtime.InteropServices;
using System;

namespace GorbInuch.Utils
{
    class ConsoleOut
    {
        public static bool CompareConsoleChar(char ch, short x, short y)
        {
            char[] readBuff = new char[1];

            ReadConsoleOutputCharacter(GetStdHandle(-11), readBuff, 1, new COORD { X = x, Y = y }, out int ReadCount);
            if (ch == readBuff[0])
                return true;
            return false;
        }
        public static bool CheckConsoleChar(short x, short y)
        {
            char[] readBuff = new char[1];

            ReadConsoleOutputCharacter(GetStdHandle(-11), readBuff, 1, new COORD { X = x, Y = y }, out int ReadCount);
            if (readBuff[0] == ' ')
                return false;
            return true;
        }


        [StructLayout(LayoutKind.Sequential)]
        struct COORD
        {
            public short X;
            public short Y;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadConsoleOutputCharacter(
        IntPtr hConsoleOutput,
        [Out] char[] lpCharacter,
        int nLength,
        COORD dwReadCoord,
        out int lpNumberOfCharsRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);
    }
}