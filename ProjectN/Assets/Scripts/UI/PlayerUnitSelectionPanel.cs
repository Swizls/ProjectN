using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerUnitSelectionPanel : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;

    private Dictionary<GameObject, Unit> unitSelectionButtons = new Dictionary<GameObject, Unit>();
    private List<Unit> allPlayerUnits;

    private void Start()
    {
        allPlayerUnits = PlayerUnitHandler.AllPlayerUnits;
        CreateButtons();
    }
    
    private void CreateButtons()
    {
        unitSelectionButtons.Add(buttonPrefab, allPlayerUnits[0]);
        RectTransform lastButtonPos = buttonPrefab.GetComponent<RectTransform>();

        for(int i = 1; i <= allPlayerUnits.Count - 1; i++)
        {
            GameObject unitSelectionButton = Instantiate(buttonPrefab);
            unitSelectionButton.transform.SetParent(transform);
            unitSelectionButton.transform.localScale = Vector3.one;

            Button buttonComponent = unitSelectionButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => SelectUnit(unitSelectionButton));
            buttonComponent.GetComponentInChildren<TextMeshProUGUI>().SetText("Unit " + (i + 1));

            RectTransform buttonRect = unitSelectionButton.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector3(lastButtonPos.anchoredPosition.x + 100, 50, 0);

            unitSelectionButtons.Add(unitSelectionButton, allPlayerUnits[i]);
            lastButtonPos = buttonRect;
        }
    }

    public void SelectUnit(GameObject button)
    {
        if(!PlayerUnitHandler.CurrentSelectedUnit.Movement.IsMoving)
            PlayerUnitHandler.SelectUnit(unitSelectionButtons[button]);
    }
}