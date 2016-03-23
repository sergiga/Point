using UnityEngine;
using System.Collections;

public enum Direction {
	Top = 0,
	Left = 1,
	Bottom = 2,
	Right = 3
};

public class Tile {

	public bool assigned;
	public int mazeId;
	public GameObject reference;
	public int wallCount;

	private Tile[] neighbours;
	private bool[] walls = { true, true, true, true };

	public Tile(GameObject reference, int mazeId) {
		this.reference = reference;
		this.mazeId = mazeId;
		assigned = false;
		wallCount = 4;
	}

	/* Set the neighbours of the Tile */
	public void SetNeighbours(Tile[] neighbours) {
		this.neighbours = neighbours;
	}

	/* Set the value of the wall in the Tile */
	public void SetWall(Direction dir, bool state) {
		walls[(int) dir] = state;
	}

	/* Return true if there is a wall in the specified direction */
	public bool IsWall(Direction dir) {
		return walls[(int) dir];
	}

	/* Set the walls of the GameObject */
	public void BuildWalls () {
		reference.transform.GetChild(0).gameObject.SetActive(walls[(int) Direction.Top]);
		reference.transform.GetChild(1).gameObject.SetActive(walls[(int) Direction.Left]);
		reference.transform.GetChild(2).gameObject.SetActive(walls[(int) Direction.Bottom]);
		reference.transform.GetChild(3).gameObject.SetActive(walls[(int) Direction.Right]);
	}
		
	/* Return the neighbour in the sspecified direction */
	public Tile GetNeighbour(int direction) {
		return neighbours[direction];
	}

	/* Carve the tile wall in the specified direction */
	public void CarveTile(int direction) {
		walls[direction] = false;
		wallCount--;
	}
}
