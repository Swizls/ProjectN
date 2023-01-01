using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour, IInteractable
{
    public void Interact(Unit unit)
    {
        unit.gameObject.SetActive(false);
    }
}
