using UnityEngine;
using System.Collections;

public class MissleAI : MonoBehaviour {

	public bool isReal = true; 
	public string ownerID = "";

	public GameObject missleImpact;
	// Use this for initialization
	float misslespeed = 1000; 


	void Start ()
	{
		rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY; 
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rigidbody.AddRelativeForce (0,misslespeed * Time.deltaTime,0);
	}

	public void setInitialSpeed(Vector3 Velocity, Vector3 AngularVelocity)
	{
		Debug.Log("Set Speeds");
		rigidbody.angularVelocity = AngularVelocity;
		rigidbody.velocity = Velocity;
	}


	void OnCollisionEnter(Collision col)
	{

	//Debug.Log("Collision!");
	//if (col.gameObject.tag == "asteroid")
	//if(col.gameObject.tag == "asteroid")
	//{
		ContactPoint contactpoint = col.contacts[0];
		Instantiate(missleImpact, contactpoint.point, Quaternion.LookRotation(contactpoint.normal));
		Destroy (gameObject, 0.1f);
	
	

		
	//}
	}
}
