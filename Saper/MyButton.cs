using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saper
{
    public class MyButton : Button
    {
        public MyButton()
        {
            this.TabStop = false;

        }
        public bool HasMine { get; set; } = false;

        public bool Flagged { get; set; } = false;

        public bool Clicked { get; set; } = false;

        public int Number { get; set; } = 0;
        
        public int x { get; set; }
        public int y { get; set; }

        public void Hit()
        {
            this.Clicked = true;
            this.Enabled = false;
            if (this.Flagged==true)
            {
                this.Flagged = false;
                this.Image = null;
            }
        }

    }
}
