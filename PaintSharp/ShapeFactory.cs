using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using US.IShape;

namespace PaintSharp
{
    public class ShapeFactory
    {
        public Dictionary<string, IShape> Prototypes = [];

        public IShape Create(String choice)
        {
            IShape shape = Prototypes[choice].Clone();
            return shape;
        }
    }
}
