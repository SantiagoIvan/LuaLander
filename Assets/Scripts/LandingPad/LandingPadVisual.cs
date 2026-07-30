using UnityEngine;
using TMPro;

public class LandingPadVisual : MonoBehaviour
{
    // Referencia al componente TextMeshPro (mesh) que muestra el multiplicador
    [SerializeField] private TextMeshPro scoreMultiplier;

    private void Awake()
    {
        // Buscar en los hijos (incluyendo inactivos) el componente TextMeshPro (mesh)
        scoreMultiplier = GetComponentInChildren<TextMeshPro>(true);

        if (scoreMultiplier == null)
        {
            Debug.LogWarning($"TextMeshPro (mesh) not found in children of {name}");
        }

        scoreMultiplier.text = "x" + GetComponent<LandingPad>().getLanderMultiplier();
    }
}
