using System;
using System.Linq;
using BenchmarkDotNet;
using BenchmarkDotNet.Tasks;

namespace BenchmarkArrayLoop
{
  [BenchmarkTask(platform: BenchmarkPlatform.X64, jitVersion: BenchmarkJitVersion.LegacyJit)]
  public class BenchmarkBoundsCheckElimination
  {
    private static readonly int[] Array = Enumerable.Range(0, int.MaxValue / 1024).ToArray();
    public static int Result;

    [Benchmark]
    public static void Foreach()
    {
      foreach (var t in Array)
        Result = t;
    }

    [Benchmark]
    public static void ArrayLength()
    {
      // ReSharper disable once ForCanBeConvertedToForeach
      for (var i = 0; i < Array.Length; i++)
        Result = Array[i];
    }

    [Benchmark]
    public static void LocalLength()
    {
      var length = Array.Length;
      for (var i = 0; i < length; i++)
        Result = Array[i];
    }
  }

  public class Program
  {
    public static void Main(string[] args)
    {
      new BenchmarkRunner().RunCompetition(new BenchmarkBoundsCheckElimination());
      Console.WriteLine(BenchmarkBoundsCheckElimination.Result);
    }
  }
}
