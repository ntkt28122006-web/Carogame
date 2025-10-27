using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace caro_game
{
    public class player
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private Image mark;

        public Image Mark
        {
            set { mark = value; }
            get { return mark; }
        }


        public player (string name, Image mark)
        {
            this.Name = name;
            this.Mark = mark;
        }




    }
}
