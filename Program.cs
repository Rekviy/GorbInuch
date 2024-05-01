using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GorbInuch
{
    class Snake
    {
        private sbyte Svecx = 1;
        private sbyte Svecy = 0;

        public void SetSvecx(sbyte x)
        {
            Svecx = x;
        }
        public void SetSvecy(sbyte y)
        {
            Svecy = y;
        }
        public sbyte GetSvecx()
        {
            return Svecx;
        }
        public sbyte GetSvecy()
        {
            return Svecy;
        }
    }
    internal class Program
    {
        // направление
        static sbyte vectx, vecty;
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
                        Console.Write(screen[i, j]);
                    }
                    Console.WriteLine();
                }
                vectx = 1;
                vecty = 0;

                gameplay(ref screen);

                Console.WriteLine("Game Over");//Временно
                Console.WriteLine("Начать новую игру? Y/N");
            } while (Console.ReadKey(true).Key == ConsoleKey.Y);
            
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

        static void gameplay(ref char[,] screen)
        {
            int slength = 3;
            Snake[] body = new Snake[slength];
            for (int i = 0; i < body.Length; i++)
                body[i] = new Snake();

            //корды спавна головы змеи
            int headx = slength+1, 
                heady = screen.GetLength(0) / 2;
            //корды хвоста
            int tailx,
                taily;
            //корды яблока
            int applex=0, appley=0;
            
            //спавн яблока
            SpawnApple(screen, headx, heady, slength, ref applex,ref appley);

            //спавн змеи
            screen[heady, headx] = '%';
            for (int i = 1; i <= slength; i++)
                screen[heady, headx-i] = '*';
                        
            while (true)
            {
                Show(screen);
                sbyte local_vectx= vectx, local_vecty= vecty;
                tailx = headx;
                taily = heady;

                heady += local_vecty;
                headx += local_vectx;
                if (heady==applex&&headx==appley)
                {
                    AppleEaten(ref body);
                    SpawnApple(screen, headx, heady, slength, ref applex, ref appley);
                    
                }
                UpdateScreen(ref screen, body, headx, heady, tailx, taily);

                sbyte tempx = body[0].GetSvecx(), tempy = body[0].GetSvecy();
                sbyte temp;
                for (int i = 1; i < body.Length; i++)
                {
                    temp = body[i].GetSvecx();
                    body[i].SetSvecx(tempx);
                    tempx = temp;

                    temp = body[i].GetSvecy();
                    body[i].SetSvecy(tempy);
                    tempy = temp;
                }

                body[0].SetSvecx(local_vectx);
                body[0].SetSvecy(local_vecty);
                
                if (headx == 0||heady==0||headx==screen.GetLength(1)-1||heady==screen.GetLength(0) - 1)
                    break;
                Console.Clear();
            }
            Console.Beep();
        }
        static void UpdateScreen(ref char[,] Screen,in Snake[] body,int headx,int heady, int tailx, int taily)
        {
            Screen[heady, headx] = '%';
            for (int i = 0; i < body.Length; i++)
            {
                Screen[taily, tailx] = '*';
                tailx -= body[i].GetSvecx();
                taily -= body[i].GetSvecy();
            }

            Screen[taily, tailx] = ' ';
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
                        
                        break;

                    default:
                        break;

                }
            }
        }
        static void AppleEaten(ref Snake[] body)
        {
            Console.Beep();
            Array.Resize<Snake>(ref body, body.Length + 1);
            body[body.Length - 1] = new Snake();
            body[body.Length - 1].SetSvecx(body[body.Length - 2].GetSvecx());
            body[body.Length - 1].SetSvecy(body[body.Length - 2].GetSvecy());
        }
        static void SpawnApple(in char[,] screen, int corhx, int corhy, int slength, ref int corax, ref int coray)
        {
            Random rnd = new Random();
            bool Test = true;
            //корды яблока(Спавн)
            corax = rnd.Next(screen.GetLength(0));
            coray = rnd.Next(screen.GetLength(1));

            //проверка спавна яблока
            while (Test)
            {
                //todo исправить баг если яблоко появляется в точке удаления хвоста
                if (corax == 0 || coray == 0 || corax == screen.GetLength(0)-1 || coray == screen.GetLength(1) - 1 || screen[corax,coray]=='%'|| screen[corax, coray] == '*')
                {
                    corax = rnd.Next(screen.GetLength(0));
                    coray = rnd.Next(screen.GetLength(1));
                    continue;
                }
               
                Test = false;
                screen[corax, coray] = '@';
            }
        }

        static void Show(in char[,] screen)
        {
            for (int i = 0; i < screen.GetLength(0); i++)
            {
                for (int j = 0; j < screen.GetLength(1); j++)
                {
                    Console.Write(screen[i, j]);
                }
                Console.WriteLine();
            }
            Thread.Sleep(200);
        }
        
    }
}
