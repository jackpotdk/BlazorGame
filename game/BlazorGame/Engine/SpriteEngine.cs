using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGame.Engine
{
    public class SpriteEngine
    {
        public int spriteCount = 0;

        public float[] vertices = new float[6 * 6 * 100];



        public long canvasHeight = 0;

        public void QueueSprites(List<Sprite> mySprites)
        {
            foreach (Sprite mySprite in mySprites)
            {
                QueueSprite(mySprite);
            }
        }

        public void QueueSprite(Sprite mySprite)
        {
            float myXupLeft = (float)((float)mySprite.X);
            float myYupLeft = (float)((float)canvasHeight-(float)mySprite.Y);

            float myXupRight = (float)((float)(mySprite.X + mySprite.sizeX));
            float myYupRight = (float)((float)canvasHeight - (float)(mySprite.Y));

            float myXdownLeft = (float)((float)mySprite.X);
            float myYdownLeft = (float)((float)canvasHeight - (float)(mySprite.Y + mySprite.sizeY));

            float myXdownRight = (float)((float)(mySprite.X + mySprite.sizeX));
            float myYdownRight = (float)((float)canvasHeight - (float)(mySprite.Y + mySprite.sizeY));

            vertices[0 + (spriteCount * 6 * 6)] = myXdownLeft;
            vertices[1 + (spriteCount * 6 * 6)] = myYdownLeft;
            vertices[2 + (spriteCount * 6 * 6)] = 0;

            vertices[3 + (spriteCount * 6 * 6)] = (float)0.0;
            vertices[4 + (spriteCount * 6 * 6)] = (float)1.0;

            vertices[6 + (spriteCount * 6 * 6)] = myXdownRight;
            vertices[7 + (spriteCount * 6 * 6)] = myYdownRight;
            vertices[8 + (spriteCount * 6 * 6)] = 0;

            vertices[9 + (spriteCount * 6 * 6)] = (float)1.0;
            vertices[10 + (spriteCount * 6 * 6)] = (float)1.0;

            vertices[12 + (spriteCount * 6 * 6)] = myXupLeft;
            vertices[13 + (spriteCount * 6 * 6)] = myYupLeft;
            vertices[14 + (spriteCount * 6 * 6)] = 0;

            vertices[15 + (spriteCount * 6 * 6)] = (float)0.0;
            vertices[16 + (spriteCount * 6 * 6)] = (float)0.0;

            vertices[18 + (spriteCount * 6 * 6)] = myXupLeft;
            vertices[19 + (spriteCount * 6 * 6)] = myYupLeft;
            vertices[20 + (spriteCount * 6 * 6)] = 0;

            vertices[21 + (spriteCount * 6 * 6)] = (float)0.0;
            vertices[22 + (spriteCount * 6 * 6)] = (float)0.0;

            vertices[24 + (spriteCount * 6 * 6)] = myXupRight;
            vertices[25 + (spriteCount * 6 * 6)] = myYupRight;
            vertices[26 + (spriteCount * 6 * 6)] = 0;

            vertices[27 + (spriteCount * 6 * 6)] = (float)1.0;
            vertices[28 + (spriteCount * 6 * 6)] = (float)0.0;

            vertices[30 + (spriteCount * 6 * 6)] = myXdownRight;
            vertices[31 + (spriteCount * 6 * 6)] = myYdownRight;
            vertices[32 + (spriteCount * 6 * 6)] = 0;

            vertices[33 + (spriteCount * 6 * 6)] = (float)1.0;
            vertices[34 + (spriteCount * 6 * 6)] = (float)1.0;

            spriteCount++;
        }
    }
}