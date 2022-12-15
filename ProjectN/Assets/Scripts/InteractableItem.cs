using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour, IInteractable
{
    public void Interact(Unit unit)
    {
        var item = GetComponent<BaseItemInfo>();
    }
}