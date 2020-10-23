using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoolsandThreads
{
    class DirectoryCopier
    {
        private List<CopyTask> copyTasks = new List<CopyTask>();
        private readonly TaskQueue taskQueue;

       public DirectoryCopier(string sourcePath, string destinationPath, TaskQueue tskQueue)
        {
            taskQueue = tskQueue;
            if (!Directory.Exists(sourcePath))
            {
                throw new Exception("Source directory doesn't exist");
            }
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }            
            EnqeueCopyTask(sourcePath, destinationPath);
        }

        private void EnqeueCopyTask(string src, string dest)
        {
            string[] files = Directory.GetFiles(src);
            foreach (string file in files)
            {
                string sourceFilePath = Path.GetFullPath(file);
                string destinationFilePath = Path.GetFullPath(dest) +@"\"+ Path.GetFileName(file);
                var task = new CopyTask(sourceFilePath, destinationFilePath);
                copyTasks.Add(task);
            }

            string[] dirs = Directory.GetDirectories(src);

            foreach (string dir in dirs)
            {                
                string sourceSubDirPath = Path.GetFullPath(dir);                
                string destinationSubDirPath = Path.GetFullPath(dest) + @"\" + Path.GetFileName(dir);
                
                if (!Directory.Exists(destinationSubDirPath))
                {                   
                    Directory.CreateDirectory(destinationSubDirPath);
                }
                EnqeueCopyTask(sourceSubDirPath, destinationSubDirPath);
            }                      
        }

        public int CountCopiedFiles()
        {
            return copyTasks.Count;
        }

        public void CopyDirectory()
        {
            foreach (CopyTask task in copyTasks)
            {
                taskQueue.EnqueueTask(task.CopyFile);
            }            
            if (!AllTasksCompleted())
            {
                Thread.Sleep(100);
            }
        }
       
        private bool AllTasksCompleted()
        {
            foreach (CopyTask task in copyTasks)
            {
                if (task.Status == CopyTask.CopyStatus.Pending)
                    return false;
            }
            return true;
        }
    }
}
