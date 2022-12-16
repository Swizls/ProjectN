using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerUnitSelectionPanel : MonoBehaviour
{
    [SerializeField] private GameObject _buttonPrefab;

    private Dictionary<GameObject, Unit> _unitSelectionButtons = new Dictionary<GameObject, Unit>();
    private List<Unit> _allPlayerUnits;

    private void Start()
    {
        _allPlayerUnits = PlayerUnitControl.Instance.AllControlableUnits;
        CreateButtons();
    }
    
    private void CreateButtons()
    {
        _unitSelectionButtons.Add(_buttonPrefab, _allPlayerUnits[0]);
        RectTransform lastButtonPos = _buttonPrefab.GetComponent<RectTransform>();

        for(int i = 1; i <= _allPlayerUnits.Count - 1; i++)
        {
            GameObject unitSelectionButton = Instantiate(_buttonPrefab);
            unitSelectionButton.transform.SetParent(transform);
            unitSelectionButton.transform.localScale = Vector3.one;

            Button buttonComponent = unitSelectionButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => SelectUnit(unitSelectionButton));
            buttonComponent.GetComponentInChildren<TextMeshProUGUI>().SetText("Unit " + (i + 1));

            RectTransform buttonRect = unitSelectionButton.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector3(lastButtonPos.anchoredPosition.x + 100, 50, 0);

            _unitSelectionButtons.Add(unitSelectionButton, _allPlayerUnits[i]);
            lastButtonPos = buttonRect;
        }
    }

    public void SelectUnit(GameObject button)
    {
        if(!PlayerUnitControl.Instance.CurrentSelectedUnit.Movement.IsMoving)
            PlayerUnitControl.Instance.SelectUnit(_unitSelectionButtons[button]);
    }
}
