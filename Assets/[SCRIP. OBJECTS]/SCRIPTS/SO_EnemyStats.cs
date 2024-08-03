using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "SO_EnemyStats", menuName = "Scriptable Objects/SO_EnemyStats")]
public class SO_EnemyStats : ScriptableObject
{
    public string enemyname;

    public float health;
    public float speed;
    public float acceleration;

    public Vector2 randomSize;

	//LOOTS
	public List<Drop> drops = new();

	public void SetStats(MonoBehaviour mono)
    {
        mono.GetComponent<NavMeshAgent>().speed = speed;
        mono.GetComponent<NavMeshAgent>().acceleration = acceleration;
        mono.GetComponent<NavMeshAgent>().avoidancePriority = Random.Range(50, 100);

        //Set Random size
        float currentRandomSize = Random.Range(randomSize.x, randomSize.y);
        mono.transform.localScale = Vector3.one * currentRandomSize;

        mono.GetComponent<Animator>().SetFloat("RotationSpeed", 1 / (currentRandomSize));
    }
}

[System.Serializable]
public class Drop
{
    public GameObject dropPrefab;
	[UnityEngine.Range(0f, 100f)]
	public float dropChance;
}
