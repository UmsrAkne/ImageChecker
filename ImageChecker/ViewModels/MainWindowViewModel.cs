using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ImageChecker.Models;
using ImageChecker.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace ImageChecker.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private readonly List<ImageContainer> imageContainers;
        private readonly IDialogService dialogService;

        private string currentDirectoryPath;
        private double scale = 0.5;
        private int x;
        private int y;
        private string statusBarText;

        private DelegateCommand<ListBox> cursorDownCommand;
        private DelegateCommand<ListBox> cursorUpCommand;
        private DelegateCommand generateImageTagCommand;
        private DelegateCommand generateDrawTagCommand;
        private DelegateCommand generateAnimeDraTagCommand;
        private DelegateCommand<ListBox> focusToListBoxCommand;
        private int imageViewWidth = 640;
        private int imageViewHeight = 360;
        private double displayScale = 1.0;

        public MainWindowViewModel(IDialogService dialogService)
        {
            imageContainers = new List<ImageContainer>()
            {
                ImageContainerA,
                ImageContainerB,
                ImageContainerC,
                ImageContainerD,
            };

            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.ImageTagReplaceBaseText))
            {
                Properties.Settings.Default.ImageTagReplaceBaseText = SettingPageViewModel.DefaultImageTagBaseText;
            }

            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.DrawTagReplaceBaseText))
            {
                Properties.Settings.Default.DrawTagReplaceBaseText = SettingPageViewModel.DefaultDrawTagBaseText;
            }

            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.AnimeDrawTagReplaceBaseText))
            {
                Properties.Settings.Default.AnimeDrawTagReplaceBaseText =
                    SettingPageViewModel.DefaultAnimeDrawTagBaseText;
            }

            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.ScalingCenter))
            {
                Properties.Settings.Default.ScalingCenter = ScalingCenter.Center.ToString();
            }

            this.dialogService = dialogService;
            ImageViewHeight = imageViewHeight;
        }

        public string CurrentDirectoryPath { get => currentDirectoryPath; private set => SetProperty(ref currentDirectoryPath, value); }

        public ImageContainer ImageContainerA { get; } = new ImageContainer("A");

        public ImageContainer ImageContainerB { get; } = new ImageContainer("B");

        public ImageContainer ImageContainerC { get; } = new ImageContainer("C");

        public ImageContainer ImageContainerD { get; } = new ImageContainer("D");

        public ObservableCollection<Tag> ClipboardHistories { get; } = new ObservableCollection<Tag>();

        public double Scale
        {
            get => scale;
            set
            {
                SetProperty(ref scale, value);
                DisplayScale = value;
                imageContainers?.ForEach(ic =>
                {
                    ic.Scale = scale;
                });

                RaisePropertyChanged(nameof(DisplayX));
                RaisePropertyChanged(nameof(DisplayY));
            }
        }

        public double DisplayScale
        {
            get => displayScale;
            private set => SetProperty(ref displayScale, value * 2);
        }

        public int X
        {
            get => x;
            set
            {
                SetProperty(ref x, value);
                imageContainers?.ForEach(ic => ic.X = value);

                RaisePropertyChanged(nameof(DisplayX));
            }
        }

        public int DisplayX
        {
            get
            {
                var fistContainer = imageContainers.FirstOrDefault();
                if (fistContainer?.CurrentFile == null)
                {
                    return 0;
                }

                var scalingCenter =
                    (ScalingCenter)Enum.Parse(typeof(ScalingCenter), Properties.Settings.Default.ScalingCenter, true);

                if (scalingCenter == ScalingCenter.TopLeft)
                {
                    return fistContainer.X * 2;
                }

                var pos = (int)((fistContainer.CurrentFile.Width * scale) - imageViewWidth) / 2 * -1;
                pos -= fistContainer.X;
                return pos * 2 * -1;
            }
        }

        public int Y
        {
            get => y;
            set
            {
                SetProperty(ref y, value);
                imageContainers?.ForEach(ic => ic.Y = value);

                RaisePropertyChanged(nameof(DisplayY));
            }
        }

        public int DisplayY
        {
            get
            {
                var firstContainer = imageContainers.FirstOrDefault();
                if (firstContainer?.CurrentFile == null)
                {
                    return 0;
                }

                var scalingCenter =
                    (ScalingCenter)Enum.Parse(typeof(ScalingCenter), Properties.Settings.Default.ScalingCenter, true);

                if (scalingCenter == ScalingCenter.TopLeft)
                {
                    return firstContainer.Y * 2;
                }

                var pos = (int)((firstContainer.CurrentFile.Height * scale) - imageViewHeight) / 2 * -1;
                pos -= firstContainer.Y;

                return pos * 2;
            }
        }

        public string StatusBarText { get => statusBarText; set => SetProperty(ref statusBarText, value); }

        public int ImageViewWidth { get => imageViewWidth; private set => SetProperty(ref imageViewWidth, value); }

        public int ImageViewHeight { get => imageViewHeight; private set => SetProperty(ref imageViewHeight, value); }

        public DelegateCommand GenerateImageTagCommand
        {
            get => generateImageTagCommand ?? (generateImageTagCommand = new DelegateCommand(() =>
            {
                string imageA = ImageContainerA.GetCurrentFileName();
                string imageB = ImageContainerB.GetCurrentFileName();
                string imageC = ImageContainerC.GetCurrentFileName();
                string imageD = ImageContainerD.GetCurrentFileName();

                string dx = 0.ToString();
                string dy = 0.ToString();
                try
                {
                    dx = DisplayX.ToString();
                    dy = DisplayY.ToString();
                }
                catch (Exception e)
                {
                    using (StreamWriter sw = new StreamWriter(@"stackTrace.txt", false, Encoding.UTF8))
                    {
                        sw.Write(e);
                    }
                }

                var baseText = Properties.Settings.Default.ImageTagReplaceBaseText;
                baseText =
                    baseText
                        .Replace("$a", imageA)
                        .Replace("$b", imageB)
                        .Replace("$c", imageC)
                        .Replace("$d", imageD)
                        .Replace("$scale", DisplayScale.ToString(CultureInfo.InvariantCulture))
                        .Replace("$x", dx)
                        .Replace("$y", dy);

                SaveHistory(baseText, true);
            }));
        }

        public DelegateCommand GenerateDrawTagCommand
        {
            get => generateDrawTagCommand ?? (generateDrawTagCommand = new DelegateCommand(() =>
            {
                 string imageA = ImageContainerA.GetCurrentFileName();
                 string imageB = ImageContainerB.GetCurrentFileName();
                 string imageC = ImageContainerC.GetCurrentFileName();
                 string imageD = ImageContainerD.GetCurrentFileName();

                 var baseText = Properties.Settings.Default.DrawTagReplaceBaseText;
                 baseText = baseText.Replace("$a", imageA).Replace("$b", imageB).Replace("$c", imageC).Replace("$d", imageD);
                 SaveHistory(baseText, false);
            }));
        }

        public DelegateCommand GenerateAnimeDraTagCommand =>
            generateAnimeDraTagCommand ?? (generateAnimeDraTagCommand = new DelegateCommand(() =>
            {
                string imageA = ImageContainerA.GetCurrentFileName();
                string imageB = ImageContainerB.GetCurrentFileName();
                string imageC = ImageContainerC.GetCurrentFileName();
                string imageD = ImageContainerD.GetCurrentFileName();

                string dx = 0.ToString();
                string dy = 0.ToString();
                try
                {
                    dx = DisplayX.ToString();
                    dy = DisplayY.ToString();
                }
                catch (Exception e)
                {
                    using (StreamWriter sw = new StreamWriter(@"stackTrace.txt", false, Encoding.UTF8))
                    {
                        sw.Write(e);
                    }
                }

                var baseText = Properties.Settings.Default.AnimeDrawTagReplaceBaseText;
                baseText =
                    baseText
                        .Replace("$a", imageA)
                        .Replace("$b", imageB)
                        .Replace("$c", imageC)
                        .Replace("$d", imageD)
                        .Replace("$scale", DisplayScale.ToString(CultureInfo.InvariantCulture))
                        .Replace("$x", dx)
                        .Replace("$y", dy);

                SaveHistory(baseText, true);
            }));

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

        public DelegateCommand<ListBox> CursorDownCommand
        {
            get => cursorDownCommand ?? (cursorDownCommand = new DelegateCommand<ListBox>((lb) =>
            {
                lb.SelectedIndex++;
            }));
        }

        public DelegateCommand<ListBox> CursorUpCommand
        {
            get => cursorUpCommand ?? (cursorUpCommand = new DelegateCommand<ListBox>((lb) =>
            {
                lb.SelectedIndex--;
            }));
        }

        public DelegateCommand ChangeImageGroupCommand => new DelegateCommand(() =>
        {
            ImageContainerB.SelectSameGroupImages(ImageContainerA.CurrentFile);
            ImageContainerC.SelectSameGroupImages(ImageContainerA.CurrentFile);
            ImageContainerD.SelectSameGroupImages(ImageContainerA.CurrentFile);
        });

        public DelegateCommand ShowSettingPageCommand => new DelegateCommand(() =>
        {
            dialogService.ShowDialog(nameof(SettingPage), default, _ => { });
        });

        public DelegateCommand<Tag> CopyFromHistoryCommand => new DelegateCommand<Tag>(tag =>
        {
            if (tag != null)
            {
                Clipboard.SetDataObject(tag.CopiedText);
            }
        });

        public DelegateCommand<Tag> SetImagesCommand => new DelegateCommand<Tag>(tag =>
        {
            if (tag != null)
            {
                ImageContainerA.SetImageByName(tag.ImageNameA);
                ImageContainerB.SetImageByName(tag.ImageNameB);
                ImageContainerC.SetImageByName(tag.ImageNameC);
                ImageContainerD.SetImageByName(tag.ImageNameD);
            }
        });

        public DelegateCommand ChangeToHdRatio => new DelegateCommand(() =>
        {
            ImageViewWidth = 640;
            ImageViewHeight = 360;
        });

        public DelegateCommand ChangeToVgaRatio => new DelegateCommand(() =>
        {
            ImageViewHeight = 640;
            ImageViewHeight = 480;
        });

        public DelegateCommand<string> MoveImageLeftCommand => new DelegateCommand<string>(target =>
        {
            const int moveFrequency = 10;
            if (target == "x")
            {
                X += moveFrequency;
            }
            else
            {
                Y += moveFrequency;
            }
        });

        public DelegateCommand<string> MoveImageRightCommand => new DelegateCommand<string>(target =>
        {
            const int moveFrequency = 10;
            if (target == "x")
            {
                X -= moveFrequency;
            }
            else
            {
                Y -= moveFrequency;
            }
        });

        public DelegateCommand<object> ChangeScaleCommand => new DelegateCommand<object>(amount =>
        {
            Scale += double.Parse((string)amount);
        });

        public DelegateCommand ChangeToOriginalSizeCommand => new DelegateCommand(() =>
        {
            foreach (var imageContainer in imageContainers)
            {
                imageContainer.X = 0;
                imageContainer.Y = 0;
            }

            Scale = 0.5;
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

        private void SaveHistory(string text, bool isImageTag)
        {
            ClipboardHistories.Insert(0, new Tag(isImageTag)
            {
                ImageNameA = ImageContainerA.GetCurrentFileName(),
                ImageNameB = ImageContainerB.GetCurrentFileName(),
                ImageNameC = ImageContainerC.GetCurrentFileName(),
                ImageNameD = ImageContainerD.GetCurrentFileName(),
                CopiedText = text,
            });

            Clipboard.SetDataObject(text);
        }
    }
}