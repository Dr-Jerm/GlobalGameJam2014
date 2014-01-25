using UnityEngine;
using System.Collections;

public class CamControl : MonoBehaviour
{
		
	public Transform shiptransform; 
	private Vector3 newCamPostion = new Vector3();  
	
	// Use this for initialization
	void Awake ()
	{

		newCamPostion.z = transform.position.z;
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		if (shiptransform != null) 
		{
			newCamPostion.x = shiptransform.position.x;
			newCamPostion.y = shiptransform.position.y;
			transform.position = newCamPostion;

			//transform.LookAt (shiptransform);
			transform.rotation = shiptransform.rotation;
		}
	}
}

