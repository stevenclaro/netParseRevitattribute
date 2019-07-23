using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Core;
using log4net.Appender;
using log4net.Layout;

namespace includelog4CodefirstRevit
{
    public class Logger
    {
        public static void Setup()
        {
            //includelog4CodefirstRevit.tools.FileOperatation.fileInit();
            //因为每次调试的时候，一旦存在这个文件，就不会追加打开。所以每次执行之前，先把历史的文件删除的

            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender();
            roller.AppendToFile = false;
            roller.File = @"EventLogByrevit.txt";//这个是C:\Program Files\Autodesk\Revit 2017\EventLogbysjkhello.txt
            roller.Layout = patternLayout;
            roller.AppendToFile = true;
            

            roller.MaxSizeRollBackups = 5;
            roller.MaximumFileSize = "1GB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = Level.Debug;//我把它调整为Debug，这样它后面的errror也会被显示。但是在一个物理的文件中，级别不一样
            hierarchy.Configured = true;
        }
    }
}
