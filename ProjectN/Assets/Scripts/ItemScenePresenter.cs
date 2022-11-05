using UnityEngine;

public class ItemScenePresenter : MonoBehaviour
{
    [SerializeField] private ItemInfo _info;

    private SpriteRenderer _sprite;

    public ItemInfo Info => _info;

    private void Start()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _sprite.sprite = _info.Sprite;
    }
}
