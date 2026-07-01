using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridManager : MonoBehaviour {

    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Hint hintPrefab;
    private GridLayoutGroup gridLayoutGroup;

    private Dictionary<Vector2i, Cell> cells;

    private void Awake() {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();

        ClearChildren();
        Grid.GenerateGrid(5);
        GenerateChildren(Grid.Instance);
        HideSolution();
    }

    private void ClearChildren() {
        for(int i = transform.childCount - 1; i >= 0; i--) {
            Destroy(transform.GetChild(i).gameObject);
        }
        cells = null;
    }

    private void GenerateChildren(Grid grid) {

        gridLayoutGroup.constraintCount = grid.Size + 2;
        GameObject spacer = new GameObject("Spacer");
        spacer.AddComponent<RectTransform>();

        Instantiate(spacer, transform);
        for (int i = 0; i < grid.Size; i++) {
            Hint hint = Instantiate(hintPrefab, transform);
            hint.SetDirection(Hint.Direction.DOWN);
            hint.SetValue(Grid.Instance.GetHintValue(false, true, i));
        }
        Instantiate(spacer, transform);

        cells = new Dictionary<Vector2i, Cell>();

        IOrderedEnumerable<KeyValuePair<Vector2i, int>> query = grid.numberInCell.OrderBy((KeyValuePair<Vector2i, int> pair) => pair.Key.y).ThenBy(pair => pair.Key.x);

        foreach (KeyValuePair<Vector2i, int> pair in query) {
            if (pair.Key.x == 0) {
                Hint hint = Instantiate(hintPrefab, transform);
                hint.SetDirection(Hint.Direction.RIGHT);
                hint.SetValue(Grid.Instance.GetHintValue(true, true, pair.Key.y));
            }

            Cell cell = Instantiate(cellPrefab, transform);
            cells[pair.Key] = cell;
            cell.SetNumber(pair.Value);


            if (pair.Key.x == grid.Size - 1) {
                Hint hint = Instantiate(hintPrefab, transform);
                hint.SetDirection(Hint.Direction.LEFT);
                hint.SetValue(Grid.Instance.GetHintValue(true, false, pair.Key.y));
            }
        }

        Instantiate(spacer, transform);
        for (int i = 0; i < grid.Size; i++) {
            Hint hint = Instantiate(hintPrefab, transform);
            hint.SetDirection(Hint.Direction.UP);
            hint.SetValue(Grid.Instance.GetHintValue(false, false, i));
        }
        Instantiate(spacer, transform);
    }

    private void ShowSolution() {

    }

    private void HideSolution() {
        foreach(Cell cell in cells.Values) {
            cell.ClearNumber();
        }
    }

}
