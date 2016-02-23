using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SampleUI.Controls
{
    public class CaptionButton : Button
    {
        static CaptionButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CaptionButton), new FrameworkPropertyMetadata(typeof(CaptionButton)));
        }

        public static readonly DependencyProperty MoverBrushProperty =
            DependencyProperty.Register("MoverBrush", typeof(Brush), typeof(CaptionButton), new PropertyMetadata(null));

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

        public static readonly DependencyProperty EnterBrushProperty = 
            DependencyProperty.Register("EnterBrush", typeof(Brush), typeof(CaptionButton), new PropertyMetadata(null));

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
    }
}
