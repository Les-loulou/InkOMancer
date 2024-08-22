using UnityEngine;
using UnityEngine.UI;

public class SC_LC_EnemyHealth : MonoBehaviour
{
	SC_LQ_EnemyGlobal enemy;
	SC_LC_PlayerGlobal player;

	[Space]
	public GameObject deathParticle;
	public GameObject healthBar;
	public Slider healthBarSlider;

	[Space]
	public float health;
	[HideInInspector] public float maxHealth;
	/*[HideInInspector] */
	public float smoothHealth;
	public float healthSpeed = 10f;

	void Start()
	{
		enemy = GetComponent<SC_LQ_EnemyGlobal>();
		player = SC_LC_PlayerGlobal.instance;

		health = enemy.enemy.health;
		maxHealth = enemy.enemy.health;
		smoothHealth = enemy.enemy.health;

		healthBarSlider.maxValue = maxHealth;
		healthBarSlider.value = maxHealth;
	}

	void Update()
	{
		health = Mathf.Clamp(health, 0f, maxHealth);
		smoothHealth = Mathf.Lerp(smoothHealth, health, Time.deltaTime * healthSpeed);

		if (healthBar != null && smoothHealth >= 0.5f)
		{
			healthBarSlider.value = smoothHealth;
		}

		if (player.inputs.damageEnemyPressed == true) //DEBUG
			Damage(-50);
	}

	public void Damage(float amount)
	{
		if (health > 0f)
		{
			health += amount;

			enemy.anim.SetTrigger("Damage");
		}

		if (health <= 0f)
			Death();
	}

	#region DEATH BEHAVIOR
	public void Death()
	{
		enemy.anim.SetTrigger("Death");
		GetComponent<Collider>().enabled = false;
	}

	public void DeathParticle()
	{
		Instantiate(deathParticle, transform.position, Quaternion.identity);
	}

	public void DestroyEnemy()
	{
		Destroy(gameObject);
	}
	#endregion
}
