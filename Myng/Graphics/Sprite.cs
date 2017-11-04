using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Myng.Graphics
{
    //every sprite will extend this class 
    abstract public class Sprite
    {
        protected Texture2D texture;
        protected Vector2 position { get; set; }
        public bool toRemove = false; //use this to mark sprite for removal

        public Sprite(Texture2D texture2D)
        {
            this.texture = texture2D;
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
