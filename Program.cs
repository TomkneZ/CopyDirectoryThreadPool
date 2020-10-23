using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace PoolsandThreads
{
    class Program
    {        
        static void Main(string[] args)
        {
            int threadCount;

            if (args.Length < 3 )
            {
                Console.WriteLine($"Not enough arguments {0} !", args.Length);
                return;
            }            
            
            try
            {
                int.TryParse(args[2], out threadCount);
            }
            catch
            {
                Console.WriteLine("Incorrect number of threads!");
                return;
            }
            
            TaskQueue tasksQueue;

            try
            {
                tasksQueue = new TaskQueue(threadCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            try
            {
                var directoryCopier = new DirectoryCopier(args[0], args[1], tasksQueue);
                directoryCopier.CopyDirectory();
                Console.WriteLine($"Copied files count: {directoryCopier.CountCopiedFiles()}");                
            }
             catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            } 
            finally
            {
                tasksQueue.Dispose();
            }
        }

    }
}