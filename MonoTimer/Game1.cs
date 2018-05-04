#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoTimer.Content;

#endregion

namespace MonoTimer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Vector2 timePosition = new Vector2(50, 400);
        Vector2 time2Position = new Vector2(50, 500);
        SpriteFont font;
        StringBuilder time;
        StringBuilder time2;
        TimeSpan timer, eTime, cTime;
        private Button button;
        TouchCollection touchCollection;
        private bool paused;
        private Texture2D gear;

        private List<Sprite> sprites = new List<Sprite>();
        private Sprite selectedSprite;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            TouchPanel.EnabledGestures =
                GestureType.Hold |
                GestureType.Tap |
                GestureType.DoubleTap;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Content.RootDirectory = "Content";
            font = Content.Load<SpriteFont>("BaseFont");
            gear = Content.Load<Texture2D>("Images/gear");

            // button

            button = new Button(new Rectangle(200,100, 200, 75), "TEXT", font);

            //TODO: use this.Content to load your game content here 
        }

        int counter = 1;


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            spriteBatch.Dispose();

        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // For Mobile devices, this logic will close the Game when the Back button is pressed
            // Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
#endif
            //touchCollection = TouchPanel.GetState();

            // handle the touch input
            HandleTouchInput();

            time = new StringBuilder();
            time2 = new StringBuilder();

            // check for button pressed
            //while (TouchPanel.IsGestureAvailable)
            //{
            //    GestureSample gs = TouchPanel.ReadGesture();
            //    switch (gs.GestureType)
            //    {
            //        case GestureType.Tap:
            //            if (button.Rectangle.Contains((int)gs.Position.X, (int)gs.Position.Y))
            //                button.Pressed = true;
            //            break;
            //    }
            //}

            // update it 
            button.Update(gameTime);

            // if the button state is "on" then
            if (button.Enabled)
            {
                // do nothing
            }
            else
            {
                //precise
                timer += gameTime.ElapsedGameTime;
            }
            
            time2.Append(timer);


            // Shows the by the second timer in a clean format
            // by second
            eTime = gameTime.TotalGameTime;

            // minutes
            if (eTime.Minutes < 10)                
                time.Append("Time - 0" + (int)(eTime.Minutes));
            
            else
                time.Append("Time - " + (int)(eTime.Minutes));
            
            //seconds
            if (eTime.Seconds < 10)
                time.Append(":0" + (int)(eTime.Seconds));
            
            else
                time.Append(":" + (int)(eTime.Seconds));


            // update all of the sprites
            foreach (Sprite sprite in sprites)
            {
                sprite.Update(gameTime, GraphicsDevice.Viewport.Bounds);
            }



            // TODO: Add your update logic here			
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Draw Bottom most layer
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();

            spriteBatch.DrawString(font, time.ToString(), timePosition, Color.Red);
            spriteBatch.DrawString(font, time2.ToString(), time2Position, Color.Red);

            button.Draw(spriteBatch);

            // draw all sprites first
            foreach (Sprite sprite in sprites)
            {
                sprite.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }


        #region Methods

        private void HandleTouchInput()
        {
            // we use raw touch points for selection, since they are more appropriate
            // for that use than gestures. so we need to get that raw touch data.
            TouchCollection touches = TouchPanel.GetState();

            // see if we have a new primary point down. when the first touch
            // goes down, we do hit detection to try and select one of our sprites.
            if (touches.Count > 0 && touches[0].State == TouchLocationState.Pressed)
            {
                // convert the touch position into a Point for hit testing
                Point touchPoint = new Point((int)touches[0].Position.X, (int)touches[0].Position.Y);

                // iterate our sprites to find which sprite is being touched. we iterate backwards
                // since that will cause sprites that are drawn on top to be selected before
                // sprites drawn on the bottom.
                selectedSprite = null;
                for (int i = sprites.Count - 1; i >= 0; i--)
                {
                    Sprite sprite = sprites[i];
                    if (sprite.HitBounds.Contains(touchPoint))
                    {
                        selectedSprite = sprite;
                        break;
                    }
                }

                if (selectedSprite != null)
                {
                    // make sure we stop selected sprites
                    selectedSprite.Velocity = Vector2.Zero;

                    // we also move the sprite to the end of the list so it
                    // draws on top of the other sprites
                    sprites.Remove(selectedSprite);
                    sprites.Add(selectedSprite);
                }
            }

            // next we handle all of the gestures. since we may have multiple gestures available,
            // we use a loop to read in all of the gestures. this is important to make sure the 
            // TouchPanel's queue doesn't get backed up with old data
            while (TouchPanel.IsGestureAvailable)
            {
                // read the next gesture from the queue
                GestureSample gesture = TouchPanel.ReadGesture();

                // we can use the type of gesture to determine our behavior
                switch (gesture.GestureType)
                {
                    // on taps, we change the color of the selected sprite
                    case GestureType.Tap:
                    case GestureType.DoubleTap:
                        if (selectedSprite != null)
                        {
                            selectedSprite.ChangeColor();
                        }
                        break;

                    // on holds, if no sprite is selected, we add a new sprite at the
                    // hold position and make it our selected sprite. otherwise we
                    // remove our selected sprite.
                    case GestureType.Hold:
                        if (selectedSprite == null)
                        {
                            // create the new sprite
                            selectedSprite = new Sprite(gear);
                            selectedSprite.Center = gesture.Position;

                            // add it to our list
                            sprites.Add(selectedSprite);
                        }
                        else
                        {
                            sprites.Remove(selectedSprite);
                            selectedSprite = null;
                        }
                        break;

                    // on drags, we just want to move the selected sprite with the drag
                    case GestureType.FreeDrag:
                        if (selectedSprite != null)
                        {
                            selectedSprite.Center += gesture.Delta;
                        }
                        break;

                    // on flicks, we want to update the selected sprite's velocity with
                    // the flick velocity, which is in pixels per second.
                    case GestureType.Flick:
                        if (selectedSprite != null)
                        {
                            selectedSprite.Velocity = gesture.Delta;
                        }
                        break;

                    // on pinches, we want to scale the selected sprite
                    case GestureType.Pinch:
                        if (selectedSprite != null)
                        {
                            // get the current and previous locations of the two fingers
                            Vector2 a = gesture.Position;
                            Vector2 aOld = gesture.Position - gesture.Delta;
                            Vector2 b = gesture.Position2;
                            Vector2 bOld = gesture.Position2 - gesture.Delta2;

                            // figure out the distance between the current and previous locations
                            float d = Vector2.Distance(a, b);
                            float dOld = Vector2.Distance(aOld, bOld);

                            // calculate the difference between the two and use that to alter the scale
                            float scaleChange = (d - dOld) * .01f;
                            selectedSprite.Scale += scaleChange;
                        }
                        break;
                }
            }

            // lastly, if there are no raw touch points, we make sure no sprites are selected.
            // this happens after we handle gestures because some gestures like taps and flicks
            // will come in on the same frame as our raw touch points report no touches and we
            // still want to use the selected sprite for those gestures.
            if (touches.Count == 0)
            {
                selectedSprite = null;
            }
        }

        #endregion
    }
}
