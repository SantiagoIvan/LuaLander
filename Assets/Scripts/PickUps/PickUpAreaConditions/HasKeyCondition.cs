using UnityEngine;

/// <summary>
/// Condition generica para "tiene la llave de tal color en el inventario". Una sola clase
/// para las 3 puertas (azul/roja/verde) en vez de una subclase por color -- cada prefab
/// variant de puerta solo configura distinto el campo requiredKey.
/// </summary>
public class HasKeyCondition : Condition
{
    [SerializeField] private ItemType requiredKey;

    public override bool verify()
    {
        return InventoryManager.Instance.HasItem(requiredKey);
    }
}
