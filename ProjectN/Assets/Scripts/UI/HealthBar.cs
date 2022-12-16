using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private TextMeshProUGUI _value;

    public void SetScale(float value)
    {
        _value.text = value.ToString();
        _rectTransform.localScale = new Vector3(value / 100, _rectTransform.localScale.y, _rectTransform.localScale.z);
    }
}
