


namespace PaintSharp.utils
{
    public class ShapeToSaveAbleOnjectConverter
    {
        public static byte[] Serialize(US_IShape.IShape record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            return null;
        }
    }
}
