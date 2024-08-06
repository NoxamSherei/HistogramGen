using HistogramGen.DataTypes;

namespace HistogramGen;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TData"></typeparam>
public class PointOfInterestChartProcessor<TKey, TData> : IPointOfInterestChartProcessor<TKey, TData>
    where TKey : notnull, IComparable, IComparable<TKey>
    where TData : notnull {

    public Func<TData, TKey> GetStartKey { get; set; }
    public Func<TData, TKey> GetEndKey { get; set; }
    //todo: write better naming
    public Func<TKey, TKey>? RoundToThreshold { get; set; } = null;
    public Func<TKey, TKey>? NextToThreshold { get; set; } = null;

    public PointOfInterestChartProcessor(Func<TData, TKey> getStartKey, Func<TData, TKey> getEndKey, Func<TKey, TKey>? roundToThreshold, Func<TKey, TKey>? nextToThreshold) {
        GetStartKey = getStartKey;
        GetEndKey = getEndKey;
        RoundToThreshold = roundToThreshold;
        NextToThreshold = nextToThreshold;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public LinkedList<KeyValuePair<TKey, PointOfInterest<TData>>> CollectPointOfInterest(ICollection<TData> collection) {
        LinkedList<KeyValuePair<TKey, PointOfInterest<TData>>> linkedList;
        SortedDictionary<TKey, PointOfInterest<TData>> pointOfInterests = new();
        foreach (var item in collection) {
            TKey start = GetStartKey(item);
            TKey end = GetEndKey(item);
            if (RoundToThreshold is not null && NextToThreshold is not null) {
                start = RoundToThreshold(start);
                end = RoundToThreshold(end);
                if (start.Equals(end)) {
                    end = NextToThreshold!(end);
                }
            }

            AddPointToChart(pointOfInterests, item, start, true);
            AddPointToChart(pointOfInterests, item, end, false);
        }
        linkedList = new(pointOfInterests);
        return linkedList;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="PointOfInterests"></param>
    /// <param name="item"></param>
    /// <param name="key"></param>
    /// <param name="isStart"></param>
    private void AddPointToChart(SortedDictionary<TKey, PointOfInterest<TData>> PointOfInterests, TData item, TKey key, bool isStart) {
        if (PointOfInterests.TryGetValue(key, out var pointOfInterest)) {
            AddPointToPoints(item, isStart, pointOfInterest);
        }
        else {
            pointOfInterest = new PointOfInterest<TData>();
            AddPointToPoints(item, isStart, pointOfInterest);
            PointOfInterests.Add(key, pointOfInterest);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="isStart"></param>
    /// <param name="pointOfInterest"></param>
    private void AddPointToPoints(TData item, bool isStart, PointOfInterest<TData> pointOfInterest) {
        if (isStart) {
            pointOfInterest.AppearedElements.Add(item);
        }
        else {
            pointOfInterest.DisappearedElements.Add(item);
        }
    }

}

/// <summary>
/// 
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TData"></typeparam>
public class PointOfInterestChartProcessorForSingleElement<TKey, TData> : IPointOfInterestChartProcessor<TKey, TData>
    where TKey : notnull, IComparable, IComparable<TKey>
    where TData : notnull {

    public Func<TData, TKey> GetStartKey { get; set; }
    //todo: write better naming
    public Func<TKey, TKey>? RoundToThreshold { get; set; } = null;
    public Func<TKey, TKey>? NextToThreshold { get; set; } = null;

    public PointOfInterestChartProcessorForSingleElement(Func<TData, TKey> getStartKey, Func<TKey, TKey>? roundToThreshold, Func<TKey, TKey>? nextToThreshold) {
        GetStartKey = getStartKey;
        RoundToThreshold = roundToThreshold;
        NextToThreshold = nextToThreshold;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public LinkedList<KeyValuePair<TKey, PointOfInterest<TData>>> CollectPointOfInterest(ICollection<TData> collection) {
        LinkedList<KeyValuePair<TKey, PointOfInterest<TData>>> linkedList;
        SortedDictionary<TKey, PointOfInterest<TData>> pointOfInterests = new();
        foreach (var item in collection) {
            TKey start = GetStartKey(item);
            TKey end = GetStartKey(item);
            if (RoundToThreshold is not null && NextToThreshold is not null) {
                start = RoundToThreshold(start);
                end = RoundToThreshold(end);
                if (start.Equals(end)) {
                    end = NextToThreshold!(end);
                }
            }

            AddPointToChart(pointOfInterests, item, start, true);
            AddPointToChart(pointOfInterests, item, end, false);
        }
        linkedList = new(pointOfInterests);
        return linkedList;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="PointOfInterests"></param>
    /// <param name="item"></param>
    /// <param name="key"></param>
    /// <param name="isStart"></param>
    private void AddPointToChart(SortedDictionary<TKey, PointOfInterest<TData>> PointOfInterests, TData item, TKey key, bool isStart) {
        if (PointOfInterests.TryGetValue(key, out var pointOfInterest)) {
            AddPointToPoints(item, isStart, pointOfInterest);
        }
        else {
            pointOfInterest = new PointOfInterest<TData>();
            AddPointToPoints(item, isStart, pointOfInterest);
            PointOfInterests.Add(key, pointOfInterest);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="isStart"></param>
    /// <param name="pointOfInterest"></param>
    private void AddPointToPoints(TData item, bool isStart, PointOfInterest<TData> pointOfInterest) {
        if (isStart) {
            pointOfInterest.AppearedElements.Add(item);
        }
        else {
            pointOfInterest.DisappearedElements.Add(item);
        }
    }

}
