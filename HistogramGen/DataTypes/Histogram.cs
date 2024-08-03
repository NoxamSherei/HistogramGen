using System.Text;

namespace HistogramGen.DataTypes;

public class Histogram<TKey, TData>
    where TData : notnull
    where TKey : notnull {
    public SortedDictionary<(TKey, TKey), List<TData>> histogram = new();
    public TKey GlobalStart => histogram.Keys.First().Item1;
    public TKey GlobalEnd => histogram.Keys.Last().Item2;
    public int TowersCount => histogram.Count;
    public int TowerMaxCount => histogram.Max(x => x.Value.Count);
    public int TowerMinCount => histogram.Min(x => x.Value.Count);
    public double TowerAverageCount => histogram.Average(x => x.Value.Count);
    public override string ToString() => ToString(null);
    public string ToString(Func<TData, string>? dataPrinter) {
        StringBuilder sb = new();
        sb.Append($"Histogram, ");
        sb.Append($"Range=[{GlobalStart},{GlobalEnd}], ");
        sb.Append($"Tower Count:{TowersCount}, ");
        sb.Append($"Tower Max:{TowerMaxCount}, ");
        sb.Append($"Tower Min:{TowerMinCount} ");
        sb.Append($"Avg:{TowerAverageCount}\n");
        foreach (var item in histogram) {
            sb.Append($"{item.Key.Item1}-{item.Key.Item2}\t {new string('█', item.Value.Count)}");
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
