namespace ImageChecker.Models
{
    using System.Drawing;
    using System.IO;
    using Prism.Mvvm;

    public class ImageFile : BindableBase
    {
        private int x = 0;
        private int y = 0;
        private int width;
        private int height;
        private Bitmap bitmap;

        public ImageFile(string filePath)
        {
            FileInfo = new FileInfo(filePath);
            if (File.Exists(filePath))
            {
                bitmap = new Bitmap(filePath);
                Width = bitmap.Width;
                Height = bitmap.Height;
            }
        }

        public int X { get => x; set => SetProperty(ref x, value); }

        public int Y { get => y; set => SetProperty(ref y, value); }

        public int Width { get => width; set => SetProperty(ref width, value); }

        public int Height { get => height; set => SetProperty(ref height, value); }

        public FileInfo FileInfo { get; private set; }

        public override string ToString()
        {
            return FileInfo.Name;
        }
    }
}
