using UnityEngine;
using System.Collections;

public enum PlayerEvent {
	None  = 0,
	Fire = 2,
	Death = 10,
	Respawn = 11
}


public class StarThirdPersonNetwork : Photon.MonoBehaviour
{
    StarThirdPersonCamera cameraScript;
    StarThirdPersonController controllerScript;

	private PlayerEvent playerevent = PlayerEvent.None;


	private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
	private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this
	private Vector3 correctPlayerAngularVelocity = Vector3.zero; //We lerp towards this
	private Vector3 correctPlayerVelocity = Vector3.zero;
	private float 	correctPlayerInputHorz = 0;
	private float 	correctPlayerInputVert = 0;

    void Awake()
    {
		cameraScript = gameObject.GetComponent<StarThirdPersonCamera>();
		controllerScript = gameObject.GetComponent<StarThirdPersonController>();

         if (photonView.isMine)
        {
            //MINE: local player, simply enable the local scripts


			print(cameraScript.gameObject.rigidbody.transform.position.x);
			cameraScript.enabled = true;
            controllerScript.enabled = true;
        }
        else
        {           

			cameraScript.enabled = false;

            controllerScript.enabled = true;
            controllerScript.isControllable = false;
        }

        gameObject.name = gameObject.name + photonView.viewID;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
		//Debug.Log("MyID:"+gameObject.name+" SerializeView");

		if (stream.isWriting)
        {
            //We own this player: send the others our data
			//stream.SendNext((int)controllerScript._shipState);
			stream.SendNext(gameObject.rigidbody.transform.position);
			stream.SendNext(gameObject.rigidbody.transform.rotation); 
			stream.SendNext(gameObject.rigidbody.angularVelocity);
			stream.SendNext(gameObject.rigidbody.velocity);
			stream.SendNext(controllerScript.inputHorz);
			stream.SendNext(controllerScript.inputVert);
			stream.SendNext(playerevent);

        }
        else
        {
            //Network player, receive data
			//controllerScript._shipState = (ShipState)(int)stream.ReceiveNext();
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
			correctPlayerAngularVelocity = (Vector3)stream.ReceiveNext();
			correctPlayerVelocity = (Vector3)stream.ReceiveNext();
			correctPlayerInputHorz = (float)stream.ReceiveNext();
			correctPlayerInputVert = (float)stream.ReceiveNext();
			playerevent = (PlayerEvent)stream.ReceiveNext();

        }
    }

   
    void Update()
    {
		//Debug.Log("MyID:"+gameObject.name);
		if (!photonView.isMine)
        {
			//Debug.Log("MyID:"+gameObject.name+" Replicating");
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			gameObject.rigidbody.transform.position = Vector3.Lerp(gameObject.rigidbody.transform.position, correctPlayerPos, Time.deltaTime * 5);
			gameObject.rigidbody.transform.rotation = Quaternion.Lerp(gameObject.rigidbody.transform.rotation, correctPlayerRot, Time.deltaTime * 5);
			gameObject.rigidbody.angularVelocity = Vector3.Lerp(gameObject.rigidbody.angularVelocity, correctPlayerAngularVelocity, Time.deltaTime * 5);
			gameObject.rigidbody.velocity = Vector3.Lerp(gameObject.rigidbody.velocity, correctPlayerVelocity, Time.deltaTime * 5);
			controllerScript.inputHorz = correctPlayerInputHorz;
			controllerScript.inputVert = correctPlayerInputVert;

			checkedPlayerEvent();
		
        }
    }

	//called by StarThirdPersonController
	public void pingPlayerEventForReplication(PlayerEvent _playerEvent)
	{
		playerevent = _playerEvent;

	}


	void checkedPlayerEvent()
	{
		if (playerevent == PlayerEvent.Death) 
		{
			controllerScript.replicatedeathevent();
			playerevent = PlayerEvent.None;
		}
		if (playerevent == PlayerEvent.Respawn) 
		{
			controllerScript.replicaterespawn();
			playerevent = PlayerEvent.None;
		}


	}

}