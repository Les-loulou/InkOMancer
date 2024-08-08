using UnityEngine;

public class SC_LQ_EnemyGlobal : MonoBehaviour
{
	public SO_EnemyStats enemy;

	public Animator anim;

    private void Awake()
    {
		enemy.SetStats(this);

		anim = GetComponent<Animator>();
	}

}
