﻿namespace ImageChecker.ViewModels
{
    using ImageChecker.Models;
    using Prism.Mvvm;

    public class MainWindowViewModel : BindableBase
    {
        private string title = "Prism Application";

        public MainWindowViewModel()
        {
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public ImageLoader ImageLoader { get; private set; } = new ImageLoader();
    }
}
