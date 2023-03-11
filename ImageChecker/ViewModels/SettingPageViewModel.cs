using System;
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

        private string imageTagBaseText;
        private string drawTagBaseText;

        public SettingPageViewModel()
        {
             ImageTagBaseText = Properties.Settings.Default.ImageTagReplaceBaseText;
             DrawTagBaseText = Properties.Settings.Default.DrawTagReplaceBaseText;
        }

        public event Action<IDialogResult> RequestClose;

        public string Title => "SettingPageViewModel";

        public string ImageTagBaseText { get => imageTagBaseText; set => SetProperty(ref imageTagBaseText, value); }

        public string DrawTagBaseText { get => drawTagBaseText; set => SetProperty(ref drawTagBaseText, value); }

        public DelegateCommand ResetImageTagBaseTextCommand => new DelegateCommand(() =>
        {
            ImageTagBaseText = DefaultImageTagBaseText;
        });

        public DelegateCommand ResetDrawTagBaseTextCommand => new DelegateCommand(() =>
        {
            DrawTagBaseText = DefaultDrawTagBaseText;
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
            Properties.Settings.Default.Save();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}