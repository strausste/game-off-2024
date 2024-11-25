using UnityEngine;

[CreateAssetMenu(fileName = "Boots", menuName = "Scriptable Objects/Items/Boots")]
public class Boots : Equipment
{
    public Vector3 modelOffsetLeft;
    public Vector3 modelOffsetRight;
    public Vector3 modelRotationLeft = Vector3.zero;
    public Vector3 modelRotationRight = Vector3.zero;
    public Vector3 modelScale = Vector3.one;
    public int speed;
}

