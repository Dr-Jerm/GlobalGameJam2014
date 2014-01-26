using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public GameObject sound;

	private Ray ray;
	private RaycastHit rayCastHit;

	void Awake()
	{
		// Initialize FB SDK              
		enabled = false;
		FB.Init(SetInit, OnHideUnity);  
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
					Camera.main.transform.position = new Vector3(0f, 20f, 1.8f);
				}
				if(rayCastHit.transform.name == "creditsButton")
				{
					sound.audio.Play();
					Camera.main.transform.position = new Vector3(0f, -20f, 1.8f);
				}
				if(rayCastHit.transform.name == "backButton1" || rayCastHit.transform.name == "backButton2")
				{
					sound.audio.Play();
					Camera.main.transform.position = new Vector3(0f, 0f, 1.8f);
				}
			}
		}
	}

	// Facebook integration initialization
	private void SetInit()                                                                       
	{                                                                                            
		FbDebug.Log("SetInit");                                                                  
		enabled = true; // "enabled" is a property inherited from MonoBehaviour                  
		if (FB.IsLoggedIn)                                                                       
		{                                                                                        
			FbDebug.Log("Already logged in");                                                    
			//OnLoggedIn();                                                                        
		}                                                                                        
	}                                                                                            

	private void OnHideUnity(bool isGameShown)                                                   
	{                                                                                            
		FbDebug.Log("OnHideUnity");                                                              
		if (!isGameShown)                                                                        
		{                                                                                        
			// pause the game - we will need to hide                                             
			Time.timeScale = 0;                                                                  
		}                                                                                        
		else                                                                                     
		{                                                                                        
			// start the game back up - we're getting focus again                                
			Time.timeScale = 1;                                                                  
		}                                                                                        
	}
}
