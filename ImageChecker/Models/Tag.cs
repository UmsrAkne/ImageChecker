namespace ImageChecker.Models
{
    public class Tag
    {
        public Tag(bool isImageTag)
        {
            ElementName = isImageTag ? "Image" : "draw";
        }

        public string ImageNameA { get; set; } = string.Empty;

        public string ImageNameB { get; set; } = string.Empty;

        public string ImageNameC { get; set; } = string.Empty;

        public string ImageNameD { get; set; } = string.Empty;

        public int X { get; set; }

        public int Y { get; set; }

        public double Scale { get; set; }

        public string CopiedText { get; set; } = string.Empty;

        private string ElementName { get; }
    }
}