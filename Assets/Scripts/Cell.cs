using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour {

    [SerializeField] private TMP_InputField inputField;

    private void Start() {
        inputField.onSelect.AddListener((_) => inputField.text = "");
    }

    public void SetNumber(int number) {
        inputField.text = number.ToString();
    }

    public void ClearNumber() {
        inputField.text = "";
    }

}
