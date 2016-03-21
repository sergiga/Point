using UnityEngine;
using System;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

	public int columns;
	public int rows;
	public int mazeCount;
	public GameObject start;
	public GameObject exit;
	public GameObject[] pathTiles;

	private List<Tile> startPoints = new List<Tile>();
	private Tile[,] board;
	private Transform gameHolder;
	private Transform boardHolder;
	private Transform pointHolder;

	private void Initialise() {
		board = new Tile[rows, columns];
	}

	private void CreateMaze() {

	}

	private void BoardSetup() {

		gameHolder = new GameObject("GamePlay").transform;
		boardHolder = new GameObject("Board").transform;
		pointHolder = new GameObject("Points").transform;
		boardHolder.parent = gameHolder;
		pointHolder.parent = gameHolder;

		for(int x = 0; x < columns; x++) {
			for(int y = 0; y < rows; y++) {

			}
		}
	}
}
