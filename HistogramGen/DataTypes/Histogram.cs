using System.Text;

namespace HistogramGen.DataTypes;

public class Histogram<TKey, TData>
    where TKey : notnull, IComparable, IComparable<TKey>
    where TData : notnull{
    public SortedDictionary<HistogramBinRange<TKey>, List<TData>> histogram = new();
    public TKey GlobalStart => histogram.Keys.First().start;
    public TKey GlobalEnd => histogram.Keys.Last().end;
    public int BinCount => histogram.Count;
    public int MaxBin => histogram.Max(x => x.Value.Count);
    public int MinBin => histogram.Min(x => x.Value.Count);
    public double AvgBin => histogram.Average(x => x.Value.Count);
    public override string ToString() => ToString(null);
    public string ToString(Func<TData, string>? dataPrinter) {
        StringBuilder sb = new();
        sb.Append($"Histogram, ");
        sb.Append($"Range=[{GlobalStart},{GlobalEnd}], ");
        sb.Append($"Count:{BinCount}, ");
        sb.Append($"Max:{MaxBin}, ");
        sb.Append($"Min:{MinBin} ");
        sb.Append($"Avg:{AvgBin}\n");
        foreach (var item in histogram) {
            sb.Append($"{item.Key.start}-{item.Key.end}\t {new string('█', item.Value.Count)}");
            if (dataPrinter != null) {
                StringBuilder sb2 = new StringBuilder();
                foreach (var value in item.Value) { sb2.Append("[" + dataPrinter(value) + "]"); }
                sb.Append($"\t{sb2.ToString()}");
            }
            sb.Append("\n");
        }
        return sb.ToString();
    }
}
