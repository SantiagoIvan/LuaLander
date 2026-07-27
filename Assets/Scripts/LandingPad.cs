using UnityEngine;

public class LandingPad : MonoBehaviour
{
    public int landerMultiplier = 1; // Multiplicador de puntaje para el lander que aterriza en este pad
    public int getLanderMultiplier()
    {
        return landerMultiplier;
    }
}
