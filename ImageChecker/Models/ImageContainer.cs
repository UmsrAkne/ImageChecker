using System.Collections.Generic;

namespace ImageChecker.Models
{
    public class ImageContainer
    {
        private string keyWord;

        public ImageContainer(string keyWord)
        {
            this.keyWord = keyWord;
        }

        public List<ImageFile> Files { get; set; } = new List<ImageFile>();

        public void Load(string directoryPath)
        {
        }
    }
}