using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using US_IShape;


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

        State state = new();
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            state.IsDrawing = true;
            state.StartPoint = e.GetPosition(drawingCanvas);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (state.IsDrawing)
            {
                state.EndPoint = e.GetPosition(drawingCanvas);

                IShape preview = _factory.Create(state.ShapeChoice);
                preview.Points.Add(state.StartPoint);
                preview.Points.Add(state.EndPoint);
                preview.Configuration = (State)state.Clone();

                drawingCanvas.Children.Clear();

                foreach (var shape in state.Shapes)
                {
                    drawingCanvas.Children.Add(shape.Draw());
                }

                drawingCanvas.Children.Add(preview.Draw());
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IShape shape = _factory.Create(state.ShapeChoice);
            shape.Points.Add(state.StartPoint);
            shape.Points.Add(state.EndPoint);
            shape.Configuration = (State)state.Clone();

            state.Shapes.Add(shape);
            state.IsDrawing = false;
        }

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
                _factory.Prototypes.Add(ability.Name, ability);
                
                var button = new Fluent.Button
                {
                    Header = ability.Name,
                    Tag = ability.Name
                };

                button.Click += (sender, args) =>
                {
                    var control = (Button)sender;
                    state.ShapeChoice = (string)control.Tag;
                };
                
                shapeAction.Items.Add(button);
            };

            if (abilities.Count > 0)
            {
                state.ShapeChoice = abilities[0].Name;
            }
        }

        private void SetLine(object sender, RoutedEventArgs e)
        {
            state.Fill = null;
            state.StrokeDashArray = null;
        }

        private void SetDashes(object sender, RoutedEventArgs e)
        {
            state.Fill = null;
            state.StrokeDashArray = [1.5];
        }

        private void NewPaint(object sender, RoutedEventArgs e)
        {
            drawingCanvas.Children.Clear();
            state.Shapes.Clear();
        }

        private void SaveImage(object sender, RoutedEventArgs e)
        {

        }

        private void FillMode(object sender, RoutedEventArgs e)
        {
            state.Fill = state.Stroke;
        }

        private void ChangeBrushSize(object sender, SelectionChangedEventArgs e)
        {
            var control = sender as ComboBox;
            var index = control?.SelectedIndex;

            state.StrokeThickness = (double)(index == null ? 1 : index + 1);
        }
    }
}