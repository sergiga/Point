using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayManager : Singleton<DisplayManager> {

	public Text displayText;
	public float displayTime;
	public float fadeTime;
	
	private IEnumerator fadeAlpha;

	public void Start() {
		Transform parent = GameObject.Find("Management").transform;
		transform.parent = parent;
		transform.name = "DisplayManager";
		displayText = GameObject.Find("DisplayManagerText").GetComponent<Text>();
	}

	/* Display a message in the bottom of the screen */
	public void DisplayMessage (string message) {
		displayText.text = message;
		SetAlpha ();
	}

	/* Set the text alpha to 1 so it can be visible */
	public void SetAlpha () {
		if (fadeAlpha != null) {
			StopCoroutine (fadeAlpha);
		}
		fadeAlpha = FadeAlpha ();
		StartCoroutine (fadeAlpha);
	}

	/* Fade away the display message after displayTime seconds */
	public IEnumerator FadeAlpha () {
		Color resetColor = displayText.color;
		resetColor.a = 1;
		displayText.color = resetColor;
		
		yield return new WaitForSeconds (displayTime);
		
		while (displayText.color.a > 0) {
			Color displayColor = displayText.color;
			displayColor.a -= Time.deltaTime / fadeTime;
			displayText.color = displayColor;
			yield return null;
		}
		yield return null;
	}
}
