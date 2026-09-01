using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Un slot individual dentro de InventoryUI. Representa un ItemType y cuantos tenes.
/// Va en un prefab con una Image (icono) y un TextMeshProUGUI (cantidad).
/// </summary>
public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;

    public void Setup(Sprite icon, int count)
    {
        if (iconImage != null)
        {
            iconImage.sprite = icon;
            iconImage.enabled = icon != null;
        }
        if (countText != null)
        {
            countText.text = "x" + count;
        }
    }
}
