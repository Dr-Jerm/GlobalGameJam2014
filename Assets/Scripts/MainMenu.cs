using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
		Debug.Log ("facebook initialized");
	}
	
	void OnGUI()
	{
		Debug.Log ("On Gui Load");
		GUI.skin = MenuSkin;
		GUILayout.Box("", MenuSkin.GetStyle("panel_welcome"));
		if (FB.IsLoggedIn)                                                                                              
		{                                                                                                                
			GUI.Label((new Rect(179 , 11, 287, 160)), "Login to Facebook", MenuSkin.GetStyle("text_only"));             
			if (GUI.Button(LoginButtonRect, "", MenuSkin.GetStyle("button_login")))                                      
			{                                                                                                            
				FB.Login("email,publish_actions", LoginCallback);                                                        
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
					Camera.main.transform.position = new Vector3(0.636013f, 20f, 1.525784f);
				}
				if(rayCastHit.transform.name == "creditsButton")
				{
					sound.audio.Play();

					//Camera.main.transform.position = new Vector3(0.636013f, -20f, 1.525784f);
				}
				if(rayCastHit.transform.name == "backButton1" || rayCastHit.transform.name == "backButton2")
				{
					sound.audio.Play();
					Camera.main.transform.position = new Vector3(0.636013f, 0f, 1.525784f);
				}
			}
		}
	}
	
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
		
		// Reqest player info and profile picture                                                                           
		FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);  
		//FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, MyPictureCallback);    
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
		
		//profile = Util.DeserializeJSONProfile(result.Text);                                                                        
		//GameStateManager.Username = profile["first_name"];                                                                         
		//friends = Util.DeserializeJSONFriends(result.Text);                                                                        
	}                                                                                                                              
	
	void MyPictureCallback(FBResult result)                                                                                        
	{                                                                                                                              
		FbDebug.Log("MyPictureCallback");                                                                                          
		
		if (result.Error != null)                                                                                                  
		{                                                                                                                          
			FbDebug.Error(result.Error);                                                                                           
			// Let's just try again                                                                                                
			//FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, MyPictureCallback);                                
			return;                                                                                                                
		}                                                                                                                          
		//GameStateManager.UserTexture = result.Texture;                                                                             
	}      
}
