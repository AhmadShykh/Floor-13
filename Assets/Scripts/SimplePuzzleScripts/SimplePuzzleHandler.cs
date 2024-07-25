using PuzzleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePuzzleHandler : CorePuzzleHandler
{
    override protected void OnSolution()
    {
        Debug.Log("Solution Found\n");
    }
}
