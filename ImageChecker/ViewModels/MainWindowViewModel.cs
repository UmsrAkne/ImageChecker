using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ImageChecker.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace ImageChecker.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private bool drawingA = true;
        private bool drawingB = true;
        private bool drawingC = true;
        private bool drawingD = true;
        private string currentDirectoryPath;
        private double scale = 0.5;
        private int x;
        private int y;
        private string imageTagReplaceBaseText;
        private string drawTagReplaceBaseText;
        private string statusBarText;

        private List<ImageContainer> imageContainers;

        private DelegateCommand generateImageTagCommand;
        private DelegateCommand generateDrawTagCommand;
        private DelegateCommand<ListBox> focusToListBoxCommand;

        public MainWindowViewModel()
        {
            ImageTagReplaceBaseText = Properties.Settings.Default.ImageTagReplaceBaseText;
            DrawTagReplaceBaseText = Properties.Settings.Default.DrawTagReplaceBaseText;

            imageContainers = new List<ImageContainer>()
            {
                ImageContainerA,
                ImageContainerB,
                ImageContainerC,
                ImageContainerD,
            };
        }

        public string CurrentDirectoryPath { get => currentDirectoryPath; set => SetProperty(ref currentDirectoryPath, value); }

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

        public ImageContainer ImageContainerA { get; } = new ImageContainer("A");

        public ImageContainer ImageContainerB { get; } = new ImageContainer("B");

        public ImageContainer ImageContainerC { get; } = new ImageContainer("C");

        public ImageContainer ImageContainerD { get; } = new ImageContainer("D");

        public double Scale
        {
            get => scale;
            set
            {
                SetProperty(ref scale, value);
                imageContainers?.ForEach(ic => ic.CurrentFile.Scale = scale);
            }
        }

        public int X
        {
            get => x;
            set
            {
                imageContainers?.ForEach(ic => ic.CurrentFile.X = value);
                SetProperty(ref x, value);
            }
        }

        public int Y
        {
            get => y;
            set
            {
                imageContainers?.ForEach(ic => ic.CurrentFile.Y = value);
                SetProperty(ref y, value);
            }
        }

        public string ImageTagReplaceBaseText
        {
            get => imageTagReplaceBaseText;
            set
            {
                SetProperty(ref imageTagReplaceBaseText, value);
                Properties.Settings.Default.ImageTagReplaceBaseText = value;
                Properties.Settings.Default.Save();
            }
        }

        public string DrawTagReplaceBaseText
        {
            get => drawTagReplaceBaseText;
            set
            {
                SetProperty(ref drawTagReplaceBaseText, value);
                Properties.Settings.Default.DrawTagReplaceBaseText = value;
                Properties.Settings.Default.Save();
            }
        }

        public string StatusBarText { get => statusBarText; set => SetProperty(ref statusBarText, value); }

        public DelegateCommand GenerateImageTagCommand
        {
            get => generateImageTagCommand ?? (generateImageTagCommand = new DelegateCommand(() =>
            {
                string imageA = DrawingA ? Path.GetFileNameWithoutExtension(ImageContainerA.CurrentFile.FileInfo.Name) : string.Empty;
                string imageB = DrawingB ? Path.GetFileNameWithoutExtension(ImageContainerB.CurrentFile.FileInfo.Name) : string.Empty;
                string imageC = DrawingC ? Path.GetFileNameWithoutExtension(ImageContainerC.CurrentFile.FileInfo.Name) : string.Empty;
                string imageD = DrawingD ? Path.GetFileNameWithoutExtension(ImageContainerD.CurrentFile.FileInfo.Name) : string.Empty;

                var baseText = ImageTagReplaceBaseText;
                baseText = baseText.Replace("$a", imageA).Replace("$b", imageB).Replace("$c", imageC).Replace("$d", imageD);
                Clipboard.SetText(baseText);
            }));
        }

        public DelegateCommand GenerateDrawTagCommand
        {
            get => generateDrawTagCommand ?? (generateDrawTagCommand = new DelegateCommand(() =>
            {
                 string imageA = DrawingA ? Path.GetFileNameWithoutExtension(ImageContainerA.CurrentFile.FileInfo.Name) : string.Empty;
                 string imageB = DrawingB ? Path.GetFileNameWithoutExtension(ImageContainerB.CurrentFile.FileInfo.Name) : string.Empty;
                 string imageC = DrawingC ? Path.GetFileNameWithoutExtension(ImageContainerC.CurrentFile.FileInfo.Name) : string.Empty;
                 string imageD = DrawingD ? Path.GetFileNameWithoutExtension(ImageContainerD.CurrentFile.FileInfo.Name) : string.Empty;

                 var baseText = drawTagReplaceBaseText;
                 baseText = baseText.Replace("$a", imageA).Replace("$b", imageB).Replace("$c", imageC).Replace("$d", imageD);
                 Clipboard.SetText(baseText);
            }));
        }

        public DelegateCommand ResetBaseTextCommand => new DelegateCommand(() =>
        {
            ImageTagReplaceBaseText = "<image a=\"$a\" b=\"$b\" c=\"$c\" d=\"$d\" scale=\"\" x=\"\" y=\"\" rotation=\"\" statusInherit=\"\" target=\"main\" />";
            DrawTagReplaceBaseText = "<draw a=\"$a\" b=\"$b\" c=\"$c\" d=\"$d\" depth=\"\" delay=\"\" target=\"main\"/>";
        });

        public DelegateCommand<ListBox> FocusToListBoxCommand
        {
            get => focusToListBoxCommand ?? (focusToListBoxCommand = new DelegateCommand<ListBox>((l) =>
            {
                var dObj = l.ItemContainerGenerator.ContainerFromIndex(l.SelectedIndex);
                if (!(dObj is ListBoxItem target))
                {
                    return;
                }

                target.Focus();
                l.SelectedItem = l.Items[l.SelectedIndex];
            }));
        }

        public DelegateCommand ChangeImageGroupCommand => new DelegateCommand(() =>
        {
            ImageContainerB.SelectSameGroupImages(ImageContainerA.CurrentFile);
            ImageContainerC.SelectSameGroupImages(ImageContainerA.CurrentFile);
            ImageContainerD.SelectSameGroupImages(ImageContainerA.CurrentFile);
        });

        public void LoadImages(string directoryPath)
        {
            CurrentDirectoryPath = directoryPath;

            foreach (var ic in imageContainers)
            {
                ic.Load(directoryPath);
                ic.SelectSameGroupImages(ImageContainerA.CurrentFile);
            }
        }
    }
}