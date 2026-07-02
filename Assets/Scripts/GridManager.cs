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
        //HideSolution();
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

        for (int y = 0; y < Grid.Instance.Size; y++) {
            Hint hint = Instantiate(hintPrefab, transform);
            hint.SetDirection(Hint.Direction.RIGHT);
            hint.SetValue(Grid.Instance.GetHintValue(true, true, y));

            for (int x = 0; x < Grid.Instance.Size; x++) {
                Cell cell = Instantiate(cellPrefab, transform);
                cells[new Vector2i(x, y)] = cell;
                cell.SetNumber(Grid.Instance.NumberInCell[new Vector2i(x, y)]);
            }

            hint = Instantiate(hintPrefab, transform);
            hint.SetDirection(Hint.Direction.LEFT);
            hint.SetValue(Grid.Instance.GetHintValue(true, false, y));
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
