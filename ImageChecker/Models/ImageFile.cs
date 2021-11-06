namespace ImageChecker.Models
{
    using System.IO;
    using Prism.Mvvm;

    public class ImageFile : BindableBase
    {

        private int x = 0;
        private int y = 0;

        public ImageFile(string filePath)
        {
            FileInfo = new FileInfo(filePath);
        }

        public int X { get => x; set => SetProperty(ref x, value); }

        public int Y { get => y; set => SetProperty(ref y, value); }

        public FileInfo FileInfo { get; private set; }

        public override string ToString()
        {
            return FileInfo.Name;
        }
    }
}
