// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using HistogramGen;

[MemoryDiagnoser]
[RPlotExporter]
public class HistogramBenchmark {

    internal record TestRecordObj(int Start, int End, string Name);
    private HistogramProcessor<int, TestRecordObj> _histogramBuilder = new(x => x.Start, x => x.End);
    private List<TestRecordObj> _data;

    [Params(10, 1_000, 10_000)]
    public int N;

    [GlobalSetup]
    public void Setup() {
        Random rnd = new Random(42);
        _data = new List<TestRecordObj>();
        for (int i = 0; i < N; i++) {
            int start = rnd.Next(0, 1000);
            int end = rnd.Next(0, 1000);
            _data.Add(new TestRecordObj(start, end, $"T-{i}"));
        }
    }

    [Benchmark]
    public void RunHistogram() => _histogramBuilder.BuildHistogram(_data);
}