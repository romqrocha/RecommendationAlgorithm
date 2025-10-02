using RecommendationAlgorithm;

namespace AlgorithmTest;

public class UnitTests
{
    private readonly FileInfo testFile = new($"{ProjectSourcePath.Value}/Data/data.csv");
    private readonly string[] predictorVariables = ["Hiking", "Caving", "Badminton"];
    private const string desiredClassLabel = "1";
    private CsvProvider CsvProvider => new(testFile);
    private InterestPredictor NaiveBayesPredictor =>
        new(CsvProvider, predictorVariables, desiredClassLabel);

    [Fact]
    public void CsvHeaderIsValidTest()
    {
        var provider = CsvProvider;
        bool validHeaderLength = provider.data.header.Length == 4;

        Assert.True(validHeaderLength);
    }

    [Fact]
    public void CsvDataIsValidTest()
    {
        var rows = CsvProvider.data.rows;

        bool validRowLengths = true;
        bool validClassLabels = true;

        foreach (var r in rows)
        {
            if (r.Length != 4)
            {
                validRowLengths = false;
                break;
            }

            string classLabel = r[3];
            if (!classLabel.Equals("0") && !classLabel.Equals("1"))
            {
                validClassLabels = false;
                break;
            }
        }

        Assert.True(validRowLengths);
        Assert.True(validClassLabels);
    }

    [Fact]
    public void CsvDataHasEnoughRowsTest()
    {
        var data = CsvProvider.data;

        const int minRows = 100;

        bool enoughRows = data.rows.Length >= minRows;

        Assert.True(enoughRows);
    }

    [Fact]
    public void ClassCountsAreCorrectTest()
    {
        var p = NaiveBayesPredictor;
        p.Predict();
        var counts = p.classLabelCounts;

        bool correct = counts["0"] == 80 && counts["1"] == 20;

        Assert.True(correct);
    }

    [Fact]
    public void JointCountsAreCorrectTest()
    {
        var p = NaiveBayesPredictor;
        p.Predict();
        var countsHas = p.predictorJointCountsYes;
        var countsDoesntHave = p.predictorJointCountsNo;

        // (after laplacian smoothing)
        bool correctHas =
            countsHas[predictorVariables[0]] == 11
            && countsHas[predictorVariables[1]] == 3
            && countsHas[predictorVariables[2]] == 6;
        bool correctDoesntHave =
            countsDoesntHave[predictorVariables[0]] == 47
            && countsDoesntHave[predictorVariables[1]] == 8
            && countsDoesntHave[predictorVariables[2]] == 22;

        Assert.True(correctHas && correctDoesntHave);
    }

    [Fact]
    public void PredictionIsValidTest()
    {
        var p = NaiveBayesPredictor;
        Dictionary<string, double> probabilities = p.Predict();

        double total = 0;
        foreach (double prob in probabilities.Values)
        {
            total += prob;
        }

        Assert.Equal(1.0, total, 0.0001);
    }

    [Fact]
    public void RegressionTest()
    {
        var p = NaiveBayesPredictor;
        Dictionary<string, double> probabilities = p.Predict();

        Assert.Equal(0.2194, probabilities["1"], 0.0001);
        Assert.Equal(0.7805, probabilities["0"], 0.0001);
    }
}
