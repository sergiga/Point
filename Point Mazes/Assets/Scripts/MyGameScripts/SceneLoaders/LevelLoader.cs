using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	public PlayerManager playerManager;
	public DisplayManager displayManager;

	public void Awake() {

		if (PlayerManager.Instance == null) {
			Instantiate(playerManager);
		}
		if (DisplayManager.Instance == null) {
			Instantiate(displayManager);
		}
	}
}
