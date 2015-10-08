using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using GlowingWindow;
using GlowingWindow.Microsoft.Windows.Shell;

namespace GlowingWindow
{
    internal enum Side
    {
        Top,
        Left,
        Bottom,
        Right
    }

    internal class SideGlow : Window
    {
        #region [private] objects

        private List<Line> _lines = new List<Line>();
        private Canvas _canvas = new Canvas();
        private WindowChrome _winChrome;
        private Side _side;
        private WindowInteropHelper _interopHelper;
        private Color _color = Colors.Gray;
        private List<byte> _alphas = new List<byte>();

        #endregion

        #region [internal] properties

        internal IntPtr Handle
        {
            get
            {
                return _interopHelper.Handle;
            }
        }

        internal Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                UpdateLinesColor();
            }
        }

        #endregion

        #region [public] properties

        public const int SIZE = 10;

        #endregion

        public SideGlow(Side side)
        {
            _side = side;
            _canvas.Background = Brushes.Transparent;
            Content = _canvas;
            Width = Height = 0;
            _interopHelper = new WindowInteropHelper(this);
            WindowStyle = System.Windows.WindowStyle.None;
            AllowsTransparency = true;
            Background = Brushes.Transparent;
            ShowInTaskbar = false;

            InitializeAlphas();
            InitializeLines();
            UpdateLinesColor();

            _winChrome = new WindowChrome
            {
                ResizeBorderThickness = new Thickness(0),
                CornerRadius = new CornerRadius(0),
                GlassFrameThickness = new Thickness(0),
                UseAeroCaptionButtons = false,
                CaptionHeight = 0
            };
            WindowChrome.SetWindowChrome(this, _winChrome);

            if (_side == Side.Top || _side == Side.Bottom)
            {
                Height = SIZE;
            }
            else
            {
                Width = SIZE;
            }
        }

        private void InitializeAlphas()
        {
            _alphas.Clear();
            _alphas.Add(85);
            _alphas.Add(64);
            _alphas.Add(46);
            _alphas.Add(25);
            _alphas.Add(19);
            _alphas.Add(10);
            _alphas.Add(07);
            _alphas.Add(02);
            _alphas.Add(01);
            _alphas.Add(00);
        }

        private void InitializeLines()
        {
            _canvas.Children.Clear();

            for (int i = 0; i < SIZE; i++)
            {
                Line l = new Line();
                l.StrokeThickness = 1.0;
                _lines.Add(l);
                _canvas.Children.Add(l);
            }
        }

        private void UpdateLinesColor()
        {
            for (int i = 0; i < SIZE; i++)
            {
                _lines[i].Stroke = new SolidColorBrush(Color.FromArgb(_alphas[i], _color.R, _color.G, _color.B));
            }
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            // 0 --> darker
            // 8 --> lighter
            Size retVal = base.ArrangeOverride(arrangeBounds);

            switch (_side)
            {
                case Side.Top:
                    for (int l = 0; l < SIZE; l++)
                    {
                        _lines[l].X1 = 0 + SIZE - l;
                        _lines[l].Y1 = SIZE - l;
                        _lines[l].X2 = arrangeBounds.Width - SIZE + l;
                        _lines[l].Y2 = SIZE - l;
                    }
                    break;
                case Side.Bottom:
                    for (int l = 0; l < SIZE; l++)
                    {
                        _lines[l].X1 = 0 + SIZE - l;
                        _lines[l].Y1 = l;
                        _lines[l].X2 = arrangeBounds.Width - SIZE + l;
                        _lines[l].Y2 = l;
                    }
                    break;
                case Side.Left:
                    for (int l = 0; l < SIZE; l++)
                    {
                        _lines[l].X1 = SIZE - l;
                        _lines[l].Y1 = SIZE - l;
                        _lines[l].X2 = SIZE - l;
                        _lines[l].Y2 = arrangeBounds.Height - SIZE + l;
                    }
                    break;
                case Side.Right:
                    for (int l = 0; l < SIZE; l++)
                    {
                        _lines[l].X1 = l;
                        _lines[l].Y1 = SIZE - l;
                        _lines[l].X2 = l;
                        _lines[l].Y2 = arrangeBounds.Height - SIZE + l;
                    }
                    break;
            }

            return retVal;
        }

        internal void SetSize(int val)
        {
            if (_side == Side.Top || _side == Side.Bottom)
            {
                Width = val + SIZE * 2;
            }
            else
            {
                Height = val + SIZE * 2;
            }
        }

        internal void SetLocation(WindowsAPI.WINDOWPOS pos)
        {
            switch (_side)
            {
                case Side.Top:
                    Left = pos.x - SideGlow.SIZE;
                    Top = pos.y - SideGlow.SIZE;
                    break;
                case Side.Bottom:
                    Left = pos.x - SideGlow.SIZE;
                    Top = pos.y + pos.cy;
                    break;
                case Side.Left:
                    Left = pos.x - SideGlow.SIZE;
                    Top = pos.y - SideGlow.SIZE;
                    break;
                case Side.Right:
                    Left = pos.x + pos.cx;
                    Top = pos.y - SideGlow.SIZE;
                    break;
            }
        }
    }
}
