using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid {

    public static event EventHandler OnGridChanged;

    public static Grid Instance { get; private set; }


    public int Size { get; private set; }
    public Dictionary<Vector2i, int> NumberInCell { get; private set; }

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
                    int pickedNumber = FindAcceptableNumber(new Vector2i(i, j), new List<int>(availableNumbers), newGrid.NumberInCell);
                    newGrid.NumberInCell[new Vector2i(i, j)] = pickedNumber;
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
        NumberInCell = new Dictionary<Vector2i, int>();
        Instance = this;
    }

    public int GetHintValue(Direction direction, int rowOrColumnNumber) {

        bool checkRow = direction == Direction.LEFT || direction == Direction.RIGHT;
        bool defaultDirection = direction == Direction.RIGHT || direction == Direction.DOWN;

        int hintValue = 0;
        int maxNumberSeen = 0;

        for (int i = 0; i < Size; i++) {
            int placeInRowOrColumn = defaultDirection ? i : Size - i - 1;
            Vector2i cellToCheck = checkRow ? new Vector2i(placeInRowOrColumn, rowOrColumnNumber) : new Vector2i(rowOrColumnNumber, placeInRowOrColumn);
            int cellValue = NumberInCell[cellToCheck];

            if (cellValue > maxNumberSeen) {
                maxNumberSeen = cellValue;
                hintValue++;
            }

        }

        return hintValue;
    }

    public override string ToString() {
        string result = "\n";

        for (int i = 0; i < Size; i++) {
            for (int j = 0; j < Size; j++) {
                result += NumberInCell[new Vector2i(i, j)] + " ";
            }
            result += "\n";
        }

        return result;
    }


}
