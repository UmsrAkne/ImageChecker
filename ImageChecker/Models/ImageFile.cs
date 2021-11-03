namespace ImageChecker.Models
{
    using System.Drawing;
    using System.IO;

    public class ImageFile
    {
        public ImageFile(string filePath)
        {
            FileInfo = new FileInfo(filePath);
        }

        public FileInfo FileInfo { get; private set; }

        public override string ToString()
        {
            return FileInfo.Name;
        }
    }
}
