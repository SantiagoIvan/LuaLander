using UnityEngine;

public class LandingPad : MonoBehaviour
{
    [SerializeField] private int landerMultiplier = 1; // Multiplicador de puntaje para el lander que aterriza en este pad
    public int getLanderMultiplier()
    {
        return landerMultiplier;
    }
}
