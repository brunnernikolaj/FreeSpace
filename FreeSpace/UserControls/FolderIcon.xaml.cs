using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using FreeSpaceLib;

namespace FreeSpace
{
	/// <summary>
	/// Interaction logic for FileIcon.xaml
	/// </summary>
	public partial class FolderIcon : BaseFolderViewIcon
	{
        public double Percent;
	    public string ParentFolder;
        public FolderIcon(FolderInfo finfo , long currentDriveSize)
		{
			InitializeComponent();
            ParentFolder = finfo.parentFolder;
            FileSystemLocation = finfo.path;
            CreationDateTime = finfo.DateCreated;
            ItemName = finfo.name;
            Percent = Math.Round((finfo.size / (double)currentDriveSize) * 100,2);
            Size = finfo.size;
            folderSizeText.Text = (finfo.size > 0) ? finfo.size.FormatDataSize() + " " + Percent + "%" : "Empty";
            folderNameText.Text = finfo.name;
            PercentRect.Width = Percent*2;

            FilesCountText.Text = finfo.FileCount.ToString();
            FoldersCountText.Text = finfo.FolderCount.ToString();
            CreationTimeDateText.Text = finfo.DateCreated.ToShortDateString();

            folderNameText.ToolTip = finfo.name;
		}

	    public bool IsChangingView;

        private void ChangeViewClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {            
            ChangeViewAni();

            if (_viewAniState)
            {
                IsChangingView = true;
                StandardView.Visibility = Visibility.Collapsed;
                DetailsView.Visibility = Visibility.Visible;
            }

            else
            {              
                StandardView.Visibility = Visibility.Visible;
                DetailsView.Visibility = Visibility.Collapsed;
                DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
                timer.Tick += (obj, args) =>
                {
                    IsChangingView = false;
                    (obj as DispatcherTimer).Stop();
                };
                timer.Start();
            }
           
        }

	    private bool _viewAniState = false;

	    private void ChangeViewAni()
	    {
	        VisualStateManager.GoToState(this, _viewAniState ? "Next" : "Next1", true);
            _viewAniState = !_viewAniState;
	    }

	    private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
	    {	    
	        var tempobj = sender as UserControl;
	        MouseEnterAni(tempobj);
	    }

	    private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
	    {
            var tempobj = sender as UserControl;
            MouseLeaveAni(tempobj);
	    }
	}
}