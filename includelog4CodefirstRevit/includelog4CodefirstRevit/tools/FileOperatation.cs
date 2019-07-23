using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace includelog4CodefirstRevit.tools
{
 public static   class FileOperatation
    {
        public static void fileInit()
        {
            string filename = @"C:\Program Files\Autodesk\Revit 2018\EventLogByrevit1.txt";
            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }

        }
    }
}
