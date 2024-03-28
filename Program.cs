using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GorbInuch
{
    internal class Program
    {
        // направление
        static sbyte vectx = 1,
                     vecty = 0;
        static void Main(string[] args)
        {
            
            const int ScreenW = 20, ScreenH = 10;
            char[,] screen = new char[ScreenH, ScreenW];

            Thread ThreadInput = new Thread(movement);

            //удаление курсора
            Console.CursorVisible = false;
           
            //инициализация экрана
            for (int i = 0; i < ScreenH; i++)
            {
                for (int j = 0; j < ScreenW; j++)
                {
                    if (i == 0 || i == ScreenH - 1)
                    {
                        screen[i, j] = '-';
                        continue;
                    }
                    if (j == 0 || j == ScreenW - 1)
                    {
                        screen[i, j] = '|';
                        continue;
                    }
                    screen[i, j] = ' ';
                }
            }

            ThreadInput.Start();

            gameplay(screen);

            Thread.Sleep(1000);
        }

        //Выводит экран с надписью змейка символами
        static void WelcomeScreen()
        {
            char[,] WellScreen = new char[Console.WindowHeight, Console.WindowWidth];

            for (int i = 0; i < WellScreen.GetLength(0); i++)
            {
                for (int j = 0; j < WellScreen.GetLength(1); j++)
                {





                    WellScreen[i, j] = ' '; 
                }
                Thread.Sleep(300);
            }
            
        }

        static void gameplay(char[,] screen)
        {
            int slength = 3;
            sbyte[,] snake = new sbyte[slength,2];
            for (int i = 0; i < snake.GetLength(0); i++)
            {
                snake[i, 0] = 1;
                snake[i, 1] = 0;
            }

            //кодры хвоста
            int cordendx, cordendy;


            //корды спавна головы змеи
            int corhx = 4, 
                corhy = screen.GetLength(0) / 2;

            //корды яблока
            int corax, coray;

            //спавн яблока
            SpawnApple(screen, corhx, corhy,out corax,out coray);

            //спавн змеи
            screen[corhy, corhx] = '%';
            for (int i = 1; i <= 3; i++)
                screen[corhy, corhx-i] = '*';
            
            bool IsDead = false;
            while (!IsDead)
            {
                Console.Clear();
                test(screen);
                cordendx = corhx;
                cordendy = corhy;

                corhy += vecty;
                corhx += vectx;
                

                
                screen[corhy, corhx] = '%';
                for (int i = 0; i < snake.GetLength(0); i++)
                {
                    screen[cordendy,cordendx] = '*';
                    cordendx -= snake[i, 0];
                    cordendy -= snake[i, 1];
                }

                screen[cordendy, cordendx] = ' ';
                sbyte tempx = snake[0, 0], tempy = snake[0, 1];
                for (int i = 1; i < snake.GetLength(0); i++)
                {
                    snake[0, 0] = snake[i, 0];
                    snake[0, 1] = snake[i, 1];

                    snake[i, 0] = tempx;
                    snake[i, 1] = tempy;

                    tempx = snake[0, 0];
                    tempy = snake[0, 1];
                }

                snake[0, 0] = vectx;
                snake[0, 1] = vecty;
                /*
                screen[corhy- vecty, corhx- vectx] = '*';
                screen[cordendy, cordendx] = ' ';

                if (screen[cordendy, cordendx + 1] =='*')
                    cordendx++;

                else if(screen[cordendy, cordendx - 1] == '*')
                    cordendx--;

                else if (screen[cordendy + 1, cordendx] == '*')
                    cordendy++;

                else
                    cordendy--;

                screen[cordendy, cordendx] = '*';
                */
                /*
                for (int temp = snake_length; temp!=0;)
                {
                    int i = vectx, j = vecty;
                    screen[corhy-j, corhx-i] = '*';
                    i += vectx; j += vecty;
                    temp--;
                }
                */

                if (corhx == 0||corhy==0||corhx==screen.GetLength(1)-1||corhy==screen.GetLength(0) - 1)
                    IsDead = true;
            }
        }
        
        static void movement()
        {
            ConsoleKey key;
            while (true)
            {
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        vectx = 0;
                        vecty = -1;
                        break;

                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        vectx = 0;
                        vecty = 1;
                        break;

                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        vectx = 1;
                        vecty = 0;
                        break;

                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        vectx = -1;
                        vecty = 0;
                        break;

                    case ConsoleKey.Escape:

                        break;

                    default:
                        break;

                }
            }
        }

        static void SpawnApple(char[,] screen, int corhx, int corhy, out int corax, out int coray)
        {
            Random rnd = new Random();
            bool Test = true;
            //корды яблока(Спавн)
            corax = rnd.Next(screen.GetLength(0));
            coray = rnd.Next(screen.GetLength(1));

            //проверка спавна яблока
            while (Test)
            {
                if (corax == 0 || coray == 0 || corax == screen.GetLength(1) || coray == screen.GetLength(0))
                {
                    corax = rnd.Next(screen.GetLength(0));
                    coray = rnd.Next(screen.GetLength(1));
                    continue;
                }
               
                if (false)
                {

                    continue;
                }
                Test = false;
                screen[corax, coray] = '@';
            }

        }

        static void test(char[,] screen)
        {
            for (int i = 0; i < screen.GetLength(0); i++)
            {
                for (int j = 0; j < screen.GetLength(1); j++)
                {
                    Console.Write(screen[i, j]);
                }
                Console.WriteLine();
            }
            Thread.Sleep(400);
        }
        
    }
}
