using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 10; // Valor de la moneda


    public int getValue()
    {
        return value;
    }

    public void getCollected()
    {
        // disparar animacion de particulas y sonido
        Destroy(gameObject);
    }
}
