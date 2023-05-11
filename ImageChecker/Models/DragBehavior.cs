using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ImageChecker.ViewModels;
using Microsoft.Xaml.Behaviors;

namespace ImageChecker.Models
{
    public class DragBehavior : Behavior<Canvas>
    {
        private Point lastPoint = new Point(0, 0);
        private MainWindowViewModel vm;

        protected override void OnAttached()
        {
            base.OnAttached();
            vm = AssociatedObject.DataContext as MainWindowViewModel;
            AssociatedObject.MouseMove += AssociatedObjectOnMouseMove;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseMove -= AssociatedObjectOnMouseMove;
        }

        private void AssociatedObjectOnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (lastPoint == new Point(0, 0))
                {
                    // マウスのボタンを押した一発目の状態。この場合は現在の値を入れて、次からは一つ前の座標に基づき移動の処理をする。
                    lastPoint = e.GetPosition(AssociatedObject);
                    return;
                }

                var distance = lastPoint - e.GetPosition(AssociatedObject);
                vm.X -= (int)distance.X;
                vm.Y -= (int)distance.Y;

                lastPoint = e.GetPosition(AssociatedObject);
            }

            if (e.LeftButton == MouseButtonState.Released)
            {
                lastPoint = new Point(0, 0);
            }
        }
    }
}