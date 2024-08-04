using HistogramGen;

namespace HistogramGen.UT;
[TestClass]
public class HistogramGeneratorTest {
    [TestMethod]
    public void HistogramForNumericValues() {
        List<TestClassObj> list = new() {
            new TestClassObj { Start = 1, End = 10  ,Name="name",Size=1},
            new TestClassObj { Start = 4, End = 6  ,Name="name",Size=1},
            new TestClassObj { Start = 2, End = 4  ,Name="name",Size=2},
            new TestClassObj { Start = 1, End = 2  ,Name="name",Size=4},
            new TestClassObj { Start = 5, End = 8  ,Name="name",Size=1},
            new TestClassObj { Start = 6, End = 9  ,Name="name",Size=5},
            new TestClassObj { Start = 2, End = 4  ,Name="name",Size=2},
            new TestClassObj { Start = 4, End = 8  ,Name="name",Size=1},
            new TestClassObj { Start = 12, End = 14  ,Name="name",Size=1},
        };
        var startKey = (TestClassObj x) => x.Start;
        var endKey = (TestClassObj x) => x.End;
        //TestExecution
        HistogramBuilder<int, TestClassObj> builder = new(startKey, endKey);
        var histogram = builder.BuildHistogram(list);

        //Verify
        Assert.IsNotNull(histogram);
        Assert.AreEqual(histogram.GlobalStart, 1);
        Assert.AreEqual(histogram.GlobalEnd, 14);
        Console.WriteLine(histogram.ToString(x => $"|{x.Name}|"));
    }

    [TestMethod]
    public void HistogramForDateTime() {
        DateTime dateTime = DateTime.Today;
        List<TestClassObjDates> list = new() {
            new TestClassObjDates { Start = dateTime.AddHours(1), End = dateTime.AddHours(10)  , Name="name", Size=1},
            new TestClassObjDates { Start = dateTime.AddHours(4), End = dateTime.AddHours(6)  , Name="name", Size=1},
            new TestClassObjDates { Start = dateTime.AddHours(2), End = dateTime.AddHours(4)  , Name="name", Size=2},
            new TestClassObjDates { Start = dateTime.AddHours(1), End = dateTime.AddHours(2)  , Name="name", Size=4},
            new TestClassObjDates { Start = dateTime.AddHours(5), End = dateTime.AddHours(8)  , Name="name", Size=1},
            new TestClassObjDates { Start = dateTime.AddHours(6), End = dateTime.AddHours(9)  , Name="name", Size=5},
            new TestClassObjDates { Start = dateTime.AddHours(2), End = dateTime.AddHours(4)  , Name="name", Size=2},
            new TestClassObjDates { Start = dateTime.AddHours(4), End = dateTime.AddHours(8)  , Name="name", Size=1},
            new TestClassObjDates { Start = dateTime.AddHours(14), End = dateTime.AddHours(16)  , Name="name", Size=1},
        };
        var startKey = (TestClassObjDates x) => x.Start;
        var endKey = (TestClassObjDates x) => x.End;

        //TestExecution
        HistogramBuilder<DateTime, TestClassObjDates> builder = new(startKey, endKey);
        var histogram = builder.BuildHistogram(list);

        //Verify
        Assert.IsNotNull(histogram);
        Assert.AreEqual(histogram.GlobalStart, dateTime.AddHours(1));
        Assert.AreEqual(histogram.GlobalEnd, dateTime.AddHours(16));
        Console.WriteLine(histogram.ToString(x => $"|{x.Name}|"));
    }
}
