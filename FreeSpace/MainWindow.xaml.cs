using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using FreeSpaceLib;

namespace FreeSpace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private long currentSizeOfSelectedItems = 0;

        public MainWindow()
        {
            InitializeComponent();

            FolderViewMain.FolderChanged += FolderViewMain_FolderChanged;
            FolderViewMain.ItemSelected += FolderViewMain_ItemSelected;
        }

        void FolderViewMain_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            currentSizeOfSelectedItems += e.Size;
            SizeCurrentSelceted.Text = currentSizeOfSelectedItems.FormatDataSize();
        }

        void FolderViewMain_FolderChanged(object sender, CurrentFolderChangedEventArgs e)
        {
            if (NewDriveButton.Visibility == System.Windows.Visibility.Collapsed)
                NewDriveButton.Visibility = System.Windows.Visibility.Visible;

            CurrentFolderNameText.Text = e.Foldername;
        }

        private void DeleteSelection(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FolderViewMain.DeleteSelection();
            SizeCurrentSelceted.Text = "0 Bytes";
            currentSizeOfSelectedItems = 0;
        }

        private bool IsSortPanelOpen = false;

        private void SortButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SortPanel.Visibility = IsSortPanelOpen ? Visibility.Collapsed : Visibility.Visible;
            IsSortPanelOpen = !IsSortPanelOpen;
        }

        private string CurrentSort = "Size";
        private bool SortDirection = true;

        private void SortTypeClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var tempobj = sender as Border;
            string sortTypeSelected = tempobj.Tag.ToString();

            if (sortTypeSelected == CurrentSort)
                SortDirection = !SortDirection;

            else
                SortDirection = false;

            switch (sortTypeSelected)
            {
                case "Name":
                    FolderViewMain.SortItems(x => x.ItemName, SortDirection);
                    break;

                case "Size":
                    FolderViewMain.SortItems(x => x.Size, !SortDirection); //SortDirection reversed because you want the largest size first
                    break;

                case "Created":
                    FolderViewMain.SortItems(x => x.CreationDateTime, SortDirection);
                    break;
            }
            CurrentSort = sortTypeSelected;
        }

        private void NewDriveButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NewDriveButton.Visibility = System.Windows.Visibility.Collapsed;
            FolderViewMain.NewDriveButtonClicked();
        }
    }
}
