namespace task19tests;

using Xunit;
using task17;
using ICommand;

public class TestCommand : ICommand
{
    private readonly int id;
    public int Counter { get; private set; } = 0;
    public TestCommand(int id) { this.id = id; }
    public bool IsCompleted => Counter >= 3;
    public void Execute()
    {
        Console.WriteLine($"Поток {id} вызов {++Counter}");
    }
}

class StepCounter
{
    private int value = 0;
    public int Next() => Interlocked.Increment(ref value) - 1;
}

class GraphCommand : ICommand
{
    private readonly StepCounter counter;
    private int remaining;
    public int Id { get; }
    public List<int> Steps { get; } = new();

    public GraphCommand(int id, int length, StepCounter counter)
    {
        Id = id;
        remaining = length;
        this.counter = counter;
    }

    public bool IsCompleted => remaining <= 0;

    public void Execute()
    {
        Steps.Add(counter.Next());
        remaining--;
    }
}

public class SchedulerTests
{
    [Fact]
    public void FiveTestCommandsRunThreeTimesThenHardStop()
    {
        var server = new ServerThread();
        var commands = new TestCommand[5];
        for (int i = 0; i < commands.Length; i++)
            commands[i] = new TestCommand(i);

        server.Start();
        foreach (var command in commands)
            server.AddCommand(command);
        server.AddCommand(new HardStop(server));
        server.ServerJoin();

        foreach (var command in commands)
            Assert.Equal(3, command.Counter);
    }

    [Fact]
    public void BuildThroughStepGraph()
    {
        const int taskCount = 5;
        const int length = 4;

        var server = new ServerThread();
        var counter = new StepCounter();
        var commands = new GraphCommand[taskCount];
        for (int i = 0; i < taskCount; i++)
            commands[i] = new GraphCommand(i, length, counter);

        server.Start();
        foreach (var command in commands)
            server.AddCommand(command);
        server.AddCommand(new SoftStop(server));
        server.ServerJoin();

        double[] ids = commands.SelectMany(c => c.Steps.Select(_ => (double)c.Id)).ToArray();
        double[] steps = commands.SelectMany(c => c.Steps.Select(step => (double)step)).ToArray();

        Assert.Equal(taskCount * length, steps.Length);

        var plot = new ScottPlot.Plot();
        var scatter = plot.Add.ScatterPoints(ids, steps);
        scatter.MarkerSize = 12;
        plot.Title("Сквозной шаг от ид задачи");
        plot.XLabel("Ид задачи");
        plot.YLabel("Сквозной шаг");

        var path = Path.Combine(FindProjectRoot(), "task19.png");
        plot.SavePng(path, 800, 600);
    }

    private static string FindProjectRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, ".git")))
            dir = dir.Parent;
        return dir?.FullName ?? AppContext.BaseDirectory;
    }
}
