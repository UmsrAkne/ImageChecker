namespace ImageChecker.Models
{
    using System.Drawing;
    using System.IO;
    using System.Text.RegularExpressions;
    using Prism.Mvvm;

    public class ImageFile : BindableBase
    {
        private int width;
        private int height;

        public ImageFile(string filePath)
        {
            FileInfo = new FileInfo(filePath);
            if (File.Exists(filePath))
            {
                Bitmap bitmap = new Bitmap(filePath);
                Width = bitmap.Width;
                Height = bitmap.Height;
            }

            if (Regex.Match(FileInfo.Name, @"[ABCD]\d\d\d\d").Success)
            {
                IsMatchingNamingRule = true;
                MatchCollection matches = Regex.Matches(FileInfo.Name, @"[ABCD](\d\d)(\d\d)");
                Index = int.Parse(matches[0].Groups[1].Value);
                SubIndex = int.Parse(matches[0].Groups[2].Value);
            }
        }

        public int Width { get => width; private set => SetProperty(ref width, value); }

        public int Height { get => height; private set => SetProperty(ref height, value); }

        public bool IsMatchingNamingRule { get; private set; }

        public int Index { get; private set; }

        private int SubIndex { get; set; }

        public FileInfo FileInfo { get; private set; }

        public override string ToString()
        {
            return FileInfo.Name;
        }
    }
}