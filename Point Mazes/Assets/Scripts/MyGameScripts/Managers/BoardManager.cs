using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	public int columns;
	public int rows;
	public int mazeCount;
	public GameObject start;
	public GameObject exit;
	public GameObject tileGameObject;

	private List<Tile> startPoints = new List<Tile>();
	private Tile[,] board;
	private Transform boardHolder;
	private MazeTemplate template;

	public void Start() {
		Initialise();
		BoardSetup();
	}

	private void Initialise() {
		TextAsset jsonFile = Resources.Load("LevelsJSON/level_1") as TextAsset;
		template = MazeTemplate.CreateFromJSON(jsonFile.text);
		columns = template.columns;
		rows = template.rows;
		board = new Tile[rows, columns];
	}

	private void BoardSetup () {
		float cellSize = 1.0f;
		float boardHeight = cellSize * rows;
		float boardWidth = cellSize * columns;
		boardHolder = new GameObject("Board").transform;
		Vector3 worldBottomLeft = transform.position - (Vector3.right * boardWidth / 2) - (Vector3.up * boardHeight / 2);

		for (int y = 0; y < rows; y++) {
			for (int x = 0; x < columns; x++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * cellSize + cellSize/2) + Vector3.up * (y * cellSize + cellSize/2);
				GameObject tile;
				Tile currentTile;
				tile = Instantiate(tileGameObject, worldPoint, Quaternion.identity) as GameObject;
				tile.transform.parent = boardHolder;
				currentTile = new Tile(tile, template.GetType(y, x));
			}
		}
	}

	private void CreateMaze() {
		
	}
}
