using UnityEngine;

// Detalle de cada level, con la posicio inicial del Lander y el level
// Puede tener tambien otra info como score maximo, cantidad variable de cosas a spawnear y otras boludeces
public class GameLevel : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private Transform landerStartPosition;

    public Transform getLanderStartPosition()
    {
        return landerStartPosition;
    }
    public int getLevel()
    {
        return level;
    }
}
