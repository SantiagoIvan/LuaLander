using UnityEngine;

public class ScoreCalculator
{
    public static float getScore(float landingVelocity, float landingAngle, float maxLandingVelocity, float maxLandingAngle, int landerMultiplier)
    {
        // Calculate the score based on landing velocity and angle
        float velocityScore = Mathf.Clamp01(1 - (landingVelocity / maxLandingVelocity));
        float angleScore = Mathf.Clamp01(1 - (landingAngle / maxLandingAngle));
        // Combine the scores (you can adjust the weighting as needed)
        float totalScore = (velocityScore + angleScore) / 2;
        // Scale the score to a range of 0 to 100
        return Mathf.Round(totalScore * 100 * landerMultiplier);
    }
}
