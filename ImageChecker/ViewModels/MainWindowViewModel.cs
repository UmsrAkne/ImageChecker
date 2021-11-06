namespace ImageChecker.ViewModels
{
    using ImageChecker.Models;
    using Prism.Mvvm;
    using System.Collections.Generic;

    public class MainWindowViewModel : BindableBase
    {
        private string title = "Prism Application";
        private bool drawingA = true;
        private bool drawingB = true;
        private bool drawingC = true;
        private bool drawingD = true;
        private string aspectRatio;

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
    }
}
