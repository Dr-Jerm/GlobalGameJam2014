using UnityEngine;
using System.Collections;

public class ShipPhysics : MonoBehaviour
{
	int thrustForce = 200; 
	int turnForce = 100; 
	int maxVelocity = 2;


		// Use this for initialization
		void Start ()
		{
			
			rigidbody.maxAngularVelocity = 1;
			rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX;
		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{
			float inputVert = Input.GetAxis("Vertical")*thrustForce;
			float inputHorz = Input.GetAxis("Horizontal")*turnForce;
			
			if(rigidbody.velocity.magnitude > maxVelocity)
			{
				inputVert = 0;
				rigidbody.velocity = rigidbody.velocity.normalized*2; 
			}

			if (rigidbody.angularVelocity.magnitude > 0) 
			{
		    	rigidbody.AddRelativeTorque ((-rigidbody.angularVelocity.normalized/2)*Time.deltaTime);
			}

			rigidbody.AddRelativeTorque(0,0,inputHorz*Time.deltaTime);
			rigidbody.AddRelativeForce(0,inputVert*Time.deltaTime,0);
		}
}

