using UnityEngine;

// Detalle de cada level, con la posicio inicial del Lander y el level
// Puede tener tambien otra info como score maximo, cantidad variable de cosas a spawnear y otras boludeces
public class GameLevel : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private Transform landerStartPosition;
    [SerializeField] private Transform cameraStartPosition;
    [SerializeField] private float zoomedOutOrthographicSize;

    public Transform getLanderStartPosition()
    {
        return landerStartPosition;
    }
    public int getLevel()
    {
        return level;
    }
    public Transform getCameraStartPosition() { return cameraStartPosition; }
    public float getZoomedOutOrthographicSize() {  return zoomedOutOrthographicSize; }
}
