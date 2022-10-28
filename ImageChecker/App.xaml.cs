using System.Windows;
using ImageChecker.ViewModels;
using ImageChecker.Views;
using Prism.Ioc;

namespace ImageChecker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<SettingPage, SettingPageViewModel>();
        }
    }
}