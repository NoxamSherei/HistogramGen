namespace HistogramGen.DataTypes;

internal class PointOfInterest<TData> where TData : notnull
{
    public List<TData> StartedElement { get; set; } = new();
    public List<TData> EndedElement { get; set; } = new();
}