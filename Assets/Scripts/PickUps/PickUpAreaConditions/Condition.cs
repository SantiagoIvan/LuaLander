using UnityEngine;

/// <summary>
/// Component base para condiciones evaluables por PickUpArea antes de contar el tiempo
/// de permanencia (ej. "tiene la llave X"). Hereda de MonoBehaviour, no es una clase C#
/// plana, para poder vivir como componente en un GameObject del prefab y ser arrastrada
/// al Inspector en el List&lt;Condition&gt; de PickUpArea.
/// </summary>
public abstract class Condition : MonoBehaviour
{
    public abstract bool verify();
}
