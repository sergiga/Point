using UnityEngine;
using System.Collections;

[System.Serializable]
public class MazeTemplate {

	public int columns;
	public int rows;
	public int[,] template;
	public Template[] temp;

	public static MazeTemplate CreateFromJSON(string jsonString) {
		MazeTemplate mt = JsonUtility.FromJson<MazeTemplate>(jsonString);
		mt.ParseTemplate();
		return mt;
	}

	private void ParseTemplate() {
		template = new int[rows,columns];
		for (int i = 0; i < rows; i++) {
			Template t = temp[i];
			for (int j = 0; j < columns; j++) {
				template[i,j] = t.row[j];
			}
		}
	}

	public int GetType(int row, int column) {
		return template[row, column];
	}
}

[System.Serializable]
public class Template {
	public int[] row;
}
