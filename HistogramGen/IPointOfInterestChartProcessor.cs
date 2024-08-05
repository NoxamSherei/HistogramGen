using HistogramGen.DataTypes;

namespace HistogramGen;

public interface IPointOfInterestChartProcessor<TKey, TData>
    where TKey : notnull, IComparable, IComparable<TKey>
    where TData : notnull {
    public LinkedList<KeyValuePair<TKey, PointOfInterest<TData>>> CollectPointOfInterest(ICollection<TData> collection);
}
