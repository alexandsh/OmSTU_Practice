namespace  HelpMe;


using System.Diagnostics;
using ScottPlot;
using task14;


class Program
{
    static double Measure(Action act, int runs)
    {
        act();
        double total = 0;
        var sw = new Stopwatch();
        for (int i = 0; i < runs; i++)
        {
            sw.Restart();
            act();
            sw.Stop();
            total += sw.Elapsed.TotalMilliseconds;
        }
        return total / runs;
    }

    static void Main()
    {
        double a = -100;
        double b = 100;
        Func<double, double> function = Math.Sin;
        double reference = 0.0;          
        double eps = 1e-4;               
        double[] steps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
        int runs = 7;
        int maxThreads = Math.Max(16, Environment.ProcessorCount * 2);
        double chosenStep = 0;
        double[] threadTimes = Array.Empty<double>();
        int optThreads = 0;
        double optMultiTime = 0;
        double singleTime = 0;

        foreach (var step in steps)
        {
            double value = DefiniteIntegral.SolveSingleThread(a, b, function, step);
            if (Math.Abs(value - reference) >= eps)
                continue;

            double sTime = Measure(() => DefiniteIntegral.SolveSingleThread(a, b, function, step), runs);

            var times = new double[maxThreads];
            for (int t = 1; t <= maxThreads; t++)
            {
                int tc = t;
                times[t - 1] = Measure(() => DefiniteIntegral.Solve(a, b, function, step, tc), runs);
            }

            int bestT = 2;
            double bestTime = times[1];
            for (int t = 2; t <= maxThreads; t++)
            {
                if (times[t - 1] < bestTime)
                {
                    bestTime = times[t - 1];
                    bestT = t;
                }
            }

            if (bestTime < sTime)
            {
                chosenStep = step;
                threadTimes = times;
                optThreads = bestT;
                optMultiTime = bestTime;
                singleTime = sTime;
                break;
            }
        }

        double speedupPercent = (singleTime - optMultiTime) / singleTime * 100.0;

        Console.WriteLine();
        Console.WriteLine($"Выбранный шаг разбиения: {chosenStep:0.0e+0}");
        Console.WriteLine($"Оптимальное число потоков: {optThreads}");
        Console.WriteLine($"Время однопоточной версии (SolveSingleThread): {singleTime:0.000} мс");
        Console.WriteLine($"Время лучшей многопоточной версии ({optThreads} потоков): {optMultiTime:0.000} мс");
        Console.WriteLine($"Выигрыш многопотока над однопотоком: {speedupPercent:0.0}%");

        var xs = new double[maxThreads];
        for (int t = 0; t < maxThreads; t++) xs[t] = t + 1;

        var plt = new Plot();
        plt.Add.Scatter(xs, threadTimes);

        var single = plt.Add.HorizontalLine(singleTime);
        single.LinePattern = LinePattern.Dashed;
        single.Color = Colors.Red;

        string projPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "performance.png"));
        plt.SavePng(projPath, 800, 600);
    }
}
