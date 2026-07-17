namespace task18tests;

using Xunit;
using task17;
using ICommand;

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
}

