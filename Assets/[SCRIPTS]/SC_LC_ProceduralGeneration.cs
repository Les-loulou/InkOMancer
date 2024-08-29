using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_LC_ProceduralGeneration : MonoBehaviour
{
	SC_LC_PlayerGlobal player;

	[Header(" ---=[ GRID GENERATION ]=---")]
	[Header("OBJECTS")]
	[SerializeField] GameObject start;
	[SerializeField] Transform gridParent;
	[SerializeField] GameObject chunkPrefab;

	[Space]
	[SerializeField] bool autoGenerate;

	[Space(2)]
	[Header("PARAMETERS")]
	[SerializeField] float xLength;
	[SerializeField] float zLength;
	[SerializeField] float chunkOffset;
	Chunk centerChunk;
	Vector3[] directions = new Vector3[4]
	{
		new Vector3(0, 0, 1),
		new Vector3(1, 0, 0),
		new Vector3(0, 0, -1),
		new Vector3(-1, 0, 0),
	};

	[SerializeField] Dictionary<Vector3, Chunk> chunks = new();
	[Space(20)]
	[Header(" ---=[ ISLANDS GENERATION ]=---")]
	[Header("OBJECTS")]
	[SerializeField] List<GameObject> prefabs = new();

	[Space(2)]
	[Header("PARAMETERS")]
	[SerializeField] Vector2 islandsCountRange;
	[Space]
	[SerializeField] Vector2 branchesRange;
	[Space]
	[SerializeField] int islandsCount;
	[SerializeField] int currentIslandsCount;

	[Space]
	[SerializeField] float waitBeforeNextIsland;

	[Space]
	[SerializeField] List<Island> islands = new();
	[SerializeField] List<Chunk> branchableChunks = new();


	private void Start()
	{
		player = SC_LC_PlayerGlobal.instance;

		GenerateGrid();

		if (autoGenerate == true)
			StartCoroutine(GenerateIslands());
	}

	void Update()
	{
		if (player.inputs.createIslandPressed == true) //DEBUG
			CreateRandomIsland();

		if (player.inputs.rerollIslandsPressed == true)
			RerollIslands();

		if (player.inputs.upIslandPressed == true)
			CreateIsland(islandsCount, new Vector3(0, 0, 1));
		if (player.inputs.rightIslandPressed == true)
			CreateIsland(islandsCount, new Vector3(1, 0, 0));
		if (player.inputs.downIslandPressed == true)
			CreateIsland(islandsCount, new Vector3(0, 0, -1));
		if (player.inputs.leftIslandPressed == true)
			CreateIsland(islandsCount, new Vector3(-1, 0, 0));
	}

	#region CHUNKS
	void GenerateGrid()
	{
		if (zLength % 2 != 0 || xLength % 2 != 0) //Checks if one of the axis is an even number
		{
			Debug.LogError("Can't create a center point to the grid. xLenght and zLenght should be even");
			return;
		}

		for (int x = 0; x <= xLength; x++) //Creates chunks on the X axis
			for (int z = 0; z <= zLength; z++) //Creates chunks on the Z axis
			{
				Chunk newChunk = new Chunk(chunkPrefab, new Vector3(x, 0, z), new List<Vector3>()); //Creates a new chunk passing the X and Z coordinates as parameters

				float xCenter = Mathf.Ceil(xLength / 2); //Finds the center of the X axis
				float zCenter = Mathf.Ceil(zLength / 2); //Finds the center of the Z axis
				if (newChunk.coordinates == new Vector3(xCenter, 0, zCenter)) //Checks if the current chunk's coordinates are the center of the grid
					centerChunk = newChunk; //Sets the center of the grid to the centerChunk variable

				chunks.Add(newChunk.coordinates, newChunk); //Adds the new chunk to the chunks dictionary using it's coordinates as a key
				newChunk.chunkInstance = Instantiate(newChunk.chunkPref, start.transform.position + newChunk.coordinates * chunkOffset, Quaternion.identity, gridParent); //Instantiates the new chunk
				newChunk.chunkInstance.name = new Vector3(x, 0, z).ToString(); //Sets the name of the chunk as the coordinates of this chunk
			}

		UpdateChunks(); //Update the possible directions of all the chunks
	}

	void UpdateChunks()
	{
		foreach (var chunk in chunks) //For each chunk in the chunks dictionary
		{
			chunk.Value.possibleDirections.Clear(); //Clears all possible directions before updating it

			for (int i = 0; i < directions.Length; i++) //For each 4 directions
				if (BoundsCheck(chunk, i)) //Checks if the directions are leading out of bounds
					if (chunks[chunk.Key + directions[i]].island == null) //If the adjacent chunk is empty
						chunk.Value.possibleDirections.Add(directions[i]); //Adds the direction of the adjacent chunk to the possible directions
		}
	}

	bool BoundsCheck(KeyValuePair<Vector3, Chunk> _chunk, int _currentDirection)
	{
		var xDirectionCheck = (_chunk.Key + directions[_currentDirection]).x; //Sets the direction check variable on the X axis
		var zDirectionCheck = (_chunk.Key + directions[_currentDirection]).z; //Sets the direction check variable on the Z axis
		if (xDirectionCheck >= 0 && zDirectionCheck >= 0 && xDirectionCheck <= xLength && zDirectionCheck <= zLength) //Checks if the direction doesn't lead out of bounds
			return true;
		else
			return false;
	}

	[ContextMenu("Clear all chunks")]
	void ClearChunks()
	{
		islands.Clear(); //Clears the islands list
		branchableChunks.Clear(); //Clears the branchable chunks list
		islandsCount = 0; //Resets the islands count
		currentIslandsCount = 0; //Resets the current islands count

		for (int i = 0; i < gridParent.transform.childCount; i++) //For each chunk
			if (gridParent.transform.GetChild(i).childCount > 0) //If the chunk contains an island
				Destroy(gridParent.transform.GetChild(i).GetChild(0).gameObject); //Destroy the island

		foreach (KeyValuePair<Vector3, Chunk> chunk in chunks) //For each chunk in the chunks dictionary
			chunk.Value.island = null; //Clears the island assigned to this chunk

		UpdateChunks(); //Update the possible directions of all chunks
	}
	#endregion

	#region ISLANDS
	IEnumerator GenerateIslands()
	{
		for (int i = 0; i < islandsCountRange.y; i++) //While the number of islands is below the max number of islands
		{
			CreateRandomIsland(); //Create a new island in a random direction

			yield return new WaitForSeconds(waitBeforeNextIsland); //Waits before the next step
		}

		if (islandsCount < islandsCountRange.x) //If the number of island is below the minimum number of islands required
			RerollIslands(); //Restarts the generation process
		else
		{
			SetBranchableChunks(); //Sets the chunks that could start new branches
			CreateRandomBranches();
		}
	}

	void CreateIsland(int _i, Vector3 _direction)
	{
		Island newIsland = new Island(null, Vector3.zero, Vector3.zero); //Creates a new islands with basic parameters

		newIsland.prefab = prefabs[Random.Range(0, prefabs.Count)]; //Sets the prefab of the island to a random prefab among the list of islands prefabs

		if (_i == 0) //If the current new island is the first one
			newIsland.coordinates = centerChunk.coordinates; //Sets the coordinates of this chunk to the center of the grid
		else
		{
			newIsland.direction = _direction; //Sets the current new island direction to the direction parameter of this function
			newIsland.coordinates = islands[_i - 1].coordinates + _direction; //Adds the direction parameter to the current new island's coordinates
		}

		currentIslandsCount++; //Adds 1 to the current islands count
		if (chunks[newIsland.coordinates].island != null)
			Debug.Log(newIsland.coordinates);

		if (chunks[newIsland.coordinates].possibleDirections.Count == 0 || chunks[newIsland.coordinates].island != null) //If the new Island is leading in a dead end
			return; //Stops the generation to go any further

		islands.Add(newIsland); //Adds the new island to the islands list
		chunks[newIsland.coordinates].island = newIsland; //Stores the new island in the corresponding chunk
		islandsCount++; //Adds 1 to the islands count

		newIsland.islandInstance = Instantiate( //Instantiates the new island
			newIsland.prefab, //Sets the prefab of the new island
			chunks[newIsland.coordinates].chunkInstance.transform.position, //Sets the position of the new island
			Quaternion.identity, //Sets the rotation of the new island
			chunks[newIsland.coordinates].chunkInstance.transform); //Sets the parent of the new island

		UpdateChunks(); //Update the possible directions of all chunks
	}

	void CreateRandomIsland()
	{
		if (islandsCount == 0) //If the current island is the first one
			CreateIsland(islandsCount, centerChunk.coordinates); //Creates a new island based on the island count and the center chunk's coordinates
		else
		{
			Chunk currentChunk = chunks[islands[islandsCount - 1].coordinates]; //Sets the currentChunk variable
			Vector3 randomDirection = currentChunk.possibleDirections[Random.Range(0, currentChunk.possibleDirections.Count)]; //Sets a random direction based on the current chunk's possible directions
			CreateIsland(islandsCount, randomDirection); //Creates a new island based on the island count and the previously set random direction
		}
	}

	void RerollIslands()
	{
		StopCoroutine(GenerateIslands());

		ClearChunks();

		StartCoroutine(GenerateIslands());
	}
	#endregion

	#region BRANCHES
	void SetBranchableChunks()
	{
		foreach (Island island in islands) //For each generated island
			foreach (Vector3 direction in chunks[island.coordinates].possibleDirections) //For each adjacent chunks to those islands
				if (chunks[island.coordinates + direction].island == null && chunks[island.coordinates + direction].possibleDirections.Count == 3) //If the chunk is empty and has 3 empty chunks surrounding it
					branchableChunks.Add(chunks[island.coordinates]); //Adds this chunk to the branchable chunks list
	}

	void CreateRandomBranches()
	{
		int randomRange = (int)Random.Range(branchesRange.x, branchesRange.y);

		for (int i = 0; i < randomRange; i++)
		{
			int randomChunk = Random.Range(0, branchableChunks.Count);
			CreateIsland(islandsCount, branchableChunks[randomChunk].possibleDirections[0]);
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
public class Chunk
{
	public GameObject chunkPref;
	public GameObject chunkInstance;
	public Vector3 coordinates;
	public List<Vector3> possibleDirections = new List<Vector3>();

	public Island island;

	public Chunk(GameObject _prefab, Vector3 _coordinates, List<Vector3> _pDirections)
	{
		this.chunkPref = _prefab;
		this.coordinates = _coordinates;
		this.possibleDirections = _pDirections;
	}
}

[System.Serializable]
public class Island
{
	public GameObject prefab;
	public GameObject islandInstance;
	public Vector3 coordinates;
	public Vector3 direction;

	public Island(GameObject _object, Vector3 _coordinates, Vector3 _direction)
	{
		this.prefab = _object;
		this.coordinates = _coordinates;
		this.direction = _direction;
	}
}

//public class BranchableChunk
//{
//	public Chunk chunk;
//	public List<Vector3> possibleDirections;
//}
#endregion