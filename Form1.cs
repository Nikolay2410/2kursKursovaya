using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2kursKursovaya
{
    public partial class Form1 : Form
    {
        List<Emitter> emitters = new List<Emitter>();
        Emitter emitter;

        GravityPointMouse point2; // добавил поле под вторую точку
        GravityPoint point;

        public Form1()
        {
            InitializeComponent();
            
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);
            tbGraviton2.Value = 100;
            tbCount.Value = 20;

            // расширенное окно для выбора цвета
            colorDialog1.FullOpen = true;
            colorDialog2.FullOpen = true;
            colorDialog3.FullOpen = true;
            colorDialog4.FullOpen = true;
            colorDialog2.Color = Color.Red;
            // установка начального цвета для colorDialog
            button1.BackColor = Color.Magenta;
            button2.BackColor = Color.Red;
            button3.BackColor = Color.Black;
            button4.BackColor = Color.White;

            this.emitter = new Emitter // создаю эмиттер и привязываю его к полю emitter
            {
                Direction = 340,
                Spreading = 120,
                SpeedMin = 10,
                SpeedMax = 20,
                ParticlesPerTick = 20,
                ColorFrom = Color.Magenta,
                ColorTo = Color.Black,
                X = 0,
                Y = 0,
            };
            emitters.Add(this.emitter); // все равно добавляю в список emitters, чтобы он рендерился и обновлялся
            point2 = new GravityPointMouse
            {
                X = picDisplay.Width / 2 - 100,
                Y = picDisplay.Height / 2,
            };

            // привязываем поля к эмиттеру
            emitter.impactPointsMouse.Add(point2);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            emitter.UpdateState(); // тут теперь обновляем эмиттер

            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black);
                emitter.Render(g); // а тут теперь рендерим через эмиттер
            }

            picDisplay.Invalidate();
        }

        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            // это не трогаем
            foreach (var emitter in emitters)
            {
                emitter.MousePositionX = e.X;
                emitter.MousePositionY = e.Y;
            }

            // а тут передаем положение мыши, в положение гравитона
            point2.X = e.X;
            point2.Y = e.Y;
        }

        private void tbGraviton2_Scroll(object sender, EventArgs e)
        {
            point2.Gravitation = tbGraviton2.Value;
            label8.Text = Convert.ToString(tbGraviton2.Value);
        }

        private void picDisplay_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                point = new GravityPoint
                {
                    X = e.X,
                    Y = e.Y,
                    ColorS = colorDialog2.Color
                };
                emitter.impactPoints.Add(point);
                point.Gravitation = tbGraviton2.Value;
                foreach (var points in emitter.impactPoints)
                {
                    points.ColorS = colorDialog2.Color;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                point = new GravityPoint
                {
                    X = e.X,
                    Y = e.Y,
                };
                if (emitter.impactPoints.Count > 0) { emitter.impactPoints.RemoveAt(emitter.impactPoints.Count - 1); }
                
            }
        }


        private void button1_Click(object sender, EventArgs e) //цвет частичек из
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета частиц
            emitter.ColorFrom = colorDialog1.Color;
            //Particle.ColorCh = colorDialog1.Color;
            button1.BackColor = colorDialog1.Color;
        }

        private void button2_Click(object sender, EventArgs e) //цвет счётчика
        {
            if (colorDialog2.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета счётчика
            button2.BackColor = colorDialog2.Color;

            foreach (var points in emitter.impactPoints)
            {
                points.ColorS = colorDialog2.Color;
            }
        }

        private void tbSize_Scroll(object sender, EventArgs e)
        {
            Emitter.Rad = tbSize.Value;
            label9.Text = Convert.ToString(tbSize.Value);
        }

        private void button3_Click(object sender, EventArgs e) //цвет частичек
        {
            if (colorDialog3.ShowDialog() == DialogResult.Cancel)
                return;
            //Particle.ColorCh2 = colorDialog3.Color;
            emitter.ColorTo = colorDialog3.Color;
            button3.BackColor = colorDialog3.Color;
        }

        private void tbCount_Scroll(object sender, EventArgs e)
        {
            emitter.ParticlesPerTick = tbCount.Value;
            label10.Text = Convert.ToString(tbCount.Value);
        }

        private void button4_Click(object sender, EventArgs e) //перекрашивание
        {
            if (colorDialog4.ShowDialog() == DialogResult.Cancel)
                return;
            //IImpactPoint.ColorP = colorDialog4.Color;
            foreach (var ColorP in emitter.impactPointsMouse)
            {
                ColorP.ColorP = colorDialog4.Color;
            }
            button4.BackColor = colorDialog4.Color;
        }
    }
}
