using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using FreeSpaceLib;


namespace FreeSpace
{
    /// <summary>
    /// Interaction logic for FolderView.xaml

    /// </summary>
    public partial class FolderView : UserControl
    {
        private long _currentDriveSize = 0;
        private string _currentParentDirectory;
        public event EventHandler<ItemSelectedEventArgs> ItemSelected;
        public event EventHandler<CurrentFolderChangedEventArgs> FolderChanged;


        public FolderView()
        {
            InitializeComponent();
            LoadHardDrives();

            MouseRightButtonUp += GoToParentFolder;
            FileSystemExplorer.DriveAnalyzeDone += DriveAnalyseComplete;
            FileSystemExplorer.ProgressUpdated += UpdateDriveAnalyseProgress;  
        }

        private void LoadHardDrives()
        {
            var driveInfo = DriveInfo.GetDrives();
            foreach (var drive in driveInfo)
            {
                if (drive.IsReady)
                {
                    var icon = new DiskIcon(drive);
                    icon.MouseLeftButtonDown += AnalyseDrive;
                    FolderViewWPanel.Children.Add(icon);
                }
            }
        }

        public void NewDriveButtonClicked()
        {           
            MoveInFileTreeAni(true);
            FolderViewWPanel.Children.Clear();
            LoadHardDrives();
        }

        private void AnalyseDrive(object sender, MouseButtonEventArgs e)
        {
            var loader = new LoadingAniControl();
            LayoutRoot.Children.Add(loader);

            var tempObj = sender as DiskIcon;
            _currentDriveSize = tempObj.DriveSize;

            FileSystemExplorer.ScanDrive(tempObj.DriveName, tempObj.DriveSpaceInUse);                      
        }

        private void DriveAnalyseComplete(object sender, FileExplorerDriveAnalyzeDoneEventArgs e)
        {
            ProgressBar.Width = 0;
            LoadingAniControl loader = LayoutRoot.Children.OfType<LoadingAniControl>().First();
            LayoutRoot.Children.Remove(loader);
            OpenDirectory(e.Drivename);
        }

        private void UpdateDriveAnalyseProgress(object sender, ProgressUpdatedEventArgs e)
        {
            ProgressBar.Width = (e.PercentDone / 100) * this.ActualWidth;
        }

        void FolderLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var tempObj = sender as FolderIcon;

            if (!tempObj.IsChangingView)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    tempObj.IsSelected = !tempObj.IsSelected;
                    ItemSelected(tempObj, new ItemSelectedEventArgs( tempObj.IsSelected ? tempObj.Size : -tempObj.Size));
                }
                else
                {
                    _currentParentDirectory = tempObj.ParentFolder;
                    OpenDirectory(tempObj.FileSystemLocation);
                }
            }
        }

        private void FileLeftButtonUp(object sender, MouseEventArgs e)
        {
            var tempObj = sender as FileIcon;

            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                tempObj.IsSelected = !tempObj.IsSelected;
                ItemSelected(tempObj, new ItemSelectedEventArgs(tempObj.IsSelected ? tempObj.Size : -tempObj.Size));
            }
        }

        private void GoToParentFolder(object sender, MouseButtonEventArgs e)
        {
            if (!String.IsNullOrEmpty(_currentParentDirectory))
                OpenDirectory(_currentParentDirectory, true);

            var list = FolderViewWPanel.Children.OfType<FolderIcon>();
            FolderIcon icon = list.First();

            DirectoryInfo dinfo = new DirectoryInfo(icon.ParentFolder);

            if (dinfo.Parent != null)
            {
                _currentParentDirectory = dinfo.Parent.FullName;
            }
        }

        private void OpenDirectory(string location, bool aniUp = false)
        {
            MoveInFileTreeAni(aniUp);
            FolderViewWPanel.Children.Clear();
            FolderChanged(this, new CurrentFolderChangedEventArgs(location));

            foreach (string folder in Directory.GetDirectories(location))
            {
                try
                {
                    var info = new FileInfo(folder);
                    var icon = new FolderIcon(FileSystemExplorer.FolderInfoDictionary[info.ToString()], _currentDriveSize);
                    icon.MouseLeftButtonUp += FolderLeftButtonUp;
                    FolderViewWPanel.Children.Add(icon);
                }
                catch (Exception ex)
                {
                    ExceptionLogger.Log(ex, "Folderview OpenDirectory");
                }
            }

            foreach (string file in Directory.GetFiles(location))
            {
                var info = new FileInfo(file);
                var icon = new FileIcon(info);
                icon.MouseLeftButtonUp += FileLeftButtonUp;
                FolderViewWPanel.Children.Add(icon);
            }
            SortItems(x => x.Size);
        }

        public void SortItems<T>(Func<BaseFolderViewIcon, T> sortFunc , bool decendingOrder = true )
        {
            foreach (FolderIcon ficon in FolderViewWPanel.Children.OfType<FolderIcon>().Sort(sortFunc, decendingOrder))
            {
                FolderViewWPanel.Children.Remove(ficon);
                FolderViewWPanel.Children.Add(ficon);
            }

            foreach (FileIcon ficon in FolderViewWPanel.Children.OfType<FileIcon>().Sort(sortFunc, decendingOrder))
            {
                FolderViewWPanel.Children.Remove(ficon);
                FolderViewWPanel.Children.Add(ficon);
            }
        }

        private bool _nextState = false;
        private bool _previousState = false;

        private void MoveInFileTreeAni(bool moveUpinFileTree)
        {
            if (!moveUpinFileTree)
            {
                VisualStateManager.GoToState(this, _nextState ? "Next1" : "Next", true);
                _nextState = !_nextState;
            }

            else
            {
                VisualStateManager.GoToState(this, _previousState ? "Previous1" : "Previous", true);
                _previousState = !_previousState;
            }
        }

        public void DeleteSelection()
        {
            var itemsSelectedForDeletion = FolderViewWPanel.Children.OfType<BaseFolderViewIcon>().ToList();

            if (itemsSelectedForDeletion.Count(x => x.IsSelected) < 1)
            {
                MessageBox.Show("Selected one or more files");
            }

            else
            {
                foreach (BaseFolderViewIcon icon in itemsSelectedForDeletion.Where(x => x.IsSelected))
                {
                    icon.DeleteAni();
                    FileSystemExplorer.DeleteDirectoryAsync(icon.FileSystemLocation);
                    DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
                    timer.Tick += (obj, args) =>
                    {
                        FolderViewWPanel.Children.Remove(icon);
                        (obj as DispatcherTimer).Stop();
                    };
                    timer.Start();
                }               
            }
        }
    }
}