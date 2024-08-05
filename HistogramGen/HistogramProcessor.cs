using HistogramGen.DataTypes;

namespace HistogramGen;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TData"></typeparam>
public class HistogramProcessor<TKey, TData>
    where TKey : notnull, IComparable, IComparable<TKey>
    where TData : notnull {

    public IPointOfInterestChartProcessor<TKey, TData> Processor { get; init; }
    public IPointOfInterestChartConverter<TKey, TData> Converter { get; init; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="getStartKey"></param>
    /// <param name="getEndKey"></param>
    public HistogramProcessor(Func<TData, TKey> getStartKey, Func<TData, TKey> getEndKey) {
        Processor = new PointOfInterestChartProcessor<TKey, TData>(getStartKey, getEndKey, null, null);
        Converter = new PointOfInterestChartConverter<TKey, TData>();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="getStartKey"></param>
    /// <param name="getEndKey"></param>
    public HistogramProcessor(Func<TData, TKey> getStartKey) {
        Processor = new PointOfInterestChartProcessor<TKey, TData>(getStartKey, null, null, null);
        Converter = new PointOfInterestChartConverter<TKey, TData>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="getStartKey"></param>
    /// <param name="getEndKey"></param>
    /// <param name="roundToThreshold"></param>
    /// <param name="nextToThreshold"></param>
    public HistogramProcessor(Func<TData, TKey> getStartKey, Func<TData, TKey> getEndKey, Func<TKey, TKey> roundToThreshold, Func<TKey, TKey> nextToThreshold) {
        Processor = new PointOfInterestChartProcessor<TKey, TData>(getStartKey, getEndKey, roundToThreshold, nextToThreshold);
        Converter = new PointOfInterestChartConverter<TKey, TData>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public Histogram<TKey, TData> BuildHistogram(ICollection<TData> collection) {
        var listOfPointOfInterest = Processor.CollectPointOfInterest(collection);
        return Converter.ConvertToHistogram(listOfPointOfInterest);
    }
}