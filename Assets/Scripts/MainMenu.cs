using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using System;

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

	void OnGUI()
	{
		GUI.Label (new Rect (300, 200, 50, 50), "Login to Facebook");             
		if (GUI.Button(new Rect(300,200,50,50), ""))
		{                                                                                                            
			FB.Login ("email,publish_actions", LoginCallback);                                                        
		}

		if (FB.IsLoggedIn)                                                   
		{                                                                    
			if (GUI.Button (new Rect(360, 200, 50, 50), "Challenge"))
			{                                                                
				onChallengeClicked();                                        
			}
			if (GUI.Button (new Rect(420, 200, 50, 50), "Brag"))
			{
				onBragClicked();
			}
		}
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
					Camera.main.transform.position = new Vector3(0f, 20f, 1.5f);
				}
				if(rayCastHit.transform.name == "creditsButton")
				{
					sound.audio.Play();
					Camera.main.transform.position = new Vector3(0f, -20f, 1.5f);
				}
				if(rayCastHit.transform.name == "backButton1" || rayCastHit.transform.name == "backButton2")
				{
					sound.audio.Play();
					Camera.main.transform.position = new Vector3(0f, 0f, 1.5f);
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
			OnLoggedIn();                                                                        
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

	void LoginCallback(FBResult result)                                                        
	{                                                                                          
		FbDebug.Log("LoginCallback");                                                          
		
		if (FB.IsLoggedIn)                                                                     
		{                                                                                      
			OnLoggedIn();                                                                      
		}                                                                                      
	}                                                                                          
	
	void OnLoggedIn()                                                                          
	{                                                                                          
		FbDebug.Log("Logged in. ID: " + FB.UserId);                                            
	}  

	private void onChallengeClicked()                                                                                              
	{
		FB.AppRequest(                                                                                                         
		              message: "Space rocks dark! Check it out.",                                                                
		              title: "Play dark space rocks with me!",                                                                               
		              callback:appRequestCallback                                                                                        
		              );                                                                                                                 
		
	}

	private void onBragClicked()                                                                                                 
	{                                                                                                                            
		FbDebug.Log("onBragClicked");
		// TODO: add in kill score
		FB.Feed(                                                                                                                 
		        linkCaption: "I just dark rocked " + 100 + " rocks!?",               
		        picture: "http://static.ddmcdn.com/gif/recipes/comet-asteroid-hubble-200-130604.jpg",                                                     
		        linkName: "Checkout my Rock Dark greatness!",                                                                 
		        link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")       
		);                                                                                                               
	}

	private void appRequestCallback (FBResult result)                                                                              
	{                                                                                                                              
		FbDebug.Log("appRequestCallback");                                                                                         
		if (result != null)                                                                                                        
		{                                                                                                                          
			var responseObject = Json.Deserialize(result.Text) as Dictionary<string, object>;                                      
			object obj = 0;                                                                                                        
			if (responseObject.TryGetValue ("cancelled", out obj))                                                                 
			{                                                                                                                      
				FbDebug.Log("Request cancelled");                                                                                  
			}                                                                                                                      
			else if (responseObject.TryGetValue ("request", out obj))                                                              
			{                                                                                                                      
				// Record that we went sent a request so we can display a message                                                  
				//lastChallengeSentTime = Time.realtimeSinceStartup;                                                                 
				FbDebug.Log("Request sent");                                                                                       
			}                                                                                                                      
		}                                                                                                                          
	}    
}
