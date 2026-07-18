namespace RoundRobinScheduler;

using ICommand;
using IScheduler;
public class RoundRobinScheduler : IScheduler
{
    private readonly Queue<ICommand> queue = new();
    private readonly object lockObj = new();

    public bool HasCommand()
    {
        lock (lockObj) { return queue.Count > 0; }
    }

    public ICommand Select()
    {
        lock (lockObj)
        {
            if (queue.Count == 0)
                throw new InvalidOperationException();
            return queue.Dequeue();
        }
    }

    public void Add(ICommand cmd)
    {
        lock (lockObj) { queue.Enqueue(cmd); }
    }
}
