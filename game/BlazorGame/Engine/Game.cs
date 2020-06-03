using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazor.Extensions.Canvas.WebGL;
using BlazorGame.Engine;
using BlazorGame.Logic;
using Blazor.Extensions;

namespace BlazorGame
{
    public class Game
    {
        private int particleCount = 0;

        private List<Particle> particleList = new List<Particle>();

        private float gravity = 1;

        public Renderer rendererInstance;


        public Game(BECanvasComponent CanvasReference)
        {
            rendererInstance = new Renderer(CanvasReference);
        }


        public async Task Initialize()
        {
            await rendererInstance.Initialize();
        }

        public async Task DoRender()
        {
            rendererInstance.spriteEngine.QueueSprites(Particle.GetSprites(particleList));
            await rendererInstance.RenderFrame();
        }

        public void DoWorld()
        {
            foreach (Particle myParticle in particleList)
            {
                myParticle.dirVertical += gravity;

                myParticle.X += myParticle.dirHorizontal;
                myParticle.Y += myParticle.dirVertical;

                float cutPoint = ((float)rendererInstance.canvasHeight - (float)myParticle.sprite.sizeY);

                if ((myParticle.Y) > cutPoint)
                {
                    myParticle.Y = cutPoint;
                    myParticle.dirVertical = myParticle.dirVertical * (float)-0.8;
                }

                if (myParticle.dirVertical > 20)
                    myParticle.dirVertical = 20;
            }
        }

        public async Task DoSpecial(int x, int y)
        {
            await Task.Run(() =>
            {
                Particle myParticle = new Particle();
                myParticle.X = x;
                myParticle.Y = y;

                myParticle.dirVertical = (float)0.1;

                particleList.Add(myParticle);

                particleCount++;

            });

        }

    }
}
