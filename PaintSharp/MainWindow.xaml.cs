using PaintSharp.Serialize;
using System.Configuration;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using US_IShape;

using SystemRectangle = System.Windows.Shapes.Rectangle;

namespace PaintSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        ShapeFactory _factory = ShapeFactory.GetInstance();
        State state = new();
        List<SolidColorBrush> _corlorList = [];

        readonly Encoding encoding = Encoding.UTF8;
        readonly string Seperator = "\r\n";
        readonly string Signature = "cukhoaimon";
        readonly int SignatureLength = 10;
        readonly string LAST_SESSION_DIR = AppDomain.CurrentDomain.BaseDirectory + @"\~LastSession";
        readonly int SIZE_LENGTH = 6;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            state.IsDrawing = true;
            state.StartPoint = e.GetPosition(drawingCanvas);

            // add a temp element
            drawingCanvas.Children.Add(new UIElement());
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

                // for avoid filckering
                drawingCanvas.Children.RemoveAt(drawingCanvas.Children.Count - 1);
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

            drawingCanvas.Children.Add(shape.Draw());
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

            foreach (var ability in abilities)
            {
                _factory.Prototypes.Add(ability.Name, ability);

                var button = new Fluent.Button
                {
                    Header = ability.Name,
                    Tag = ability.Name,
                    Icon = ability.Preview,
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

            //get all possible color
            PropertyInfo[] colors = typeof(Brushes).GetProperties();
            var listTring = colors.Select(color => color.Name).ToList();

            _corlorList = listTring.Select(color =>
            {
                Color c = (Color)ColorConverter.ConvertFromString(color);
                return new SolidColorBrush(c);
            }).ToList();

            ColorListView.ItemsSource = _corlorList;

            // LOAD THE PREVIOUS SESSION
            FileStream stream;
            try
            {
                stream = new(path: LAST_SESSION_DIR, mode: FileMode.Open, access: FileAccess.Read, share: FileShare.Read);
            }
            catch { return; }

            var buffer = new byte[SignatureLength];
            int byteRead = stream.Read(buffer, 0, SignatureLength);

            string _signature = encoding.GetString(buffer);

            if (_signature != Signature) return;
            
            stream.Position += 2; // flush "/r/n"

            buffer = new byte[SIZE_LENGTH];
            stream.Read(buffer, 0, SIZE_LENGTH);

            while (encoding.GetString(buffer.SkipLast(4).ToArray()) != Seperator)
            {
                var intSize = Int16.Parse(encoding.GetString(buffer));
                var byteData = new byte[intSize];

                stream.Read(byteData, 0, intSize);

                var stringData = encoding.GetString(byteData);

                var shapeFound = ShapeSerializer.Deserialize(stringData);
                state.Shapes.Add(shapeFound);
                stream.Position += 2; // flush "/r/n"

                stream.Read(buffer, 0, SIZE_LENGTH);
            }
            stream.Close();

            foreach (var shape in state.Shapes)
            {
                drawingCanvas.Children.Add(shape.Draw());
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
            TakeScreenShot(drawingCanvas);
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

        private void SetCorlor(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var selectedRectangle = btn?.Content as SystemRectangle;
            var selectedColor = selectedRectangle?.Fill;

                
            if (selectedColor == null) return;

            if (state.Fill != null) state.Fill = (SolidColorBrush)selectedColor;

            state.Stroke = (SolidColorBrush)selectedColor;
        }
        private void TakeScreenShot(object control)
        {
            FrameworkElement element = control as FrameworkElement;
            if (element == null)
                throw new Exception("Invalid parameter");

            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)element.ActualWidth , (int)element.ActualHeight + 120 , 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(element);
            PngBitmapEncoder pngImage = new();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            _ = dialog.ShowDialog();

            var selectedPath = dialog.SelectedPath + @"\image.png";

            using (Stream fileStream = File.Create(selectedPath))
            {
                pngImage.Save(fileStream);
            }
        }

        private void RibbonWindow_Closed(object sender, EventArgs e)
        {
            var drewShapes = state.Shapes;
            var protocols = drewShapes.Select(shape =>
            {
                var proto = ShapeSerializer.Serialize(shape);
                return encoding.GetBytes(proto);
            }).ToList();

            using FileStream stream = new(path: LAST_SESSION_DIR, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None);

            // Length = 10
            var signatureBuffer = encoding.GetBytes(Signature);
            var seperatorBuffer = encoding.GetBytes(Seperator);

            stream.Write(signatureBuffer, 0, signatureBuffer.Length);
            stream.Write(seperatorBuffer, 0, seperatorBuffer.Length);
        
            foreach(var shape in protocols)
            {
                var stringSize = shape.Length.ToString();
                if (stringSize.Length < SIZE_LENGTH) {
                    stringSize = "0" + stringSize;
                }

                var size = encoding.GetBytes(stringSize);
                stream.Write(size, 0, size.Length);
                stream.Write(seperatorBuffer, 0, seperatorBuffer.Length);
                stream.Write(shape, 0, shape.Length);
                stream.Write(seperatorBuffer, 0, seperatorBuffer.Length);
            }

            stream.Write(seperatorBuffer, 0, seperatorBuffer.Length);
            stream.Close();
        }
    }
}