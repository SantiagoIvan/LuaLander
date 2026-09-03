using UnityEngine;

/// <summary>
/// Genera puntos random sobre el perimetro de un rectangulo que envuelve lo que ve
/// la camara en este instante, expandido "margin" unidades hacia afuera. Pensado para
/// llamarse en cada spawn (no una sola vez), ya que la camara sigue al Lander y se mueve.
/// 
/// Tambien genera puntos random dentro de la camara, alrededor del Lander (Lander.Instance.transform.position).
/// </summary>
public class ScreenSpawnPointGenerator : MonoBehaviour
{
    public static ScreenSpawnPointGenerator Instance { get; private set; }

    [SerializeField] private Camera targetCamera;
    [SerializeField] private float margin = 2f;
    [SerializeField] private float playerSpawnMargin = 4f;

    private void Awake()
    {
        Instance = this;
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    public Vector2 GetRandomPointOutsideCamera()
    {
        float halfHeight = targetCamera.orthographicSize + margin;
        float halfWidth = halfHeight * targetCamera.aspect;

        Vector2 center = targetCamera.transform.position;
        float minX = center.x - halfWidth;
        float maxX = center.x + halfWidth;
        float minY = center.y - halfHeight;
        float maxY = center.y + halfHeight;

        // Elijo uno de los 4 lados al azar, y un punto random a lo largo de ese lado
        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0: // arriba
                return new Vector2(Random.Range(minX, maxX), maxY);
            case 1: // abajo
                return new Vector2(Random.Range(minX, maxX), minY);
            case 2: // izquierda
                return new Vector2(minX, Random.Range(minY, maxY));
            default: // derecha (case 3)
                return new Vector2(maxX, Random.Range(minY, maxY));
        }
    }

    public Vector2 GetRandomPointAroundPlayer()
    {
        Vector2 center = Lander.Instance.transform.position;

        return new Vector2(
            Random.Range(center.x - playerSpawnMargin, center.x + playerSpawnMargin),
            Random.Range(center.y - playerSpawnMargin, center.y + playerSpawnMargin)
        );
    }
}
