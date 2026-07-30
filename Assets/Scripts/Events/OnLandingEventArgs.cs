using System;

public class OnLandingEventArgs : EventArgs
{
    public LandingResult landingResult;
    public float landingSpeed;
    public float landingAngle;
    public int multiplier;
    public int finalScore;
}