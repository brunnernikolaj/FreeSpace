using System.IO;
using System.Windows.Media;
using System.Windows.Controls;
using FreeSpaceLib;

namespace FreeSpace
{
    /// <summary>
    /// Interaction logic for DiskIcon.xaml
    /// </summary>
    public partial class DiskIcon : UserControl
    {
        public long DriveSize;
        public long DriveSpaceInUse;
        public string DriveName;

        public DiskIcon(DriveInfo Dinfo)
        {
            InitializeComponent();

            DriveSpaceInUse = Dinfo.TotalSize - Dinfo.TotalFreeSpace;
            DriveSize = Dinfo.TotalSize;
            DriveName = Dinfo.Name;

            DiskNameText.Text = Dinfo.Name.Remove(1) + " Drive";
            DiskUsageText.Text = ("Usage : " + (Dinfo.TotalSize - Dinfo.TotalFreeSpace).FormatDataSize() + " of " + Dinfo.TotalSize.FormatDataSize());

            SetPercentBar(((double)(Dinfo.TotalSize - Dinfo.TotalFreeSpace) / Dinfo.TotalSize) * 100);
        }

        private void SetPercentBar(double percent)
        {
            SolidColorBrush brush = null;
            if (percent > 80)
            {
                brush = new SolidColorBrush(Color.FromRgb(173, 3, 3)); //Red
            }

            else if (percent > 50)
            {
                brush = new SolidColorBrush(Color.FromRgb(230, 217, 0)); //Yellow
            }

            else if (percent > 25)
            {
                brush = new SolidColorBrush(Color.FromRgb(42, 173, 42)); //Green
            }
            PercentRect.Fill = brush;
            PercentRect.Width = percent * 1.58; // 1.58 correspond to 158 the width of the percentbar
        }
    }
}