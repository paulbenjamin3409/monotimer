#region File Description
//-----------------------------------------------------------------------------
// Button.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoTimer.Content;

#endregion

namespace MonoTimer
{
    public class Button : IBase
    {
        public Vector2 Position { get; set; }

        public Rectangle Rectangle { get; set; }

        public SpriteFont Font { get; set; }
        public string Text { get; set; }

        public Color colorOne { get; set; }

        private Color colorTwo { get; set; }

        public bool Pressed { get; set; }

        public bool Enabled { get; set; }

        public Button(Rectangle rec, string text, SpriteFont font)
        {
            Rectangle = rec;
            Text = text;
            Font = font;
            Pressed = false;
            colorOne = Color.White;
            counter = 2;
        }
        public void LoadContent(ContentManager content)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 fontCenter = Font.MeasureString(Text) / 2;
            spriteBatch.DrawString(Font, Text, new Vector2(Rectangle.Center.X, Rectangle.Center.Y), colorOne, 0, fontCenter, 1f, SpriteEffects.None, 0);
        }
        bool isTimerOn = false;
        float timeSinceLastShot;
        private int counter { get; set; }

        public void Update(GameTime gameTime)
        {
            if (isTimerOn)
            {
                timeSinceLastShot += (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (timeSinceLastShot > .5f)
                {
                    isTimerOn = false;

                }
            }
            else
            {
                if (Pressed)
                {
                    Pressed = false;
                    isTimerOn = true;
                    colorOne = Color.Black;
                    // flip fop the enabled status when pressed
                    Enabled = (Enabled != true);
                }
                else
                {
                    colorOne = Color.White;
                }
            }




        }
    }
}