using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Mapeo compartido ItemType -> Sprite. Vive como asset (no como campo de un componente
/// puntual) para que todos los InventoryUI de todos los Levels apunten al mismo lugar,
/// en vez de tener que configurar la misma lista de iconos en cada prefab de nivel.
/// </summary>
[CreateAssetMenu(fileName = "ItemIconDatabase", menuName = "Inventory/Item Icon Database")]
public class ItemIconDatabase : ScriptableObject
{
    [SerializeField] private List<ItemIcon> icons;

    [Serializable]
    private class ItemIcon
    {
        public ItemType type;
        public Sprite icon;
    }

    public Sprite GetIcon(ItemType type)
    {
        foreach (ItemIcon entry in icons)
        {
            if (entry.type == type)
            {
                return entry.icon;
            }
        }
        return null;
    }
}
