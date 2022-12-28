using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableStorage : MonoBehaviour, IInteractable
{
    private const float DISTANCE_TO_INTERACT = 1.5f;

    private LootCrate _lootCrate;

    private void Start()
    {
        _lootCrate = GetComponent<LootCrate>();
    }

    public void Interact(Unit unit)
    {
        if (Vector3.Distance(transform.position, unit.transform.position) < DISTANCE_TO_INTERACT)
        {
            unit.Inventory.GetItemsFromStorage(_lootCrate);
            PlayerInput.Instance.OpenInventory();
        }
    }
}