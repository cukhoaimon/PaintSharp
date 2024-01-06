using System.Windows;
using System.Windows.Media.Imaging;

using ProtoBuf;


namespace US_IShape
{
    public abstract class IShape
    {
        public State? Configuration { get; set; }
        public abstract string Name { get; }
        public List<Point> Points { get; set; } = [];
        public abstract UIElement Draw();
        public abstract IShape Clone();
        public BitmapImage? Preview { get; set; }
    }
}
