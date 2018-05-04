using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace MonoTimer.Content.Button
{
    /// <summary>
    /// Roughly and some taken from
    /// https://github.com/CartBlanche/MonoGame-Samples/blob/master/Graphics3DSample/Buttons/Clickable.cs
    /// </summary>
    public class Clickbase : DrawableGameComponent
    {
        readonly Rectangle _rectangle;
        private bool _isTouching;
        private bool _wasTouching;

        protected Rectangle Rectangle
        {
            get { return _rectangle; }

        }

        #region Properties

        public bool IsTouching
        {
            get { return _isTouching; }
        }

        public bool IsClicked
        {
            get { return (_wasTouching == true) && (_isTouching == false); }
        }

        #endregion

        public Clickbase(Game game, Rectangle targetRectangle) : base(game)
        {
            _rectangle = targetRectangle;
        }

        protected void HandleInput()
        {
            _wasTouching = _isTouching;
            _isTouching = false;

            TouchCollection touches = TouchPanel.GetState();

            if (touches.Count > 0)
            {
                var touch = touches[0];
                var position = touch.Position;


                Rectangle touchRect = new Rectangle((int)touch.Position.X - 5, (int)touch.Position.Y - 5,
                    10, 10);

                if (_rectangle.Intersects(touchRect))
                    _isTouching = true;
            }

        }
    }
}
