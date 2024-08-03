using HistogramGen;

namespace HistogramGen.UT;

[TestClass]
public class HistogramGenerator {
    private class TestClassObj {
        public int Start { get; set; } = 0;
        public int End { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public int Size { get; set; } = 0;
    }
    private record TestRecordObj(int Start, int End, string Name);
    [TestMethod]
    public void FirstTest() {
        List<TestClassObj> list = new() {
            new TestClassObj { Start = 1, End = 10  ,Name="name",Size=1},
            new TestClassObj { Start = 4, End = 6  ,Name="name",Size=1},
            new TestClassObj { Start = 2, End = 4  ,Name="name",Size=2},
            new TestClassObj { Start = 1, End = 2  ,Name="name",Size=4},
            new TestClassObj { Start = 5, End = 8  ,Name="name",Size=1},
            new TestClassObj { Start = 6, End = 9  ,Name="name",Size=5},
            new TestClassObj { Start = 2, End = 4  ,Name="name",Size=2},
            new TestClassObj { Start = 4, End = 8  ,Name="name",Size=1},
        };
        var startKey = (TestClassObj x) => x.Start;
        var endKey = (TestClassObj x) => x.End;
        foreach (var item in list) {
            Console.WriteLine($"OBJ {item.Name} start={startKey(item)} end={endKey(item)}");
        }
        HistogramBuilder<int, TestClassObj> builder = new(startKey, endKey);
        var histogram = builder.BuildHistogram(list);

        Assert.IsNotNull(histogram);
        Assert.AreEqual(histogram.GlobalStart, 1);
        Assert.AreEqual(histogram.GlobalEnd, 10);
    }
}
