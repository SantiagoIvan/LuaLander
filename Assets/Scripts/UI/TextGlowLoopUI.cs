using TMPro;
using UnityEngine;

/// <summary>
/// Hace pulsar en loop el Glow del material de un texto TMP (requiere que el
/// material de la fuente tenga el shader Distance Field con Glow habilitado
/// en el Inspector del material).
/// Colocar en el mismo GameObject que el TextMeshProUGUI (o TextMeshPro).
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class TextGlowLoopUI : MonoBehaviour
{
    [Header("Pulso de Glow")]
    [SerializeField] private float minGlow = 0f;
    [SerializeField] private float maxGlow = 0.8f;
    [SerializeField] private float glowSpeed = 3f;
    private bool active = false;

    private TMP_Text textMesh;
    private Material material;

    private void Awake()
    {
        textMesh = GetComponent<TMP_Text>();

        // fontMaterial (no fontSharedMaterial) crea una instancia unica del material
        // para este componente, asi no afecta a otros textos que usen el mismo asset.
        material = textMesh.fontMaterial;

        if (!material.HasProperty(ShaderUtilities.ID_GlowPower))
        {
            Debug.LogWarning($"El material de {gameObject.name} no tiene la propiedad Glow. " +
                              "Verificá que el shader sea 'TextMeshPro/Distance Field' y que el foldout Glow este habilitado.");
        }

        
    }

    private void OnEnable()
    {
        if (material != null)
        {
            material.EnableKeyword(ShaderUtilities.Keyword_Glow);
        }
    }

    private void OnDisable()
    {
        if (material != null)
        {
            material.DisableKeyword(ShaderUtilities.Keyword_Glow);
        }
    }

    private void Update()
    {
        if (!active) return;
        // Time.unscaledTime para que siga pulsando aunque el juego este en pausa (Time.timeScale = 0)
        float t = (Mathf.Sin(Time.unscaledTime * glowSpeed) + 1f) / 2f; // normaliza el seno a 0..1
        float glow = Mathf.Lerp(minGlow, maxGlow, t);

        material.SetFloat(ShaderUtilities.ID_GlowPower, glow);
    }

    private void OnDestroy()
    {
        // Limpieza: si se creo una instancia de material, la destruimos para no dejar basura en memoria
        if (material != null)
        {
            Destroy(material);
        }
    }
    public void setTitle(string newTitle)
    {
        this.textMesh.text = newTitle;
    }
}