using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
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

        //zmienne globalne do obsługi myszki
        private Point MouseAktualnaPoz, MouseNowaPoz, formPoz, formNowaPoz;
        private bool mouseDown = false;

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
        //Aktualny czas 
       
            
        public MainForm()
        {
            srWidth = this.ClientSize.Width / 2;
            srHeight = this.ClientSize.Height / 2;

            ramka = 18;

            InitializeComponent();
            inicjujNarzedzia();
            

            this.Paint += new PaintEventHandler(MainForm_Paint);
            //double bufering braknieprzyjemnego odświerzania
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //pozbywanie się z paska zadań
            this.ShowInTaskbar = false;
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
      
        //metoda do rysowania tegesów
        void rysuj(Graphics graphics)

        {
            //anti-aliasing
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //
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
            //
            promien = 61;
            stopnie = 360 / 4;

            for (int i = 1; i <= 4; i++)
            {
                graphics.DrawEllipse(pioro, -1 * obliczX(i * stopnie + 90, promien) - 1, -1 * obliczY(i * stopnie + 90, promien) - 1, 2, 2);
            }
            //aktualny czas 
            czas = DateTime.Now;
            godziny = czas.Hour;
            minuty = czas.Minute;
            sekundy = czas.Second + (czas.Millisecond * 0.001);

            godzinyTic = 2.0 * Math.PI * (godziny + minuty / 60.0) / 12.0; promien=30;
            //Cień na tarczę 
            Point pktSrodek = new Point(0, 0);
            Point pktCienGodzina = new Point((int)((promien * Math.Sin(godzinyTic)) + 2), (int)((-(promien) * Math.Cos(godzinyTic)) + 2));
            graphics.DrawLine(cienTarczyKolor, pktSrodek, pktCienGodzina);
            Point pktWskGodzina = new Point((int)(promien * Math.Sin(godzinyTic)), (int)(-(promien) * Math.Cos(godzinyTic)));
            graphics.DrawLine(pioro, pktSrodek, pktWskGodzina);
            //minuty i sekundy 
            minutyTic = 2.0 * Math.PI * (minuty + sekundy / 60.0) / 60.0; promien = 57;

            Point pktCienMinuta = new Point((int)(promien * Math.Sin(minutyTic) + 2), (int)(-(promien) * Math.Cos(minutyTic) + 2));
            graphics.DrawLine(cienTarczyKolor, pktSrodek, pktCienMinuta);
            Point pktWskMinuta = new Point((int)(promien * Math.Sin(minutyTic)), (int)(-(promien) * Math.Cos(minutyTic)));
            graphics.DrawLine(pioro, pktSrodek, pktWskMinuta);

            sekundyTic = 2.0 * Math.PI * (minuty + sekundy / 60.0);
            Point pktCienSekunda = new Point((int)(promien * Math.Sin(sekundyTic)), (int)(-(promien) * Math.Cos(sekundyTic)));
            graphics.DrawLine(pioroSek, pktSrodek, pktCienSekunda);
            Point pktWskSekunda = new Point((int)(promien * Math.Sin(sekundyTic)), (int)(-(promien) * Math.Cos(sekundyTic)));
            graphics.DrawLine(pioroSek, pktSrodek, pktWskSekunda);
        }
      
        private void timer1_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void zamknijToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        //NIE DZIAŁA - ZAMYKANIE ZEGARKA PRZEZ ROZWIJANE MENU- NIE DZIALA
        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {

        }
        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.ContextMenuStrip.Show(Control.MousePosition);
            }
        }

        //NIE DZIAŁA -PRZESUWANIE ZEGARKA NACISNIĘCIEM PRZYCISKU MYSZY-NIE DZIAŁA 
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                MouseAktualnaPoz = Control.MousePosition;
                formPoz = Location;
            }
        }
        //Jeśli mysz wciśnieta wylicza lokalizacje i przesuwa zegarek 
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown == true)
            {
                MouseNowaPoz = Control.MousePosition;
                formNowaPoz.X = MouseNowaPoz.X - MouseAktualnaPoz.X + formPoz.X;
                formNowaPoz.Y = MouseNowaPoz.Y - MouseAktualnaPoz.Y + formPoz.Y;
                Location = formNowaPoz;
                formPoz = formNowaPoz;
                MouseAktualnaPoz = MouseNowaPoz;
            }
        }
        //zdarzenie po puszczeniu przycisku myszy 
        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mouseDown = false;
        }
    }
}
