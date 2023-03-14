﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
                    if (ic.CurrentFile != null)
                    {
                        ic.CurrentFile.Scale = scale;
                    }
                });
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
                int displayValue = (int)(value / 0.5);
                SetProperty(ref x, displayValue);
                imageContainers?.ForEach(ic =>
                {
                    if (ic.CurrentFile != null)
                    {
                        ic.CurrentFile.X = value;
                    }
                });
            }
        }

        public int Y
        {
            get => y;
            set
            {
                int displayValue = (int)(value / 0.5);
                SetProperty(ref y, displayValue);
                imageContainers?.ForEach(ic =>
                {
                    if (ic.CurrentFile != null)
                    {
                        ic.CurrentFile.Y = value;
                    }
                });
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

                var baseText = Properties.Settings.Default.ImageTagReplaceBaseText;
                baseText =
                    baseText
                        .Replace("$a", imageA)
                        .Replace("$b", imageB)
                        .Replace("$c", imageC)
                        .Replace("$d", imageD)
                        .Replace("$scale", DisplayScale.ToString(CultureInfo.InvariantCulture))
                        .Replace("$x", X.ToString())
                        .Replace("$y", Y.ToString());

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