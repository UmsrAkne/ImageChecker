using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageChecker.Models
{
    public class ImageContainer
    {
        private readonly string keyChar;

        public ImageContainer(string keyChar)
        {
            this.keyChar = keyChar;
        }

        public List<ImageFile> Files { get; set; } = new List<ImageFile>();

        public List<ImageFile> FilteredFiles { get; set; } = new List<ImageFile>();

        public ImageFile CurrentFile { get; set; }

        public void Load(string directoryPath)
        {
            Files = Directory.GetFiles(directoryPath)
                .Where(path => path.EndsWith(".png") || path.EndsWith(".jpg"))
                .Where(path => Path.GetFileName(path).Contains(keyChar))
                .Select(path => new ImageFile(path))
                .ToList();

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