namespace ImageChecker.Models
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using Prism.Mvvm;

    public class ImageLoader : BindableBase
    {
        private List<ImageFile> imageFilesA = new List<ImageFile>();
        private List<ImageFile> imageFilesB = new List<ImageFile>();
        private List<ImageFile> imageFilesC = new List<ImageFile>();
        private List<ImageFile> imageFilesD = new List<ImageFile>();

        public List<ImageFile> ImageFilesA { get => imageFilesA; private set => SetProperty(ref imageFilesA, value); }

        public List<ImageFile> ImageFilesB { get => imageFilesB; private set => SetProperty(ref imageFilesB, value); }

        public List<ImageFile> ImageFilesC { get => imageFilesC; private set => SetProperty(ref imageFilesC, value); }

        public List<ImageFile> ImageFilesD { get => imageFilesD; private set => SetProperty(ref imageFilesD, value); }

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
