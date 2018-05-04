using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTimer.Content
{
    interface IBase
    {
        void LoadContent(ContentManager content);

        void Draw(SpriteBatch spriteBatch);

        void Update(GameTime gameTime);

    }
}
