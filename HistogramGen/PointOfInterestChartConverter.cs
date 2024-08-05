using HistogramGen.DataTypes;

namespace HistogramGen;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TData"></typeparam>
public class PointOfInterestChartConverter<TKey, TData> : IPointOfInterestChartConverter<TKey, TData>
    where TKey : notnull, IComparable, IComparable<TKey>
    where TData : notnull {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    /// <param name="linkedList"></param>
    public Histogram<TKey, TData> ConvertToHistogram(LinkedList<KeyValuePair<TKey, PointOfInterest<TData>>> linkedList) {
        List<TData> currentElements = new();
        Histogram<TKey, TData> result = new();
        for (var it = linkedList.First; it?.Next != null; it = it.Next) {
            currentElements.AddRange(it.Value.Value.AppearedElements);
            HistogramBinRange<TKey> range = new(it.Value.Key, it.Next.Value.Key);
            result.histogramData.Add(range, currentElements.ToList());
            currentElements = currentElements.Except(it.Next.Value.Value.DisappearedElements).ToList();
        }
        return result;
    }
}
