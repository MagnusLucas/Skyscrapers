using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline)), RequireComponent(typeof(TMP_InputField))]
public class CellHighlighter : MonoBehaviour {

    private Color baseColor;
    private Color highlightColor;


    private void Awake() {

        TMP_InputField inputField = GetComponent<TMP_InputField>();
        Outline outline = GetComponent<Outline>();

        baseColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, 0);
        highlightColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, 1);

        outline.effectColor = baseColor;

        inputField.onEndEdit.AddListener((_) => outline.effectColor = baseColor);
        inputField.onSelect.AddListener((_) => outline.effectColor = highlightColor);
    }

}
