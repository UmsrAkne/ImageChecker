namespace ImageChecker.Models
{
    using System.Drawing;
    using System.IO;

    public class ImageFile
    {
        public ImageFile(string filePath)
        {
            FileInfo = new FileInfo(filePath);
            Bitmap = new Bitmap(filePath);
        }

        public Bitmap Bitmap { get; private set; }

        public FileInfo FileInfo { get; private set; }

        public override string ToString()
        {
            return FileInfo.Name;
        }
    }
}
