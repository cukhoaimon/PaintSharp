using System.Windows;

namespace IShape
{
    public abstract class IShape
    {
        public abstract string Name { get; }
        public List<Point> Points { get; set; } = new List<Point>();

        public abstract UIElement Draw();
        public abstract IShape Clone();
    }
}
