using UnityEngine;
using System.Collections;

public enum Direction {
	Top = 0,
	Left = 1,
	Bottom = 2,
	Right = 3
};

public class Tile : MonoBehaviour {

	private GameObject reference;

	private int type;
	private Tile[] neighbours;
	private bool[] walls;

	public Tile(GameObject reference, int type) {
		this.reference = reference;
		this.type = type;
		neighbours = new Tile[4];
		walls = new bool[4];
	}

	/* Set the neighbours of the Tile */
	public void SetNeighbours(Tile top, Tile left, Tile bottom, Tile right) {
		neighbours[(int) Direction.Top] = top;
		neighbours[(int) Direction.Left] = left;
		neighbours[(int) Direction.Bottom] = bottom;
		neighbours[(int) Direction.Right] = right;
	}

	/* Set the value of the wall in the Tile */
	public void SetWall(Direction dir, bool state) {
		walls[(int) dir] = state;
	}
}
