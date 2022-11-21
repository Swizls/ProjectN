using UnityEngine;

[CreateAssetMenu(fileName ="Default action data", menuName = "Misc/ActionData")]
public class ActionData : ScriptableObject
{
    [SerializeField] AudioClip clip;
    [SerializeField] int cost;

    public AudioClip Clip => clip;
    public int Cost => cost;
}
