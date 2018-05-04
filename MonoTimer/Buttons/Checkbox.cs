using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;
using MonoTimer.Content.Button;

namespace MonoTimer.Content.Buttons
{
    public class Checkbox : Clickbase
    {
        #region Fields
        readonly string asset;
        Texture2D textureOn;
        bool isChecked;

        #region Public accessors
        public bool IsChecked { get { return isChecked; } }
        #endregion
        #endregion

        #region Initialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game">The Game object</param>
        /// <param name="textureName">Texture name</param>
        /// <param name="targetRectangle">Position of the component on the screen</param>
        /// <param name="isChecked">Initial state of the checkbox</param>
        public Checkbox(Game game, string textureName, Rectangle targetRectangle, bool isChecked)
            : base(game, targetRectangle)
        {
            asset = textureName;
            this.isChecked = isChecked;
        }

        /// <summary>
        /// Load the texture
        /// </summary>
        protected override void LoadContent()
        {
            textureOn = Game.Content.Load<Texture2D>(asset);
            base.LoadContent();
        }
        #endregion

        #region Update and render
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            HandleInput();
            isChecked = IsClicked ? !isChecked : isChecked;
            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Begin();
            Game.SpriteBatch.Draw(textureOn, Rectangle,
                IsChecked ? Color.Yellow : Color.White);
            Game.SpriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion
    }
}
