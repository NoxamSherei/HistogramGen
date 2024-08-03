using HistogramGen.DataTypes;

namespace HistogramGen;
/// <summary>
/// 
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TData"></typeparam>
public class HistogramBuilder<TKey, TData>
    where TData : notnull
    where TKey : notnull {

    #region Properties

    public Func<TData, TKey> StartKey { get; set; }
    public Func<TData, TKey> EndKey { get; set; }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startKey"></param>
    /// <param name="endKey"></param>
    public HistogramBuilder(Func<TData, TKey> startKey, Func<TData, TKey> endKey) {
        StartKey = startKey;
        EndKey = endKey;
    }

    #region Methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public Histogram<TKey, TData> BuildHistogram(ICollection<TData> collection) {
        var listOfPointOfInterest = PreparePointOfInterestCollection(collection);
        Histogram<TKey, TData> result = new();
        ConvertToHistogram(result, listOfPointOfInterest);
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    private LinkedList<KeyValuePair<TKey, PointOfInterest<TData>>> PreparePointOfInterestCollection(ICollection<TData> collection) {
        LinkedList<KeyValuePair<TKey, PointOfInterest<TData>>> linkedList;
        SortedDictionary<TKey, PointOfInterest<TData>> pointOfInterests = new();
        foreach (var item in collection) {
            TKey start = StartKey(item);
            TKey end = EndKey(item);
            AddPoint(pointOfInterests, item, start, true);
            AddPoint(pointOfInterests, item, end, false);
        }
        linkedList = new(pointOfInterests);
        return linkedList;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    /// <param name="linkedList"></param>
    private void ConvertToHistogram(Histogram<TKey, TData> result, LinkedList<KeyValuePair<TKey, PointOfInterest<TData>>> linkedList) {
        List<TData> currentElements = new();
        for (var it = linkedList.First; it?.Next != null; it = it.Next) {
            currentElements.AddRange(it.Value.Value.Start);
            (TKey start, TKey end) range = new(it.Value.Key, it.Next.Value.Key);
            result.histogram.Add(range, currentElements.ToList());
            currentElements = currentElements.Except(it.Next.Value.Value.End).ToList();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="PointOfInterests"></param>
    /// <param name="item"></param>
    /// <param name="key"></param>
    /// <param name="isStart"></param>
    private void AddPoint(SortedDictionary<TKey, PointOfInterest<TData>> PointOfInterests, TData item, TKey key, bool isStart) {
        if (PointOfInterests.TryGetValue(key, out var pointOfInterest)) {
            AddElem(item, isStart, pointOfInterest);
        }
        else {
            pointOfInterest = new PointOfInterest<TData>();
            AddElem(item, isStart, pointOfInterest);
            PointOfInterests.Add(key, pointOfInterest);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="isStart"></param>
    /// <param name="pointOfInterest"></param>
    private void AddElem(TData item, bool isStart, PointOfInterest<TData> pointOfInterest) {
        if (isStart) {
            pointOfInterest.Start.Add(item);
        }
        else {
            pointOfInterest.End.Add(item);
        }
    }
    #endregion
}