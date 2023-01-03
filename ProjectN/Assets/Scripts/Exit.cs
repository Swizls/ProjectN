using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour, IInteractable
{
    private const float MAX_DISTNACE_TO_INTERACT = 1.8f;
    public void Interact(Unit unit)
    {
        if(Vector2.Distance(unit.transform.position, transform.position) < MAX_DISTNACE_TO_INTERACT)
            unit.gameObject.SetActive(false);
    }
}
