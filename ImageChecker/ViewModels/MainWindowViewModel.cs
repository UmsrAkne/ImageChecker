namespace ImageChecker.ViewModels
{
    using ImageChecker.Models;
    using Prism.Mvvm;

    public class MainWindowViewModel : BindableBase
    {
        private string title = "Prism Application";
        private bool drawingA = true;
        private bool drawingB = true;
        private bool drawingC = true;
        private bool drawingD = true;

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

        public void LoadImages(string directoryPath)
        {
            ImageLoader.Load(directoryPath);
        }
    }
}
