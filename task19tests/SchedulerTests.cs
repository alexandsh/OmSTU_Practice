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
}
