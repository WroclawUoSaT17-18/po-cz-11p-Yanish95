using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZegAnalogowy
{
    public partial class MainForm : Form
    {
        private Rectangle rect;
        private LinearGradientBrush obramowanieKolor;
        private SolidBrush tarczaKolor;
        private SolidBrush liczbyKolor;
        private SolidBrush podpisKolor;
        private Pen cienTarczyKolor;
        private Pen pioro;
        private Pen pioroSek;
        private int srednica;


        //wspolrzedne srodka okna
        int srWidth;
        int srHeight;

        //czas w danej chwili
        int minuty;
        int godziny;
        double sekundy;

        //rotacje wskazowek
        double minutyTic;
        double godzinyTic;
        double sekundyTic;

        float promien; //dlugosc wskazowki
        int ramka; //szerokość czarnej ramki
        int stopnie; //odleglosc miedzy liczbami

        DateTime czas;

        public MainForm()
        {
            srWidth = this.ClientSize.Width / 2;
            srHeight = this.ClientSize.Height / 2;

            ramka = 18;

            InitializeComponent();
            inicjujNarzedzia();

            this.Paint += new PaintEventHandler(MainForm_Paint);

        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            rysuj(e.Graphics);
        }
        //metody zwraczające wspołrzędne dla cyfr i dekoracji
        private float obliczX(float stopnie, float r)
        {
            return (float)(r * Math.Cos((Math.PI / 180) * stopnie));
        }
        private float obliczY(float stopnie, float r)
        {
            return (float)(r * Math.Sin((Math.PI / 180) * stopnie));
        }

        // metoda do zainicjowania wszystkich obiektów graficznych 
        void inicjujNarzedzia()
        {
            srednica = 120;
            rect = new Rectangle(this.ClientSize.Width / 2 - srednica / 2, this.ClientSize.Height / 2 - srednica / 2, srednica, srednica);

            obramowanieKolor = new LinearGradientBrush(rect, Color.FromArgb(0, 0, 0), Color.FromArgb(60, 60, 60), 60);
            tarczaKolor = new SolidBrush(Color.WhiteSmoke);
            liczbyKolor = new SolidBrush(Color.FromArgb(10, 10, 10));
            podpisKolor = new SolidBrush(Color.Blue);
            cienTarczyKolor = new Pen(Color.FromArgb(180, 180, 180), 3);
            //parametry końcówek piór 
            pioro = new Pen(Color.FromArgb(10, 10, 10), 4);
            pioroSek = new Pen(Color.Red, 2);
            pioro.EndCap = LineCap.ArrowAnchor;
            pioro.StartCap = LineCap.RoundAnchor;
            pioroSek.EndCap = LineCap.ArrowAnchor;
            pioroSek.StartCap = LineCap.RoundAnchor;
            cienTarczyKolor.EndCap = LineCap.ArrowAnchor;
            cienTarczyKolor.StartCap = LineCap.RoundAnchor;
        }

        void rysuj(Graphics graphics)
        {
            srWidth = this.ClientSize.Width / 2;
            srHeight = this.ClientSize.Height / 2;
            ramka = 18;
            graphics.FillEllipse(obramowanieKolor, srWidth - srednica / 2 - ramka / 2, srHeight - srednica / 2 - ramka / 2, srednica + ramka, srednica + ramka);
            graphics.FillEllipse(tarczaKolor, rect);
            graphics.DrawEllipse(cienTarczyKolor, rect);
            //przeniesienie do środka okna 
            graphics.TranslateTransform(srWidth, srHeight);
            //rozmiar typ i formatowanie czcionki 
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            Font textFont = new Font("Century", 12F, FontStyle.Bold);
            //zdefiniowanie położenia liczb
                stopnie = 360 / 12;
            promien = 49;
            // dodawanie liczb 
            for (int i = 1; i <= 12; i++)
            {
                graphics.DrawString(i.ToString(), textFont, liczbyKolor, -1 * obliczX(i * stopnie + 90, promien) + 1, -1 * obliczY(i * stopnie + 90, promien) + 2, format);
            }

        }


    }
}
