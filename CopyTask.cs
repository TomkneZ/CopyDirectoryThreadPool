using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace PoolsandThreads
{
    class CopyTask
    {
        public string destinationFilePath;
        public string sourceFilePath;
        public enum CopyStatus
        {
            Pending,
            Success,
            Failed
        }
        public CopyStatus Status;

        public CopyTask (string src, string dest)
        {
            sourceFilePath = src;
            destinationFilePath = dest;
            Status = CopyStatus.Pending;
        }

        public void CopyFile()
        {
            try
            {
                File.Copy(sourceFilePath, destinationFilePath, true);                
                Status = CopyStatus.Success;
            }
            catch
            {
                Status = CopyStatus.Failed;
            }
            
        }
    }
}
