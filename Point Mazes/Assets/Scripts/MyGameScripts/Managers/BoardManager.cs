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
	public GameObject tileGO;
	public GameObject[] pathsGO;

	private List<Tile> startPoints;
	private Tile[,] board;
	private Transform boardHolder;
	private MazeTemplate template;

	private float maxBoardWidth = 5.0f;
	private float maxBoardHeight = 7.0f;

	public void Start() {
		Initialise();
		BoardSetup();
		for (int i = 0; i < mazeCount; i++) {
			CarvePath(startPoints[i], startPoints[i].mazeId / 10);
		}
		LayoutPath();
	}

	/* Reset the board and read the Level Template to create */
	private void Initialise() {
		TextAsset jsonFile = Resources.Load("LevelsJSON/level_1") as TextAsset;
		template = MazeTemplate.CreateFromJSON(jsonFile.text);
		columns = template.columns;
		rows = template.rows;
		board = new Tile[rows, columns];
		startPoints = new List<Tile>();
		mazeCount = 0;
	}

	/* Lay out the Tiles and set their neighbours Tiles */
	private void BoardSetup () {
		float boardHeight = Mathf.Clamp(rows, rows, maxBoardHeight);
		float boardWidth = Mathf.Clamp(columns, columns, maxBoardWidth);
		float cellSize = Mathf.Min(boardHeight / rows, boardWidth / columns);
		boardHolder = new GameObject("Board").transform;

		Vector3 worldTopLeft = transform.position - (Vector3.right * (cellSize * columns) / 2) + (Vector3.up * (cellSize * rows) / 2);

		for (int y = 0; y < rows; y++) {
			for (int x = 0; x < columns; x++) {
				Vector3 worldPoint = worldTopLeft + Vector3.right * (x * cellSize + cellSize/2) - Vector3.up * (y * cellSize + cellSize/2);
				GameObject tile;
				Tile currentTile;
				tile = Instantiate(tileGO, worldPoint, Quaternion.identity) as GameObject;
				tile.transform.parent = boardHolder;
				tile.transform.localScale = new Vector3(cellSize, cellSize, cellSize);
				tile.name = "Tile" + y + x;
				currentTile = new Tile(tile, template.GetId(y, x));
				board[y, x] = currentTile;
				if (template.GetId(y, x) >= 10) { 
					startPoints.Add(currentTile);
					mazeCount++;
				}
			}
		}

		for (int y = 0; y < rows; y++) {
			for (int x = 0; x < columns; x++) {
				Tile[] neighbours = new Tile[4];
				neighbours[(int) Direction.Top] = (y-1 < 0) ? null : board[y-1, x];
				neighbours[(int) Direction.Left] = (x-1 < 0) ? null : board[y, x-1];
				neighbours[(int) Direction.Bottom] = (y+1 == rows) ? null : board[y+1, x];
				neighbours[(int) Direction.Right] = (x+1 == columns) ? null : board[y, x+1];
				board[y, x].SetNeighbours(neighbours);
			}
		}
	}

	/* Create the path of the maze with backtracking */
	private void CarvePath (Tile start, int mazeId) {
		Tile currentTile = start;
		int[] neighbours = new int[4];
		List<Tile> stack = new List<Tile> ();

		stack.Add(currentTile);
		while (stack.Count > 0) {
			int freeNeighbourCount = 0;

			currentTile.assigned = true;
			for (int i = 0; i < 4; i++) {
				Direction neighbourDir = (Direction) i;
				Tile neighbour = currentTile.GetNeighbour(i);
				if (neighbour != null && neighbour.mazeId == mazeId && !neighbour.assigned) {
					neighbours[freeNeighbourCount] = i;
					freeNeighbourCount++;
				}
			}
			if (freeNeighbourCount > 0) {
				int carveDir = neighbours[Random.Range(0,freeNeighbourCount)];
				currentTile.CarveTile(carveDir);
				currentTile = currentTile.GetNeighbour(carveDir);
				currentTile.CarveTile((carveDir + 2) % 4);
				stack.Add(currentTile);
			}
			else {
				stack.Remove(currentTile);
				currentTile.BuildWalls();
				/*if (currentTile.walls.wallCount == 3) {
					deadEnds.Add(currentTile);
					currentTile.SetTileType (TileType.DeadEnd);
				} */
				if (stack.Count > 0) {
					currentTile = stack[stack.Count-1];
				}
			}
		}
	}

	/* Lay out the path of each maze */
	private void LayoutPath() {
		for (int y = 0; y < rows; y++) {
			for (int x = 0; x < columns; x++) {
				Tile tile = board[y, x];
				Vector3 rotation = Vector3.zero;
				int renderIndex = -1;

				if (tile.wallCount == 3) {
					for (int i = 0; i < 4; i++) {
						if (!tile.IsWall((Direction) i)) {
							rotation.z = 90 * i;
						}
					}
					renderIndex = 0;
				} 
				else if (tile.wallCount == 2) {
					if (!tile.IsWall(Direction.Top) && !tile.IsWall(Direction.Bottom)) { 
						rotation.z = 90;
						renderIndex = 1;
					} 
					else if(!tile.IsWall(Direction.Left) && !tile.IsWall(Direction.Right)) {
						rotation.z = 0;
						renderIndex = 1;
					}
					else {
						if (!tile.IsWall(Direction.Right) && !tile.IsWall(Direction.Bottom)) {
							rotation.z = 0;
						} 
						else if (!tile.IsWall(Direction.Top) && !tile.IsWall(Direction.Right)) {
							rotation.z = 90;
						}
						else if (!tile.IsWall(Direction.Top) && !tile.IsWall(Direction.Left)) {
							rotation.z = 180;
						}
						else {
							rotation.z = 270;
						}
						renderIndex = 2;
					}
				}
				else if (tile.wallCount == 1) {
					for (int i = 0; i < 4; i++) {
						if (tile.IsWall((Direction) i)) {
							rotation.z = 90 * i;
						}
					}
					renderIndex = 3;		
				} 
				else {
					renderIndex = 4;
				}

				GameObject go = Instantiate (pathsGO[renderIndex], Vector3.zero, Quaternion.identity) as GameObject;
				go.transform.parent = tile.reference.transform;
				go.transform.localPosition = Vector3.zero;
				go.transform.localScale = Vector3.one;
				go.transform.eulerAngles = rotation;
			}
		}
	}

	/* Initializes the level and calls the previous functions to lay out the game board */
	public void SetupScene() {
		Initialise();
		BoardSetup();
		for (int i = 0; i < mazeCount; i++) {
			CarvePath(startPoints[i], startPoints[i].mazeId / 10);
		}
	}
}
