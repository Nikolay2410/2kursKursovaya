﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2kursKursovaya
{
    public class Emitter
    {
        List<Particle> particles = new List<Particle>();
        public List<IImpactPoint> impactPoints = new List<IImpactPoint>();
        public List<IImpactPoint> impactPointsMouse = new List<IImpactPoint>();

        public static int Rad = 1;
        public int X; // координата X центра эмиттера, будем ее использовать вместо MousePositionX
        public int Y; // соответствующая координата Y 
        public int Direction = 90; // вектор направления в градусах куда сыпет эмиттер
        public int Spreading = 120; // разброс частиц относительно Direction
        public int SpeedMin = 1; // начальная минимальная скорость движения частицы
        public int SpeedMax = 10; // начальная максимальная скорость движения частицы
        public int RadiusMin = 2 + Rad; // минимальный радиус частицы
        public int RadiusMax = 10 + Rad; // максимальный радиус частицы
        public int LifeMin = 20;
        public int LifeMax = 100;

        public int ParticlesPerTick = 1; // добавил новое поле

        //public Color ColorFrom = Color.White; // начальный цвет частицы
        //public Color ColorTo = Color.FromArgb(0, Color.Black); // конечный цвет частиц

        public int MousePositionX;
        public int MousePositionY;

        public float GravitationX = 0;
        public float GravitationY = 1; // пусть гравитация будет силой один пиксель за такт, нам хватит

        public virtual Particle CreateParticle()
        {
            var particle = new ParticleColorful();
            particle.FromColor = Particle.ColorCh;
            particle.ToColor = Particle.ColorCh2;

            return particle;
        }

        public void UpdateState()
        {
            int particlesToCreate = ParticlesPerTick; // фиксируем счетчик сколько частиц нам создавать за тик

            foreach (var particle in particles)
            {
                if (particle.Life <= 0) // если частицы умерла
                {
                    if (particlesToCreate > 0)
                    {
                        /* у нас как сброс частицы равносилен созданию частицы */
                        particlesToCreate -= 1; // поэтому уменьшаем счётчик созданных частиц на 1
                        ResetParticle(particle);
                    }
                }
                else
                {
                    particle.X += particle.SpeedX;
                    particle.Y += particle.SpeedY;

                    particle.Life -= 1;
                    foreach (var point in impactPoints)
                    {
                        point.ImpactParticle(particle);
                    }
                    foreach (var point in impactPointsMouse)
                    {
                        point.ImpactParticle(particle);
                    }

                    particle.SpeedX += GravitationX;
                    particle.SpeedY += GravitationY;
                }
            }

            while (particlesToCreate >= 1)
            {
                particlesToCreate -= 1;
                var particle = CreateParticle();
                ResetParticle(particle);
                particles.Add(particle);
            }
        }

        public int ParticlesCount = 500;
        // добавил новый метод, виртуальным, чтобы переопределять можно было
        public virtual void ResetParticle(Particle particle)
        {
            var color = particle as ParticleColorful;
            color.FromColor = Particle.ColorCh;
            color.ToColor = Particle.ColorCh2;

            particle.Life = Particle.rand.Next(LifeMin, LifeMax);

            particle.X = X;
            particle.Y = Y;

            var direction = Direction
                + (double)Particle.rand.Next(Spreading)
                - Spreading / 2;

            var speed = Particle.rand.Next(SpeedMin, SpeedMax);

            particle.SpeedX = (float)(Math.Cos(direction / 180 * Math.PI) * speed);
            particle.SpeedY = -(float)(Math.Sin(direction / 180 * Math.PI) * speed);

            particle.Radius = Particle.rand.Next(RadiusMin+ Rad, RadiusMax + Rad);
        }

        public void Render(Graphics g)
        {
            // не трогаем
            foreach (var particle in particles)
            {
                particle.Draw(g);
            }

            foreach (var point in impactPoints)
            {
                point.Render(g); // это добавили
            }
            foreach (var point in impactPointsMouse)
            {
                point.Render(g); // это добавили
            }
        }
    }
}
