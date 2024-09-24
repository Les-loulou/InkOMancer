using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;

[CreateAssetMenu(fileName = "SO_Rune", menuName = "Scriptable Objects/SO_Rune")]
public class SO_Rune : ScriptableObject
{

    public string runeName;

    public GameObject runeScript;

    public int currentLevel;

    public List<float> inkAmount;
    public List<float> costInk;
    public List<float> damage;

    public float remainingInk;
    public float actualInkCost;
    public float actualDamage;


    public void ResetStats()
    {
        remainingInk = inkAmount[currentLevel];
        actualInkCost = costInk[currentLevel];
        actualDamage = damage[currentLevel];
    }

    public void levelUp()
    {
        currentLevel++;
    }

    public void CostRune()
    {
        remainingInk -= actualInkCost;
    }

}
