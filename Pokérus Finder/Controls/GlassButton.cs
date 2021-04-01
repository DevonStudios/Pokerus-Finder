using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FRLG_RSE_PID_to_Frame.Controls
{
    public partial class GlassButton : Button
    {
        bool hovered = false;

        public GlassButton()
        {
            InitializeComponent();
            timer.Interval = animationLength / framesCount;
            base.BackColor = Color.Transparent;
            BackColor = Color.Black;
            ForeColor = Color.White;
            OuterBorderColor = Color.Transparent;
            InnerBorderColor = Color.Black;
            ShineColor = Color.White;
            GlowColor = Color.FromArgb(-7488001); //unchecked((int)(0xFF8DBDFF)));
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Opaque, false);
        }

        #region " Fields and Properties "

        private Color _backColor;
        private bool _fadeOnFocus;
        private Color _glowColor;
        private Color _innerBorderColor;
        private bool _isMouseDown;
        private Color _outerBorderColor;
        private Color _shineColor;

        [DefaultValue(typeof(Color), "Black")]
        public new virtual Color BackColor
        {
            get { return _backColor; }
            set
            {
                if (!_backColor.Equals(value))
                {
                    _backColor = Color.AntiqueWhite;
                    UseVisualStyleBackColor = false;
                    CreateFrames();
                    OnBackColorChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(typeof(Color), "White")]
        public new virtual Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = Color.Black;
            }
        }

        [DefaultValue(typeof(Color), "Black"), Category("Appearance"), Description("The inner border color of the control.")]
        public virtual Color InnerBorderColor
        {
            get { return _innerBorderColor; }
            set
            {
                if (_innerBorderColor != value)
                {
                    _innerBorderColor = value;
                    CreateFrames();

                    if (IsHandleCreated)
                    {
                        Invalidate();
                    }
                    OnInnerBorderColorChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(typeof(Color), "White"), Category("Appearance"), Description("The outer border color of the control.")]
        public virtual Color OuterBorderColor
        {
            get { return _outerBorderColor; }
            set
            {
                _outerBorderColor = Color.Transparent;
                if (_outerBorderColor != value)
                {
                    //_outerBorderColor = value;
                    _outerBorderColor = Color.Transparent;
                    CreateFrames();
                    if (IsHandleCreated)
                    {
                        Invalidate();
                    }
                    OnOuterBorderColorChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(typeof(Color), "White"), Category("Appearance"), Description("The shine color of the control.")]
        public virtual Color ShineColor
        {
            get { return _shineColor; }
            set
            {
                if (_shineColor != value)
                {
                    //_shineColor = value;
                    _shineColor = SystemColors.Window;
                    CreateFrames();
                    if (IsHandleCreated)
                    {
                        Invalidate();
                    }
                    OnShineColorChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(typeof(Color), "255,141,189,255"), Category("Appearance"), Description("The glow color of the control.")]
        public virtual Color GlowColor
        {
            get { return _glowColor; }
            set
            {
                if (_glowColor != value)
                {
                    _glowColor = value;
                    CreateFrames();
                    if (IsHandleCreated)
                    {
                        Invalidate();
                    }
                    OnGlowColorChanged(EventArgs.Empty);
                }
            }
        }

        [Browsable(true)]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        [DefaultValue(false), Category("Appearance"), Description("Indicates whether the button should fade in and fade out when it is getting and loosing the focus.")]
        public virtual bool FadeOnFocus
        {
            get { return _fadeOnFocus; }
            set
            {
                if (_fadeOnFocus != value)
                {
                    _fadeOnFocus = value;
                }
            }
        }

        private bool IsPressed
        {
            get { return (_isMouseDown); }
        }

        [Browsable(false)]
        public PushButtonState State
        {
            get
            {
                if (!Enabled)
                {
                    return PushButtonState.Disabled;
                }
                if (IsPressed)
                {
                    return PushButtonState.Pressed;
                }
                if (IsDefault)
                {
                    return PushButtonState.Default;
                }
                return PushButtonState.Normal;
            }
        }

        #endregion

        #region " Events "

        [Description("Event raised when the value of the InnerBorderColor property is changed."), Category("Property Changed")]
        public event EventHandler InnerBorderColorChanged;

        protected virtual void OnInnerBorderColorChanged(EventArgs e)
        {
            if (InnerBorderColorChanged != null)
            {
                InnerBorderColorChanged(this, e);
            }
        }

        [Description("Event raised when the value of the OuterBorderColor property is changed."), Category("Property Changed")]
        public event EventHandler OuterBorderColorChanged;

        protected virtual void OnOuterBorderColorChanged(EventArgs e)
        {
            if (OuterBorderColorChanged != null)
            {
                OuterBorderColorChanged(this, e);
            }
        }

        [Description("Event raised when the value of the ShineColor property is changed."), Category("Property Changed")]
        public event EventHandler ShineColorChanged;

        protected virtual void OnShineColorChanged(EventArgs e)
        {
            if (ShineColorChanged != null)
            {
                ShineColorChanged(this, e);
            }
        }

        [Description("Event raised when the value of the GlowColor property is changed."), Category("Property Changed")]
        public event EventHandler GlowColorChanged;

        protected virtual void OnGlowColorChanged(EventArgs e)
        {
            if (GlowColorChanged != null)
            {
                GlowColorChanged(this, e);
            }
        }

        #endregion

        protected override bool ShowFocusCues
        {
            get { return false; }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x201 || m.Msg == 0x203)  //MouseDown || DoubleClick
            {
                _isMouseDown = true;
            }
            else if (m.Msg == 0x202)  //MouseUp
            {
                _isMouseDown = false;
            }
            else if (m.Msg == 0x7)  //Enter
            {
                FadeIn();
            }
            else if (m.Msg == 0x8 && hovered == false)  //Leave
            {
                FadeOut();
            }
            else if (m.Msg == 0x2A1)  //MouseHover
            {
                hovered = true;
                FadeIn();
            }
            else if (m.Msg == 0x2A3)  //MouseLeave
            {
                if (!ContainsFocus)
                {
                    FadeOut();
                }
                hovered = false;
            }
            base.WndProc(ref m);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            CreateFrames();
            base.OnSizeChanged(e);
        }

        #region " Painting "

        private Button _imageButton;

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawButtonBackgroundFromBuffer(e.Graphics);
            DrawForegroundFromButton(e);
            DrawButtonForeground(e.Graphics);

            if (Paint != null)
            {
                Paint(this, e);
            }
        }

        public new event PaintEventHandler Paint;

        private void DrawButtonBackgroundFromBuffer(Graphics graphics)
        {
            int frame;
            if (!Enabled)
            {
                frame = FRAME_DISABLED;
            }
            else if (IsPressed)
            {
                frame = FRAME_PRESSED;
            }
            else if (!IsAnimating && _currentFrame == 0)
            {
                frame = FRAME_NORMAL;
            }
            else
            {
                if (!HasAnimationFrames)
                {
                    CreateFrames(true);
                }
                frame = FRAME_ANIMATED + _currentFrame;
            }
            if (_frames == null || _frames.Count == 0)
            {
                CreateFrames();
            }
            graphics.DrawImage(_frames[frame], Point.Empty);
        }

        private Image CreateBackgroundFrame(bool pressed, bool hovered, bool animating, bool enabled, float glowOpacity)
        {
            Rectangle rect = ClientRectangle;
            if (rect.Width <= 0)
            {
                rect.Width = 1;
            }
            if (rect.Height <= 0)
            {
                rect.Height = 1;
            }
            Image img = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.Clear(Color.Transparent);
                DrawButtonBackground(g, rect, pressed, hovered, animating, enabled, _outerBorderColor, _backColor, _glowColor, _shineColor, _innerBorderColor, glowOpacity);
            }
            return img;
        }

        private static void DrawButtonBackground(Graphics g, Rectangle rectangle, bool pressed, bool hovered, bool animating, bool enabled, Color outerBorderColor, Color backColor, Color glowColor,
                                                 Color shineColor, Color innerBorderColor, float glowOpacity)
        {
            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = rectangle;
            rect.Width--;
            rect.Height--;
            using (GraphicsPath bw = CreateRoundRectangle(rect, 4))
            {
                using (var p = new Pen(outerBorderColor))
                {
                    g.DrawPath(p, bw);
                }
            }
            rect.X++;
            rect.Y++;
            rect.Width -= 2;
            rect.Height -= 2;
            Rectangle rect2 = rect;
            rect2.Height >>= 1;

            using (GraphicsPath bb = CreateRoundRectangle(rect, 2))
            {
                int opacity = pressed ? 0xcc : 0x7f;
                using (Brush br = new SolidBrush(Color.FromArgb(opacity, backColor)))
                {
                    g.FillPath(br, bb);
                }
            }

            if ((hovered || animating) && !pressed)
            {
                using (GraphicsPath clip = CreateRoundRectangle(rect, 2))
                {
                    g.SetClip(clip, CombineMode.Intersect);
                    using (GraphicsPath brad = CreateBottomRadialPath(rect))
                    {
                        using (var pgr = new PathGradientBrush(brad))
                        {
                            var opacity = (int)(0xB2 * glowOpacity + .5f);
                            RectangleF bounds = brad.GetBounds();
                            pgr.CenterPoint = new PointF((bounds.Left + bounds.Right) / 2f, (bounds.Top + bounds.Bottom) / 2f);
                            pgr.CenterColor = Color.FromArgb(opacity, glowColor);
                            pgr.SurroundColors = new[] { Color.FromArgb(0, glowColor) };
                            g.FillPath(pgr, brad);
                        }
                    }
                    g.ResetClip();
                }
            }

            if (rect2.Width > 0 && rect2.Height > 0)
            {
                rect2.Height++;
                using (GraphicsPath bh = CreateTopRoundRectangle(rect2, 2))
                {
                    rect2.Height++;
                    int opacity = 0x99;
                    if (pressed | !enabled)
                    {
                        opacity = (int)(.4f * opacity + .5f);
                    }
                    using (var br = new LinearGradientBrush(rect2, Color.FromArgb(opacity, shineColor), Color.FromArgb(opacity / 3, shineColor), LinearGradientMode.Vertical))
                    {
                        g.FillPath(br, bh);
                    }
                }
                rect2.Height -= 2;
            }

            using (GraphicsPath bb = CreateRoundRectangle(rect, 3))
            {
                using (var p = new Pen(innerBorderColor))
                {
                    g.DrawPath(p, bb);
                }
            }
            g.SmoothingMode = sm;
        }

        private void DrawButtonForeground(Graphics g)
        {
            if (Focused && ShowFocusCues)
            {
                Rectangle rect = ClientRectangle;
                rect.Inflate(-4, -4);
                ControlPaint.DrawFocusRectangle(g, rect);
            }
        }

        private void DrawForegroundFromButton(PaintEventArgs pevent)
        {
            if (_imageButton == null)
            {
                _imageButton = new Button();
                _imageButton.Parent = new TransparentControl();
                _imageButton.SuspendLayout();
                _imageButton.BackColor = Color.Transparent;
                _imageButton.FlatAppearance.BorderSize = 0;
                _imageButton.FlatStyle = FlatStyle.Flat;
            }
            else
            {
                _imageButton.SuspendLayout();
            }
            _imageButton.AutoEllipsis = AutoEllipsis;

            if (Enabled)
            {
                _imageButton.ForeColor = ForeColor;
            }
            else
            {
                _imageButton.ForeColor = Color.FromArgb((3 * ForeColor.R + _backColor.R) >> 2, (3 * ForeColor.G + _backColor.G) >> 2, (3 * ForeColor.B + _backColor.B) >> 2);
            }
            _imageButton.Font = Font;
            _imageButton.RightToLeft = RightToLeft;
            _imageButton.Image = Image;

            if (Image != null && !Enabled)
            {
                Size size = Image.Size;
                var newColorMatrix = new float[5][];
                newColorMatrix[0] = new[] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f };
                newColorMatrix[1] = new[] { 0.2577f, 0.2577f, 0.2577f, 0f, 0f };
                newColorMatrix[2] = new[] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f };
                var arr = new float[5];
                arr[3] = 1f;
                newColorMatrix[3] = arr;
                newColorMatrix[4] = new[] { 0.38f, 0.38f, 0.38f, 0f, 1f };
                var matrix = new ColorMatrix(newColorMatrix);
                var disabledImageAttr = new ImageAttributes();
                disabledImageAttr.ClearColorKey();
                disabledImageAttr.SetColorMatrix(matrix);
                _imageButton.Image = new Bitmap(Image.Width, Image.Height);

                using (Graphics gr = Graphics.FromImage(_imageButton.Image))
                {
                    gr.DrawImage(Image, new Rectangle(0, 0, size.Width, size.Height), 0, 0, size.Width, size.Height, GraphicsUnit.Pixel, disabledImageAttr);
                }
            }
            _imageButton.ImageAlign = ImageAlign;
            _imageButton.ImageIndex = ImageIndex;
            _imageButton.ImageKey = ImageKey;
            _imageButton.ImageList = ImageList;
            _imageButton.Padding = Padding;
            _imageButton.Size = Size;
            _imageButton.Text = Text;
            _imageButton.TextAlign = TextAlign;
            _imageButton.TextImageRelation = TextImageRelation;
            _imageButton.UseCompatibleTextRendering = UseCompatibleTextRendering;
            _imageButton.UseMnemonic = UseMnemonic;
            _imageButton.ResumeLayout();
            InvokePaint(_imageButton, pevent);

            if (_imageButton.Image != null && _imageButton.Image != Image)
            {
                _imageButton.Image.Dispose();
                _imageButton.Image = null;
            }
        }

        private static GraphicsPath CreateRoundRectangle(Rectangle rectangle, int radius)
        {
            var path = new GraphicsPath();
            int l = rectangle.Left;
            int t = rectangle.Top;
            int w = rectangle.Width;
            int h = rectangle.Height;
            int d = radius << 1;
            path.AddArc(l, t, d, d, 180, 90); // topleft
            path.AddLine(l + radius, t, l + w - radius, t); // top
            path.AddArc(l + w - d, t, d, d, 270, 90); // topright
            path.AddLine(l + w, t + radius, l + w, t + h - radius); // right
            path.AddArc(l + w - d, t + h - d, d, d, 0, 90); // bottomright
            path.AddLine(l + w - radius, t + h, l + radius, t + h); // bottom
            path.AddArc(l, t + h - d, d, d, 90, 90); // bottomleft
            path.AddLine(l, t + h - radius, l, t + radius); // left
            path.CloseFigure();
            return path;
        }

        private static GraphicsPath CreateTopRoundRectangle(Rectangle rectangle, int radius)
        {
            var path = new GraphicsPath();
            int l = rectangle.Left;
            int t = rectangle.Top;
            int w = rectangle.Width;
            int h = rectangle.Height;
            int d = radius << 1;
            path.AddArc(l, t, d, d, 180, 90); // topleft
            path.AddLine(l + radius, t, l + w - radius, t); // top
            path.AddArc(l + w - d, t, d, d, 270, 90); // topright
            path.AddLine(l + w, t + radius, l + w, t + h); // right
            path.AddLine(l + w, t + h, l, t + h); // bottom
            path.AddLine(l, t + h, l, t + radius); // left
            path.CloseFigure();
            return path;
        }

        private static GraphicsPath CreateBottomRadialPath(Rectangle rectangle)
        {
            var path = new GraphicsPath();
            RectangleF rect = rectangle;
            rect.X -= rect.Width * .35f;
            rect.Y -= rect.Height * .15f;
            rect.Width *= 1.7f;
            rect.Height *= 2.3f;
            path.AddEllipse(rect);
            path.CloseFigure();
            return path;
        }

        private class TransparentControl : Control
        {
            protected override void OnPaintBackground(PaintEventArgs pevent)
            { }

            protected override void OnPaint(PaintEventArgs e)
            { }
        }

        #endregion

        #region " Animation Support "

        private const int FRAME_DISABLED = 0;
        private const int FRAME_PRESSED = 1;
        private const int FRAME_NORMAL = 2;
        private const int FRAME_ANIMATED = 3;

        private const int animationLength = 10;
        private const int framesCount = 10;
        private int _currentFrame;
        private int _direction;
        private List<Image> _frames;

        private bool HasAnimationFrames
        {
            get { return _frames != null && _frames.Count > FRAME_ANIMATED; }
        }

        private bool IsAnimating
        {
            get { return _direction != 0; }
        }

        private void CreateFrames()
        {
            CreateFrames(false);
        }

        private void CreateFrames(bool withAnimationFrames)
        {
            DestroyFrames();

            if (!IsHandleCreated)
            {
                return;
            }
            if (_frames == null)
            {
                _frames = new List<Image>();
            }
            _frames.Add(CreateBackgroundFrame(false, false, false, false, 0));
            _frames.Add(CreateBackgroundFrame(true, true, false, true, 0));
            _frames.Add(CreateBackgroundFrame(false, false, false, true, 0));

            if (!withAnimationFrames)
            {
                return;
            }
            for (int i = 0; i < framesCount; i++)
            {
                _frames.Add(CreateBackgroundFrame(false, true, true, true, i / (framesCount - 1F)));
            }
        }

        private void DestroyFrames()
        {
            if (_frames != null)
            {
                while (_frames.Count > 0)
                {
                    _frames[_frames.Count - 1].Dispose();
                    _frames.RemoveAt(_frames.Count - 1);
                }
            }
        }

        private void FadeIn()
        {
            _direction = 1;
            timer.Enabled = true;
        }

        private void FadeOut()
        {
            _direction = -1;
            timer.Enabled = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!timer.Enabled)
            {
                return;
            }
            Refresh();
            _currentFrame += _direction;

            if (_currentFrame == -1)
            {
                _currentFrame = 0;
                timer.Enabled = false;
                _direction = 0;
                return;
            }
            if (_currentFrame == framesCount)
            {
                _currentFrame = framesCount - 1;
                timer.Enabled = false;
                _direction = 0;
            }
        }
        #endregion
    }
}