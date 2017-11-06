using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace Myng.Graphics
{
    //every sprite will extend this class
    abstract public class Sprite
    {
        protected Texture2D texture;
        protected Vector2 position { get; set; }
        public bool toRemove = false; //use this to mark sprite for removal

        //for easy collision checking
        protected Rectangle rectangle 
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            }
        }


        public Sprite(Texture2D texture2D, Vector2 position)
        {
            this.texture = texture2D;
            this.position = position;
        }

        //this method will take care of pretty much everything thats happening
        //should be overridden in every child class (unless the child has no functionality whatsoever)
        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {
            
        }

        //default Draw method
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }    

    }
}
