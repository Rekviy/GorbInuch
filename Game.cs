using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GorbInuch
{

    internal class Game
    {
        // направление
        static sbyte vectx, vecty;
        static int score = 0;
        static int Delay = 100;
        public static int height { get; set; } = 10;
        public static int width { get; set; } = 20;
        static void Main(string[] args)
        {
            Thread ThreadInput = new Thread(movement);
            ThreadInput.Start();

            
            
            //удаление курсора
            Console.CursorVisible = false;
            do
            {
                Console.Clear();
                score = 0;
                ThreadInput.IsBackground = false;
                gameplay();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nGame Over\n");//Временно
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Яблок съедено: "+score);
                Console.ResetColor();
                ThreadInput.IsBackground = true;
                Console.WriteLine("\nНачать новую игру? Y/N");

            } while (Console.ReadKey(true).Key == ConsoleKey.Y);
            
        }

        //Будет выводить экран с надписью змейка символами
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
        
        static void Initialization(out Snake[] body,out int applex,out int appley, out int tailx,out int taily)
        {
            Console.CursorTop = 1;
            for (int i = 1; i <= height; i++)
            {
                if (i == 1 || i == height)
                    Console.WriteLine(new string('-', width));
                else
                {
                    Console.WriteLine("|" + new string(' ', width - 2) + "|");
                    continue;
                }
            }
            //временно
            Snake.blength = 3;
            Snake.Settings(Snake.blength + 1, height / 2);
            
            body = new Snake[Snake.blength];
            for (int i = 0; i < body.Length; i++)
                body[i] = new Snake(1,0);
            
            //корды хвоста
            tailx = 0;
            taily = 0;

            //спавн яблока
            SpawnApple(tailx,taily,out applex, out appley);

            //спавн змейки
            for (int i = 0;i < Snake.blength+1; i++)
            {
                Console.SetCursorPosition(Snake.headx-i, Snake.heady);
                if (i==0)
                    Console.Write(Snake.head);
                else
                    Console.Write(Snake.body);
            }

            
            vectx = 1;
            vecty = 0;
        }
        static void gameplay()
        {
            Initialization(out Snake[] body, out int applex, out int appley, out int tailx, out int taily);
#if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif
            while (true)
            {
                Show(score);
                sbyte local_vectx = vectx, local_vecty = vecty;

                tailx = Snake.headx;
                taily = Snake.heady;

                Snake.heady += local_vecty;
                Snake.headx += local_vectx;

                
                if (Snake.headx == 0 || Snake.headx == width || Snake.heady == height || Snake.heady == 0 ||
                    ConsoleOut.CompareConsoleChar('*',(short)(Snake.headx),(short)(Snake.heady)))
                    break;

                UpdateScreen(body, tailx, taily);

                

                
                if (Snake.heady == appley && Snake.headx == applex)
                {
                    Console.Beep();
                    score++;
                    if (Snake.blength == body.Length)
                        ExtendSnake(ref body);
                    Snake.blength++;
                    SpawnApple(tailx, taily, out applex, out appley);
                }


                for (int i = Snake.blength - 1; i > 0; i--)
                {
                    body[i].Vectx = body[i - 1].Vectx;
                    body[i].Vecty = body[i - 1].Vecty;
                }
                body[0].Vectx = local_vectx;
                body[0].Vecty = local_vecty;

#if DEBUG
                sw.Stop();
                Console.SetCursorPosition(0, height + 2);
                Console.WriteLine(sw.ElapsedMilliseconds);
                sw.Restart();
#endif
            }
            Console.Beep();
        }
        static void UpdateScreen(in Snake[] body, int tailx, int taily)
        {
            Console.SetCursorPosition(Snake.headx,Snake.heady);
            Console.Write(Snake.head);
            for (int i = 0; i < Snake.blength; i++)
            {
                Console.SetCursorPosition(tailx, taily);
                Console.Write(Snake.body);
                tailx -= body[i].Vectx;
                taily -= body[i].Vecty;
            }
            Console.SetCursorPosition(tailx , taily);
            Console.Write(' ');
        }
        
        static void ExtendSnake(ref Snake[] body)
        {
            Array.Resize<Snake>(ref body, body.Length + 3);
            body[Snake.blength-1].Vectx = body[Snake.blength].Vectx;
            body[Snake.blength-1].Vecty = body[Snake.blength].Vecty;
        }
        static void SpawnApple(int tailx, int taily, out int applex, out int appley)
        {
            Random rnd = new Random();
            //корды яблока(Спавн)
            applex = rnd.Next(1,width-1);
            appley = rnd.Next(1,height-1);

            //проверка спавна яблока
            while (ConsoleOut.CheckConsoleChar( (short)(applex), (short)(appley)) || appley == taily || applex == tailx)

            {
                applex = rnd.Next(1, width - 1);
                appley = rnd.Next(1, height - 1);
            }
            Console.SetCursorPosition(applex, appley); 
            Console.Write('@');
            
        }
        
        static void Show(in int score)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write("Текущий счет: "+score);
            Thread.Sleep(Delay);
        }
        static void movement()
        {
            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        if (vecty == 1)
                            break;
                        vectx = 0;
                        vecty = -1;
                        break;

                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        if (vecty == -1)
                            break;
                        vectx = 0;
                        vecty = 1;
                        break;

                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        if (vectx == -1)
                            break;
                        vectx = 1;
                        vecty = 0;
                        break;

                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        if (vectx == 1)
                            break;
                        vectx = -1;
                        vecty = 0;
                        break;
                    case ConsoleKey.Escape:
                        return;
                    default:
                        break;

                }
                Thread.Sleep(Delay+30);
            }
        }
    }
}
