using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
/// <summary>
/// Singleton por nivel: cada Level (prefab/escena) tiene su propio GameObject "Inventory"
/// con este componente. A proposito NO usa DontDestroyOnLoad, para que el inventario se
/// resetee solo al cargar un nivel nuevo, en vez de persistir entre niveles.
///
/// Generico para cualquier categoria de item (llaves, pociones, etc): todos comparten
/// el mismo diccionario y los mismos metodos, identificados por ItemType.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    // ItemType como clave: es un enum, un conjunto fijo y finito de valores con nombre,
    // hashea directo por el int subyacente (rapido) y no tiene riesgo de typos como un string.
    private readonly Dictionary<ItemType, int> itemCounts = new Dictionary<ItemType, int>();

    public event EventHandler<OnItemAddedEventArgs> OnItemAdded;
    public event EventHandler<OnItemUsedEventArgs> OnItemUsed;

    private void Awake()
    {
        Instance = this;
    }

    public void AddItem(ItemType type, int amount = 1)
    {
        itemCounts.TryGetValue(type, out int currentCount);
        itemCounts[type] = currentCount + amount;
        Debug.Log($"Item collected: {type}. Total {type}: {itemCounts[type]}");
        OnItemAdded?.Invoke(this, new OnItemAddedEventArgs { itemType = type });
    }

    public int GetItemCount(ItemType type)
    {
        itemCounts.TryGetValue(type, out int count);
        return count;
    }

    public bool HasItem(ItemType type)
    {
        return GetItemCount(type) > 0;
    }

    /// <summary>Descuenta una unidad del item dado (ej. al usar una llave para abrir una puerta, o tomar una pocion). Devuelve false si no tenias ninguna.</summary>
    public bool UseItem(ItemType type)
    {
        int currentCount = GetItemCount(type);
        if (currentCount <= 0)
        {
            return false;
        }
        itemCounts[type] = currentCount - 1;
        OnItemUsed?.Invoke(this, new OnItemUsedEventArgs { itemType = type });
        return true;
    }
    public string GetInventorySummary()
    {
        string summary = $"Inventory Summary:\nTotal items: {itemCounts.Values.Sum()}\n";
        foreach (var kvp in itemCounts)
        {
            summary += $"{kvp.Key}: {kvp.Value}\n";
        }
        return summary;
    }
    public int TotalItems() { return itemCounts.Count; }

    /// <summary>Snapshot de solo lectura de todo lo que hay en el inventario. Pensado para que la UI lo recorra al redibujarse.</summary>
    public IReadOnlyDictionary<ItemType, int> GetAllItems()
    {
        return itemCounts;
    }
}
