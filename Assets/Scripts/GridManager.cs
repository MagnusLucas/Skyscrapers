using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridManager : MonoBehaviour {

    [SerializeField] private Cell cellPrefab;
    private GridLayoutGroup gridLayoutGroup;

    private Dictionary<Vector2, Cell> cells;

    private void Awake() {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();

        ClearChildren();
        Grid.GenerateGrid(5);
        GenerateChildren(Grid.Instance);
    }

    private void ClearChildren() {
        for(int i = transform.childCount - 1; i >= 0; i--) {
            Destroy(transform.GetChild(i).gameObject);
        }
        cells = null;
    }

    private void GenerateChildren(Grid grid) {
        cells = new Dictionary<Vector2, Cell>();

        gridLayoutGroup.constraintCount = grid.Size;
        foreach (KeyValuePair<Vector2, int> pair in grid.numberInCell) {
            Cell cell = Instantiate(cellPrefab, transform);
            cells[pair.Key] = cell;
            cell.SetNumber(pair.Value);
        }
    }

}
