using System.Data;

namespace HistogramGen;

public class HistogramProcessorBuilder<TKey, TData>
    where TKey : notnull, IComparable, IComparable<TKey>
    where TData : notnull {

    //todo: write better naming
    public Func<TData, TKey>? GetStartKeyFunc { get; private set; } = null;
    public Func<TData, TKey>? GetEndKeyFunc { get; private set; } = null;
    public Func<TKey, TKey>? RoundToThresholdFunc { get; private set; } = null;
    public Func<TKey, TKey>? NextToThresholdFunc { get; private set; } = null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="getStartKeyFunc"></param>
    /// <param name="getEndKeyFunc"></param>
    /// <returns></returns>
    public HistogramProcessorBuilder<TKey, TData> SetGetRangeKeyFunc(Func<TData, TKey> getStartKeyFunc, Func<TData, TKey> getEndKeyFunc) {
        GetStartKeyFunc = getStartKeyFunc;
        GetEndKeyFunc = getEndKeyFunc;
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="getStartKeyFunc"></param>
    /// <returns></returns>
    public HistogramProcessorBuilder<TKey, TData> SetGetKeyFunc(Func<TData, TKey> getStartKeyFunc) {
        GetStartKeyFunc = getStartKeyFunc;
        GetEndKeyFunc = null;
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roundToThresholdFunc"></param>
    /// <param name="nextToThresholdFunc"></param>
    /// <returns></returns>
    public HistogramProcessorBuilder<TKey, TData> SetGroupingFunction(Func<TKey, TKey> roundToThresholdFunc, Func<TKey, TKey> nextToThresholdFunc) {
        RoundToThresholdFunc = roundToThresholdFunc;
        NextToThresholdFunc = nextToThresholdFunc;
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public HistogramProcessorBuilder<TKey, TData> ClearGroupingFunction() {
        RoundToThresholdFunc = null;
        NextToThresholdFunc = null;
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public HistogramProcessor<TKey, TData> Build() {
        if (GetStartKeyFunc is null) {
            throw new ArgumentNullException();
        }
        if (RoundToThresholdFunc is null ^ NextToThresholdFunc is null) {
            throw new ArgumentNullException();
        }

        IPointOfInterestChartProcessor<TKey, TData> preHistogramProcessor;
        IPointOfInterestChartConverter<TKey, TData> histogramConverter;

        if (GetEndKeyFunc is null) {
            preHistogramProcessor = new PointOfInterestChartProcessorForSingleElement<TKey, TData>(GetStartKeyFunc, RoundToThresholdFunc, NextToThresholdFunc);
            histogramConverter = new PointOfInterestChartConverterForSingleElement<TKey, TData>();
        }
        else {
            preHistogramProcessor = new PointOfInterestChartProcessor<TKey, TData>(GetStartKeyFunc, GetEndKeyFunc, RoundToThresholdFunc, NextToThresholdFunc);
            histogramConverter = new PointOfInterestChartConverter<TKey, TData>();
        }


        return new HistogramProcessor<TKey, TData>(preHistogramProcessor, histogramConverter);
    }
}
