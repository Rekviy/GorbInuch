using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorbInuch
{
    struct Snake
    {
        public static char head { get; set; }
        public static char body { get; set; }
        public static int blength { get; set; }
        public static short headx { get; set; }
        public static short heady { get; set; }
        public sbyte Vectx { get; set; }
        public sbyte Vecty { get; set; }

        public Snake(sbyte vectx, sbyte vecty)
        {
            this.Vectx = vectx;
            this.Vecty = vecty;
        }
        public static void Settings(short headx, short heady,int blength = 3,char head = '%',char body = '*')
        {
            Snake.head = head;
            Snake.body = body;
            Snake.headx = headx;
            Snake.heady = heady;
            Snake.blength = blength;
        }
    }
}
