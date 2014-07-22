using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FreeSpaceLib
{
    static public class ExceptionLogger
    {
        static public void Log(Exception exception, string location )
        {
            using (StreamWriter writer = new StreamWriter("Log.txt",true))
            {
                writer.WriteLine();
                writer.WriteLine("An error orcurred in {0}", location);
                writer.WriteLine("Exception: " + exception.Message );
            }
        }
    }
}
