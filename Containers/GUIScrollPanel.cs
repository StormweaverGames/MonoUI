using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoUI
{
    /// <summary>
    /// Represents a scrollable GUI container
    /// <b>STILL A W.I.P</b>
    /// </summary>
    public class GUIScrollPanel : GUIContainer, IDisposable
    {
        protected ScrollBar _verticalScroll;
        protected MouseState _prevMouseState;

        protected bool _autoScroll;

        /// <summary>
        /// The render target for the internal components
        /// </summary>
        protected RenderTarget2D _internalTarget;
        /// <summary>
        /// The width of the scroll bar
        /// </summary>
        protected int _scrollSize = 8;
        /// <summary>
        /// The rectangle representing the scroll housing
        /// </summary>
        protected Rectangle _internalItemBounds;

        /// <summary>
        /// How far down this panel is scrolled
        /// </summary>
        protected float _scrollValue = 0.0f;
        /// <summary>
        /// Gets or sets whether this control should automatically scroll to the bottom
        /// </summary>
        public virtual bool AutoScroll
        {
            get { return _autoScroll; }
            set { _autoScroll = value; }
        }

        public override Rectangle Bounds
        {
            get
            {
                return base.Bounds;
            }
            set
            {
                base.Bounds = value;

                if (_internalItemBounds.IsEmpty)
                {
                    _internalItemBounds = value;
                    _internalItemBounds.Width -= _scrollSize;
                    _verticalScroll.TotalBounds = value.Height;
                }                

                _verticalScroll.Bounds = new Rectangle(Bounds.Width - _scrollSize, 0, _scrollSize, Bounds.Height);
                _verticalScroll.ShownBounds = value.Height;
            }
        }

        /// <summary>
        /// Creates a new GUI scroll panel
        /// </summary>
        /// <param name="graphics">The graphics device to bind to</param>
        /// <param name="parent">The parent container</param>
        public GUIScrollPanel(GraphicsDevice graphics, GUIContainer parent) : base(graphics, parent) 
        {
            _prevMouseState = Mouse.GetState();

            _verticalScroll = new ScrollBar(graphics, this);
            _verticalScroll.ScrollValueChanged += VerticalScrollValueChanged;
        }

        void VerticalScrollValueChanged(object sender, float scrollValue)
        {
            _scrollValue = scrollValue * (_internalItemBounds.Height - _bounds.Height);
            _spriteBatch.Begin();
            EndInvalidate();
        }

        public void ResizeInternal(int newWidth, int newHeight)
        {
            _internalItemBounds.Width = newWidth;
            _internalItemBounds.Height = newHeight;

            _verticalScroll.TotalBounds = newHeight;

            if (AutoScroll)
            {
                _verticalScroll.ScrollValue = 1.0f;
            }

            Invalidating = true;
        }

        public override void AddComponent(IGUI component)
        {
            component.BoundsChanged += ComponentBoundsChanged;
            base.AddComponent(component);
        }

        protected virtual void ComponentBoundsChanged(object sender, Rectangle newBounds)
        {
            if (newBounds.Bottom > _internalItemBounds.Bottom)
                ResizeInternal(_internalItemBounds.Width, newBounds.Bottom);
        }

        public override void Update()
        {
            MouseState mState = Mouse.GetState();

            int scrollChange = mState.ScrollWheelValue - _prevMouseState.ScrollWheelValue;

            if (scrollChange != 0 && Focused)
            {
                _verticalScroll.ScrollValue -= scrollChange / 1000.0f * _verticalScroll.Ratio;
            }

            _prevMouseState = mState;

            base.Update();
        }

        /// <summary>
        /// Begins invalidation on this control
        /// </summary>
        /// <returns></returns>
        protected override bool BeginInvalidate()
        {
            if (base.BeginInvalidate())
            {
                if (_internalTarget == null || _internalTarget.Bounds != _internalItemBounds)
                    _internalTarget = new RenderTarget2D(_graphics, _internalItemBounds.Width, _internalItemBounds.Height);

                _graphics.SetRenderTarget(_internalTarget);
                _graphics.Clear(_backColor);

                _effect.CurrentTechnique.Passes[0].Apply();
                return true;
            }
            else
            {
                Invalidating = false;
                return false;
            }
        }

        /// <summary>
        /// Called when this control invalidates
        /// </summary>
        protected override void Invalidate()
        {
            foreach (IGUI control in _controls)
            {
                if (control != _verticalScroll && control != null && control.Visible && control.Image != null)
                {
                    _spriteBatch.Draw(control.Image, control.Bounds, control.Enabled ? Color.White : DISABLED_COLOR);
                }
            }
        }

        /// <summary>
        /// Ends invalidation on this control
        /// </summary>
        protected override void EndInvalidate()
        {
            _spriteBatch.End();

            _graphics.SetRenderTarget(_renderTarget);
            _graphics.Clear(_backColor);

            _effect.CurrentTechnique.Passes[0].Apply();
            _graphics.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, _cornerVerts, 0, 4);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_internalTarget, new Vector2(0, -(int)_scrollValue), _colorMultiplier);
            _spriteBatch.Draw(_verticalScroll.Image, _verticalScroll.Bounds, Color.White);
            _spriteBatch.End();
            
            if (_parent != null)
                _parent.Invalidating = true;
        }

        public override void MouseDown(MouseEventArgs e)
        {
            if (_verticalScroll.ScreenBounds.Contains(e.Position))
                _verticalScroll.MouseDown(e);
            else
            {
                foreach(IGUI control in _controls)
                {
                    if (control != _verticalScroll && control.Bounds.Contains(e.Position - new Vector2(X, (int)(-_scrollValue * _internalItemBounds.Height))))
                    {
                        control.MouseDown(e);
                    }
                }
            }
        }
        
        /// <summary>
        /// Disposes of this object and free's it's resources
        /// </summary>
        new public void Dispose()
        {
            _renderTarget.Dispose();
            _internalTarget.Dispose();
        }

        /// <summary>
        /// Gets or sets the scroll value for this panel
        /// </summary>
        public float Scroll
        {
            get { return _verticalScroll.ScrollValue; }
            set { _verticalScroll.ScrollValue = value; }
        }
    }
}
