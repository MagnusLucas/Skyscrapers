using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour {

    [SerializeField] private TMP_InputField inputField;

    public void SetNumber(int number) {
        inputField.text = number.ToString();
    }

}
