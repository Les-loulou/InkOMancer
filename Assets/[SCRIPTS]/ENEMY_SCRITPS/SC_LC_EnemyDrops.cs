using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_LC_EnemyDrops : MonoBehaviour
{
	[SerializeField] SO_EnemyStats enemySO;
	public List<GameObject> currentDrops = new();

	[Space]
	[Header(" DROPS PARAMETERS")]
	[SerializeField] Vector2 xRange;
	[SerializeField] Vector2 zRange;
	[SerializeField] float itemDropRate;

	Vector3 enemyBasePos;

	[Space]
	[Header(" RAYCASTS")]
	[SerializeField] LayerMask blockingLayers;
	[SerializeField] float voidMaxDistance;

	RaycastHit basePosHit;
	RaycastHit voidCheckHit;
	RaycastHit obstacleCheckHit;

	void Awake()
	{
		GenerateDrops(); //Generates the drops pool of the enemy 
	}

	private void Update()
	{
		SetBasePosition(); //Sets the base position of the enemy
	}

	public void GenerateDrops()
	{
		foreach (var currentDrop in enemySO.drops) //For each drop in the enemy's drops pool
			Roll(currentDrop); //Rolls the chance for this drop to get added to the enemy's drops pool
	}

	public void Roll(Drop _currentDrop)
	{
		float randomChance = Random.Range(0f, 100f); //Sets a random value of chance between 0 and 100

		if (randomChance <= _currentDrop.dropChance) //If the previously rolled random chance is inferior or equal to the drop's chance
		{
			currentDrops.Add(_currentDrop.dropPrefab); //Adds the drop's prefab to the enemy's drops pool
			Roll(_currentDrop); //Rolls for another drop to add to the enemy's drop pool
		}
	}

	void SetBasePosition()
	{
		Vector3 origin = transform.position + Vector3.up; //Sets the origin of the raycast slightly above the enemy's position
		Vector3 direction = Vector3.down; /*Sets the direction of the raycast to point down*/
		//Debug.DrawRay(origin, direction * 5f, Color.red);
		if (Physics.Raycast(origin, direction, out basePosHit, 5f, blockingLayers)) //If the raycast hits the ground layer
			enemyBasePos = basePosHit.point; //Sets the enemyBasePos variable to this position
	}


	public IEnumerator DropLootsCoroutine()
	{
		int dropCount = currentDrops.Count; //Sets the dropCount variable to the number of current drops in the enemy's drops pool

		for (int i = 0; i < currentDrops.Count; i++) //For each drop in the enemy's drops pool
		{
			GameObject newDrop = Instantiate(currentDrops[i], NewPosition(currentDrops[i]), Quaternion.identity); //Instantiates this drop to a new position and store it in the newDrop variable

			//Debug.Log("Void Check : " + VoidCheck(newDrop) + " - Obstacle Check : " + ObstacleCheck(newDrop));
			while (VoidCheck(newDrop) == true || ObstacleCheck(newDrop) == true) //While the current drop is set in the void or in/behind an obstacle
				newDrop.transform.position = NewPosition(newDrop); //Sets the current drop position to a new valid position

			dropCount--; //Substract 1 to the drops count
			if (dropCount == 0) //If the drops count reaches 0
				Destroy(gameObject); //Destroys the enemy

			yield return new WaitForSeconds(itemDropRate); //Waits before the next instantiation
		}
	}

	Vector3 NewPosition(GameObject _currentDrop)
	{
		float randomX = Random.Range(xRange.x, xRange.y); //Sets a random value on X
		float randomZ = Random.Range(zRange.x, zRange.y); //Sets a random value on Z

		float xPos = transform.localPosition.x + randomX; //Sets X position based on the enemy's position + the random X position
		float yPos = _currentDrop.transform.position.y + enemyBasePos.y; //Sets Y position based on the drop's Y position + the enemy's base Y position
		float zPos = transform.localPosition.z + randomZ; //Sets Z position based on the enemy's position + the random Z position

		return new Vector3(xPos, yPos, zPos); //Returns a new Vector3 based on the previous axis
	}

	bool VoidCheck(GameObject _currentDrop)
	{
		Vector3 origin = _currentDrop.transform.position + new Vector3(0f, 1f, 0f); //Sets the origin of the raycast slightly above the drop's position
		Vector3 direction = Vector3.down; /*Sets the direction of the raycast to point down*/
		//Debug.DrawRay(voidRayOrigin, voidRayDirection * maxDistance, Color.red, 50f);
		if (Physics.Raycast(origin, direction, out voidCheckHit, voidMaxDistance, blockingLayers)) //If the drop is set in the void
			return false;
		else //If the drop isn't set in the void
			return true;
	}

	bool ObstacleCheck(GameObject _currentDrop)
	{
		Vector3 origin = transform.position; //Sets the origin of the raycast to the enemy's position
		Vector3 direction = _currentDrop.transform.position - transform.position; //Sets the direction of the raycast to point to the drop's position
		float obstacleRayDistance = Vector3.Distance(transform.position, _currentDrop.transform.position); //Sets the direction of the raycast to be the length between the enemy's position and the drop's position
		//Debug.DrawRay(obstacleRayOrigin, obstacleRayDirection, Color.red, 50f);
		if (Physics.Raycast(origin, direction, out obstacleCheckHit, obstacleRayDistance, blockingLayers)) //If the drop is set in or behind an obstacle
			return true;
		else //If the drop isn't set in or behind an obstacle
			return false;
	}

	public void DropItems()
	{
		StartCoroutine(DropLootsCoroutine());
	}
}
