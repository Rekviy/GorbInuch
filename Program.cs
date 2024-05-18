using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GorbInuch
{
   
    internal class Program
    {
        // направление
        static sbyte vectx, vecty;
        static int score = 0;
        static int GameSpeed = 100;
        static void Main(string[] args)
        {
            const int ScreenW = 20, ScreenH = 10;
            char[,] screen = new char[ScreenH, ScreenW];
            
            Thread ThreadInput = new Thread(movement);
            ThreadInput.Start();

            //удаление курсора
            Console.CursorVisible = false;
            do
            {
                score = 0;
                ThreadInput.IsBackground = false;
                gameplay(ref screen);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nGame Over\n");//Временно
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Яблок съедено: "+score);
                Console.ResetColor();
                Console.WriteLine("\nНачать новую игру? Y/N");

                ThreadInput.IsBackground = true;
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
        static void Initialization(ref char[,] screen,out Snake[] body,out int headx,out int heady,out int applex,out int appley, out int tailx,out int taily)
        {
            //инициализация экрана
            for (int i = 0; i < screen.GetLength(0); i++)
            {
                for (int j = 0; j < screen.GetLength(1); j++)
                {
                    if (i == 0 || i == screen.GetLength(0) - 1)
                    {
                        screen[i, j] = '-';
                        continue;
                    }
                    if (j == 0 || j == screen.GetLength(1) - 1)
                    {
                        screen[i, j] = '|';
                        continue;
                    }
                    screen[i, j] = ' ';
                    Console.Write(screen[i, j]);
                }
                Console.WriteLine();
            }

            int slength = 3;
            body = new Snake[slength];
            for (int i = 0; i < body.Length; i++)
                body[i] = new Snake() {Vectx = 1};
                
                

            //корды спавна головы змеи
            headx = slength + 1;
            heady = screen.GetLength(0) / 2;
            //корды хвоста
            tailx = 0;
            taily = 0;

            //спавн яблока
            SpawnApple(ref screen, tailx,taily,out applex, out appley);

            //спавн змейки
            screen[heady, headx] = '%';
            for (int i = 1; i <= slength; i++)
                screen[heady, headx - i] = '*';

            vectx = 1;
            vecty = 0;
        }
        static void gameplay(ref char[,] screen)
        {
            Initialization(ref screen, out Snake[] body, out int headx, out int heady, out int applex, out int appley, out int tailx, out int taily);
#if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif
            while (true)
            {
                Show(screen,score);
                sbyte local_vectx = vectx, local_vecty = vecty;
                tailx = headx;
                taily = heady;

                heady += local_vecty;
                headx += local_vectx;

                if (screen[heady, headx] == '|' || screen[heady, headx] == '-' || screen[heady, headx] == '*')
                    break;

                UpdateScreen(ref screen, body, headx, heady, tailx, taily);

                
                for (int i = body.Length-1; i > 0; i--)
                {
                    body[i].Vectx = body[i - 1].Vectx;
                    body[i].Vecty = body[i - 1].Vecty;
                }
                body[0].Vectx = local_vectx;
                body[0].Vecty = local_vecty;

                if (heady == appley && headx == applex){
                    score++;
                    AppleEaten(ref body);
                    SpawnApple(ref screen, tailx, taily, out applex, out appley);}

                Console.Clear();
#if DEBUG
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);
                sw.Restart();
#endif
            }
            Console.Beep();
        }
        static void UpdateScreen(ref char[,] Screen,in Snake[] body,int headx,int heady, int tailx, int taily)
        {
            Screen[heady, headx] = '%';
            for (int i = 0; i < body.Length; i++)
            {
                Screen[taily, tailx] = '*';
                tailx -= body[i].Vectx;
                taily -= body[i].Vecty;
            }

            Screen[taily, tailx] = ' ';
        }
        
        static void AppleEaten(ref Snake[] body)
        {
            Console.Beep();
            Array.Resize<Snake>(ref body, body.Length + 1);
            body[body.Length - 1].Vectx = body[body.Length - 2].Vectx;
            body[body.Length - 1].Vecty = body[body.Length - 2].Vecty;
        }
        static void SpawnApple(ref char[,] screen, int tailx, int taily, out int applex, out int appley)
        {
            Random rnd = new Random();
            //корды яблока(Спавн)
            applex = rnd.Next(1,screen.GetLength(1)-2);
            appley = rnd.Next(1,screen.GetLength(0)-2);

            //проверка спавна яблока
            while (screen[appley, applex] == '%' || screen[appley, applex] == '*' || appley == taily || applex == tailx)
            {
                applex = rnd.Next(1, screen.GetLength(1) - 2);
                appley = rnd.Next(1, screen.GetLength(0) - 2);
            }
            screen[appley, applex] = '@';
        }

        static void Show(in char[,] screen,in int score)
        {
            for (int i = 0; i < screen.GetLength(0); i++)
            {
                for (int j = 0; j < screen.GetLength(1); j++)
                {
                    Console.Write(screen[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("Текущий счет: "+score);
            Thread.Sleep(GameSpeed);
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
                Thread.Sleep(GameSpeed+20);
            }
        }
    }
}
