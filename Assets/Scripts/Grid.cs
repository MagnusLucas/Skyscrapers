using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid {

    public static event EventHandler OnGridChanged;

    public static Grid Instance { get; private set; }

    public int Size { get; private set; }
    public Dictionary<Vector2, int> numberInCell { get; private set; }

    public static void GenerateGrid(int gridSize) {
        Grid newGrid = new Grid();
        newGrid.Size = gridSize;

        for (int i = 0; i < gridSize; i++) {
            for (int j = 0; j < gridSize; j++) {
                newGrid.numberInCell[new Vector2(i, j)] = 0;
            }
        }

        OnGridChanged?.Invoke(Instance, EventArgs.Empty);
        Debug.Log(Instance);
    }

    private Grid() {
        numberInCell = new Dictionary<Vector2, int>();
        Instance = this;
    }

    public override string ToString() {
        string result = "\n";

        for (int i = 0; i < Size; i++) {
            for (int j = 0; j < Size; j++) {
                result += numberInCell[new Vector2(i, j)] + " ";
            }
            result += "\n";
        }

        return result;
    }


}
