using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Items/Weapon")]
public class Weapon : Item
{
    public int attack;
    public Vector3 modelOffset;
    public Vector3 modelScale = Vector3.one;
    public Vector3 modelRotation = Vector3.zero;
    public float hitboxSize;
    public Vector3 hitboxOffset;
}
