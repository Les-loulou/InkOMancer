using UnityEngine;

public class SC_LQ_GlobalEnemy : MonoBehaviour
{

    public SO_EnemyStats myStats;

    private void Awake()
    {
        myStats.SetStats(this);
    }

}
