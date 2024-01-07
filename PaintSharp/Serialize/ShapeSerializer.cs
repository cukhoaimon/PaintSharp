namespace PaintSharp.Serialize
{
    public static class ShapeSerializer
    {
        public static string Serialize(US_IShape.IShape shape)
        {
            var shapeProtocol = new ShapeProtocol(shape);

            var properties = from p in shapeProtocol.GetType().GetProperties()
                             where p.GetValue(shapeProtocol, null) != null
                             select p.Name + "=" + p.GetValue(shapeProtocol, null);

            return String.Join(";", properties.ToArray());
        }

        public static US_IShape.IShape Deserialize(string rawData)
        {
            var _raw = rawData.Split(';').ToDictionary(p => p.Split("=")[0], p => p.Split("=")[1]);
            var deserialized = new ShapeProtocol();

            var _properties = typeof(ShapeProtocol).GetProperties();
            foreach (var property in _properties)
            {
                try
                {
                    var value = _raw[property.Name];
                    property.SetValue(deserialized, value, null);
                }
                catch { }
            }

            return ShapeProtocol.ConvertBack(deserialized);
        }
    }
}
