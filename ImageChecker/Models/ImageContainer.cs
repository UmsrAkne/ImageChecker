using System.Collections.Generic;
using System.IO;
using System.Linq;
using Prism.Mvvm;

namespace ImageChecker.Models
{
    public class ImageContainer : BindableBase
    {
        private readonly string keyChar;
        private List<ImageFile> filteredFiles = new List<ImageFile>();
        private ImageFile currentFile;

        public ImageContainer(string keyChar)
        {
            this.keyChar = keyChar;
        }

        public List<ImageFile> FilteredFiles
        {
            get => filteredFiles; private set => SetProperty(ref filteredFiles, value);
        }

        public ImageFile CurrentFile { get => currentFile; set => SetProperty(ref currentFile, value); }

        private List<ImageFile> Files { get; set; } = new List<ImageFile>();

        public void Load(string directoryPath)
        {
            Files = Directory.GetFiles(directoryPath)
                .Where(path => path.EndsWith(".png") || path.EndsWith(".jpg"))
                .Where(path => Path.GetFileName(path).Contains(keyChar))
                .Select(path => new ImageFile(path))
                .ToList();

            FilteredFiles = Files.ToList();
            CurrentFile = Files.FirstOrDefault();
        }

        public void SelectSameGroupImages(ImageFile baseImageFile)
        {
            if (keyChar == "A")
            {
                return;
            }

            int groupIndex = baseImageFile.Index;

            FilteredFiles = Files.Where(imageFile => imageFile.Index == groupIndex).ToList();
            CurrentFile = FilteredFiles.FirstOrDefault();
        }
    }
}