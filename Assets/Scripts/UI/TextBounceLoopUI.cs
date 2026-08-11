using TMPro;
using UnityEngine;

/// <summary>
/// Anima letra por letra un texto TextMeshPro (o TextMeshProUGUI) con un
/// bounce vertical + escala en loop, tipo titulo "Victoria".
/// Colocar en el mismo GameObject que el componente TMP_Text.
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class TextBounceLoopUI : MonoBehaviour
{
    [Header("Bounce (movimiento vertical)")]
    [SerializeField] private float bounceHeight = 10f;
    [SerializeField] private float bounceSpeed = 5f;

    [Header("Escala (agrandar / achicar)")]
    [SerializeField] private float scaleAmount = 0.15f; // +/- sobre la escala base
    [SerializeField] private float scaleSpeed = 5f;

    [Header("Desfasaje entre letras")]
    [SerializeField] private float letterDelay = 0.15f; // que tan "en cascada" se ve la ola

    private TMP_Text textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        AnimateText();
    }

    private void AnimateText()
    {
        textMesh.ForceMeshUpdate();
        TMP_TextInfo textInfo = textMesh.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
                continue;

            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

            // Centro del caracter (para escalar desde su propio centro, no desde el origen)
            Vector3 charMid = (vertices[vertexIndex + 0] + vertices[vertexIndex + 2]) / 2f;

            float offset = i * letterDelay;

            // Bounce vertical
            // Time.unscaledTime sirve para cuando ganas o perdes y tiras un Time.scale 0 para que nada se mueva. Bueno me pasaba que las letras se quedaban congeladas
            // Asi que usamos Time.unscaledTime para que no dependa del scale.
            float yOffset = Mathf.Sin(Time.unscaledTime * bounceSpeed + offset) * bounceHeight;
            yOffset = Mathf.Abs(yOffset); // que rebote hacia arriba, no que baje del baseline

            // Escala pulsante
            float scale = 1f + Mathf.Sin(Time.unscaledTime * scaleSpeed + offset) * scaleAmount;

            for (int v = 0; v < 4; v++)
            {
                Vector3 vertex = vertices[vertexIndex + v];
                vertex -= charMid;               // llevar al origen local del caracter
                vertex *= scale;                 // escalar
                vertex += charMid;                // devolver a su posicion
                vertex.y += yOffset;              // aplicar bounce
                vertices[vertexIndex + v] = vertex;
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}