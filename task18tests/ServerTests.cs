namespace task18tests;

using System.Diagnostics;
using Xunit;
using task17;
using ICommand;

class TimedCommand : ICommand
{
    private int remaining;
    private readonly Stopwatch sw;
    private readonly List<double> times;
    private bool recorded;

    public TimedCommand(int length, Stopwatch sw, List<double> times)
    {
        remaining = length;
        this.sw = sw;
        this.times = times;
    }

    public bool IsCompleted => remaining <= 0;

    public void Execute()
    {
        if (remaining > 0)
        {
            Thread.SpinWait(20000);
            remaining--;
        }

        if (remaining <= 0 && !recorded)
        {
            recorded = true;
            lock (times)
                times.Add(sw.Elapsed.TotalMilliseconds);
        }
    }
}

class LongCommand : ICommand
{
    private int remaining;
    public LongCommand(int length) { remaining = length; }
    public bool IsCompleted => remaining <= 0;
    public void Execute()
    {
        if (remaining > 0)
            remaining--;
    }
}

class TestCmd : ICommand
{
    public bool flag = false;
    public bool IsCompleted { get; private set; } = false;
    public void Execute()
    {
        flag = true;
        IsCompleted = true;
    }
}

public class ServerTests
{
    [Fact]
    public void LongCommandExecuted()
    {
        var server = new ServerThread();
        var cmd1 = new LongCommand(3);
        var cmd2 = new LongCommand(2);

        server.Start();
        server.AddCommand(cmd1);
        server.AddCommand(cmd2);
        server.AddCommand(new SoftStop(server));
        server.ServerJoin();

        Assert.True(cmd1.IsCompleted);
        Assert.True(cmd2.IsCompleted);
    }

    [Fact]
    public void TestCmdExecuted()
    {
        var server = new ServerThread();
        var cmd = new TestCmd();

        server.Start();
        server.AddCommand(cmd);
        server.AddCommand(new SoftStop(server));
        server.ServerJoin();

        Assert.True(cmd.flag);
        Assert.True(cmd.IsCompleted);
    }

    [Fact]
    public void RenderProgressChart()
    {
        var times = new List<double>();
        var sw = Stopwatch.StartNew();

        var server = new ServerThread();
        server.Start();

        const int total = 200;
        var rng = new Random(42);
        for (int i = 0; i < total; i++)
            server.AddCommand(new TimedCommand(rng.Next(1, 30), sw, times));

        server.AddCommand(new SoftStop(server));
        server.ServerJoin();
        sw.Stop();

        Assert.Equal(total, times.Count);

        times.Sort();
        var xs = new double[total + 1];
        var ys = new double[total + 1];
        xs[0] = 0;
        ys[0] = 0;
        for (int i = 0; i < total; i++)
        {
            xs[i + 1] = times[i];
            ys[i + 1] = (i + 1) * 100.0 / total;
        }

        var plot = new ScottPlot.Plot();
        plot.Add.Scatter(xs, ys);
        plot.Title("Task completion progress vs time");
        plot.XLabel("Time, ms");
        plot.YLabel("Progress, %");

        var path = Path.Combine(FindProjectRoot(), "task18.png");
        plot.SavePng(path, 800, 600);

        Assert.True(File.Exists(path));
    }

    private static string FindProjectRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "practice2026.slnx")))
            dir = dir.Parent;
        return dir?.FullName ?? Directory.GetCurrentDirectory();
    }
}

