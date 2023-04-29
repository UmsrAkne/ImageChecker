using System;
using ImageChecker.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace ImageChecker.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SettingPageViewModel : BindableBase, IDialogAware
    {
        public const string DefaultImageTagBaseText =
            "<image a=\"$a\" b=\"$b\" c=\"$c\" d=\"$d\" scale=\"$scale\" x=\"$x\" y=\"$y\" rotation=\"0\" target=\"main\" />";

        public const string DefaultDrawTagBaseText =
            "<draw a=\"$a\" b=\"$b\" c=\"$c\" d=\"$d\" depth=\"0.1\" target=\"main\" />";

        public const string DefaultAnimeDrawTagBaseText =
            "<anime name=\"draw\" a=\"$a\" b=\"$b\" c=\"$c\" d=\"$d\" scale=\"$scale\" x=\"$x\" y=\"$y\" target=\"main\" />";

        private string imageTagBaseText;
        private string drawTagBaseText;
        private string animeDrawTagBaseText;
        private ScalingCenter scalingCenter;
        private bool scalingCenterIsCenter;
        private bool scalingCenterIsTopLeft;

        public SettingPageViewModel()
        {
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.ScalingCenter))
            {
                scalingCenter = ScalingCenter.Center;
            }
            else
            {
                scalingCenter =
                    (ScalingCenter)Enum.Parse(typeof(ScalingCenter), Properties.Settings.Default.ScalingCenter, true);
            }

            if (scalingCenter == ScalingCenter.Center)
            {
                ScalingCenterIsCenter = true;
            }

            if (scalingCenter == ScalingCenter.TopLeft)
            {
                ScalingCenterIsTopLeft = true;
            }

            ImageTagBaseText = Properties.Settings.Default.ImageTagReplaceBaseText;
            DrawTagBaseText = Properties.Settings.Default.DrawTagReplaceBaseText;
            animeDrawTagBaseText = Properties.Settings.Default.AnimeDrawTagReplaceBaseText;
        }

        public event Action<IDialogResult> RequestClose;

        public string Title => "SettingPageViewModel";

        public string ImageTagBaseText { get => imageTagBaseText; set => SetProperty(ref imageTagBaseText, value); }

        public string DrawTagBaseText { get => drawTagBaseText; set => SetProperty(ref drawTagBaseText, value); }

        public string AnimeDrawTagBaseText { get => animeDrawTagBaseText; set => SetProperty(ref animeDrawTagBaseText, value); }

        public bool ScalingCenterIsCenter { get => scalingCenterIsCenter; set => SetProperty(ref scalingCenterIsCenter, value); }

        public bool ScalingCenterIsTopLeft { get => scalingCenterIsTopLeft; set => SetProperty(ref scalingCenterIsTopLeft, value); }

        public DelegateCommand ResetImageTagBaseTextCommand => new DelegateCommand(() =>
        {
            ImageTagBaseText = DefaultImageTagBaseText;
        });

        public DelegateCommand ResetDrawTagBaseTextCommand => new DelegateCommand(() =>
        {
            DrawTagBaseText = DefaultDrawTagBaseText;
        });

        public DelegateCommand ResetAnimeDrawTagBaseTextCommand => new DelegateCommand(() =>
        {
            AnimeDrawTagBaseText = DefaultAnimeDrawTagBaseText;
        });

        public DelegateCommand<object> SetScalingCenterCommand => new DelegateCommand<object>(param =>
        {
            scalingCenter = (ScalingCenter)param;
        });

        public DelegateCommand ExitCommand => new DelegateCommand(() =>
        {
            RequestClose?.Invoke(default);
        });

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
            Properties.Settings.Default.ImageTagReplaceBaseText = ImageTagBaseText;
            Properties.Settings.Default.DrawTagReplaceBaseText = DrawTagBaseText;
            Properties.Settings.Default.DrawTagReplaceBaseText = AnimeDrawTagBaseText;
            Properties.Settings.Default.ScalingCenter = scalingCenter.ToString();
            Properties.Settings.Default.Save();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}