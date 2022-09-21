using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;
public class TestTaskCode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestFunc();
    }

    private async void TestFunc()
    {
        Debug.Log("0");
        var task = AsyncTest();
        Debug.Log("8");
        await task;
        Debug.Log("9");
    }

    private async Task AsyncTest()
    {
        Debug.Log("1");
        await Task.Run(async () => {
            for (int i = 0; i < 3; i++)
            {
                await Task.Delay(1000);
                Debug.Log($"{i}/2");
            }
        });
        Debug.Log("2");
    }

    private async void TestTask()
    {
        Debug.Log("coffee is ready");

        //Task test = async () => {
        //    await Task.Delay(1);
        //    Debug.Log("Egg 0");
        //};
        var testA = TestA();
        await testA;
        Debug.Log("TestA 2");

        var eggsTask = FryEggs(2);
        Debug.Log("eggs are ready");

        var baconTask = FryBacon(3);
        Debug.Log("bacon is ready");

        var tasks = new List<Task> { eggsTask, baconTask };
        while (tasks.Count > 0)
        {
            Task finishedTask = await Task.WhenAny(tasks);
            
            tasks.Remove(finishedTask);
        }
        await baconTask;
        //var toastTask = ToastBread(2);
        //var toast = await toastTask;
        //Debug.Log("toast is ready");

        Debug.Log("Breakfast is ready!");
    }

    public void TestTaskCompletionSource()
    {
        //TaskCompletionSource<int> tcs1 = new TaskCompletionSource<int>();
        //Task<int> t1 = tcs1.Task;


        //Task.Factory.StartNew(() => {
        //    Thread.Sleep(100);
        //    tcs1.SetResult(15);
        //});

        //Debug.Log("Start1");
        Stopwatch sw = Stopwatch.StartNew();
        //Debug.Log("Start2");
        //int result = t1.Result;
        //Debug.Log("Start3");
        //sw.Stop();
        //Debug.Log("Start4");
        //Debug.Log(string.Format("(ElapsedTime={0}): t1.Result={1} (expected 15) ", sw.ElapsedMilliseconds, result)) ;



        TaskCompletionSource<int> tcs2 = new TaskCompletionSource<int>();
        Task<int> t2 = tcs2.Task;

        Task.Factory.StartNew(() =>
        {
            Thread.Sleep(500);
            tcs2.SetException(new InvalidOperationException("Simulated Exception"));
        });

        sw = Stopwatch.StartNew();
        try
        {
            int result = t2.Result;
            Debug.Log("t2.Result succeeded. THIS WAS NOT EXPECTED.");
        }
        catch(AggregateException e)
        {
            Debug.Log(string.Format("(ElapsedTime={0}): ", sw.ElapsedMilliseconds)) ;
            Debug.Log(string.Format("The following exceptions have been thrown by t2.Result: (THIS WAS EXPECTED)")) ;
            for (int j = 0; j < e.InnerExceptions.Count; j++)
            {
                Debug.Log(string.Format("\n-------------------------------------------------\n{0}", e.InnerExceptions[j].ToString())) ;
            }
        }

    }

    public void BasicTask()
    {
        Action<object> action = (obj) =>
        {
            Debug.Log(string.Format("Task={0}, obj={1}, Thread={2}", Task.CurrentId, obj, Thread.CurrentThread.ManagedThreadId));
        };

        Task t1 = new Task(action, "alpha");

        Task t2 = Task.Factory.StartNew(action, "beta");
        t2.Wait();

        t1.Start();
        Debug.Log(string.Format("t1 has been launched. (Main Thread={0})", Thread.CurrentThread.ManagedThreadId));
        t1.Wait();

        string taskData = "delta";
        Task t3 = Task.Run(() => {
            Debug.Log(string.Format("Task={0}, obj={1}, Thread={2}", Task.CurrentId, taskData, Thread.CurrentThread.ManagedThreadId));
        });
        t3.Wait();

        Task t4 = new Task(action, "gamma");
        t4.RunSynchronously();
        t4.Wait();
    }

    internal class Bacon { }
    internal class Coffee { }
    internal class Egg { }
    internal class Juice { }
    internal class Toast { }

    private static Juice PourOJ()
    {
        Debug.Log("Pouring orange juice");
        return new Juice();
    }

    private static void ApplyJam(Toast toast) =>
        Debug.Log("Putting jam on the toast");

    private static void ApplyButter(Toast toast) =>
        Debug.Log("Putting butter on the toast");

    private static async Task<Toast> ToastBread(int slices)
    {
        for (int slice = 0; slice < slices; slice++)
        {
            Debug.Log("Putting a slice of bread in the toaster");
        }
        Debug.Log("Start toasting...");
        await Task.Delay(3000);
        Debug.Log("Remove toast from toaster");

        return new Toast();
    }

    private static async Task<Bacon> FryBacon(int slices)
    {
        Debug.Log("Bacon 0");
        await Task.Delay(3000);
        Debug.Log("Bacon 1");

        return new Bacon();
    }

    private static async Task<Egg> FryEggs(int howMany)
    {
        Debug.Log("Egg 0");
        await Task.Delay(3000);
        Debug.Log("Egg 1");

        return new Egg();
    }
    private static async Task TestA()
    {
        Debug.Log("TestA 0");
        Task a = new Task(async () =>
        {
            Debug.Log("Delay 0");
            await Task.Delay(1000);
            Debug.Log("Delay 1");
        });
        a.Start();
        Debug.Log("Delay Start");
        await a;

        //await Task.Run(async () =>
        //{
        //    Debug.Log("Delay 2");
        //    await Task.Delay(1000);
        //    Debug.Log("Delay 3");
        //}).ConfigureAwait(true);
        
        Debug.Log("TestA 1");
        //await a;
    }

    private static Coffee PourCoffee()
    {
        Debug.Log("Pouring coffee");
        return new Coffee();
    }

}
public class TestTask
{
    public static async Task Main()
    {
        await Task.Run(() => {
            // Just loop.
            int ctr = 0;
            for (ctr = 0; ctr <= 10000; ctr++)
            { 
            }
            Debug.Log(string.Format("Finished {0} loop iterations", ctr));
        });
    }
}
