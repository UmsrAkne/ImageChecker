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
        private bool drawing;
        private int selectedIndex;
        private ImageFile currentFile;

        public ImageContainer(string keyChar)
        {
            this.keyChar = keyChar;
        }

        public List<ImageFile> FilteredFiles
        {
            get => filteredFiles; private set => SetProperty(ref filteredFiles, value);
        }

        public ImageFile CurrentFile
        {
            get => currentFile;
            set
            {
                if (currentFile != null && value != null)
                {
                    value.Scale = currentFile.Scale;
                    value.X = currentFile.X;
                    value.Y = currentFile.Y;
                }

                SetProperty(ref currentFile, value);
            }
        }

        public bool Drawing { get => drawing; set => SetProperty(ref drawing, value); }

        public int SelectedIndex { get => selectedIndex; set => SetProperty(ref selectedIndex, value); }

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
            Drawing = Files.Count != 0;
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

        public string GetCurrentFileName()
        {
            if (!Drawing || CurrentFile == null)
            {
                return string.Empty;
            }

            return Path.GetFileNameWithoutExtension(CurrentFile.FileInfo.FullName);
        }

        public void SetImageByName(string name)
        {
            Drawing = !string.IsNullOrWhiteSpace(name);
            if (Drawing)
            {
                CurrentFile =
                    FilteredFiles.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f.FileInfo.Name) == name);
            }
        }
    }
}