namespace ImageChecker.Models
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    public class ImageLoader
    {
        public List<ImageFile> ImageFilesA { get; private set; } = new List<ImageFile>();

        public List<ImageFile> ImageFilesB { get; private set; } = new List<ImageFile>();

        public List<ImageFile> ImageFilesC { get; private set; } = new List<ImageFile>();

        public List<ImageFile> ImageFilesD { get; private set; } = new List<ImageFile>();

        public void Load(string directoryPath)
        {
            List<string> fileNames = Directory.GetFiles(directoryPath, "*.png").Concat(Directory.GetFiles(directoryPath, "*.jpg")).ToList();

            ImageFilesA = fileNames.Where(name => name.Contains("A")).Select(name => new ImageFile(name)).ToList();
            ImageFilesB = fileNames.Where(name => name.Contains("B")).Select(name => new ImageFile(name)).ToList();
            ImageFilesC = fileNames.Where(name => name.Contains("C")).Select(name => new ImageFile(name)).ToList();
            ImageFilesD = fileNames.Where(name => name.Contains("D")).Select(name => new ImageFile(name)).ToList();
        }
    }
}
