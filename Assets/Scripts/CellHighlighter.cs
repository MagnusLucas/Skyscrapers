using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline)), RequireComponent(typeof(TMP_InputField))]
public class CellHighlighter : MonoBehaviour {


    private TMP_InputField inputField;
    private Outline outline;

    private Color baseColor;
    private Color highlightColor;


    private void Awake() {

        inputField = GetComponent<TMP_InputField>();
        outline = GetComponent<Outline>();

        baseColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, 0);
        highlightColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, 1);

        outline.effectColor = baseColor;

        inputField.onSelect.AddListener((_) => Highlight());
        inputField.onDeselect.AddListener((_) => ClearHighlight());
    }

    private void Highlight() {
        outline.effectColor = highlightColor;
    }
    private void ClearHighlight() {
        outline.effectColor = baseColor;
    }

    private void OnDestroy() {
        if (inputField != null) {
            inputField.onSelect?.RemoveListener((_) => Highlight());
            inputField.onDeselect?.RemoveListener((_) => ClearHighlight());
        }
    }

}
