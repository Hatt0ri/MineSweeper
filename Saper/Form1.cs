using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saper
{
    public partial class Form1 : Form
    {
        //  ========================== Wartości

        int b_wymiar = 25;  //  25x25
        bool end = false;
        static int size = 28;
        static int bomby = size*2;
        double seconds = 0; //  licznik
        int picked = bomby; //  ilosc bomb wskazowka

        //Image flaga = Image.FromFile(@"C:\Users\Karol\Desktop\saper\flag.png"); //  @ bierze pod uwage 
        Bitmap flaga = new Bitmap(@"saper\flag.png");
        Bitmap mine = new Bitmap(@"saper\mine.jpg");
        Bitmap rmine = new Bitmap("saper\\rmine.jpg");
        Bitmap ssmile = new Bitmap(@"saper\smile.jpg");
        Bitmap osmile = new Bitmap(@"saper\osmile.jpg");
        Bitmap vsmile = new Bitmap(@"saper\vsmile.jpg");
        Image dsmile = new Bitmap(@"saper\dsmile.jpg");
        Bitmap l1 = new Bitmap(@"saper\1.png");
        Bitmap l2 = new Bitmap(@"saper\2.png");
        Bitmap l3 = new Bitmap(@"saper\3.png");
        Bitmap l4 = new Bitmap(@"saper\4.png");
        Bitmap l5 = new Bitmap(@"saper\5.png");
        Bitmap l6 = new Bitmap(@"saper\6.png");
        Bitmap l7 = new Bitmap(@"saper\7.png");
        Bitmap l8 = new Bitmap(@"saper\8.png");

        // ================= Deklaracje obiektów/tablic
        MyButton[,] button;
        Panel bar;
        Panel board;
        Button smile;
        Label czas;
        Label pick;


        public Form1()
        {
            InitializeComponent();
            
            ustaw_gre(size, bomby);
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;   //  blokuje rozmiar
         //   this.
            this.MouseDown += new MouseEventHandler(bd); //przypisanie handlera
            this.MouseUp += new MouseEventHandler(bu);
            smile.MouseDown += new MouseEventHandler(sm); //przypisanie handlera


        }

        private void sm(object sender, MouseEventArgs e)
        {
            NowaGra(bomby);
        }

        private void bd(object sender, MouseEventArgs e)
        {
            smile.Image = osmile;
        }
        private void bu(object sender, MouseEventArgs e)
        {
            smile.Image = ssmile;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ustaw_gre(int amm=15/*Ilosc pol*/, int bombs=10)
        {
            size=amm;
            button=new MyButton[size,size];   //  Deklaruje tablice 2D
            bomby = bombs;

            //  bar
            bar = new Panel();
            bar.Location = new Point(0,0);
            bar.Size = new Size(b_wymiar*size, 62); 
            this.Controls.Add(bar);
            //smile
            smile = new Button();
            smile.Location = new Point((int)bar.Width / 2 - 20, 5+6);
            smile.Size = new Size(46, 46);
            bar.Controls.Add(smile);
            smile.Image = ssmile;
            smile.TabStop = true;
            //smile.ImageAlign = ContentAlignment.MiddleCenter;
            //smile.BackgroundImageLayout = ImageLayout.Stretch;
            //Napisy
            pick = label1;
            pick.Location = new Point((int)bar.Width/6,bar.Height/2 - pick.Height/2);
            pick.BackColor = Color.Black;
            pick.ForeColor = Color.Yellow;
            pick.Visible = true;
            bar.Controls.Add(pick);
            //czas
            czas = label2;
            czas.Location = new Point((int)bar.Width / 6*5-czas.Width/2, bar.Height / 2 - pick.Height / 2);
            czas.BackColor = Color.Black;
            czas.ForeColor = Color.Yellow;
            czas.Visible = true;
            bar.Controls.Add(czas);
            czas.Text = "00,0";


            //  SZACHOWNICA
            board = new Panel();
            board.Location = new Point(0, bar.Height);
            board.Size = new Size(b_wymiar * size, b_wymiar*size);
            board.MinimumSize = board.Size;
            
            //board.Controls.Add(l);
            this.Controls.Add(board);


            //  Inicjalizacja obiektów i przypisanie wartości
            Size s = new Size(b_wymiar, b_wymiar);
            for (int X=0;X < size;X++)
            {
                for (int Y=0;Y < size; Y++)
                {
                    button[X,Y] = new MyButton(); //Deklaracka obiektu
                    button[X, Y].ImageAlign = ContentAlignment.MiddleCenter;
                    button[X, Y].BackgroundImageLayout = ImageLayout.Stretch;

                    button[X, Y].Size = s;  //b_wymiar
                    button[X, Y].Location = new Point(b_wymiar*X,b_wymiar* Y);
                    button[X, Y].x = X;
                    button[X, Y].y = Y; //  zapamietuje wspolrzedne 
                    board.Controls.Add(button[X,Y]);    //  przypisanie pól do szachownicy

                    button[X,Y].MouseDown += new MouseEventHandler(button_Click); //przypisanie handlera
                    button[X, Y].MouseMove += new MouseEventHandler(button_Move);

                }
            }

            //  Losowanie bomb
            NowaGra(bomby);


        }

        private void button_Click(object s, System.Windows.Forms.MouseEventArgs e)
        {
            if (end == true)
                return;

            var myb = s as MyButton;    //  interpretacja obiektu jako obiektu klasy MyButton
            if (myb == null)
            {
                return;
            }

            bu(myb, e);            //  Buzka


            if (timer1.Enabled==false)
            {
                timer1.Enabled = true;
            }
            if (e.Button == MouseButtons.Left) //  Lwy przycisk myszy!  <===============================================
            {
                if (myb.Flagged == true)
                    return;

                myb.Enabled = false;
                if (myb.HasMine == true)
                {
                    myb.BackgroundImage = rmine;
                    smile.Image = dsmile;
                    myb.Clicked = true;
                    timer1.Enabled = false;

                    end = true;
                    foreach (MyButton m in button)
                    {
                        if (m.HasMine == true && m.Clicked == false)
                        {
                            m.Image = null;
                            m.BackgroundImage = mine;

                        }
                    }
                }
                else
                {
                    Check(myb.x, myb.y);    //  Algorytm otwierania, jezeli nie wcisnieto bomby
                }
            }

            if (e.Button== MouseButtons.Right)
            {
                if (myb.Flagged == true)
                {
                    myb.Flagged = false;
                    myb.Image = null;
                    pick.Text = Convert.ToString(++picked);
                    
                }
                else if (myb.Flagged== false)
                {
                    myb.Image = flaga;
                    myb.Flagged = true;
                    myb.BackgroundImageLayout = ImageLayout.Stretch;
                    pick.Text = Convert.ToString(--picked);
                }
            }

        }   //  Klikniecie

        private void button_Move(object s, MouseEventArgs e)
        {
             this.TabIndex = 0;
        }

        private List<MyButton> getSquere(int x, int y)
        {
            MyButton m = button[x, y];
            List<MyButton> Squere =new List<MyButton>();
            //XXX
            //XOX
            //XXX
            int[] indexes = new int[]
            {
                -1,-1,
                0,-1,
                1,-1,
                -1,0,
                1,0,
                -1,1,
                0,1,
                1,1
            };

            for (int i = 0; i < indexes.Length; i++)
            {
                int dx = indexes[i];
                int dy = indexes[++i];

                int nX = m.x + dx;
                int nY = m.y + dy;

                if (nX >= 0 && nX < size
                        && nY >= 0 && nY < size)
                {
                    Squere.Add(button[nX, nY]);
                }
            }

            return Squere;
        }   //  Pobiera listę sąsiednich pól

        //=======================================   Check   -Działa rekurencyjnie
        List<MyButton> squere;
        private void Check( int x, int y)
        {
            //  Tutaj nie uwzglednia juz min
            if (button[x, y].Clicked)
                return;
            if (button[x, y].Flagged)   //  opcjonalne
                return;

            button[x, y].Clicked = true;    //  zaznacza do pomijania

            if (button[x, y].Number > 0)
            {
                button[x, y].Hit();
                DajL(button[x,y]);
            }
            else  //  Jezeli puste pole
            {

                button[x, y].Hit(); //  odslania
                squere = getSquere(x, y);
                foreach(MyButton m in squere)
                {
                    Check(m.x, m.y);
                }
            }

            int poles = 0;
            int flags = 0;
            foreach(MyButton m in button)
            {
                if (m.Clicked == true)
                    poles++;
                if (m.Flagged == true && m.HasMine==true)
                    flags++;
            }
            if (button.Length-poles==bomby || flags==bomby) //  jezeli wygrana
            {
                smile.Image = vsmile;
                end = true;
                timer1.Enabled = false;
            }

        }

        private void NowaGra(int bombsamm)
        {
            foreach (MyButton m in button)
            {
                end = false;
                m.Clicked = false;
                m.HasMine = false;
                m.Flagged = false;
                m.Number = 0;
                m.Enabled = true;
                m.BackgroundImage = null;
                m.Image = null;
                smile.Image = ssmile;
                pick.Text = Convert.ToString(picked = bombsamm);
                czas.Text = Convert.ToString(seconds = 0);
                timer1.Enabled = false;
            }

                bomby = bombsamm;
                int tmp = bomby;
                //  Losowanie bomb
                Random rnd = new Random();
                while (bomby > 0)
                {
                    int tx, ty;
                    tx = rnd.Next(0, size);
                    ty = rnd.Next(0, size);
                    if (button[tx, ty].HasMine == false && button[tx, ty].Number < 6)
                    {
                        button[tx, ty].HasMine = true;

                        if (tx < size - 1)
                            button[tx + 1, ty].Number++;
                        if (tx < size - 1 && ty < size - 1)
                            button[tx + 1, ty + 1].Number++;
                        if (ty < size - 1)
                            button[tx, ty + 1].Number++;

                        if (tx < size - 1 && ty > 0)
                            button[tx + 1, ty - 1].Number++;
                        if (tx > 0 && ty < size - 1)
                            button[tx - 1, ty + 1].Number++;
                        if (tx > 0)
                            button[tx - 1, ty].Number++;
                        if (ty > 0)
                            button[tx, ty - 1].Number++;
                        if (tx > 0 && ty > 0)
                            button[tx - 1, ty - 1].Number++;

                        bomby--;
                    }
                 }
            bomby = tmp;
                
        }

        void DajL(MyButton m)
        {
            if (m.Number == 0 || m.HasMine)
                return;
            switch (m.Number)
            {
                case 1:
                    {
                        m.Image = null;
                        m.BackgroundImage = l1;
                        break;
                    }
                case 2:
                    {
                        m.Image = null;
                        m.BackgroundImage = l2;
                        break;
                    }
                case 3:
                    {
                        m.Image = null;
                        m.BackgroundImage = l3;
                        break;
                    }
                case 4:
                    {
                        m.Image = null;
                        m.BackgroundImage = l4;
                        break;
                    }
                case 5:
                    {
                        m.Image = null;
                        m.BackgroundImage = l5;
                        break;
                    }
                case 6:
                    {
                        m.Image = null;
                        m.BackgroundImage = l6;
                        break;
                    }
                case 7:
                    {
                        m.Image = null;
                        m.BackgroundImage = l7;
                        break;
                    }
                case 8:
                    {
                        m.Image = null;
                        m.BackgroundImage = l8;
                        break;
                    }
            }
            m.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            czas.Text = Convert.ToString(seconds+=0.1);
        }
    }
}
