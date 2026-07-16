namespace task17;

using System.Collections.Concurrent;
using ICommand;
using IScheduler;
using RoundRobinScheduler;
public class ServerThread
{
    private Thread? thread;
    private BlockingCollection<ICommand> queue = new();
    private bool needHardStop = false;
    private bool needSoftStop = false;
    private IScheduler scheduler = new RoundRobinScheduler();
    public void Start()
    {
        thread = new Thread(Work);
        thread.Start();
    }

    public void AddCommand(ICommand command)
    {
        queue.Add(command);
    }

    public void ServerJoin()
    {
        thread?.Join();
    }

    private void Work()
    {
        while (!needHardStop)
        {
            if (scheduler.HasCommand())
            {
                var cmd = scheduler.Select();
                cmd.Execute();
                if (!cmd.IsCompleted)
                    scheduler.Add(cmd);
            }
            else
            {
                ICommand command;
                try
                {
                    command = queue.Take();
                }
                catch
                {
                    break;
                }
                command.Execute();
                if (!command.IsCompleted)
                    scheduler.Add(command);

                if (needSoftStop && queue.Count == 0 && !scheduler.HasCommand())
                    needHardStop = true;
            }
        }
    }

    public void HardStop()
    {
        if (Thread.CurrentThread != thread)
            throw new InvalidOperationException("hardstop only from current thread");
        
        needHardStop = true;
        queue.CompleteAdding();
    }

    public void SoftStop()
    {
        if (Thread.CurrentThread != thread)
            throw new InvalidOperationException("softstop only from current thread");
        
        needSoftStop = true;
        queue.CompleteAdding();
    }
}

public class HardStop : ICommand
{
    private ServerThread server;
    public HardStop(ServerThread server) { this.server = server; }
    public bool IsCompleted => true;
    public void Execute() { server.HardStop(); }
}

public class SoftStop : ICommand
{
    private ServerThread  server;
    public SoftStop(ServerThread server) { this.server = server; }
    public bool IsCompleted => true;
    public void Execute() { server.SoftStop(); }
}
