using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public GameObject sound;

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
				{
					sound.audio.Play();
					Application.LoadLevel("game");
				}
				if(rayCastHit.transform.name == "helpButton")
				{
					sound.audio.Play();
					Camera.main.transform.position = new Vector3(0.636013f, 20f, 1.525784f);
				}
				if(rayCastHit.transform.name == "creditsButton")
				{
					sound.audio.Play();
					Camera.main.transform.position = new Vector3(0.636013f, -20f, 1.525784f);
				}
				if(rayCastHit.transform.name == "backButton1" || rayCastHit.transform.name == "backButton2")
				{
					sound.audio.Play();
					Camera.main.transform.position = new Vector3(0.636013f, 0f, 1.525784f);
				}
			}
		}
	}
}