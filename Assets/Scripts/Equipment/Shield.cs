using UnityEngine;

[CreateAssetMenu(fileName = "Shield", menuName = "Scriptable Objects/Shield")]
public class Shield : ScriptableObject
{
    public int damageProtection;
    public GameObject model;
}
