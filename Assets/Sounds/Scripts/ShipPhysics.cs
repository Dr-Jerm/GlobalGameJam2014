using UnityEngine;
using System.Collections;

public class ShipPhysics : MonoBehaviour
{
	public float shipHealth = 100; 
	public float shipHealthMax = 100; 
	public bool shipIsDead = false; 

	public GameObject collisionSparks;

	int thrustForce = 200; 
	int turnForce = 50; 
	int maxVelocity = 4;

	Light rearthruster_light; 
	Light frontthruster_light; 
	Light leftrearthruster_light; 
	Light leftfrontthruster_light; 
	Light rightrearthruster_light; 
	Light rightfrontthruster_light; 

	float rearthruster_brightness = 2.0f;
	float frontthruster_brightness = 1.5f;
	float turningthruster_brightness = .5f;


		// Use this for initialization
		void Start ()
		{
			rigidbody.maxAngularVelocity = 1;
			rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY; 
		}
		
		void Awake ()
		{
			//rearthruster_light = GameObject.Find("rearthruster_pointlight").GetComponent<Light>();
			rearthruster_light = GameObject.Find("rearthruster_pointlight").GetComponent<Light>();
			frontthruster_light = GameObject.Find("frontthruster_pointlight").GetComponent<Light>();
			leftrearthruster_light= GameObject.Find("leftrearthruster_pointlight").GetComponent<Light>();
			leftfrontthruster_light= GameObject.Find("leftfrontthruster_pointlight").GetComponent<Light>();
			rightrearthruster_light= GameObject.Find("rightrearthruster_pointlight").GetComponent<Light>();
			rightfrontthruster_light= GameObject.Find("rightfrontthruster_pointlight").GetComponent<Light>();
		}
		// Update is called once per frame
		void FixedUpdate ()
		{
			
			float inputVert = Input.GetAxis("Vertical")*thrustForce;
			float inputHorz = Input.GetAxis("Horizontal")*turnForce;

			//Debug.Log ("-" + Input.GetAxis ("Vertical"));
			rearthruster_light.intensity = Input.GetAxis ("Vertical")*rearthruster_brightness;
			frontthruster_light.intensity = -Input.GetAxis ("Vertical")*frontthruster_brightness;
			leftrearthruster_light.intensity = (Input.GetAxis ("Horizontal"))*turningthruster_brightness;
			leftfrontthruster_light.intensity = -(Input.GetAxis ("Horizontal"))*turningthruster_brightness;
			rightrearthruster_light.intensity = -(Input.GetAxis ("Horizontal"))*turningthruster_brightness;
			rightfrontthruster_light.intensity = (Input.GetAxis ("Horizontal"))*turningthruster_brightness;

			
			if(rigidbody.velocity.magnitude > maxVelocity)
			{
				inputVert = 0;
				rigidbody.velocity = rigidbody.velocity.normalized*maxVelocity; 
			}

			if (rigidbody.angularVelocity.magnitude > 0) 
			{
		    	rigidbody.AddRelativeTorque ((-rigidbody.angularVelocity.normalized/2)*Time.deltaTime);
			}

			rigidbody.AddRelativeTorque(0,0,inputHorz*Time.deltaTime);
			rigidbody.AddRelativeForce(0,inputVert*Time.deltaTime,0);
		}

		void OnCollisionEnter(Collision col)
		{
			//if (col.gameObject.tag == "asteroid")
			if(col.gameObject.name == "Sphere")
			{
				ContactPoint contactpoint = col.contacts[0];
				Vector3 impactvel = col.gameObject.rigidbody.GetRelativePointVelocity(contactpoint.point);
				
				// object collided with something called "Ground":
				// do whatever you want: set a new direction, set a boolean variable, etc.
				Debug.Log("Collision Asteroid!!!"+impactvel.magnitude);
				takedamage(impactvel.magnitude*-2);
				Instantiate(collisionSparks, contactpoint.point, Quaternion.LookRotation(contactpoint.normal));

			}
		}

		//returns true if damage causes death. 
		bool takedamage(float _damage)
		{
			Debug.Log("Damage:"+_damage+" Health:"+shipHealth);
			shipHealth += _damage;
			if (shipHealth > shipHealthMax) 
			{
				shipHealth = shipHealthMax;
			} 
			else if (shipHealth < 0) 
			{
				shipHealth = 0;
				shipIsDead = true;
				Debug.Log("You Died");
				return true; 
			} 
			return false; 
		}
		
		void deathevent(){
			
		}
}

