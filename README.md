# RecommendationAlgorithm
A very rough prototype of an algorithm I want to use for a social media platform.

## Model Description & Variables
This is a Naive Bayes model that calculates the probability that a user has a target interest, given their current top 3 interests. It bases the probability on a dataset of the top 3 interests of other users (these are the predictor variables), and whether or not those users also have that target interest (the class label is either 1 when a user has the target interest or 0 when they don't).

## Accuracy
We know that there are patterns in what people are interested in. For example, it's more likely that someone is interested in programming if we know they like computers. This model can detect these patterns from user data and make a prediction based on it. Therefore, if you aren't so different from other people, this model will be fairly accurate. One problem, though, is it assumes that its three predictor variables are independent. Almost nothing in the real world is independent, and neither are your top 3 interests.

## Code & Tests
This solution has 2 projects: one for the actual algorithm and one for unit tests (the tests are very rough)
