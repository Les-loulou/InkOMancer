using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SC_LC_RoomsGeneration : MonoBehaviour
{
	SC_LC_PlayerGlobal player;

	[Header(" ---=[ GRID GENERATION ]=---")]
	[Header("OBJECTS")]
	[SerializeField] GameObject start;
	[SerializeField] Transform gridParent;
	[SerializeField] GameObject chunkPrefab;

	[Space(2)]
	[Header("PARAMETERS")]
	[SerializeField] float xLength;
	[SerializeField] float zLength;
	[SerializeField] Chunk centerChunk;
	[SerializeField] float chunkOffset;

	[SerializeField] Dictionary<Vector3, Chunk> chunks = new();

	[Space(20)]
	[Header(" ---=[ ISLANDS GENERATION ]=---")]
	[Header("OBJECTS")]
	[SerializeField] List<GameObject> prefabs = new();

	[Space(2)]
	[Header("VALUES")]
	[SerializeField] int islandsCount;
	[SerializeField] int minIslands;
	[SerializeField] int maxIslands;

	[SerializeField] List<Island> islands = new();
	Vector3[] directions = new Vector3[4]
	{
		new Vector3(0, 0, 1),
		new Vector3(1, 0, 0),
		new Vector3(0, 0, -1),
		new Vector3(-1, 0, 0),
	};

	void Awake()
	{
		GenerateGrid();
	}

	private void Start()
	{
		player = SC_LC_PlayerGlobal.instance;
		
		StartCoroutine(GenerateIslands());
	}

	void Update()
	{
		if (player.inputs.createIslandPressed == true) //DEBUG
			CreateIsland();

		if (player.inputs.upIslandPressed == true)
			NewIslandDebug(new Vector3(0, 0, 1));
		if (player.inputs.rightIslandPressed == true)
			NewIslandDebug(new Vector3(1, 0, 0));
		if (player.inputs.downIslandPressed == true)
			NewIslandDebug(new Vector3(0, 0, -1));
		if (player.inputs.leftIslandPressed == true)
			NewIslandDebug(new Vector3(-1, 0, 0));
	}

	void GenerateGrid()
	{
		if (zLength % 2 != 0 || xLength % 2 != 0) //Checks if one of the axis is an even number
		{
			Debug.LogError("Can't create a center point to the grid. The xLenght and zLenght should be even");
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
			}

		UpdatePossibleDirections(); //Update the possible directions of all the chunks
	}

	IEnumerator GenerateIslands()
	{
		for (int i = 0; i < maxIslands; i++) //Generate a set number of islands
		{
			CreateIsland(); //Creates each island

			yield return new WaitForSeconds(0.05f); //Waits before the next step
		}

		if (islandsCount < minIslands) //If the number of island is below the minimum number of islands required
			RerollIslands(); //Restarts the generation process
	}

	void CreateIsland()
	{
		GameObject newPrefab = prefabs[Random.Range(0, prefabs.Count)]; //Chooses a random island in the islands list
		Island newIsland = new Island(newPrefab, Vector3.zero, Vector3.zero); //Creates a temporary new Island and stores it in the newIsland variable

		if (islandsCount == 0) //If the created island is the first one
			newIsland.coordinates = centerChunk.coordinates; //Sets the coordinates of the island to the center chunk
		else
		{
			Island previousIsland = islands[islandsCount - 1]; //Stores the previous island in a variable
			Vector3 newCoords = previousIsland.coordinates + previousIsland.direction; //Stores the coordinates of the new islands in a variable

			newIsland.coordinates = newCoords; //Sets the coordinates of the island to the new coordinates
		}

		if (chunks[newIsland.coordinates].possibleDirections.Count == 0) //If the new Island is leading in a dead end
			return; //Stops the generation to go any further

		newIsland.direction = chunks[newIsland.coordinates].possibleDirections[Random.Range(0, chunks[newIsland.coordinates].possibleDirections.Count)]; //Randomly sets the island's direction to one of the chunk's possible directions

		chunks[newIsland.coordinates].island = newIsland; //Stores the new island in the corresponding chunk
		islands.Add(newIsland); //Stores the new island in the islands list
		islandsCount = islands.Count;

		newIsland.islandInstance = Instantiate( //Instantiates the new island
			newIsland.prefab, //Sets the prefab of the new island
			chunks[newIsland.coordinates].chunkInstance.transform.position, //Sets the position of the new island
			Quaternion.identity, //Sets the rotation of the new island
			chunks[newIsland.coordinates].chunkInstance.transform); //Sets the parent of the new island

		UpdatePossibleDirections(); //Update the possible directions of all chunks
	}

	void RerollIslands()
	{
		StopCoroutine(GenerateIslands());

		ClearAll();

		StartCoroutine(GenerateIslands());
	}

	void UpdatePossibleDirections()
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

	#region DEBUG
	[ContextMenu("Clear all chunks")]
	void ClearAll()
	{
		islands.Clear(); //Clears the islands list
		islandsCount = 0;

		for (int i = 0; i < gridParent.transform.childCount; i++) //For each chunk
			if (gridParent.transform.GetChild(i).childCount > 0) //If the chunk contains an island
				Destroy(gridParent.transform.GetChild(i).GetChild(0).gameObject); //Destroy the island

		foreach (KeyValuePair<Vector3, Chunk> chunk in chunks) //For each chunk in the chunks dictionary
			chunk.Value.island = null; //Clears the island assigned to this chunk

		UpdatePossibleDirections();
	}

	void NewIslandDebug(Vector3 _direction)
	{
		GameObject newPrefab = prefabs[Random.Range(0, prefabs.Count)]; //Chooses a random island in the islands list
		Island newIsland = new Island(newPrefab, Vector3.zero, Vector3.zero); //Creates a temporary new Island and stores it in the newIsland variable

		islandsCount = 0; //Sets the count of islands to 0
		if (islandsCount == 0) //If the created island is the first one
			newIsland.coordinates = centerChunk.coordinates; //Sets the coordinates of the island to the center chunk
		else
		{
			Island previousIsland = islands[islandsCount - 1]; //Stores the previous island in a variable
			Vector3 newCoords = previousIsland.coordinates + _direction; //Stores the coordinates of the new islands in a variable

			newIsland.coordinates = newCoords; //Sets the coordinates of the island to the new coordinates
		}

		if (chunks[newIsland.coordinates].possibleDirections.Count == 0)
		{
			Debug.Log("Reached a dead end");
			return;
		}

		newIsland.direction = chunks[newIsland.coordinates].possibleDirections[Random.Range(0, chunks[newIsland.coordinates].possibleDirections.Count)]; //Randomly sets the island's direction to one of the chunk's possible directions

		chunks[newIsland.coordinates].island = newIsland; //Stores the new island in the corresponding chunk
		islands.Add(newIsland); //Stores the new island in the islands list

		newIsland.islandInstance = Instantiate( //Instantiates the new island
			newIsland.prefab, //Sets the prefab of the new island
			chunks[newIsland.coordinates].chunkInstance.transform.position, //Sets the position of the new island
			Quaternion.identity, //Sets the rotation of the new island
			chunks[newIsland.coordinates].chunkInstance.transform); //Sets the parent of the new island

		UpdatePossibleDirections(); //Update the possible directions of all chunks
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
	#endregion
}


#region CLASSES
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
#endregion