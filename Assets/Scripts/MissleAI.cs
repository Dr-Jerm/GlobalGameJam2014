using UnityEngine;
using System.Collections;

public class MissleAI : MonoBehaviour {

	public bool isReal = true; 
	public string ownerID = "";

	public GameObject missleImpact;
	// Use this for initialization
	float misslespeed = 1000; 

	private Vector3 newPos = Vector3.zero; //We lerp towards this
	private Quaternion newRot = Quaternion.identity; //We lerp towards this
	private Vector3 newAngularVelocity = Vector3.zero; //We lerp towards this
	private Vector3 newPlayerVelocity = Vector3.zero;

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
		//PhotonNetwork.Destroy(gameObject);
		Destroy (gameObject, 0);
	}

//	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//	{
//		//Debug.Log("MyID:"+gameObject.name+" SerializeView");
//		
//		if (stream.isWriting)
//		{
//			//We own this player: send the others our data
//			//stream.SendNext((int)controllerScript._shipState);
//			stream.SendNext(gameObject.rigidbody.transform.position);
//			stream.SendNext(gameObject.rigidbody.transform.rotation); 
//			stream.SendNext(gameObject.rigidbody.angularVelocity);
//			stream.SendNext(gameObject.rigidbody.velocity);
//			stream.SendNext(playerevent);
//			
//		}
//		else
//		{
//			//Network player, receive data
//			//controllerScript._shipState = (ShipState)(int)stream.ReceiveNext();
//			newPos = (Vector3)stream.ReceiveNext();
//			newRot = (Quaternion)stream.ReceiveNext();
//			newVelocity = (Vector3)stream.ReceiveNext();
//			newVelocity = (Vector3)stream.ReceiveNext();
//
//			newplayerevent = (PlayerEvent)stream.ReceiveNext();
//			
//		}
//	}
//	
//	
//	void Update()
//	{
//		//Debug.Log("MyID:"+gameObject.name);
//		if (!photonView.isMine)
//		{
//			//Debug.Log("MyID:"+gameObject.name+" Replicating");
//			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
//			gameObject.rigidbody.transform.position = Vector3.Lerp(gameObject.rigidbody.transform.position, correctPlayerPos, Time.deltaTime * 5);
//			gameObject.rigidbody.transform.rotation = Quaternion.Lerp(gameObject.rigidbody.transform.rotation, correctPlayerRot, Time.deltaTime * 5);
//			gameObject.rigidbody.angularVelocity = Vector3.Lerp(gameObject.rigidbody.angularVelocity, correctPlayerAngularVelocity, Time.deltaTime * 5);
//			gameObject.rigidbody.velocity = Vector3.Lerp(gameObject.rigidbody.velocity, correctPlayerVelocity, Time.deltaTime * 5);
//			controllerScript.inputHorz = correctPlayerInputHorz;
//			controllerScript.inputVert = correctPlayerInputVert;
//			controllerScript.inputJump = correctPlayerInputJump;
//			if(correctplayerevent != playerevent)
//			{
//				playerevent = correctplayerevent;
//			}
//			checkedPlayerEvent();
//			
//		}
}
