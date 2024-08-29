using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SC_LC_PlayerHealth : MonoBehaviour
{
	SC_LC_PlayerGlobal player;

	public Slider healthBar;
    public TMP_Text healthDisplay;

    public float maxHealth;
    public float currentHealth;

	void Awake()
	{
		healthBar.maxValue = maxHealth;
		healthBar.value = maxHealth;

		currentHealth = maxHealth;
	}

	void Start()
	{
		player = SC_LC_PlayerGlobal.instance;
	}

	void Update()
    {
		currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

		healthDisplay.text = currentHealth.ToString() + " / " + maxHealth.ToString();

		healthBar.value = currentHealth;

		if (player.inputs.damagePlayerPressed == true) //DEBUG
			UpdateHealth(-20f);
	}

	public void UpdateHealth(float _amount)
	{
		currentHealth += _amount;
	}
}
