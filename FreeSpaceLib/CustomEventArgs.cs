using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeSpace
{
    public class FileExplorerDriveAnalyzeDoneEventArgs : EventArgs
    {
        public string Drivename;
        public FileExplorerDriveAnalyzeDoneEventArgs(string driveName)
        {
            Drivename = driveName;
        }
    }

    public class ProgressUpdatedEventArgs : EventArgs
    {
        public double PercentDone;
        public ProgressUpdatedEventArgs(double percentDone)
        {
            PercentDone = percentDone;
        }
    }

    public class ItemSelectedEventArgs : EventArgs
    {
        public long Size;
        public ItemSelectedEventArgs(long itemSize)
        {
            Size = itemSize;
        }
    }

    public class CurrentFolderChangedEventArgs : EventArgs
    {
        public string Foldername;
        public CurrentFolderChangedEventArgs(string foldername)
        {
            Foldername = foldername;
        }
    }

}
