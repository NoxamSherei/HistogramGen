namespace HistogramGen.DataTypes;

public class PointOfInterest<TData>
{
    public List<TData> Start { get; set; } = new();
    public List<TData> End { get; set; } = new();
}
