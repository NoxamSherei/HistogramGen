using HistogramGen.DataTypes;

namespace HistogramGen;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TData"></typeparam>
public class HistogramBuilder<TKey, TData>
    where TKey : notnull, IComparable, IComparable<TKey>
    where TData : notnull {

    #region Properties

    public Func<TData, TKey> GetStartKey { get; set; }
    public Func<TData, TKey> GetEndKey { get; set; }
    //todo: write better naming
    public Func<TKey, TKey>? RoundToThreshold { get; set; } = null;
    public Func<TKey, TKey>? NextToThreshold { get; set; } = null;

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="getStartKey"></param>
    /// <param name="getEndKey"></param>
    public HistogramBuilder(Func<TData, TKey> getStartKey, Func<TData, TKey> getEndKey) {
        GetStartKey = getStartKey;
        GetEndKey = getEndKey;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="getStartKey"></param>
    /// <param name="getEndKey"></param>
    public HistogramBuilder(Func<TData, TKey> getStartKey, Func<TData, TKey> getEndKey, Func<TKey, TKey> roundToThreshold, Func<TKey, TKey> nextToThreshold) {
        GetStartKey = getStartKey;
        GetEndKey = getEndKey;
        RoundToThreshold = roundToThreshold;
        NextToThreshold = nextToThreshold;
    }

    #region Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public Histogram<TKey, TData> BuildHistogram(ICollection<TData> collection) {
        var listOfPointOfInterest = CollectPointOfInterest(collection);
        Histogram<TKey, TData> result = ConvertToHistogram(listOfPointOfInterest);
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    private LinkedList<KeyValuePair<TKey, PointOfInterest<TData>>> CollectPointOfInterest(ICollection<TData> collection) {
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
    /// <param name="result"></param>
    /// <param name="linkedList"></param>
    private Histogram<TKey, TData> ConvertToHistogram(LinkedList<KeyValuePair<TKey, PointOfInterest<TData>>> linkedList) {
        List<TData> currentElements = new();
        Histogram<TKey, TData> result = new();
        for (var it = linkedList.First; it?.Next != null; it = it.Next) {
            currentElements.AddRange(it.Value.Value.StartedElement);
            HistogramBinRange<TKey> range = new(it.Value.Key, it.Next.Value.Key );
            result.histogram.Add(range, currentElements.ToList());
            currentElements = currentElements.Except(it.Next.Value.Value.EndedElement).ToList();
        }
        return result;
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
            pointOfInterest.StartedElement.Add(item);
        }
        else {
            pointOfInterest.EndedElement.Add(item);
        }
    }

    #endregion
}