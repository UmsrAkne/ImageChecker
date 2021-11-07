﻿namespace ImageChecker.ViewModels
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using System.Linq;
    using ImageChecker.Models;
    using Prism.Mvvm;
    using System.IO;
    using Prism.Commands;
    using System.Windows;

    public class MainWindowViewModel : BindableBase
    {
        private string title = "Prism Application";
        private bool drawingA = true;
        private bool drawingB = true;
        private bool drawingC = true;
        private bool drawingD = true;
        private double scale = 0.5;
        private string aspectRatio;

        private DelegateCommand generateImageTagCommand;

        public MainWindowViewModel()
        {
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public string samplePath
        {
            get; set;
        } = "";

        public double Scale { get => scale; private set => SetProperty(ref scale, value); }

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

        public List<string> AspectRatioStrings { get; private set; } = new List<string>() { "4:3", "16:9", "16:10" };

        public string AspectRatio { get => aspectRatio; set => SetProperty(ref aspectRatio, value); }

        public void LoadImages(string directoryPath)
        {
            ImageLoader.Load(directoryPath);
        }

        public void LoadXML(string xmlFilePath)
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
                    imgFile.X = int.Parse(l.Attribute("x").Value);
                    imgFile.Y = int.Parse(l.Attribute("y").Value);
                }
            });
        }

        public DelegateCommand GenerateImageTagCommand
        {
            get => generateImageTagCommand ?? (generateImageTagCommand = new DelegateCommand(() =>
            {
                string imageA = DrawingA ? Path.GetFileNameWithoutExtension(ImageLoader.CurrentImageFileA.FileInfo.Name) : string.Empty;
                string imageB = DrawingB ? Path.GetFileNameWithoutExtension(ImageLoader.CurrentImageFileB.FileInfo.Name) : string.Empty;
                string imageC = DrawingC ? Path.GetFileNameWithoutExtension(ImageLoader.CurrentImageFileC.FileInfo.Name) : string.Empty;
                string imageD = DrawingD ? Path.GetFileNameWithoutExtension(ImageLoader.CurrentImageFileD.FileInfo.Name) : string.Empty;

                Clipboard.SetText($"<image a=\"{imageA}\" b=\"{imageB}\" c=\"{imageC}\" d=\"{imageD}\" />");
            }));
        }
    }
}
