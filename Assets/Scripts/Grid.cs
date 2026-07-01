using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid {

    public static event EventHandler OnGridChanged;

    public static Grid Instance { get; private set; }


    public int Size { get; private set; }
    public Dictionary<Vector2i, int> numberInCell { get; private set; }

    public static void GenerateGrid(int gridSize) {
        while (true) {
            if (TryGenerateGrid(gridSize)) break;
        }
    }

    private static bool TryGenerateGrid(int gridSize) {
        Grid newGrid = new Grid();
        newGrid.Size = gridSize;

        for (int i = 0; i < gridSize; i++) {
            List<int> availableNumbers = AvailableNumbers(gridSize);
            for (int j = 0; j < gridSize; j++) {
                try {
                    int pickedNumber = FindAcceptableNumber(new Vector2i(i, j), new List<int>(availableNumbers), newGrid.numberInCell);
                    newGrid.numberInCell[new Vector2i(i, j)] = pickedNumber;
                    availableNumbers.Remove(pickedNumber);
                }catch(Exception e) {
                    Debug.LogException(e);
                    return false;
                }
            }
        }

        OnGridChanged?.Invoke(Instance, EventArgs.Empty);
        return true;
    }

    private static List<int> AvailableNumbers(int gridSize) {
        List<int> availableNumbers = new List<int>();
        for (int i = 1; i <= gridSize; i++) {
            availableNumbers.Add(i);
        }

        System.Random rng = new System.Random();

        List<int> shuffled = availableNumbers.OrderBy(_ => rng.Next()).ToList();
        return shuffled;
    }

    private static int FindAcceptableNumber(Vector2i position, List<int> possibleNumbers, Dictionary<Vector2i, int> setNumbers) {
        if (possibleNumbers.Count < 1) {
            throw new Exception("No possible number!");
        }

        int candidate = possibleNumbers[0];
        if (IsNumberAcceptable(position, candidate, setNumbers)) return candidate;

        possibleNumbers.RemoveAt(0);

        return FindAcceptableNumber(position, possibleNumbers, setNumbers);
    }

    private static bool IsNumberAcceptable(Vector2i position, int number, Dictionary<Vector2i, int> setNumbers) {
        foreach (KeyValuePair<Vector2i, int> pair in setNumbers) {
            if (pair.Value == number) {
                if (pair.Key.x == position.x || pair.Key.y == position.y) return false;
            }
        }
        return true;
    }

    private Grid() {
        numberInCell = new Dictionary<Vector2i, int>();
        Instance = this;
    }

    public int GetHintValue(bool checkRow, bool defaultDirection, int rowOrColumnNumber) {
        List<KeyValuePair<Vector2i, int>> relevantNumbers = new List<KeyValuePair<Vector2i, int>>();

        foreach(KeyValuePair<Vector2i, int> pair in numberInCell) {
            if (checkRow) {
                if (pair.Key.y == rowOrColumnNumber) {
                    relevantNumbers.Add(pair);
                }
            } else {
                if (pair.Key.x == rowOrColumnNumber) {
                    relevantNumbers.Add(pair);
                }
            }
        }

        int multiplier = 1;
        if (!defaultDirection) multiplier = -1;


        IEnumerable<KeyValuePair<Vector2i, int>> query;
        if (checkRow) {
            query = relevantNumbers.OrderBy((KeyValuePair<Vector2i, int> pair) => pair.Key.x * multiplier);
        } else {
            query = relevantNumbers.OrderBy((KeyValuePair<Vector2i, int> pair) => pair.Key.y * multiplier);
        }

        int hintValue = 0;
        int maxNumberSeen = 0;

        foreach (KeyValuePair<Vector2i, int> pair in query) {
            if (pair.Value > maxNumberSeen) {
                maxNumberSeen = pair.Value;
                hintValue++;
            }
        }

        return hintValue;
    }

    public override string ToString() {
        string result = "\n";

        foreach (KeyValuePair<Vector2i, int> pair in numberInCell) {
            result += pair.Value + " ";
            if (pair.Key.x == Size - 1) result += "\n";
        }

        return result;
    }


}

public struct Vector2i {
    public int x;
    public int y;

    public Vector2i(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public override string ToString() {
        return "(" + x + "," + y + ")";
    }

}