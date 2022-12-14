using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndTurnHandler : MonoBehaviour
{
    public static Action playerTurn;
    public static Action enemyTurn;

    public static bool isPlayerTurn;

    private void Start() => isPlayerTurn = true;

    public static void EndTurn()
    {
        isPlayerTurn = !isPlayerTurn;

        if (isPlayerTurn)
            playerTurn?.Invoke();
        else
            enemyTurn?.Invoke();
    } 
}
