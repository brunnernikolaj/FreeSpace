using System.IO;
using System.Windows.Controls;
using FreeSpaceLib;

namespace FreeSpace
{
	/// <summary>
	/// Interaction logic for FileIcon.xaml
	/// </summary>
	public partial class FileIcon : BaseFolderViewIcon
	{
        
		public FileIcon(FileInfo finfo)
		{
			this.InitializeComponent();
		    CreationDateTime = finfo.CreationTime;
		    ItemName = finfo.Name;
		    FileNameText.Text = finfo.Name;
            FileSizeText.Text = finfo.Length.FormatDataSize();
		    Size = finfo.Length;
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