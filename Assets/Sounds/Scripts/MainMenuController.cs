using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

  	public Texture buttonPanel;
	public Texture background;

	private float scale;
	// Use this for initialization
	void Start () {
		Screen.showCursor = true;
	}

	void OnGUI () {

		int width = Screen.width;
		int height = Screen.height;

		drawBackground(width,height);
		Rect panelPos = drawButtonPanel(width,height);
		drawButtons(panelPos);

	}

	// Update is called once per frame
	void Update () {

	}

	void drawButtons (Rect panelPos) {
		int xOffset = 20;
		int yOffset = 27;
		int width = 150;
		int height = 100;

		Rect buttonRect = new Rect (panelPos.x+xOffset,panelPos.y+yOffset,width,height);
		Rect scaledRect = scaleRect(buttonRect, this.scale);

		if (GUI.Button (scaledRect, "Start")) {
			Application.LoadLevel ("game"); ;
		}
	}

	Rect scaleRect(Rect o, float scale) {
		return new Rect(o.x/scale, o.y*scale, o.width*scale, o.height*scale);
	}

	Rect drawButtonPanel(int screenWidth, int screenHeight) {
		int imgHeight = 361;
		int imgWidth = 867;

		float newWidth = screenWidth * 0.9F;
		this.scale = newWidth/imgWidth;
		float newHeight = imgHeight*this.scale;

		int xPos = (int)(screenWidth/2) - (int)(newWidth/2);
		int yPos = screenHeight - (int)newHeight;

		Rect rect = new Rect(xPos, yPos, newWidth, newHeight);

		GUI.DrawTexture(rect, buttonPanel);

		return rect;
	}

	void drawBackground (int screenWidth, int screenHeight) {
		int imgHeight = 720;
		int imgWidth = 1280;

		float newHeight = screenHeight;
		float newWidth = (float)imgWidth*((float)screenHeight/(float)imgHeight);

		Debug.Log (imgWidth + "*" + screenHeight + "/" + imgHeight + "=" + newWidth);

		int xPos = (int)(screenWidth/2) -(int)(newWidth/2);
		int yPos = 0;

		GUI.DrawTexture(new Rect(xPos, yPos, newWidth, newHeight), background);
	}
}
