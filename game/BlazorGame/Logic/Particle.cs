using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BlazorGame.Engine;

namespace BlazorGame.Logic
{
    public class Particle
    {
        public Sprite sprite;

        public float X = 0;
        public float Y = 0;

        // Replace this with a vector!
        public float dirHorizontal = 0;
        public float dirVertical = 0;

        public Particle()
        {
            sprite = new Sprite();
        }

        public static List<Sprite> GetSprites(List<Particle> myParticles)
        {
            List<Sprite> mySprites = new List<Sprite>();

            foreach (Particle myParticle in myParticles)
            {
                myParticle.sprite.X = (int)myParticle.X;
                myParticle.sprite.Y = (int)myParticle.Y;
                myParticle.sprite.sizeX = 64;
                myParticle.sprite.sizeY = 64;

                mySprites.Add(myParticle.sprite);

            }

            return mySprites;
        }

    }

}
