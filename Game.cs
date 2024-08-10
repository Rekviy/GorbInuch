using GorbInuch.Utils;
using GorbInuch.Classes;
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

    public class Game
    {
        // направление
        static sbyte vectx, vecty;
        static int score = 0;
        static int delay = 100;

        private static short height;
        public static short Height
        { 
            get {  return (short) (height - 1); }
            set { height = (short)(value + 1); }
        }
        public static short Width { get; set; } = 20;
        public static void Delay(int difficulty)
        {
            switch(difficulty)
            {
                case 1:
                    delay = 150;
                    break;
                case 2:
                    delay = 120;
                    break;
                case 3:
                    delay = 80;
                    break;

                default:
                    delay = 120;
                    break;
            }
        }
        public static void Start()
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
                Gameplay();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nGame Over\n");//Временно
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Яблок съедено: "+score);
                Console.ResetColor();
                ThreadInput.IsBackground = true;
                Console.WriteLine("\nНачать новую игру? Y/N");

            } while (Console.ReadKey(true).Key == ConsoleKey.Y);
            
        }

        static void Initialization(out Snake[] body)
        {
            Console.CursorTop = 1;
            for (int i = 1; i < height; i++)
            {
                if (i == 1 || i == height-1)
                    Console.WriteLine(new string('-', Width));
                else
                {
                    Console.WriteLine("|" + new string(' ', Width - 2) + "|");
                    continue;
                }
            }

            Snake.Start_Position((short)(Snake.Body_Length + 1),(short)(height / 2));
            
            body = new Snake[Snake.Body_Length+5];
            for (int i = 0; i < body.Length; i++)
                body[i] = new Snake(1,0);
            
            //спавн змейки
            for (int i = 0;i < Snake.Body_Length+1; i++)
            {
                Console.SetCursorPosition(Snake.HeadX-i, Snake.HeadY);
                if (i==0)
                    Console.Write(Snake.Head);
                else
                    Console.Write(Snake.Body);
            }
            
            //спавн яблока
            Apple.SpawnApple(ref body);

            vectx = 1;
            vecty = 0;
        }
        static void Gameplay()
        {
            Initialization(out Snake[] body);
#if DEBUG
            Stopwatch sw = new Stopwatch();
            
#endif
            while (true)
            {
                sw.Start();
                Console.SetCursorPosition(0, 0);
                Console.Write("Текущий счет: " + score);
                
                sbyte local_vectx = vectx, local_vecty = vecty;

                Snake.TailX = Snake.HeadX;
                Snake.TailY = Snake.HeadY;

                Snake.HeadY += local_vecty;
                Snake.HeadX += local_vectx;

                
                if (Snake.HeadX == 0 || Snake.HeadX == Width-1 || Snake.HeadY == height-1 || Snake.HeadY == 1 ||
                    ConsoleOut.CompareConsoleChar('*',Snake.HeadX,Snake.HeadY))
                    break;
                
                UpdateScreen(body);


                if (Snake.HeadY == Apple.AppleY && Snake.HeadX == Apple.AppleX)
                {
                    Console.Beep(1800, 80);
                    score++;
                    if (Snake.Body_Length == body.Length)
                        Snake.ExtendSnake(ref body);

                    Snake.Body_Length++;
                    Apple.SpawnApple(ref body);
                }

                
                for (int i = Snake.Body_Length - 1; i > 0; i--)
                {
                    body[i].VectX = body[i - 1].VectX;
                    body[i].VectY = body[i - 1].VectY;
                }
                body[0].VectX = local_vectx;
                body[0].VectY = local_vecty;

#if DEBUG
                sw.Stop();
                Console.SetCursorPosition(0, height + 2);
                Console.WriteLine(sw.ElapsedMilliseconds);
                sw.Reset();
#endif
                Thread.Sleep(delay);
            }
            Console.Beep();
        }
        static void UpdateScreen(in Snake[] body)
        {
            Console.SetCursorPosition(Snake.HeadX,Snake.HeadY);
            Console.Write(Snake.Head);
            for (int i = 0; i < Snake.Body_Length; i++)
            {
                Console.SetCursorPosition(Snake.TailX, Snake.TailY);
                Console.Write(Snake.Body);
                Snake.TailX -= body[i].VectX;
                Snake.TailY -= body[i].VectY;
            }
            Console.SetCursorPosition(Snake.TailX, Snake.TailY);
            Console.Write(' ');
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
                Thread.Sleep(delay+10);
            }
        }
    }
}
