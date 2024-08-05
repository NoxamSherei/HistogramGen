using System.Runtime.CompilerServices;

namespace HistogramGen.DataTypes;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TKey"></typeparam>
public class HistogramBinRange<TKey> :
    IComparable,
    IComparable<HistogramBinRange<TKey>>
    where TKey : notnull, IComparable, IComparable<TKey> {
    public TKey start { get; init; }
    public TKey end { get; init; }

    private const int isLower = -1;
    private const int isHigger = 1;
    private const int isEqual = 0;

    public HistogramBinRange(TKey start, TKey end) {
        this.start = start;
        this.end = end;
    }

    /// <summary>
    /// Compares this object to another object, returning an instance of System.Relation.
    /// Null is considered less than any instance.
    /// If object is not of type Double, this method throws an ArgumentException.
    /// Returns a value less than zero if this  object 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public int CompareTo(object? other) {
        if (other == null) {
            return isHigger;
        }

        if (other is HistogramBinRange<TKey> d) {
            return CompareTo(d);
        }

        throw new ArgumentException("Incorrect Types to Compare");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(HistogramBinRange<TKey>? other) {
        if (other == null) {
            return isHigger;
        }
        int compareToStart = start.CompareTo(other.start);
        int compareToEnd = end.CompareTo(other.end);

        if (compareToStart is isLower) return isLower;
        if (compareToEnd is isHigger) return isHigger;
        return isEqual;
    }
}
