using UnityEngine;
using System.Collections;

public class StarThirdPersonNetwork : Photon.MonoBehaviour
{
    StarThirdPersonCamera cameraScript;
    StarThirdPersonController controllerScript;

	private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
	private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this


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
        if (stream.isWriting)
        {
            //We own this player: send the others our data
			stream.SendNext((int)controllerScript._shipState);
			stream.SendNext(gameObject.rigidbody.transform.position);
			stream.SendNext(gameObject.rigidbody.transform.rotation); 
        }
        else
        {
            //Network player, receive data
			controllerScript._shipState = (ShipState)(int)stream.ReceiveNext();
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();

        }
    }

   
    void Update()
    {
        if (!photonView.isMine)
        {
            
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
            gameObject.rigidbody.transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
			gameObject.rigidbody.transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);
        }
    }

}