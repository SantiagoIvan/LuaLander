using System;
using UnityEngine;

/// <summary>
/// Ejecuta una funcion cada X segundos, en loop, sin necesidad de usar Coroutines
/// ni que la clase que la llama sea un MonoBehaviour.
///
/// Uso:
///   PeriodicFunction.Create(() => Debug.Log("Tick!"), 2f);
///
/// Para detenerlo, guardate la referencia devuelta y llamala:
///   PeriodicFunction pf = PeriodicFunction.Create(MyMethod, 1f);
///   ...
///   pf.DestroySelf();
/// </summary>
public class PeriodicFunction : MonoBehaviour
{
    /// <summary>
    /// Crea el ejecutor periodico. Internamente instancia un GameObject "invisible"
    /// que se encarga de correr el timer, ya que una funcion estatica no puede
    /// tener Update() por si sola.
    /// </summary>
    /// <param name="action">Funcion a ejecutar cada vez que se cumple el intervalo.</param>
    /// <param name="timerValue">Intervalo en segundos entre cada ejecucion.</param>
    public static PeriodicFunction Create(Action action, float timerValue)
    {
        GameObject gameObject = new GameObject("PeriodicFunction", typeof(PeriodicFunction));
        PeriodicFunction periodicFunction = gameObject.GetComponent<PeriodicFunction>();
        periodicFunction.Setup(action, timerValue);
        return periodicFunction;
    }

    private Action action;
    private float timerValue;
    private float timer;

    private void Setup(Action action, float timerValue)
    {
        this.action = action;
        this.timerValue = timerValue;
        this.timer = timerValue;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            // Se suma en vez de resetear directo a timerValue para no perder precision
            // si el frame se paso un poco del intervalo.
            timer += timerValue;
            action?.Invoke();
        }
    }

    /// <summary>Detiene y destruye el ejecutor periodico.</summary>
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}