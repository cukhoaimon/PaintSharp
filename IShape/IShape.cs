using System.Windows;

namespace US_IShape
{
    public abstract class IShape
    {
        public State? Configuration { get; set; }
        public abstract string Name { get; }
        public List<Point> Points { get; set; } = [];
        public abstract UIElement Draw();
        public abstract IShape Clone();
    }
}
