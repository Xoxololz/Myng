using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myng.Graphics
{
    abstract public class Projectile : Sprite, ICloneable
    {
        protected Sprite parent { get; set; }

        public Vector2 direction;
        public float speed = 0f; 

        private float timer = 0f;
        public float lifespan = 3f;

        public Projectile(Texture2D texture2D)
            : base(texture2D)
        {

        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer > lifespan)
            {
                toRemove = true;
            }

            position += direction * speed;
        }

        public abstract object Clone();
    }
}
