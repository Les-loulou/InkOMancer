using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_LC_ProceduralGenerationIslands : MonoBehaviour
{
	SC_LC_PlayerGlobal player;

	SC_LC_ProceduralGenerationGrid gridScript;

	[Space(10)]
	[SerializeField] Generations islandsGenerationMode;
	[Space(2)]
	[SerializeField] float waitBeforeNextIsland;
	Coroutine islandGenerationCoroutine;

	[Space(2)]
	[Header("OBJECTS")]
	[SerializeField] List<GameObject> prefabs = new();

	[Space(2)]
	[Header("PARAMETERS")]
	[SerializeField] int minIslandsCountRange;
	[SerializeField] int maxIslandsCountRange;
	[Space]
	[SerializeField] int minBranchesRange;
	[SerializeField] int maxBranchesRange;
	[Space]
	public int currentIslandsStep;

	[Space]
	public List<Island> islands = new();
	public List<Chunk> branchableChunks = new();
	public List<Branch> branches = new();

	public Island test;

	enum Generations { StepByStep, Instant }

	void Start()
	{
		player = SC_LC_PlayerGlobal.instance;

		gridScript = GetComponent<SC_LC_ProceduralGenerationGrid>();

		switch (islandsGenerationMode)
		{
			case Generations.StepByStep:
				islandGenerationCoroutine = StartCoroutine(StepByStepIslandsGeneration());
				break;
			case Generations.Instant:
				InstantIslandsGeneration();
				break;
		}
	}

	void Update()
	{
		if (player.inputs.generateRandomIslandPressed == true) //DEBUG
			CreateRandomIsland();

		if (player.inputs.rerollIslandsPressed == true)
			RerollIslands();

		if (player.inputs.generateIslandPressed == true)
			CreateIsland(islands.Count, new Vector3(player.inputs.newIslandDirection.x, 0f, player.inputs.newIslandDirection.y));
	}

	#region ISLANDS
	IEnumerator StepByStepIslandsGeneration()
	{
        for (int i = 0; i < maxIslandsCountRange; i++)
        {
			CreateRandomIsland();
			yield return new WaitForSeconds(waitBeforeNextIsland);
        }

		AfterMainBranch();
	}

	void InstantIslandsGeneration()
	{
		for (int i = 0; i < maxIslandsCountRange; i++)
			CreateRandomIsland();

		AfterMainBranch();
	}

	void AfterMainBranch()
	{
		if (currentIslandsStep < minIslandsCountRange) //If the number of island is below the minimum number of islands required
			RerollIslands(); //Restarts the generation process
		else
		{
			SetBranchableChunks(); //Sets the chunks that could start new branches
								   //CreateRandomBranches();
		}
	}

	void CreateIsland(int _i, Vector3 _direction)
	{
		Island newIsland = new Island(null, Vector3.zero, Vector3.zero); //Creates a new islands with basic parameters

		newIsland.prefab = prefabs[Random.Range(0, prefabs.Count)]; //Sets the prefab of the island to a random prefab among the list of islands prefabs

		if (_i == 0) //If the current new island is the first one
			newIsland.coordinates = gridScript.centerChunk.coordinates; //Sets the coordinates of this chunk to the center of the grid
		else
		{
			newIsland.direction = _direction; //Sets the current new island direction to the direction parameter of this function
			newIsland.coordinates = islands[_i - 1].coordinates + _direction; //Adds the current new island's coordinates using the previous island's coordinates
		}

		currentIslandsStep++; //Adds 1 to the current islands count
		//if (gridScript.chunks[newIsland.coordinates].island != null)
		//{
		//	Debug.Log("An island already exists at : " + newIsland.coordinates);
		//	test = gridScript.chunks[newIsland.coordinates].island;
		//}

		if (gridScript.chunks[newIsland.coordinates].possibleDirections.Count == 0 || gridScript.chunks[newIsland.coordinates].island != null) //If the new Island is leading in a dead end
		{
			test = gridScript.chunks[newIsland.coordinates].island;
			return; //Stops the generation to go any further
		}

		islands.Add(newIsland); //Adds the new island to the islands list
		gridScript.chunks[newIsland.coordinates].island = newIsland; //Stores the new island in the corresponding chunk

		newIsland.instance = Instantiate( //Instantiates the new island
			newIsland.prefab, //Sets the prefab of the new island
			gridScript.chunks[newIsland.coordinates].instance.transform.position, //Sets the position of the new island
			Quaternion.identity, //Sets the rotation of the new island
			gridScript.chunks[newIsland.coordinates].instance.transform); //Sets the parent of the new island

		gridScript.UpdateChunks(); //Update the possible directions of all chunks
	}

	void CreateRandomIsland()
	{
		if (islands.Count == 0) //If the current island is the first one
			CreateIsland(islands.Count, gridScript.centerChunk.coordinates); //Creates a new island based on the island count and the center chunk's coordinates
		else
		{
			Chunk currentChunk = gridScript.chunks[islands[islands.Count - 1].coordinates]; //Sets the currentChunk variable
			Vector3 randomDirection = currentChunk.possibleDirections[Random.Range(0, currentChunk.possibleDirections.Count)]; //Sets a random direction based on the current chunk's possible directions
			CreateIsland(islands.Count, randomDirection); //Creates a new island based on the island count and the previously set random direction
		}
	}

	void RerollIslands()
	{
		switch (islandsGenerationMode)
		{
			case Generations.StepByStep:
				StopCoroutine(islandGenerationCoroutine);
				gridScript.ClearChunks();
				islandGenerationCoroutine = StartCoroutine(StepByStepIslandsGeneration());
				break;
			case Generations.Instant:
				gridScript.ClearChunks();
				InstantIslandsGeneration();
				break;
		}
	}
	#endregion

	#region BRANCHES
	void SetBranchableChunks()
	{
		foreach (Island island in islands) //For each generated island
			foreach (Vector3 direction in gridScript.chunks[island.coordinates].possibleDirections) //For each adjacent chunks to those islands
				if (gridScript.chunks[island.coordinates + direction].island == null && gridScript.chunks[island.coordinates + direction].possibleDirections.Count == 3) //If the chunk is empty and has 3 empty chunks surrounding it
					branchableChunks.Add(gridScript.chunks[island.coordinates + direction]); //Adds this chunk to the branchable chunks list
	}

	void CreateRandomBranches()
	{
		int randomRange = (int)Random.Range(minBranchesRange, maxBranchesRange);

		for (int i = 0; i < randomRange; i++)
		{
			int randomChunk = Random.Range(0, branchableChunks.Count);
			CreateIsland(islands.Count, branchableChunks[randomChunk].possibleDirections[0]);
		}

		//CreateIsland();
	}
	#endregion

	#region OLD
	void TestCreateRoom1()
	{
		//	Vector3 roomPos = Vector3.zero; //Sets of the room position variable
		//	Vector3 roomDir = directions[UnityEngine.Random.Range(0, directions.Length)]; //Sets the room's direction variable to a random direction

		//	if (roomCount != 0) //Check if it's not the first room
		//		while (roomDir == rooms[roomCount - 1].direction * -1) //Prevents direction from going back
		//			roomDir = directions[UnityEngine.Random.Range(0, directions.Length)];

		//	//Check possible directions


		//	if (roomCount == 0) //First room
		//		roomPos = start.transform.position; //Sets the room position to the starting point
		//	else //Next rooms
		//	{
		//		roomPos = rooms[roomCount - 1].position + rooms[roomCount - 1].direction * space; //Sets the room to the previously set position

		//		//foreach (Room room in rooms)
		//		//{
		//		//	if (roomPos == room.position)
		//		//	{
		//		//		print("same");
		//		//		rooms[roomCount - 1].direction = directions[UnityEngine.Random.Range(0, directions.Length)];
		//		//		roomPos = rooms[roomCount - 1].position + rooms[roomCount - 1].direction * space;
		//		//	}
		//		//}
		//	}

		//	Room newRoom = new Room(roomPref, roomPos, roomDir, roomCount); //Create a new Room using previously generated parameters
		//	Instantiate(newRoom.prefab, roomPos, Quaternion.identity); //Instantiate the room
		//	rooms.Add(newRoom); //Add the room to the Rooms list
		//	roomCount++; //Add 1 to the room count
	}
	void TestCreateRoom2()
	{
		//Room newRoom = new Room(roomPref, Vector3.zero, Vector3.zero, new List<Vector3>(), 0, 0); //Create a new room with basic parameters
		//rooms.Add(newRoom);

		//if (roomCount == 0)
		//{
		//	newRoom.position = start.transform.position; //Sets the first room to the start position
		//	foreach (Room room in rooms)
		//	{
		//		for (int i = 0; i < directions.Length; i++)
		//		{
		//			Vector3 posCheck = directions[i] * space;
		//			if (posCheck == room.position)
		//				return;
		//			else
		//				newRoom.possibleDirections.Add(directions[i]);
		//		}
		//	}
		//	newRoom.direction = newRoom.possibleDirections[Random.Range(0, newRoom.possibleDirections.Count)]; //Chooses a random direction for the next room to generate
		//	roomCount = 0; //Sets the count of rooms to 0
		//	directionCount = 0; //Sets the count in one direction to 0
		//}

		//else
		//{
		//	newRoom.position = rooms[roomCount - 1].direction * space; //Sets the first room to the start position
		//	for (int i = 0; i < directions.Length; i++)
		//	{
		//		Vector3 posCheck = directions[i] * space;
		//		if (posCheck == room.position)
		//			return;
		//		else
		//			newRoom.possibleDirections.Add(directions[i]);
		//	}
		//	Debug.Log("other");
		//	newRoom.direction = newRoom.possibleDirections[Random.Range(0, newRoom.possibleDirections.Count)]; //Chooses a random direction for the next room to generate
		//}

		//Instantiate(newRoom.prefab, newRoom.position, Quaternion.identity); //Instantiate the room
		//rooms[roomCount] = newRoom;
		//roomCount++;
	}
	IEnumerator TestGenerateIslands()
	{
		//	for (int i = 0; i < maxIslands; i++) //Generate a set number of islands
		//	{
		//		CreateIsland(); //Creates each island

		yield return new WaitForSeconds(waitBeforeNextIsland); //Waits before the next step
															   //	}

		//	if (islandsCount < minIslands) //If the number of island is below the minimum number of islands required
		//		RerollIslands(); //Restarts the generation process
	}
	void TestCreateIsland()
	{
		//	GameObject newPrefab = prefabs[Random.Range(0, prefabs.Count)]; //Chooses a random island in the islands list
		//	Island newIsland = new Island(newPrefab, Vector3.zero, Vector3.zero); //Creates a temporary new Island and stores it in the newIsland variable

		//	if (islandsCount == 0) //If the created island is the first one
		//	{
		//		newIsland.coordinates = centerChunk.coordinates; //Sets the coordinates of the island to the center chunk
		//	}
		//	else
		//	{
		//		Island previousIsland = islands[islandsCount - 1]; //Stores the previous island in a variable
		//		Vector3 newCoords = previousIsland.coordinates + previousIsland.direction; //Stores the coordinates of the new islands in a variable

		//		newIsland.coordinates = newCoords; //Sets the coordinates of the island to the new coordinates
		//	}

		//	if (chunks[newIsland.coordinates].possibleDirections.Count == 0) //If the new Island is leading in a dead end
		//		return; //Stops the generation to go any further

		//	newIsland.direction = chunks[newIsland.coordinates].possibleDirections[Random.Range(0, chunks[newIsland.coordinates].possibleDirections.Count)]; //Randomly sets the island's direction to one of the chunk's possible directions

		//	chunks[newIsland.coordinates].island = newIsland; //Stores the new island in the corresponding chunk
		//	islands.Add(newIsland); //Stores the new island in the islands list
		//	islandsCount = islands.Count;

		//	newIsland.islandInstance = Instantiate( //Instantiates the new island
		//		newIsland.prefab, //Sets the prefab of the new island
		//		chunks[newIsland.coordinates].chunkInstance.transform.position, //Sets the position of the new island
		//		Quaternion.identity, //Sets the rotation of the new island
		//		chunks[newIsland.coordinates].chunkInstance.transform); //Sets the parent of the new island

		//	UpdateChunks(); //Update the possible directions of all chunks
	}
	void NewIslandDebug(Vector3 _direction)
	{
		//	GameObject newPrefab = prefabs[Random.Range(0, prefabs.Count)]; //Chooses a random island in the islands list
		//	Island newIsland = new Island(newPrefab, Vector3.zero, Vector3.zero); //Creates a temporary new Island and stores it in the newIsland variable

		//	islandsCount = 0; //Sets the count of islands to 0
		//	if (islandsCount == 0) //If the created island is the first one
		//		newIsland.coordinates = centerChunk.coordinates; //Sets the coordinates of the island to the center chunk
		//	else
		//	{
		//		Island previousIsland = islands[islandsCount - 1]; //Stores the previous island in a variable
		//		Vector3 newCoords = previousIsland.coordinates + _direction; //Stores the coordinates of the new islands in a variable

		//		newIsland.coordinates = newCoords; //Sets the coordinates of the island to the new coordinates
		//	}

		//	if (chunks[newIsland.coordinates].possibleDirections.Count == 0)
		//	{
		//		Debug.Log("Reached a dead end");
		//		return;
		//	}

		//	newIsland.direction = chunks[newIsland.coordinates].possibleDirections[Random.Range(0, chunks[newIsland.coordinates].possibleDirections.Count)]; //Randomly sets the island's direction to one of the chunk's possible directions

		//	chunks[newIsland.coordinates].island = newIsland; //Stores the new island in the corresponding chunk
		//	islands.Add(newIsland); //Stores the new island in the islands list

		//	newIsland.islandInstance = Instantiate( //Instantiates the new island
		//		newIsland.prefab, //Sets the prefab of the new island
		//		chunks[newIsland.coordinates].chunkInstance.transform.position, //Sets the position of the new island
		//		Quaternion.identity, //Sets the rotation of the new island
		//		chunks[newIsland.coordinates].chunkInstance.transform); //Sets the parent of the new island

		//	UpdateChunks(); //Update the possible directions of all chunks
	}
	#endregion
}

#region CLASSES
[System.Serializable]
public class Island
{
	public GameObject prefab;
	public GameObject instance;
	public Vector3 coordinates;
	public Vector3 direction;

	public Island(GameObject _object, Vector3 _coordinates, Vector3 _direction)
	{
		this.prefab = _object;
		this.coordinates = _coordinates;
		this.direction = _direction;
	}
}

[System.Serializable]
public class Branch
{
	public int count = 0;
	public List<Chunk> chunks;
}
#endregion