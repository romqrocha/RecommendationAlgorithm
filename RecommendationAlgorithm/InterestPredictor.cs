namespace RecommendationAlgorithm;

public class InterestPredictor
{
    private readonly CsvProvider dataProvider;
    private readonly string[] predictorVariables;
    private readonly string desiredClassLabel;
    public Dictionary<string, int> predictorJointCountsYes;
    public Dictionary<string, int> predictorJointCountsNo;
    public Dictionary<string, int> classLabelCounts;

    public InterestPredictor(CsvProvider provider, string[] predictorVars, string desiredLabel)
    {
        dataProvider = provider;
        predictorVariables = predictorVars;
        desiredClassLabel = desiredLabel;
        predictorJointCountsYes = [];
        predictorJointCountsNo = [];
        classLabelCounts = [];
        foreach (string classLabel in provider.data.classLabels)
        {
            classLabelCounts.Add(classLabel, 0);
        }
        foreach (string predictor in predictorVars)
        {
            predictorJointCountsYes.Add(predictor, 0);
            predictorJointCountsNo.Add(predictor, 0);
        }
    }

    public Dictionary<string, double> Predict()
    {
        string[][] rows = dataProvider.data.rows;

        foreach (string[] row in rows)
        {
            foreach (string predictor in predictorJointCountsYes.Keys)
            {
                if (row.Contains(predictor) && row.Last().Equals(desiredClassLabel))
                {
                    // matches predictor and class label is "1"
                    predictorJointCountsYes[predictor]++;
                }
                else if (row.Contains(predictor))
                {
                    // matches predictor and class label is "0"
                    predictorJointCountsNo[predictor]++;
                }
            }
            // counting total class labels
            string classLabel = row.Last();
            classLabelCounts[classLabel]++;
        }

        foreach (string predictor in predictorJointCountsYes.Keys)
        {
            // laplacian smoothing
            predictorJointCountsYes[predictor]++;
            predictorJointCountsNo[predictor]++;
        }

        Dictionary<string, double> evidenceTerms = [];
        foreach (string classLabel in classLabelCounts.Keys)
        {
            Dictionary<string, int> jointCounts = classLabel.Equals(desiredClassLabel)
                ? predictorJointCountsYes
                : predictorJointCountsNo;

            double currEvidenceTerm = 1.0;
            foreach (int predictorCount in jointCounts.Values)
            {
                // +targetPredictors.Length to account for Laplacian smoothing
                int totalTargetCount = classLabelCounts[classLabel] + predictorVariables.Length;
                currEvidenceTerm *= (double)predictorCount / totalTargetCount;
            }
            currEvidenceTerm *= (double)classLabelCounts[classLabel] / rows.Length;
            evidenceTerms[classLabel] = currEvidenceTerm;
        }

        // normalized constant
        double evidence = 0;
        foreach (double evidenceTerm in evidenceTerms.Values)
        {
            evidence += evidenceTerm;
        }

        Dictionary<string, double> probabilities = [];
        foreach (string classLabel in evidenceTerms.Keys)
        {
            probabilities[classLabel] = evidenceTerms[classLabel] / evidence;
        }

        return probabilities;
    }
}
