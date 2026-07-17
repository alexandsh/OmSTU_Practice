namespace task17;

using System.Collections.Concurrent;
using ICommand;
public class ServerThread
{
    private Thread? thread;
    private BlockingCollection<ICommand> queue = new();
    private bool needHardStop = false;
    private bool needSoftStop = false;

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
            ICommand command;
            try
            {
                command = queue.Take();
            }
            catch
            {
                break;
            }
            
            try
            {
                command.Execute();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle?.Invoke(e, command);
            }

            if (needSoftStop && queue.Count == 0)
            {
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
    public void Execute() { server.HardStop(); }
}

public class SoftStop : ICommand
{
    private ServerThread  server;
    public SoftStop(ServerThread server) { this.server = server; }
    public void Execute() { server.SoftStop(); }
}
