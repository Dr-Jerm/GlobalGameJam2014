using UnityEngine;
using System.Collections;



public class StarThirdPersonController : MonoBehaviour
{

	StarThirdPersonNetwork networkScript;

	public float shipHealth = 100; 
	public float shipHealthMax = 100;

	public bool shipIsDead = false;

	public GameObject collisionSparks;
	public GameObject deathExplosion;
	public GameObject respawnSparks;

	public float inputVert=0;
	public float inputHorz=0;
	public float inputJump=0;
	
	int thrustForce = 200; 
	int turnForce = 50; 
	int maxVelocity = 4;
	
	Light rearthruster_light; 
	Light frontthruster_light; 
	Light leftrearthruster_light; 
	Light leftfrontthruster_light; 
	Light rightrearthruster_light; 
	Light rightfrontthruster_light; 
	Light radar_light;

	ParticleSystem rearthruster_jet; 
	ParticleSystem  frontthruster_jet; 
	ParticleSystem  leftrearthruster_jet; 
	ParticleSystem  leftfrontthruster_jet; 
	ParticleSystem  rightrearthruster_jet; 
	ParticleSystem  rightfrontthruster_jet; 
	
	MeshCollider meshcollider; 
	MeshRenderer meshRender; 
	
	float rearthruster_brightness = 1f;
	float frontthruster_brightness = 1f;
	float turningthruster_brightness = 0.5f;
	float radar_brightness_coeficient = 1f;
	float radarpower = 7; 
	float radarpowerMax = 7; 
	float radarbrightness;

	int spawntimermax = 10;
	float spawntimer = 10;
	
	public bool isControllable = true;
	void Start ()
	{
		rigidbody.maxAngularVelocity = 1;
		rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY; 
	}
	void Awake()
    {
		networkScript = gameObject.GetComponent<StarThirdPersonNetwork>();

		rearthruster_light = gameObject.transform.Find("rearthruster_pointlight").GetComponent<Light>();
		frontthruster_light = gameObject.transform.Find("frontthruster_pointlight").GetComponent<Light>();
		leftrearthruster_light= gameObject.transform.Find("leftrearthruster_pointlight").GetComponent<Light>();
		leftfrontthruster_light= gameObject.transform.Find("leftfrontthruster_pointlight").GetComponent<Light>();
		rightrearthruster_light= gameObject.transform.Find("rightrearthruster_pointlight").GetComponent<Light>();
		rightfrontthruster_light= gameObject.transform.Find("rightfrontthruster_pointlight").GetComponent<Light>();
		radar_light= gameObject.transform.Find("radar_pointlight").GetComponent<Light>();

		rearthruster_jet = gameObject.transform.Find("rearJet").GetComponent<ParticleSystem>();
		frontthruster_jet = gameObject.transform.Find("frontJet").GetComponent<ParticleSystem>();
		leftrearthruster_jet = gameObject.transform.Find("leftrearJet").GetComponent<ParticleSystem>();
		leftfrontthruster_jet = gameObject.transform.Find("leftfrontJet").GetComponent<ParticleSystem>();
		rightrearthruster_jet = gameObject.transform.Find("rightrearJet").GetComponent<ParticleSystem>();
		rightfrontthruster_jet = gameObject.transform.Find("rightfrontJet").GetComponent<ParticleSystem>();
		
		meshcollider = gameObject.transform.Find("spaceship_mesh").transform.Find("space_frigate_6:spaceship").GetComponent<MeshCollider>(); 
		meshRender = gameObject.transform.Find("spaceship_mesh").transform.Find("space_frigate_6:spaceship").GetComponent<MeshRenderer>();
    }

	void FixedUpdate ()
	{
		if (isControllable) 
		{
			if (!shipIsDead) 
			{
				updateLife();	
			} 
			else 
			{
				updateDead();
			}

		}
		updateThrusterLigthts ();
	}


	void OnCollisionEnter(Collision col)
	{
		//Debug.Log("Collision!");
		//if (col.gameObject.tag == "asteroid")
		if(col.gameObject.tag == "asteroid")
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

	void updateThrusterLigthts()
	{
		//Debug.Log ("-" + Input.GetAxis ("Vertical"));
		if(rearthruster_light != null &&
		   frontthruster_light != null &&
		   leftrearthruster_light != null &&
		   leftfrontthruster_light != null &&
		   rightrearthruster_light != null &&
		   rightfrontthruster_light != null &&
		   meshcollider != null &&
		   meshRender != null)
		{
			rearthruster_light.intensity = inputVert * rearthruster_brightness;
			frontthruster_light.intensity = -inputVert * frontthruster_brightness;
			leftrearthruster_light.intensity = -inputHorz * turningthruster_brightness;
			leftfrontthruster_light.intensity = inputHorz * turningthruster_brightness;
			rightrearthruster_light.intensity = inputHorz * turningthruster_brightness;
			rightfrontthruster_light.intensity = -inputHorz * turningthruster_brightness;
			radar_light.intensity = radarbrightness;

			if(inputVert > 0){
				rearthruster_jet.Emit(1);
			}else if(inputVert < 0){
				frontthruster_jet.Emit(1);
			}
			else{
				rearthruster_jet.Stop();
				frontthruster_jet.Stop();
			}
			if(inputHorz > 0)
			{
				leftrearthruster_jet.Emit(1);
				rightfrontthruster_jet.Emit(1);
			}else if(inputHorz < 0)
			{
				leftfrontthruster_jet.Emit(1);
				rightrearthruster_jet.Emit(1);
			}else{
				leftrearthruster_jet.Stop();
				rightfrontthruster_jet.Stop();
				leftfrontthruster_jet.Stop();
				rightrearthruster_jet.Stop();
			}



		


		}
		else
		{
			rearthruster_light = gameObject.transform.Find("rearthruster_pointlight").GetComponent<Light>();
			frontthruster_light = gameObject.transform.Find("frontthruster_pointlight").GetComponent<Light>();
			leftrearthruster_light= gameObject.transform.Find("leftrearthruster_pointlight").GetComponent<Light>();
			leftfrontthruster_light= gameObject.transform.Find("leftfrontthruster_pointlight").GetComponent<Light>();
			rightrearthruster_light= gameObject.transform.Find("rightrearthruster_pointlight").GetComponent<Light>();
			rightfrontthruster_light= gameObject.transform.Find("rightfrontthruster_pointlight").GetComponent<Light>();
			radar_light= gameObject.transform.Find("radar_pointlight").GetComponent<Light>();

			rearthruster_jet = gameObject.transform.Find("rearJet").GetComponent<ParticleSystem>();
			frontthruster_jet = gameObject.transform.Find("frontJet").GetComponent<ParticleSystem>();
			leftrearthruster_jet = gameObject.transform.Find("leftrearJet").GetComponent<ParticleSystem>();
			leftfrontthruster_jet = gameObject.transform.Find("leftfrontJet").GetComponent<ParticleSystem>();
			rightrearthruster_jet = gameObject.transform.Find("rightrearJet").GetComponent<ParticleSystem>();
			rightfrontthruster_jet = gameObject.transform.Find("rightfrontJet").GetComponent<ParticleSystem>();


			meshcollider = gameObject.transform.Find("spaceship_mesh").transform.Find("space_frigate_6:spaceship").GetComponent<MeshCollider>(); 
			meshRender = gameObject.transform.Find("spaceship_mesh").transform.Find("space_frigate_6:spaceship").GetComponent<MeshRenderer>();
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
			deathevent();
			return true; 
		} 
		return false; 
	}
	


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //	Debug.DrawRay(hit.point, hit.normal);
        if (hit.moveDirection.y > 0.01f)
            return;
    }
	void updateLife()
	{
		inputVert = Input.GetAxis ("Vertical");
		inputHorz = Input.GetAxis ("Horizontal");
		inputJump = Input.GetAxis ("Jump");

		if(radarpower > 5)
		{
			radar_brightness_coeficient = 0.75f;
		}
		else
		{
			radar_brightness_coeficient = (radarpower/5.0f)*0.75f;
		}


		if (radarpower > 0) 
		{
			radarbrightness = inputJump*radar_brightness_coeficient;
		}
		else
		{
			radarbrightness = 0; 
		}
		radarpower -= radarbrightness*2;
		radarpower += 0.2f;


		if (radarpower > radarpowerMax) 
		{
			radarpower = radarpowerMax;
		}
		else if(radarpower < 0) 
		{
			radarpower = 0;
		}

		print (radarpower +" : "+ radarbrightness);


		rigidbody.AddRelativeTorque (0, 0, inputHorz * -turnForce * Time.deltaTime);
		rigidbody.AddRelativeForce (0, inputVert * thrustForce * Time.deltaTime, 0);
		
	}

	void updateDead()
	{
		inputVert = 0;
		inputHorz = 0;
		spawntimer -= 2*Time.deltaTime;
		Debug.Log ("updateDead" + spawntimer);
		if(spawntimer <= 0)
		{
			respawn();
		}
	}
	void deathevent()
	{
		print("You Died");
		Instantiate(deathExplosion, gameObject.rigidbody.transform.position, gameObject.rigidbody.transform.rotation);

		rigidbody.AddRelativeTorque (rigidbody.angularVelocity*5);
		rigidbody.AddRelativeForce (rigidbody.velocity*5);

		meshRender.enabled = false;
		meshcollider.enabled = false;

		shipIsDead = true;
		spawntimer = spawntimermax; 
		networkScript.pingPlayerEventForReplication (PlayerEvent.Death);
		
	}
	public void replicatedeathevent()
	{
		print("MyID:"+gameObject.name+" DIED!");
		Instantiate(deathExplosion, gameObject.rigidbody.transform.position, gameObject.rigidbody.transform.rotation);
		meshRender.enabled = false;
		meshcollider.enabled = false;

	}

	void respawn()
	{
		meshRender.enabled = true;
		meshcollider.enabled = true;
		Instantiate(respawnSparks, gameObject.rigidbody.transform.position, gameObject.rigidbody.transform.rotation);

		shipIsDead = false;
		shipHealth = shipHealthMax; 
		networkScript.pingPlayerEventForReplication (PlayerEvent.Respawn);
	}


	public void replicaterespawn()
	{
		meshRender.enabled = true;
		meshcollider.enabled = true;
		print ("replication respawn");
		Instantiate(respawnSparks, gameObject.rigidbody.transform.position, gameObject.rigidbody.transform.rotation);
	}
	
    public void Reset()
    {
        gameObject.tag = "Player";
    }


}