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

        private ImageFile currentImageFileA;
        private ImageFile currentImageFileB;
        private ImageFile currentImageFileC;
        private ImageFile currentImageFileD;

        public bool Loaded { get; private set; }

        public List<ImageFile> ImageFilesA { get => imageFilesA; private set => SetProperty(ref imageFilesA, value); }

        public List<ImageFile> ImageFilesB { get => imageFilesB; private set => SetProperty(ref imageFilesB, value); }

        public List<ImageFile> ImageFilesC { get => imageFilesC; private set => SetProperty(ref imageFilesC, value); }

        public List<ImageFile> ImageFilesD { get => imageFilesD; private set => SetProperty(ref imageFilesD, value); }

        public List<ImageFile> ImageFiles { get; set; }

        public ImageFile CurrentImageFileA { get => currentImageFileA; set => SetProperty(ref currentImageFileA, value); }

        public ImageFile CurrentImageFileB { get => currentImageFileB; set => SetProperty(ref currentImageFileB, value); }

        public ImageFile CurrentImageFileC { get => currentImageFileC; set => SetProperty(ref currentImageFileC, value); }

        public ImageFile CurrentImageFileD { get => currentImageFileD; set => SetProperty(ref currentImageFileD, value); }

        public void Load(string directoryPath)
        {
            List<string> fileNames = Directory.GetFiles(directoryPath, "*.png").Concat(Directory.GetFiles(directoryPath, "*.jpg")).ToList();

            ImageFilesA = fileNames.Where(name => Path.GetFileName(name).Contains("A")).Select(name => new ImageFile(name)).ToList();
            ImageFilesB = fileNames.Where(name => Path.GetFileName(name).Contains("B")).Select(name => new ImageFile(name)).ToList();
            ImageFilesC = fileNames.Where(name => Path.GetFileName(name).Contains("C")).Select(name => new ImageFile(name)).ToList();
            ImageFilesD = fileNames.Where(name => Path.GetFileName(name).Contains("D")).Select(name => new ImageFile(name)).ToList();

            CurrentImageFileA = ImageFilesA.FirstOrDefault();
            CurrentImageFileB = ImageFilesB.FirstOrDefault();
            CurrentImageFileC = ImageFilesC.FirstOrDefault();
            CurrentImageFileD = ImageFilesD.FirstOrDefault();

            ImageFiles = new List<ImageFile>(ImageFilesA);
            ImageFiles.AddRange(ImageFilesB);
            ImageFiles.AddRange(ImageFilesC);
            ImageFiles.AddRange(ImageFilesD);

            Loaded = ImageFiles.Count > 0;
        }
    }
}
