using UnityEngine;

[CreateAssetMenu(fileName = "SO_PlayerStats", menuName = "Scriptable Objects/SO_PlayerStats")]
public class SO_PlayerStats : ScriptableObject
{
    public float pv;
    public float movementSpeed;
    public float sprintMultiplier;
    public float radiusAttack;
}
