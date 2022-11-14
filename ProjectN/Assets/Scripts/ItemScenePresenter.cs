using UnityEngine;

public class ItemScenePresenter : MonoBehaviour
{
    [SerializeField] private BaseItemInfo _info;

    private SpriteRenderer _sprite;

    public BaseItemInfo Info => _info;

    private void Start()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _sprite.sprite = _info.Sprite;
    }
}
