using UnityEngine;

[CreateAssetMenu(fileName = "Shield", menuName = "Scriptable Objects/Shield")]
public class Shield : Item
{
    public Vector3 modelOffset;
    public Vector3 modelScale = Vector3.one;
    public Vector3 modelRotation = Vector3.zero;
    public int damageProtection;
}
