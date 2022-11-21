using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndTurnHandler : MonoBehaviour
{
    public static Action turnEnd;

    public static void EndTurn() => turnEnd?.Invoke();
}
