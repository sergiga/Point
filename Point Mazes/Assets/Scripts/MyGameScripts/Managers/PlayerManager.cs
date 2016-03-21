using UnityEngine;
using System.Collections;

public class PlayerManager : Singleton<PlayerManager> {

	private int points;
	private int currentLevel;

	public void Start() {
		Transform parent = GameObject.Find("Management").transform;
		transform.parent = parent;
		transform.name = "PlayerManager";
		points = PlayerPrefs.GetInt("playerPoints");
		currentLevel = PlayerPrefs.GetInt("currentLevel");
	}
}
