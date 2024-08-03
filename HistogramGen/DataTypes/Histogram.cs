using System.Drawing;
using System.Text;

namespace HistogramGen.DataTypes;
public class Histogram<TKey, TData>
    where TData : notnull
    where TKey : notnull
{
    public SortedDictionary<(TKey, TKey), List<TData>> histogram = new();
    public TKey GlobalStart => histogram.Keys.First().Item1;
    public TKey GlobalEnd => histogram.Keys.Last().Item2;
    public int GlobalCount => histogram.Count;
    public int Max => histogram.Max(x => x.Value.Count);
    public int Min => histogram.Min(x => x.Value.Count);
    public override string ToString() => ToString(null);
    public string ToString(Func<TData, string>? dataPrinter)
    {
        StringBuilder sb = new();
        sb.Append($"Histogram, Range=[{GlobalStart},{GlobalEnd}]\n");
        foreach (var item in histogram)
        {
            sb.Append($"{item.Key.Item1}-{item.Key.Item2}\t {new string('█', item.Value.Count)}");
            if (dataPrinter != null)
            {
                StringBuilder sb2 = new StringBuilder();
                foreach (var value in item.Value) { sb2.Append("[" + dataPrinter(value) + "]"); }
                sb.Append($"\t{sb2.ToString()}");
            }
            sb.Append("\n");
        }
        return sb.ToString();
    }
}
