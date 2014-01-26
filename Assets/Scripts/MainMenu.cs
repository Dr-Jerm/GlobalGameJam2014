using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	private Ray ray;
	private RaycastHit rayCastHit;
	
	void Awake()
	{
	}
	
	void Update ()
	{
		if(Input.GetMouseButtonDown(0))
		{
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			
			if(Physics.Raycast (ray, out rayCastHit))
			{
				if(rayCastHit.transform.name == "playButton")
					Application.LoadLevel("game");
			}
		}
	}
}