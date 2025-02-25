using System.Collections.Generic;
using UnityEngine;

public class SC_LC_ProceduralGenerationGrid : MonoBehaviour
{
	public SC_LC_ProceduralGenerationIslands islandsScript;

	[Space(2)]
	[Header("OBJECTS")]
	public GameObject start;
	public Transform gridParent;
	public GameObject chunkPrefab;

	[Space(2)]
	[Header("PARAMETERS")]
	public float xLength;
	public float zLength;
	public float chunkOffset;
	[HideInInspector] public Chunk centerChunk;
	Vector3[] directions = new Vector3[4]
	{
		new Vector3(0, 0, 1),
		new Vector3(1, 0, 0),
		new Vector3(0, 0, -1),
		new Vector3(-1, 0, 0),
	};

	//List<Chunk> serializedChunks = new();
	[HideInInspector] public Dictionary<Vector3, Chunk> chunks = new();

	private void Start()
	{
		GenerateGrid();

		//foreach (var chunk in chunks)
		//	serializedChunks.Add(chunk.Value);
	}

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
				Chunk newChunk = new Chunk(new Vector3(x, 0, z), new List<Vector3>()); //Creates a new chunk passing the X and Z coordinates as parameters

				int xCenter = Mathf.RoundToInt(xLength / 2); //Finds the center of the X axis
				int zCenter = Mathf.RoundToInt(xLength / 2); //Finds the center of the Z axis
				if (newChunk.coordinates == new Vector3(xCenter, 0, zCenter)) //Checks if the current chunk's coordinates are the center of the grid
					centerChunk = newChunk; //Sets the center of the grid to the centerChunk variable

				chunks.Add(newChunk.coordinates, newChunk); //Adds the new chunk to the chunks dictionary using it's coordinates as a key
				newChunk.instance = Instantiate(chunkPrefab, start.transform.position + newChunk.coordinates * chunkOffset, Quaternion.identity, gridParent); //Instantiates the new chunk
				newChunk.instance.name = new Vector3(x, 0, z).ToString(); //Sets the name of the chunk as the coordinates of this chunk
			}

		UpdateChunks(); //Update the possible directions of all the chunks
	}

	public void UpdateChunks()
	{
		foreach (var chunk in chunks) //For each chunk in the chunks dictionary
		{
			chunk.Value.possibleDirections.Clear(); //Clears all possible directions before updating them

			foreach (Vector3 direction in directions) //For each 4 directions
				if (BoundsCheck(chunk, direction) == true) //Checks if the directions are leading inside of
					if (chunks.ContainsKey(chunk.Key + direction) && chunks[chunk.Key + direction] != null)
						if (chunks[chunk.Key + direction].island == null) //If the adjacent chunk is empty
							chunk.Value.possibleDirections.Add(direction); //Adds the direction of the adjacent chunk to the possible directions
		}
	}

	bool BoundsCheck(KeyValuePair<Vector3, Chunk> _chunk, Vector3 _currentDirection)
	{
		var xDirectionCheck = (_chunk.Key + _currentDirection).x; //Sets the direction check variable on the X axis
		var zDirectionCheck = (_chunk.Key + _currentDirection).z; //Sets the direction check variable on the Z axis
		if (xDirectionCheck >= 0 && zDirectionCheck >= 0 && xDirectionCheck <= xLength && zDirectionCheck <=zLength) //Checks if the direction doesn't lead out of bounds
			return true;
		else
			return false;
	}

	[ContextMenu("Clear all chunks")]
	public void ClearChunks()
	{
		islandsScript.islands.Clear(); //Clears the islands list
		islandsScript.branchableChunks.Clear(); //Clears the branchable chunks list
		islandsScript.currentIslandsStep = 0; //Resets the current islands count

		for (int i = 0; i < gridParent.transform.childCount; i++) //For each chunk
			if (gridParent.transform.GetChild(i).childCount > 0) //If the chunk contains an island
				Destroy(gridParent.transform.GetChild(i).GetChild(0).gameObject); //Destroy the island

		foreach (KeyValuePair<Vector3, Chunk> chunk in chunks) //For each chunk in the chunks dictionary
			chunk.Value.island = null; //Clears the island assigned to this chunk

		UpdateChunks(); //Update the possible directions of all chunks
	}
}

#region CLASSES
[System.Serializable]
public class Chunk
{
	[HideInInspector] public GameObject instance;
	public Vector3 coordinates;
	public List<Vector3> possibleDirections = new List<Vector3>();

	[Space]
	public Island island;

	public Chunk(Vector3 _coordinates, List<Vector3> _pDirections)
	{
		this.coordinates = _coordinates;
		this.possibleDirections = _pDirections;
	}
}
#endregion
