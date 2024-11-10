using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon : ScriptableObject
{
    public int attack;
    public GameObject model;
    public Vector3 modelOffset;
    public Vector3 modelScale = Vector3.one;
    public float hitboxSize;
    public Vector3 hitboxOffset;
}
