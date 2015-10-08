using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SampleUI.Controls
{
    public class CaptionButton : Button
    {
        ////鼠标移到上面依赖项属性
        //public static readonly DependencyProperty MoverBrushProperty = DependencyProperty.Register(
        //    "MoverBrush", typeof(Brush), typeof(CaptionButton), new PropertyMetadata(null));

        //public Brush MoverBrush
        //{
        //    get { return GetValue(MoverBrushProperty) as Brush; }
        //    set { SetValue(MoverBrushProperty, value); }
        //}

        ////鼠标按下依赖项属性
        //public static readonly DependencyProperty EnterBrushProperty = DependencyProperty.Register(
        //    "EnterBrush", typeof (Brush), typeof (CaptionButton), new PropertyMetadata(null));

        //public Brush EnterBrush
        //{
        //    get { return GetValue(EnterBrushProperty) as Brush; }
        //    set { SetValue(EnterBrushProperty, value); }
        //}

        //static CaptionButton()
        //{
        //    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CaptionButton), new FrameworkPropertyMetadata(typeof(CaptionButton)));
        //}

        //public CaptionButton()
        //{
        //    base.Content = "";
        //    base.Background = Brushes.Orange;
        //}

        public static readonly DependencyProperty MoverBrushProperty;
		public static readonly DependencyProperty EnterBrushProperty;
		public Brush MoverBrush
		{
			get
			{
                return base.GetValue(MoverBrushProperty) as Brush;
			}
			set
			{
                base.SetValue(MoverBrushProperty, value);
			}
		}
		public Brush EnterBrush
		{
			get
			{
                return base.GetValue(EnterBrushProperty) as Brush;
			}
			set
			{
                base.SetValue(EnterBrushProperty, value);
			}
		}
		static CaptionButton()
		{
            MoverBrushProperty = DependencyProperty.Register("MoverBrush", typeof(Brush), typeof(CaptionButton), new PropertyMetadata(null));
            EnterBrushProperty = DependencyProperty.Register("EnterBrush", typeof(Brush), typeof(CaptionButton), new PropertyMetadata(null));
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CaptionButton), new FrameworkPropertyMetadata(typeof(CaptionButton)));
		}
        public CaptionButton()
		{
			base.Content = "";
			base.Background = Brushes.Orange;
		}
    }
}
