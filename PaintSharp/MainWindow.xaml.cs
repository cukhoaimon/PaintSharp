using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using US.IShape;

namespace PaintSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        ShapeFactory _factory = new();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isDrawing = true;
            _start = e.GetPosition(drawingCanvas);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                _end = e.GetPosition(drawingCanvas);

                Title = $"{_start.X}, {_start.Y} => {_end.X}, {_end.Y}";

                IShape preview = _factory.Create(_choice);
                preview.Points.Add(_start);
                preview.Points.Add(_end);

                drawingCanvas.Children.Clear();

                foreach (var shape in _shapes)
                {
                    drawingCanvas.Children.Add(shape.Draw());
                }

                drawingCanvas.Children.Add(preview.Draw());
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IShape shape = _factory.Create(_choice);
            shape.Points.Add(_start);
            shape.Points.Add(_end);

            _shapes.Add(shape);

            isDrawing = false;
        }

        bool isDrawing = false;
        Point _start;
        Point _end;
        string _choice; // Line
        List<IShape> _shapes = [];
        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var abilities = new List<IShape>();

            // Do tim cac kha nang
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            var fis = (new DirectoryInfo(folder)).GetFiles("*.dll");

            foreach (var fi in fis)
            {
                var assembly = Assembly.LoadFrom(fi.FullName);
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (type.IsClass & (!type.IsAbstract))
                    {
                        if (typeof(IShape).IsAssignableFrom(type))
                        {
                            var shape = Activator.CreateInstance(type) as IShape;
                            abilities.Add(shape!);
                        }
                    }
                }
            }

            _factory = new ShapeFactory();
            foreach (var ability in abilities)
            {
                _factory.Prototypes.Add(
                    ability.Name, ability
                );
                var btn = new Fluent.Button
                {
                    Header = ability.Name,
                    Tag = ability.Name

                };
                btn.Click += (sender, args) =>
                {
                    var control = (Button)sender;
                    _choice = (string)control.Tag;
                };
                
                shapeAction.Items.Add(btn);
            };

            if (abilities.Count > 0)
            {
                _choice = abilities[0].Name;
            }
        }
    }
}