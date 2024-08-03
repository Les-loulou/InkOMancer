using UnityEngine;
using UnityEngine.UI;

public class SC_LC_EnemyHealth : MonoBehaviour
{
	SC_LQ_EnemyGlobal enemy;

	[Space]
	public GameObject deathParticle;
	public Slider healthBar;

	[Space]
	public float health;
	[HideInInspector] public float maxHealth;
	[HideInInspector] public float smoothHealth;
	public float healthSpeed = 10f;

	void Start()
	{
		enemy = SC_LQ_EnemyGlobal.instance;

		health = enemy.stats.health;
		maxHealth = enemy.stats.health;
		smoothHealth = enemy.stats.health;

		healthBar.maxValue = maxHealth;
		healthBar.value = maxHealth;
	}

	void Update()
	{
		health = Mathf.Clamp(health, 0f, maxHealth);
		smoothHealth = Mathf.Lerp(smoothHealth, health, Time.deltaTime * healthSpeed);

		if (healthBar != null && smoothHealth >= 0.5f)
			healthBar.value = smoothHealth;
		else
		{
			//Destroy(healthBar);
		}

		if (Input.GetKeyDown(KeyCode.I)) //DEBUG
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
		//Instantiate(deathParticle, transform.position, Quaternion.identity);
	}

	public void DestroyEnemy()
	{
		Destroy(gameObject);
	}
	#endregion
}
