namespace ImageChecker.Models
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    public class ImageLoader
    {
        public void Load(string directoryPath)
        {
            List<string> fileNames = Directory.GetFiles(directoryPath, "*.png").Concat(Directory.GetFiles(directoryPath, "*.jpg")).ToList();
            List<Bitmap> bitmaps = fileNames.Select(fn => new Bitmap(fn)).ToList();
        }
    }
}
