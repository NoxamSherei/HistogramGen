namespace HistogramGen.DataTypes;

public class PointOfInterest<TData> where TData : notnull
{
    public List<TData> AppearedElements { get; set; } = new();
    public List<TData> DisappearedElements { get; set; } = new();
}