﻿using System;
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
                body[i] = new Snake();

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
                        
            while (true)
            {
                Show(screen);
                sbyte local_vectx = vectx, local_vecty = vecty;
                tailx = headx;
                taily = heady;

                heady += local_vecty;
                headx += local_vectx;

                if (screen[heady, headx] == '|' || screen[heady, headx] == '-' || screen[heady, headx] == '*')
                    break;

                UpdateScreen(ref screen, body, headx, heady, tailx, taily);

                if (heady==appley&&headx==applex)
                {
                    AppleEaten(ref body);
                    SpawnApple(ref screen,tailx,taily, out applex, out appley);
                    
                }
                
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
        
        static void AppleEaten(ref Snake[] body)
        {
            Console.Beep();
            Array.Resize<Snake>(ref body, body.Length + 1);
            body[body.Length - 1] = new Snake();
            body[body.Length - 1].SetSvecx(body[body.Length - 2].GetSvecx());
            body[body.Length - 1].SetSvecy(body[body.Length - 2].GetSvecy());
        }
        static void SpawnApple(ref char[,] screen, int tailx, int taily, out int applex, out int appley)
        {
            Random rnd = new Random();
            bool Test = true;
            //корды яблока(Спавн)
            applex = rnd.Next(1,screen.GetLength(1)-2);
            appley = rnd.Next(1,screen.GetLength(0)-2);

            //проверка спавна яблока
            while (Test)
            {
                if (screen[appley,applex]=='%'|| screen[appley, applex] == '*'||appley ==taily|| applex==tailx)
                {
                    applex = rnd.Next(1, screen.GetLength(1) - 2);
                    appley = rnd.Next(1, screen.GetLength(0) - 2);
                    continue;
                }
               
                Test = false;
                screen[appley, applex] = '@';
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
    }
}
