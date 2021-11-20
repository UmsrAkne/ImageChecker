namespace ImageChecker.ViewModels
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Xml.Linq;
    using ImageChecker.Models;
    using Prism.Commands;
    using Prism.Mvvm;

    public class MainWindowViewModel : BindableBase
    {
        private bool drawingA = true;
        private bool drawingB = true;
        private bool drawingC = true;
        private bool drawingD = true;
        private double scale = 0.5;
        private int x;
        private int y;

        private DelegateCommand generateImageTagCommand;
        private DelegateCommand generateDrawTagCommand;
        private DelegateCommand<ListBox> focusToListBoxCommand;

        public MainWindowViewModel()
        {
        }

        public bool DrawingA
        {
            get => drawingA;
            set => SetProperty(ref drawingA, value);
        }

        public bool DrawingB
        {
            get => drawingB;
            set => SetProperty(ref drawingB, value);
        }

        public bool DrawingC
        {
            get => drawingC;
            set => SetProperty(ref drawingC, value);
        }

        public bool DrawingD
        {
            get => drawingD;
            set => SetProperty(ref drawingD, value);
        }

        public ImageLoader ImageLoader { get; private set; } = new ImageLoader();

        public double Scale
        {
            get => scale;
            set
            {
                SetProperty(ref scale, value);
                ImageLoader.ImageFiles?.ForEach(img => img.Scale = scale);
            }
        }

        public int X
        {
            get => x;
            set
            {
                ImageLoader.ImageFiles?.ForEach(img => img.X = value);
                SetProperty(ref x, value);
            }
        }

        public int Y
        {
            get => y;
            set
            {
                ImageLoader.ImageFiles?.ForEach(img => img.Y = value);
                SetProperty(ref y, value);
            }
        }

        public DelegateCommand GenerateImageTagCommand
        {
            get => generateImageTagCommand ?? (generateImageTagCommand = new DelegateCommand(() =>
            {
                if (ImageLoader.Loaded)
                {
                    string imageA = DrawingA ? Path.GetFileNameWithoutExtension(ImageLoader.CurrentImageFileA.FileInfo.Name) : string.Empty;
                    string imageB = DrawingB ? Path.GetFileNameWithoutExtension(ImageLoader.CurrentImageFileB.FileInfo.Name) : string.Empty;
                    string imageC = DrawingC ? Path.GetFileNameWithoutExtension(ImageLoader.CurrentImageFileC.FileInfo.Name) : string.Empty;
                    string imageD = DrawingD ? Path.GetFileNameWithoutExtension(ImageLoader.CurrentImageFileD.FileInfo.Name) : string.Empty;

                    Clipboard.SetText($"<image a=\"{imageA}\" b=\"{imageB}\" c=\"{imageC}\" d=\"{imageD}\" />");
                }
            }));
        }

        public DelegateCommand GenerateDrawTagCommand
        {
            get => generateDrawTagCommand ?? (generateDrawTagCommand = new DelegateCommand(() =>
            {
                if (ImageLoader.Loaded)
                {
                    string imageA = DrawingA ? Path.GetFileNameWithoutExtension(ImageLoader.CurrentImageFileA.FileInfo.Name) : string.Empty;
                    string imageB = DrawingB ? Path.GetFileNameWithoutExtension(ImageLoader.CurrentImageFileB.FileInfo.Name) : string.Empty;
                    string imageC = DrawingC ? Path.GetFileNameWithoutExtension(ImageLoader.CurrentImageFileC.FileInfo.Name) : string.Empty;
                    string imageD = DrawingD ? Path.GetFileNameWithoutExtension(ImageLoader.CurrentImageFileD.FileInfo.Name) : string.Empty;

                    Clipboard.SetText($"<draw a=\"{imageA}\" b=\"{imageB}\" c=\"{imageC}\" d=\"{imageD}\" />");
                }
            }));
        }

        public DelegateCommand<ListBox> FocusToListBoxCommand
        {
            get => focusToListBoxCommand ?? (focusToListBoxCommand = new DelegateCommand<ListBox>((l) => Keyboard.Focus(l)));
        }

        public void LoadImages(string directoryPath)
        {
            ImageLoader.Load(directoryPath);
        }

        public void LoadXML(string xmlFilePath)
        {
            if (ImageLoader.Loaded)
            {
                XDocument xDocument = XDocument.Load(xmlFilePath);
                XElement xElement = xDocument.Element("root");
                IEnumerable<XElement> locations = xElement.Elements("location");
                locations.ToList().ForEach(l =>
                {
                    ImageFile imgFile = ImageLoader.ImageFiles.FirstOrDefault(img =>
                    {
                        return Path.GetFileNameWithoutExtension(img.FileInfo.Name) == l.Attribute("name").Value;
                    });

                    if (imgFile != null)
                    {
                        imgFile.Scale = Scale;
                        imgFile.DefaultX = int.Parse(l.Attribute("x").Value);
                        imgFile.DefaultY = int.Parse(l.Attribute("y").Value);
                    }
                });
            }
        }
    }
}
