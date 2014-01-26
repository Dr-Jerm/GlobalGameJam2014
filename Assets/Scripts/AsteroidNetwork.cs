using UnityEngine;
using System.Collections;


public class AsteroidNetwork : Photon.MonoBehaviour
{
 
	private Vector3 correctAsteroidPos = Vector3.zero; //We lerp towards this
	private Quaternion correctAsteroidRot = Quaternion.identity; //We lerp towards this
	private Vector3 correctAsteroidAngularVelocity = Vector3.zero; //We lerp towards this
	private Vector3 correctAsteroidVelocity = Vector3.zero;

  private bool dirty = false;
	
    void Awake()
    {
         if (photonView.isMine)
        {
            //MINE: local asteroid, simply enable the local scripts
        }
        else
        {           

        }

        gameObject.name = gameObject.name + photonView.viewID;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
		//Debug.Log("MyID:"+gameObject.name+" SerializeView");

		if (stream.isWriting)
        {
            //We own this asteroid: send the others our data
            //Debug.Log("MyID:"+gameObject.name+" Writing: "+gameObject.rigidbody.transform.position + " ");
            Debug.Log("Writing Data!");
			stream.SendNext(gameObject.rigidbody.transform.position);
			stream.SendNext(gameObject.rigidbody.transform.rotation); 
			stream.SendNext(gameObject.rigidbody.angularVelocity);
			stream.SendNext(gameObject.rigidbody.velocity);
        }
        else
        {
          Debug.Log("Receiving Data!");
            //Network asteroid, receive data
            correctAsteroidPos = (Vector3)stream.ReceiveNext();
            correctAsteroidRot = (Quaternion)stream.ReceiveNext();
			correctAsteroidAngularVelocity = (Vector3)stream.ReceiveNext();
			correctAsteroidVelocity = (Vector3)stream.ReceiveNext();
            dirty = true;
        }
    }

   
    void Update()
    {
		//Debug.Log("MyID:"+gameObject.name);
		if (!photonView.isMine && dirty)
        {
			Debug.Log("MyID:"+gameObject.name+" Replicating: "+gameObject.rigidbody.transform.position + " ");
			//Update remote asteroid (smooth this, this looks good, at the cost of some accuracy)
			gameObject.rigidbody.transform.position = Vector3.Lerp(gameObject.rigidbody.transform.position, correctAsteroidPos, Time.deltaTime * 5);
			gameObject.rigidbody.transform.rotation = Quaternion.Lerp(gameObject.rigidbody.transform.rotation, correctAsteroidRot, Time.deltaTime * 5);
			gameObject.rigidbody.angularVelocity = Vector3.Lerp(gameObject.rigidbody.angularVelocity, correctAsteroidAngularVelocity, Time.deltaTime * 5);
			gameObject.rigidbody.velocity = Vector3.Lerp(gameObject.rigidbody.velocity, correctAsteroidVelocity, Time.deltaTime * 5);
		  dirty = false;
        }
    }
}