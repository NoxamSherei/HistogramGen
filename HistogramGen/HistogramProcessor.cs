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
    /// <param name="processor"></param>
    /// <param name="converter"></param>
    public HistogramProcessor(IPointOfInterestChartProcessor<TKey, TData> processor, IPointOfInterestChartConverter<TKey, TData> converter) {
        Processor = processor;
        Converter = converter;
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