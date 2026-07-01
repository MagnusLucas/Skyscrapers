using TMPro;
using UnityEngine;

public class Hint : MonoBehaviour {
    
    public enum Direction {
        LEFT,
        UP,
        RIGHT,
        DOWN
    }

    [SerializeField] Transform arrowHookPoint;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;

    public void SetDirection(Direction direction) {
        arrowHookPoint.Rotate(Vector3.forward, -(int)direction * 90);
    }

    public void SetValue(int value) {
        textMeshProUGUI.text = value.ToString();
    }

}
