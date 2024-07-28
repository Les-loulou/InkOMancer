using System.Collections.Generic;
using UnityEngine;

public class SC_LC_RoomsGeneration : MonoBehaviour
{
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
	[SerializeField] GameObject islandPref;

	[Space(2)]
	[Header("VALUES")]
	[SerializeField] int islandCount;

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

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space)) //DEBUG
			CreateIsland();

		if (Input.GetKeyDown(KeyCode.UpArrow))
			NewIslandDebug(new Vector3(0, 0, 1));
		if (Input.GetKeyDown(KeyCode.RightArrow))
			NewIslandDebug(new Vector3(1, 0, 0));
		if (Input.GetKeyDown(KeyCode.DownArrow))
			NewIslandDebug(new Vector3(0, 0, -1));
		if (Input.GetKeyDown(KeyCode.LeftArrow))
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
				Chunk newChunk = new Chunk(chunkPrefab, new Vector3(x, 0, z), new List<Vector3>()); //Creates a new chunk passing the position in X and Z as parameters

				float xCenter = Mathf.Ceil(xLength / 2); //Finds the center of the X axis
				float zCenter = Mathf.Ceil(zLength / 2); //Finds the center of the Z axis
				if (newChunk.chunkPos == new Vector3(xCenter, 0, zCenter)) //Checks if the current chunk position is the center of the grid
					centerChunk = newChunk; //Sets the center of the grid to the centerChunk variable

				chunks.Add(newChunk.chunkPos, newChunk); //Adds the new chunk to the chunks dictionary using it's position as a key
				newChunk.chunkInstance = Instantiate(newChunk.chunkPref, start.transform.position + newChunk.chunkPos * chunkOffset, Quaternion.identity, gridParent); //Instantiates the new chunk
			}

		UpdatePossibleDirections(); //Update the possible directions of all to chunks
	}

	void CreateIsland()
	{
		Island newIsland = new Island(islandPref, Vector3.zero, Vector3.zero); //Creates a temporary new Island and stores it in the newIsland variable

		if (islandCount == 0) //If the created island is the first one
			SetupIsland(newIsland, centerChunk.chunkPos); //Sets the position and direction of the first island on the center chunk
		else
		{
			Island previousIsland = islands[islandCount - 1]; //Stores the previous island in a variable
			Vector3 newPosition = previousIsland.position + previousIsland.direction; //Stores the position of the new islands in a variable

			SetupIsland(newIsland, newPosition); //Sets the position and direction of the next islands on the center chunk
		}

		islandCount++; //Adds one to the count of islands
		chunks[newIsland.position].island = newIsland; //Stores the new island in the corresponding chunk
		islands.Add(newIsland); //Stores the new island in the islands list

		newIsland.islandInstance = Instantiate( //Instantiates the new island
			newIsland.prefab, //Sets the prefab of the new island
			chunks[newIsland.position].chunkInstance.transform.position, //Sets the position of the new island
			Quaternion.identity, //Sets the rotation of the new island
			chunks[newIsland.position].chunkInstance.transform); //Sets the parent of the new island

		UpdatePossibleDirections(); //Update the possible directions of all to chunks
	}

	void SetupIsland(Island _newIsland, Vector3 _position)
	{
		_newIsland.position = _position; //Sets the position of the island
		_newIsland.direction = chunks[_newIsland.position].possibleDirections[Random.Range(0, chunks[_newIsland.position].possibleDirections.Count)]; //Randomly sets the island's direction to one of the chunk's possible directions
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

	bool BoundsCheck(KeyValuePair<Vector3, Chunk> _chunk, int _i)
	{
		var xDirectionCheck = (_chunk.Key + directions[_i]).x; //Sets the direction check variable on the X axis
		var zDirectionCheck = (_chunk.Key + directions[_i]).z; //Sets the direction check variable on the Z axis
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
		islandCount = 0; //Sets the count of islands to 0

		for (int i = 0; i < gridParent.transform.childCount; i++) //For each chunk
			if (gridParent.transform.GetChild(i).childCount > 0) //If the chunk contains an island
				Destroy(gridParent.transform.GetChild(i).GetChild(0).gameObject); //Destroy the island

		foreach (KeyValuePair<Vector3, Chunk> chunk in chunks) //For each chunk in the chunks disctionary
			chunk.Value.island = null; //Clears the island assigned to this chunk

	}

	void NewIslandDebug(Vector3 _direction)
	{
		Island newIsland = new Island(islandPref, Vector3.zero, Vector3.zero);

		if (islandCount == 0)
			SetupIsland(newIsland, centerChunk.chunkPos);
		else
		{
			Island previousIsland = islands[islandCount - 1];
			Vector3 newPosition = previousIsland.position + previousIsland.direction;

			SetupIsland(newIsland, previousIsland.position + _direction);
		}

		islandCount++;
		chunks[newIsland.position].island = newIsland;
		islands.Add(newIsland);

		newIsland.islandInstance = Instantiate(newIsland.prefab, chunks[newIsland.position].chunkInstance.transform.position, Quaternion.identity, chunks[newIsland.position].chunkInstance.transform);
		UpdatePossibleDirections();
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
	public Vector3 chunkPos;
	public List<Vector3> possibleDirections = new List<Vector3>();

	public Island island;

	public Chunk(GameObject _prefab, Vector3 _position, List<Vector3> _pDirections)
	{
		this.chunkPref = _prefab;
		this.chunkPos = _position;
		this.possibleDirections = _pDirections;
	}
}

[System.Serializable]
public class Island
{
	public GameObject prefab;
	public GameObject islandInstance;
	public Vector3 position;
	public Vector3 direction;

	public Island(GameObject _object, Vector3 _position, Vector3 _direction)
	{
		this.prefab = _object;
		this.position = _position;
		this.direction = _direction;
	}
}
#endregion