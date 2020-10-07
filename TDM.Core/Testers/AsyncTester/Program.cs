using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTester
{
    class Program
    {
        static async Task Main()
        {
            var tasks = new List<Task>();

            for(var i = 0; i < 2000; i++)
            {
                //var num = i;
                //var task = Task.Factory.StartNew((async () => await FooAsync(num)), CancellationToken.None, TaskCreationOptions.RunContinuationsAsynchronously, TaskScheduler.FromCurrentSynchronizationContext());
                var task = FooAsync(i);
                tasks.Add(task);
            }
            
            await Task.WhenAll(tasks.ToArray());
        }

        static async Task FooAsync(int num)
        {
            //Thread.Sleep(100);
            //Console.WriteLine($"{num}: Starting on thread {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(1000);
            Console.WriteLine($"{num}: Ending on thread {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(100);
        }
    }
}
