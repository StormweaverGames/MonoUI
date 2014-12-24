using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoUI
{
    public class ScrollBar : GUIElement
    {
        protected float _scrollValue = 0.0f;
        protected float _shownRatio = 1.0f;

        protected int _minScrollCentre;
        protected int _maxScrollCentre;

        protected int _scrollSize;
        protected int _scrollBarSize;
        protected int _scrollRange;

        protected Texture2D _headers;
        protected Texture2D _scroll;
        protected ScrollBarMode _mode;

        protected Rectangle _topHeader;
        protected Rectangle _bottomHeader;
        protected Rectangle _scrollRect;

        protected int _shownBounds = 1;
        protected int _totalBounds = 1;

        public event ScrollValueChangedEvent ScrollValueChanged;

        public int ShownBounds
        {
            get { return _shownBounds; }
            set
            {
                _shownBounds = value;
                _shownRatio = (float)_shownBounds / _totalBounds;
                Invalidating = true;
                CalcScrollBounds();
            }
        }
        public int TotalBounds
        {
            get { return _totalBounds; }
            set
            {
                _totalBounds = value;
                _shownRatio = (float)_shownBounds / _totalBounds;
                Invalidating = true;
                CalcScrollBounds();
            }
        }
        public float ScrollValue
        {
            get { return _scrollValue; }
            set
            {
                _scrollValue = value< 0 ? 0 : value > 1 ? 1 : value;
                CalcScrollBounds();
                Invalidating = true;

                if (ScrollValueChanged != null)
                    ScrollValueChanged.Invoke(this, ScrollValue);
            }
        }
        public float Ratio
        {
            get { return _shownRatio; }
        }

        public ScrollBar(GraphicsDevice graphics, GUIContainer parent) : base(graphics, parent)
        {
            _headers = StaticContentLoader.GetItem<Texture2D>("UpArrow");
            _scroll = StaticContentLoader.GetItem<Texture2D>("ScrollBar");
            _backColor = Color.White;
        }

        protected void CalcScrollBounds()
        {
            _scrollSize = (int)(_scrollBarSize * _shownRatio);
            _scrollSize = _scrollSize < 5 ? 5 : _scrollSize;

            _minScrollCentre = _bounds.Width + _scrollSize / 2;
            _maxScrollCentre = _bounds.Height - _bounds.Width - _scrollSize / 2;
            _scrollRange = _maxScrollCentre - _minScrollCentre;

            _scrollRect = new Rectangle(0, (int)(_minScrollCentre + _scrollRange * _scrollValue) - _scrollSize / 2, _bounds.Width, _scrollSize);
        }

        protected override void Resized()
        {
            base.Resized();
            _scrollBarSize = _bounds.Height - _bounds.Width * 2;


            _topHeader = new Rectangle(0, 0, _bounds.Width, _bounds.Width);
            _bottomHeader = new Rectangle(0, _bounds.Height - _bounds.Width, _bounds.Width, _bounds.Width);
            
            CalcScrollBounds();
        }

        public override void Update()
        {
            base.Update();
        }

        protected override void Invalidate()
        {
            _spriteBatch.Draw(_headers, _topHeader, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1.0f);
            _spriteBatch.Draw(_headers, _bottomHeader, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipVertically, 1.0f);
            _spriteBatch.Draw(_scroll, _scrollRect, Color.White);
        }

        public override void MouseDown(MouseEventArgs e)
        {
            base.MouseDown(e);

            if (_topHeader.Contains(e.Position))
            {
                _scrollValue -= 0.01f;
            }
            else if (_bottomHeader.Contains(e.Position))
            {
                _scrollValue += 0.01f;
            }
            else
            {
                _scrollValue = (e.Y - _bounds.Width) / (float)_scrollBarSize;
            }
            _scrollValue = MathHelper.Clamp(_scrollValue, 0, 1.0f);

            ScrollValue = _scrollValue;
        }

        public enum ScrollBarMode { Vertical, Horizontal };
    }

    public delegate void ScrollValueChangedEvent(object sender, float scrollValue);
}
