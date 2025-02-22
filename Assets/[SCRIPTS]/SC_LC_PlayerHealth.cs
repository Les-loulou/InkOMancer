using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SC_LC_PlayerHealth : MonoBehaviour
{
	SC_LC_PlayerGlobal player;

	public Slider healthBar;
    public TMP_Text currentHealthDisplay;
    public TMP_Text maxHealthDisplay;

	[Space]
    public float maxHealth;
    public float previousMaxHealth;
    public float targetMaxHealth;
	[Space]
	public float currentHealth;
	float previousHealth;
	public float targetHealth;
	public float healthPercentage;
	[Space]
	public AnimationCurve healthSpeedCurve;
	public AnimationCurve maxHealthSpeedCurve;
    float elapsedTime;
	public float smoothTimeHealth;
	public float smoothTimeMaxHealth;

	[Space]
	public float debugHealthChange;
	public float debugMaxHealthChange;

	void Awake()
	{
		healthBar.maxValue = 100f; //Sets the maximum value of the slider to the maxHealth variable
		healthBar.value = healthPercentage; //Sets the value of the slider to the maxHealth variable

		previousMaxHealth = maxHealth;
		targetMaxHealth = maxHealth;

		currentHealth = maxHealth; //Sets the current health of the player to the maxHealth variable
		previousHealth = currentHealth; //Sets the previous health of the player to the currentHealth variable
		targetHealth = currentHealth; //Sets the target health of the player to the currentHealth variable
	}

	void Start()
	{
		player = SC_LC_PlayerGlobal.instance;
	}

	void Update()
    {
		elapsedTime += Time.deltaTime; //Keeps track of the time elapsed
		float normalizedTime = elapsedTime / smoothTimeHealth; //Divides the elapsed time by the smoothTime variable
		float healthCurve = healthSpeedCurve.Evaluate(normalizedTime);
		float maxHealthCurve = maxHealthSpeedCurve.Evaluate(normalizedTime);

		maxHealth = Mathf.Clamp(maxHealth, 0f, Mathf.Infinity); //Clamps the current health of the player between 0 and the maxHealth value
		targetMaxHealth = Mathf.Clamp(targetMaxHealth, 0f, Mathf.Infinity);
		currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); //Clamps the current health of the player between 0 and the maxHealth value
		targetHealth = Mathf.Clamp(targetHealth, 0f, maxHealth); //Clamps the target health of the player between 0 and the maxHealth value

		currentHealthDisplay.text = currentHealth.ToString("F0"); //Displays current health value of the player on the health bar
		maxHealthDisplay.text = maxHealth.ToString("F0"); //Displays maximum health value of the player on the health bar

		healthPercentage = (targetHealth / maxHealth) * 100f; //Sets a percentage based on the target health and the max health values
		currentHealth = Mathf.Lerp(previousHealth, targetHealth, healthCurve); //Smoothly moves the current health value based on the animation curve
		maxHealth = Mathf.Lerp(previousMaxHealth, targetMaxHealth, maxHealthCurve);

		healthBar.value = currentHealth / maxHealth * 100f; //Moves the bar based on the current health value

		#region DEBUG
		if (player.inputs.changePlayerHealthPressed == true)
			ChangeHealthValue(debugHealthChange);
		if (player.inputs.changePlayerMaxHealthPressed == true)
			ChangeMaxHealthValue(debugMaxHealthChange);
		#endregion
	}

	public void ChangeHealthValue(float _amount)
	{
		elapsedTime = 0f; //Resets the elapsed time variable
		previousHealth = currentHealth; //Sets the previous health value to the currentHealth variable
		targetHealth += _amount; //Adds the passed amount parameter to the targetHealth variable
	}

	public void ChangeMaxHealthValue(float _amount)
	{
		elapsedTime = 0f; //Resets the elapsed time variable
		previousMaxHealth = maxHealth;
		targetMaxHealth += _amount;
	}
}
