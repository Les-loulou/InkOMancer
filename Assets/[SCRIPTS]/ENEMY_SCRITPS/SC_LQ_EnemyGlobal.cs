using UnityEngine;

public class SC_LQ_EnemyGlobal : MonoBehaviour
{
	public static SC_LQ_EnemyGlobal instance;

	public SO_EnemyStats stats;

	public Animator anim;

    private void Awake()
    {
		#region SINGLETON
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		#endregion

		stats.SetStats(this);

		anim = GetComponent<Animator>();
	}

}
