using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoolsandThreads
{
    class TaskQueue : IDisposable
    {
        private readonly Thread[] threads;
        public delegate void TaskDelegate();
        private readonly ConcurrentQueue<TaskDelegate> taskQueue = new ConcurrentQueue<TaskDelegate>();
        private bool work = true;

        public TaskQueue(int threadCount)
        {
            if (threadCount <= 0)
            {
                throw new Exception("Incorrect thread count");
            }

            threads = new Thread[threadCount];
            for (var i = 0; i < threadCount; i++)
            {
                var thread = new Thread(new ThreadStart(Run));
                threads[i] = thread;
                thread.Start();
            }
        }

        public void EnqueueTask(TaskDelegate task)
        {
            taskQueue.Enqueue(task);
        }

        public void Run()
        {
            while (work)
            {
                //TaskDelegate task = GetTask();
                //task();
                GetTask()();
            }
        }

        private TaskDelegate GetTask()
        {
            TaskDelegate task;
            while (!taskQueue.TryDequeue(out task))
                Thread.Sleep(100);
            return task;
        }

        public void Dispose()
        {
            foreach (Thread th in threads)
            {
                th.Interrupt();
            }
        }

        public void Wait()
        {
            foreach (Thread th in threads)
            {
                th.Join();
            }
        }
    }
}
