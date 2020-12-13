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

        GravityPoint point2; // добавил поле под вторую точку
        GravityPoint point;

        public Form1()
        {
            InitializeComponent();
            
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);
            tbGraviton2.Value = 100;

            this.emitter = new Emitter // создаю эмиттер и привязываю его к полю emitter
            {
                Direction = 0,
                Spreading = 120,
                SpeedMin = 10,
                SpeedMax = 20,
                ColorFrom = Color.Gold,
                ColorTo = Color.FromArgb(0, Color.Red),
                ParticlesPerTick = 10,
                X = 0,
                Y = 0,
            };
            emitters.Add(this.emitter); // все равно добавляю в список emitters, чтобы он рендерился и обновлялся
            point2 = new GravityPoint
            {
                X = picDisplay.Width / 2 - 100,
                Y = picDisplay.Height / 2,
            };

            // привязываем поля к эмиттеру
            emitter.impactPoints.Add(point2);

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
            point2.Power = tbGraviton2.Value;
            point2.Gravitation = tbGraviton2.Value;
        }

        private void picDisplay_MouseClick(object sender, MouseEventArgs e)
        {
            var color = Color.Red;
            if (e.Button == MouseButtons.Left)
            {
                switch(comboBox1.SelectedIndex)
                {
                    case 0: //красный
                        color = Color.Red;
                        break;
                    case 1: //синий
                        color = Color.Blue;
                        break;
                    case 2: //зелёный
                        color = Color.Green;
                        break;
                    case 3: //фиолетовый
                        color = Color.Purple;
                        break;
                }
                if (comboBox1.SelectedIndex == 1)
                {
                    color = Color.Blue;
                }
                point = new GravityPoint
                {
                    X = e.X,
                    Y = e.Y,
                    ColorS = color
                };
                emitter.impactPoints.Add(point);
                point.Gravitation = tbGraviton2.Value;
            }
            else if (e.Button == MouseButtons.Right)
            {
                point = new GravityPoint
                {
                    X = e.X,
                    Y = e.Y,
                };
                if (emitter.impactPoints.Count > 1) { emitter.impactPoints.RemoveAt(emitter.impactPoints.Count - 1); }
                
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //счётчик
        {
            if (comboBox1.SelectedIndex == 1)
            {
                foreach (var points in emitter.impactPoints)
                {
                    points.ColorS = Color.Blue;
                }
            }
            switch (comboBox1.SelectedIndex)
            {
                case 0: //красный
                    foreach (var points in emitter.impactPoints)
                    {
                        points.ColorS = Color.Red;
                    }
                    break;
                case 1: //синий
                    foreach (var points in emitter.impactPoints)
                    {
                        points.ColorS = Color.Blue;
                    }
                    break;
                case 2: //зелёный
                    foreach (var points in emitter.impactPoints)
                    {
                        points.ColorS = Color.Green;
                    }
                    break;
                case 3: //фиолетовый
                    foreach (var points in emitter.impactPoints)
                    {
                        points.ColorS = Color.Purple;
                    }
                    break;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) //частички
        {
            Particle.ColorCh = Color.Blue;
            switch (comboBox2.SelectedIndex)
            {
                case 0: //красный
                    Particle.ColorCh = Color.Red;
                    break;
                case 1: //синий
                    Particle.ColorCh = Color.Blue;
                    break;
                case 2: //зелёный
                    Particle.ColorCh = Color.Green;
                    break;
                case 3: //фиолетовый
                    Particle.ColorCh = Color.Purple;
                    break;
                case 4: //розовый
                    Particle.ColorCh = Color.Magenta;
                    break;
                case 5: //белый
                    Particle.ColorCh = Color.White;
                    break;
            }
        }
    }
}
