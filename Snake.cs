using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorbInuch
{
    struct Snake
    {
        private sbyte Svecx;
        public sbyte Vectx
        {
            get { return Svecx; }
            set { Svecx = value; }
        }

        private sbyte Svecy;
        public sbyte Vecty
        {
            get { return Svecy; }
            set { Svecy = value; }
        }
    }
}
