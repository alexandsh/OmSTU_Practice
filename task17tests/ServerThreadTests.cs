namespace task17tests;

using Xunit;
using task17;
using ICommand;

class Command : ICommand
{
    public bool flag = false;
    public void Execute()
    {
        flag = true;
    }
}

public class ServerThreadTests
{
    [Fact]
    public void HardStopCorrect()
    {
        var server = new ServerThread();
        var command1 = new Command();
        
        server.Start();
        server.AddCommand(command1);
        server.AddCommand(command1);
        server.AddCommand(new HardStop(server));
        server.ServerJoin();

        Assert.True(command1.flag);
    }

    [Fact]
    public void SoftStopCorrect()
    {
        var server = new ServerThread();
        var command1 = new Command();
        var command2 = new Command();
        
        server.Start();
        server.AddCommand(command1);
        server.AddCommand(new SoftStop(server));
        server.AddCommand(command2);
        server.ServerJoin();

        Assert.True(command1.flag);
        Assert.True(command2.flag);
    }    

}
