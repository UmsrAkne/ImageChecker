using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace ImageChecker.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SettingPageViewModel : BindableBase, IDialogAware
    {
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
            ImageTagBaseText = "<image a=\"$a\" b=\"$b\" c=\"$c\" d=\"$d\" scale=\"1.0\" x=\"0\" y=\"0\" rotation=\"0\" target=\"main\" />";
        });

        public DelegateCommand ResetDrawTagBaseTextCommand => new DelegateCommand(() =>
        {
            DrawTagBaseText = "<draw a=\"$a\" b=\"$b\" c=\"$c\" d=\"$d\" depth=\"0.1\" target=\"main\" />";
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