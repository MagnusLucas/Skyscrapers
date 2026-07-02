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
            AddHint(Direction.DOWN, i);
        }
        Instantiate(spacer, transform);


        cells = new Dictionary<Vector2i, Cell>();

        for (int y = 0; y < Grid.Instance.Size; y++) {
            AddHint(Direction.RIGHT, y);
            for (int x = 0; x < Grid.Instance.Size; x++) {
                AddCell(new Vector2i(x, y));
            }
            AddHint(Direction.LEFT, y);
        }

        Instantiate(spacer, transform);
        for (int i = 0; i < grid.Size; i++) {
            AddHint(Direction.UP, i);
        }
        Instantiate(spacer, transform);
    }

    private void AddCell(Vector2i position) {
        Cell cell = Instantiate(cellPrefab, transform);
        cells[position] = cell;
        cell.SetNumber(Grid.Instance.NumberInCell[position]);
    }

    private void AddHint(Direction direction, int lineNumber) {
        Hint hint = Instantiate(hintPrefab, transform);
        hint.SetDirection(direction);
        hint.SetValue(Grid.Instance.GetHintValue(direction, lineNumber));
    }

    private void ShowSolution() {
        for (int i = 0; i < Grid.Instance.Size; i++) {
            for (int j = 0; j < Grid.Instance.Size; j++) {
                Vector2i position = new Vector2i(i, j);
                cells[position].SetNumber(Grid.Instance.NumberInCell[position]);
            }
        }
    }

    private void HideSolution() {
        foreach(Cell cell in cells.Values) {
            cell.ClearNumber();
        }
    }

}
