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
    public TMP_Text healthDisplay;

	[Space]
    public float maxHealth;
	[Space]
	public float currentHealth;
	public float previousHealth;
	public float targetHealth;
	[Space]
	public AnimationCurve healthSpeedCurve;
    float elapsedTime;
	public float smoothTime;

	[Space]
	public float debugHealth;

	void Awake()
	{
		healthBar.maxValue = maxHealth; //Sets the maximum value of the slider to the maxHealth variable
		healthBar.value = maxHealth; //Sets the value of the slider to the maxHealth variable

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
		float normalizedTime = elapsedTime / smoothTime; //Divides the elapsed time by the smoothTime variable
		float curveValue = healthSpeedCurve.Evaluate(normalizedTime); //Keeps track of the current value of the animation curve based on the normalizedTime variable

		currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); //Clamps the current health of the player between 0 and the maxHealth value
		targetHealth = Mathf.Clamp(targetHealth, 0f, maxHealth); //Clamps the target health of the player between 0 and the maxHealth value

		healthDisplay.text = currentHealth.ToString("F0") + " / " + maxHealth.ToString("F0"); //Displays the values of the current health and the maximum health of the player on the health bar

		currentHealth = Mathf.Lerp(previousHealth, targetHealth, curveValue); //Smoothly moves the current health value based on the animation curve

		healthBar.value = currentHealth; //Moves the bar based on the current health value

		#region DEBUG
		if (player.inputs.damagePlayerPressed == true) //DEBUG
			Inflict(debugHealth);
		#endregion
	}

	public void Inflict(float _amount)
	{
		elapsedTime = 0f; //Resets the elapsed time variable
		previousHealth = currentHealth; //Sets the previous health value to the currentHealth variable
		targetHealth += _amount; //Adds the passed amount parameter to the targetHealth variable
	}
}
