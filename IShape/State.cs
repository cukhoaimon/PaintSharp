using ProtoBuf;
using System.Windows;
using System.Windows.Media;

namespace US_IShape
{
    public class State : ICloneable
    {
        public bool IsDrawing {  get; set; }
        public Point StartPoint {  get; set; }
        public Point EndPoint { get; set; }
        
        public DoubleCollection? StrokeDashArray { get; set; }
        
        public SolidColorBrush Stroke { get; set; }
        public SolidColorBrush Fill { get; set; }
        public double StrokeThickness { get; set; }
        public string ShapeChoice { get; set; }
        public List<IShape> Shapes { get; set; }
        public State() {
            IsDrawing = false;
            Shapes = [];
            ShapeChoice = String.Empty;
            Stroke = Brushes.Black;
            Fill = null;
            StrokeThickness = 1;
        }

        public object Clone()
        {
            return (State) MemberwiseClone();
        }
    }
}
