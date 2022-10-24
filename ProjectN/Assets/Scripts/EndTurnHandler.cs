using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndTurnHandler : MonoBehaviour
{
    public static Action turnEnd;

    public void EndTurn()
    {
        Debug.Log("Turn ended");
        turnEnd?.Invoke();
    }
}
