using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FreeSpace
{
    public class BaseFileIcon : UserControl
    {
        public string FileSystemLocation;
        public long Size;
        public string ItemName;
        public DateTime CreationDateTime;
        public bool IsSelected = false;

        public BaseFileIcon()
        {
            DoubleAnimation ani = new DoubleAnimation { From = 0, To = 1, Duration = TimeSpan.FromMilliseconds(700) };
            var sb = new Storyboard();

            Storyboard.SetTarget(ani, this);
            Storyboard.SetTargetProperty(ani, new PropertyPath(OpacityProperty));
            sb.Children.Add(ani);
            sb.Begin();
        }

        public void DeleteAni()
        {
            DoubleAnimation ani = new DoubleAnimation { From = 1, To = 0, Duration = TimeSpan.FromMilliseconds(500), EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseIn } };
            var sb = new Storyboard();

            Storyboard.SetTarget(ani, this);
            Storyboard.SetTargetProperty(ani, new PropertyPath(OpacityProperty));
            sb.Children.Add(ani);
            sb.Begin();
        }


        public void MouseLeaveAni(FrameworkElement element)
        {
            var ani = new ColorAnimation
            {
                From = Color.FromArgb(255, 88, 155, 251),
                To = IsSelected ? Color.FromArgb(255, 193, 225, 255) : Color.FromArgb(255, 245, 245, 245),
                Duration = TimeSpan.FromMilliseconds(200)
            };

            var sb = new Storyboard();
            sb.Children.Add(ani);
            Storyboard.SetTarget(ani, element);
            Storyboard.SetTargetProperty(ani, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));
            sb.Begin();
        }

        public void MouseEnterAni(FrameworkElement element)
        {
            var ani = new ColorAnimation
            {
                From = IsSelected ? Color.FromArgb(255, 193, 225, 255) : Color.FromArgb(255, 245, 245, 245),
                To = Color.FromArgb(255, 88, 155, 251),
                Duration = TimeSpan.FromMilliseconds(200)
            };

            var sb = new Storyboard();
            sb.Children.Add(ani);
            Storyboard.SetTarget(ani, element);
            Storyboard.SetTargetProperty(ani, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));
            sb.Begin();
        }
    }
}
