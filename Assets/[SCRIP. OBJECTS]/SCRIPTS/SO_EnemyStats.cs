using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "SO_EnemyStats", menuName = "Scriptable Objects/SO_EnemyStats")]
public class SO_EnemyStats : ScriptableObject
{
    public string enemyname;

    public int life;
    public float speed;
    public float acceleration;

    public void SetStats(MonoBehaviour mono)
    {
        mono.GetComponent<NavMeshAgent>().speed = speed;
        mono.GetComponent<NavMeshAgent>().acceleration = acceleration;
        mono.GetComponent<NavMeshAgent>().avoidancePriority = Random.Range(50, 100);
    }


}
