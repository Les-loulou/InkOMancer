using UnityEngine;

public class SC_LQ_EnemyGlobal : MonoBehaviour
{
	public SO_EnemyStats stats;

	public Animator anim;

    private void Awake()
    {
		stats.SetStats(this);

		anim = GetComponent<Animator>();
	}

}
