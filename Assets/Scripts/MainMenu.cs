using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using System;

public class MainMenu : MonoBehaviour {
	public GameObject sound;

	private Ray ray;
	private RaycastHit rayCastHit;

	private static List<object>                 friends         = null;
	private static Dictionary<string, string>   profile         = null;
	private bool    haveUserPicture       = false;

	enum LoadingState 
	{
		WAITING_FOR_INIT,
		WAITING_FOR_INITIAL_PLAYER_DATA,
		DONE
	};
	
	private LoadingState loadingState = LoadingState.WAITING_FOR_INIT;
	
	void Awake()
	{
		// Initialize FB SDK              
		enabled = false;
		Debug.Log ("test");
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

	void OnLoggedIn()
	{
		FbDebug.Log("Logged in. ID: " + FB.UserId);
		
		// Reqest player info and profile picture
		FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);
		//TODO: put these in
		//FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, MyPictureCallback);
		// Load high scores
		//QueryScores();
	}

	void APICallback(FBResult result)
	{
		FbDebug.Log("APICallback");
		if (result.Error != null)
		{
			FbDebug.Error(result.Error);
			// Let's just try again
			FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);
			return;
		}
		
		profile = Util.DeserializeJSONProfile(result.Text);
		//GameStateManager.Username = profile["first_name"];
		friends = Util.DeserializeJSONFriends(result.Text);
		checkIfUserDataReady();
	}

	void MyPictureCallback(FBResult result)
	{
		FbDebug.Log("MyPictureCallback");
		
		if (result.Error != null)
		{
			FbDebug.Error(result.Error);
			// Let's just try again
			FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, MyPictureCallback);
			return;
		}
		
		//GameStateManager.UserTexture = result.Texture;
		haveUserPicture = true;
		checkIfUserDataReady();
	}

	void checkIfUserDataReady()
	{
		FbDebug.Log("checkIfUserDataReady");
		//if (loadingState == LoadingState.WAITING_FOR_INITIAL_PLAYER_DATA && haveUserPicture && !string.IsNullOrEmpty(GameStateManager.Username))
		//{
		//	FbDebug.Log("user data ready");
		//	loadingState = LoadingState.DONE;
		//}
	}
}
