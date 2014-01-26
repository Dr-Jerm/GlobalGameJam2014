using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using System;

public class MainMenu : MonoBehaviour {
	public GameObject sound;
	public GUISkin MenuSkin;
	public Rect LoginButtonRect;                // Position of login button

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
		if (GUI.Button (new Rect (460, 260, 20, 20), "Lo"))
		{                                                                                                            
			FB.Login ("email,publish_actions", LoginCallback);                                                        
		}
		if (FB.IsLoggedIn)                                                   
		{                                                                    
			if (GUI.Button (new Rect(460, 280, 20, 20), "Ch"))
			{                                                                
				onChallengeClicked();                                        
			}
			if (GUI.Button (new Rect(460, 300, 20, 20), "Br"))
			{
				onBragClicked();
			}
			if (GUI.Button (new Rect(530, 280, 20, 20), "Pu"))
			{
				onScorePublishClicked();
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

	private void onScorePublishClicked()
	{
		var query = new Dictionary<string, string>();
		query["score"] = "5";
		FB.API("/me/scores", Facebook.HttpMethod.POST, delegate(FBResult r) { FbDebug.Log("Result: " + r.Text); }, query);
	}

	private void QueryScores()
	{
		FB.API("/app/scores?fields=score,user.limit(20)", Facebook.HttpMethod.GET, ScoresCallback);
	}
	
	void ScoresCallback(FBResult result) 
	{
		FbDebug.Log("ScoresCallback");
		/*if (result.Error != null)
		{
			FbDebug.Error(result.Error);
			return;
		}
		
		scores = new List<object>();
		List<object> scoresList = Util.DeserializeScores(result.Text);
		
		foreach(object score in scoresList) 
		{
			var entry = (Dictionary<string,object>) score;
			var user = (Dictionary<string,object>) entry["user"];
			
			string userId = (string)user["id"];
			
			if (string.Equals(userId,FB.UserId))
			{
				// This entry is the current player
				int playerHighScore = getScoreFromEntry(entry);
				FbDebug.Log("Local players score on server is " + playerHighScore);
				if (playerHighScore < GameStateManager.Score)
				{
					FbDebug.Log("Locally overriding with just acquired score: " + GameStateManager.Score);
					playerHighScore = GameStateManager.Score;
				}
				
				entry["score"] = playerHighScore.ToString();
				GameStateManager.HighScore = playerHighScore;
			}
			
			scores.Add(entry);
			if (!friendImages.ContainsKey(userId))
			{
				// We don't have this players image yet, request it now
				FB.API(Util.GetPictureURL(userId, 128, 128), Facebook.HttpMethod.GET, pictureResult =>
				       {
					if (pictureResult.Error != null)
					{
						FbDebug.Error(pictureResult.Error);
					}
					else
					{
						friendImages.Add(userId, pictureResult.Texture);
					}
				});
			}
		}
		
		// Now sort the entries based on score
		scores.Sort(delegate(object firstObj,
		                     object secondObj)
		            {
			return -getScoreFromEntry(firstObj).CompareTo(getScoreFromEntry(secondObj));
		}
		);*/
	}
}	