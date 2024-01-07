using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using US_IShape;

namespace PaintSharp
{
    public sealed class ShapeFactory
    {
        private static ShapeFactory? _instance;
        private ShapeFactory() { }
        public static ShapeFactory GetInstance()
        {
            _instance ??= new ShapeFactory();
            return _instance;
        }

        public Dictionary<string, IShape> Prototypes = [];

        public IShape Create(String choice)
        {
            IShape shape = Prototypes[choice].Clone();
            return shape;
        }
    }
}
