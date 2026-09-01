using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

/// <summary>
/// Muestra el contenido de InventoryManager en el Canvas. Se re-dibuja entero cada vez
/// que el inventario cambia (simple: destruye los slots viejos e instancia los nuevos),
/// en vez de tratar de actualizar cada slot individualmente.
/// </summary>
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryItem inventoryItemPrefab;
    [SerializeField] private ItemIconDatabase iconDatabase; // asset compartido, mismo en todos los Levels
    [SerializeField] private RectTransform background;
    [SerializeField] private float backgroundMargin = 20f;
    private float itemHeight;
    private InventoryManager inventory;

    // Slots instanciados por este script. Se usa para saber exactamente que borrar en
    // cada refresh, en vez de asumir que "todos los hijos de itemsContainer son mios" --
    // Background tambien puede vivir ahi (con Layout Element > Ignore Layout) y no queremos destruirlo.
    private readonly List<InventoryItem> spawnedItems = new List<InventoryItem>();

    private void Start()
    {
        inventory = InventoryManager.Instance;
        inventory.OnItemAdded += InventoryManager_OnItemAdded;
        inventory.OnItemUsed += InventoryManager_OnItemUsed;

        // El GridLayoutGroup le impone su propio Cell Size a cada item instanciado, pisando
        // el sizeDelta que tenga el prefab -- por eso hay que leer la altura de ahi, no del prefab.
        GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();
        itemHeight = gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y;

        ShowInventory();
    }

    // OnItemAddedEventArgs y OnItemUsedEventArgs no heredan de EventArgs (son clases sueltas),
    // asi que no se puede usar un solo handler compartido tipo (object, EventArgs) por contravarianza
    // de delegados -- hace falta un metodo por cada tipo de evento, aunque hagan lo mismo.
    private void InventoryManager_OnItemAdded(object sender, OnItemAddedEventArgs e)
    {
        ShowInventory();
    }

    private void InventoryManager_OnItemUsed(object sender, OnItemUsedEventArgs e)
    {
        ShowInventory();
    }

    private void ShowInventory()
    {
        Debug.Log(inventory.GetInventorySummary());
        // Limpiar solo los slots que instancie yo, no todo lo que haya en itemsContainer
        foreach (InventoryItem item in spawnedItems)
        {
            Destroy(item.gameObject);
        }
        spawnedItems.Clear();

        foreach (KeyValuePair<ItemType, int> entry in inventory.GetAllItems())
        {
            if (entry.Value <= 0)
            {
                continue; // no mostrar items que llegaron a 0 (ej. se gastaron todos)
            }

            InventoryItem item = Instantiate(inventoryItemPrefab, gameObject.transform);
            item.Setup(iconDatabase.GetIcon(entry.Key), entry.Value);
            spawnedItems.Add(item);
        }

        UpdateBackgroundHeight();
    }

    private void UpdateBackgroundHeight()
    {
        if (background == null)
        {
            return;
        }

        bool hasItems = spawnedItems.Count > 0;
        background.gameObject.SetActive(hasItems);

        if (!hasItems)
        {
            return; // no hace falta tocar el sizeDelta de algo que esta oculto
        }

        float newHeight = itemHeight * spawnedItems.Count + backgroundMargin;
        Vector2 size = background.sizeDelta;
        size.y = newHeight;
        background.sizeDelta = size;

        // El borde superior del Background esta anclado en el mismo punto donde arrancan
        // los items (Anchor/Pivot Y = 1 en ambos), asi que el margen agregado en la altura
        // solo queda abajo. Subo el Background la mitad del margen para repartirlo simetrico
        // arriba y abajo de la columna de items.
        Vector2 position = background.anchoredPosition;
        position.y = backgroundMargin / 2f;
        background.anchoredPosition = position;
    }
}
