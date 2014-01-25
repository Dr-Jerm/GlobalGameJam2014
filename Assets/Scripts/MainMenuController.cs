using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

  void OnGUI () {
    if (GUI.Button (new Rect (10,10,150,100), "I am a button")) {
      print ("You clicked the button!");
    }
  }
	
	// Update is called once per frame
	void Update () {
	
	}
}
