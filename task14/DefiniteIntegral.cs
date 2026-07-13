namespace task14;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsnumber)
    {
        var threads = new Thread[threadsnumber];
        var borderThread = (b - a) / threadsnumber;
        var res = 0.0;
        var barrier = new Barrier(threadsnumber);
        var locker = new object();

        for (int i = 0; i < threadsnumber; i++)
        {
            var tindex = i;
            threads[i] = new Thread(() =>
            {
                var bLeft = a + tindex * borderThread;
                var bRight = tindex == threadsnumber ? b : bLeft + borderThread;
                var tSum = 0.0;

                for (var j = bLeft; j < bRight; j += step)
                {
                    tSum += 0.5 * (function(j) + function(Math.Min(j + step, bRight))) * ((Math.Min(j + step, bRight)) - j);
                }

                barrier.SignalAndWait();

                lock (locker)
                {
                    res += tSum;
                }
            });
            threads[i].Start();
        }

        foreach (var t in threads)
        {
            t.Join();
        }
        return res;
    }

    public static double SolveSingleThread(double a, double b, Func<double, double> function, double step)
    {
        var res = 0.0;

        for (var i = a; i < b; i += step)
        {
            res +=  0.5 * (function(i) + function(Math.Min(i + step, b))) * (Math.Min(i + step, b) - i);
        }
        
        return res;
    }
}
