using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorbInuch.Classes
{
    public struct Snake
    {
        public static short HeadX { get; set; }
        public static short HeadY { get; set; }
        public static short TailX {  get; set; }
        public static short TailY { get; set; }
        public static int Body_Length { get; set; }
        public sbyte VectX { get; set; }
        public sbyte VectY { get; set; }

        public static char Head { get;private set; }
        public static char Body { get; private set; }

        public Snake(sbyte vectx, sbyte vecty)
        {
            this.VectX = vectx;
            this.VectY = vecty;
        }
        public static void Start_Position(short HeadX, short HeadY)
        {
            Snake.HeadX = HeadX;
            Snake.HeadY = HeadY;
        }
        public static void Settings(int BodyLength = 3,char Head = '%',char Body = '*')
        {
            Snake.Head = Head;
            Snake.Body = Body;
            Snake.Body_Length = BodyLength;
        }
        public static void ExtendSnake(ref Snake[] body)
        {
            Array.Resize<Snake>(ref body, body.Length + 10);
            body[Snake.Body_Length - 1].VectX = body[Snake.Body_Length].VectX;
            body[Snake.Body_Length - 1].VectY = body[Snake.Body_Length].VectY;
        }

    }
}
