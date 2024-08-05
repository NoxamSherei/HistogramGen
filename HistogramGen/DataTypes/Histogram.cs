using System.Text;

namespace HistogramGen.DataTypes;

public class Histogram<TKey, TData>
    where TKey : notnull, IComparable, IComparable<TKey>
    where TData : notnull {

    public SortedDictionary<HistogramBinRange<TKey>, List<TData>> histogramData = new();

    public TKey GlobalStart => histogramData.Keys.First().start;
    public TKey GlobalEnd => histogramData.Keys.Last().end;
    public int BinCount => histogramData.Count;
    public int MaxCountInBins => histogramData.Max(bin => bin.Value.Count);
    public int MinCountInBins => histogramData.Min(bin => bin.Value.Count);
    public double AverageCountInBins => histogramData.Average(bin => bin.Value.Count);

    public override string ToString() => ToString(null);
    public string ToString(Func<TData, string>? dataPrinter) {
        StringBuilder sb = new();
        sb.Append($"Histogram, ");
        sb.Append($"Range=[{GlobalStart},{GlobalEnd}], ");
        sb.Append($"Count:{BinCount}, ");
        sb.Append($"Max:{MaxCountInBins}, ");
        sb.Append($"Min:{MinCountInBins} ");
        sb.Append($"Avg:{AverageCountInBins}\n");
        foreach (var item in histogramData) {
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
