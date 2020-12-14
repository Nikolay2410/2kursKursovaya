using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2kursKursovaya
{
    public abstract class IImpactPoint
    {
        public float X; // ну точка же, вот и две координаты
        public float Y;

        public int X1; // координата X центра эмиттера, будем ее использовать вместо MousePositionX
        public int Y1; // соответствующая координата Y 
        public int Direction = 90; // вектор направления в градусах куда сыпет эмиттер
        public int Spreading = 120; // разброс частиц относительно Direction
        public int SpeedMin = 1; // начальная минимальная скорость движения частицы
        public int SpeedMax = 10; // начальная максимальная скорость движения частицы
        public int RadiusMin = 2; // минимальный радиус частицы
        public int RadiusMax = 10; // максимальный радиус частицы
        public int LifeMin = 20;
        public int LifeMax = 100;

        public Color ColorS = Color.Red;
        public Color ColorP = Color.White;

        // абстрактный метод с помощью которого будем изменять состояние частиц
        // например притягивать
        public abstract void ImpactParticle(Particle particle);

        // базовый класс для отрисовки точечки
        public virtual void Render(Graphics g)
        {
            g.FillEllipse(
                    new SolidBrush(Color.Red),
                    X - 5,
                    Y - 5,
                    10,
                    10
                );
        }
    }

    public class GravityPoint : IImpactPoint
    {
        int count = 0; // количество точек
        int Opacity = 0;
        public override void ImpactParticle(Particle particle)
        {
            float gX = X - particle.X;
            float gY = Y - particle.Y;

            double r = Math.Sqrt(gX * gX + gY * gY); // считаем расстояние от центра точки до центра частицы
            if (r + particle.Radius < Gravitation / 2) // если частица оказалось внутри окружности
            {
                count++;
                if (Opacity < 255 && count % 3 == 0)
                {
                    Opacity++;
                }
                particle.Life = 0;
            }
        }
        public int Gravitation = 100;
        public override void Render(Graphics g)
        {
            // буду рисовать окружность с диаметром равным Power
            g.FillEllipse(
                new SolidBrush(Color.FromArgb(Opacity, ColorS)),
                X - Gravitation / 2,
                Y - Gravitation / 2,
                Gravitation,
                Gravitation
            );
            g.DrawEllipse(
                new Pen(Color.White),
                X - Gravitation / 2,
                Y - Gravitation / 2,
                Gravitation,
                Gravitation
            );

            var stringFormat = new StringFormat(); // создаем экземпляр класса
            stringFormat.Alignment = StringAlignment.Center; // выравнивание по горизонтали
            stringFormat.LineAlignment = StringAlignment.Center; // выравнивание по вертикали

            g.DrawString(
                $"{count}",
                new Font("Verdana", 10),
                new SolidBrush(Color.White),
                X,
                Y,
                stringFormat // передаем инфу о выравнивании
            );
        }
    }

    public class GravityPointMouse : IImpactPoint
    {
        int count = 0; // количество точек
        public override void ImpactParticle(Particle particle)
        {
            float gX = X - particle.X;
            float gY = Y - particle.Y;

            double r = Math.Sqrt(gX * gX + gY * gY); // считаем расстояние от центра точки до центра частицы
            if (r + particle.Radius < Gravitation / 2) // если частица оказалось внутри окружности
            {
                var partColor = particle as ParticleColorful;
                partColor.FromColor = ColorP;
                count++;
            }
        }
        public int Gravitation = 100;
        public override void Render(Graphics g)
        {
            // буду рисовать окружность с диаметром равным Power
            g.DrawEllipse(
                new Pen(Color.White),
                X - Gravitation / 2,
                Y - Gravitation / 2,
                Gravitation,
                Gravitation
            );

            var stringFormat = new StringFormat(); // создаем экземпляр класса
            stringFormat.Alignment = StringAlignment.Center; // выравнивание по горизонтали
            stringFormat.LineAlignment = StringAlignment.Center; // выравнивание по вертикали

            g.DrawString(
                $"{count}",
                new Font("Verdana", 10),
                new SolidBrush(Color.White),
                X,
                Y,
                stringFormat // передаем инфу о выравнивании
            );
        }
    }

}


