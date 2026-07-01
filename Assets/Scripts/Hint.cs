using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Hint : MonoBehaviour {
    
    public enum Direction {
        LEFT,
        UP,
        RIGHT,
        DOWN
    }

    [SerializeField] Transform arrowHookPoint;

    public void SetDirection(Direction direction) {
        arrowHookPoint.Rotate(Vector3.forward, -(int)direction * 90);
    }

}
