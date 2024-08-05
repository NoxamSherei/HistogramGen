using HistogramGen.DataTypes;

namespace HistogramGen;

public interface IPointOfInterestChartConverter<TKey, TData>
    where TKey : notnull, IComparable, IComparable<TKey>
    where TData : notnull {
    public Histogram<TKey, TData> ConvertToHistogram(LinkedList<KeyValuePair<TKey, PointOfInterest<TData>>> linkedList);
}
