// See https://aka.ms/new-console-template for more information
using System.Threading.Tasks;

Console.WriteLine("Hello, World!");
List<Task<int>> tasks = [];
CancellationTokenSource cancellationTokenS = new CancellationTokenSource();

for (int i = 0; i < 100; i++)
{
    tasks.Add(RunTimesAsync(i, i, cancellationTokenS.Token));
}


// after tasks completed, print errors
foreach (Task task in tasks)
    _ = task.ContinueWith(t => { if (t.IsFaulted) { Console.WriteLine(t.Exception.Message); } });


try
{
    Task.WaitAny([..tasks]); // wait for at least one task to be completed (optional)
    cancellationTokenS.Cancel(); // cancels all requests to save resources (optional)
    Task.WaitAll([.. tasks]); // wait for all requests to be either completed or cancelled to end the asynchronicity
}
catch
{
}

Console.Write("Completed Tasks Ended with = ");
foreach (Task<int> task in tasks)
    if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
        Console.Write(task.Result + " ");

Console.WriteLine(".");
Console.WriteLine("All Tasks Completed.. phew...");


/**
 * runs loop from x to y inclusive.
 * cancellation token provides an option to cancel the process
 * every line on the given function is syncronous except `await Task.Delay(200, cancellationToken).ConfigureAwait(false);`
 * function throws an error if provided range has any value < 10
 */
static async Task<int> RunTimesAsync(int from, int to, CancellationToken cancellationToken)
{
    
    // await Task.Yield(); 
    for (int i = from; i <= to; i++)
    {
        await Task.Delay(200, cancellationToken).ConfigureAwait(false);
        if (i < 10)
        {
            throw new Exception($"RUN TIMES {i} FAILED");
        }
        Console.WriteLine(i);
    }
    return to;
}