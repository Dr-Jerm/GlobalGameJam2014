using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

  private string id;
  private GameObject asteroid;
  private Vector2 pos;
  private Vector2 vel;
  private Rigidbody asteroidRigidBody;

  // Use this for initialization
  void Start () {
    Debug.Log("AsteroidSpawned");
    this.asteroid = GameObject.CreatePrimitive(PrimitiveType.Sphere);
	this.asteroid.transform.localScale = new Vector3(3f,3f,3f);
    this.asteroidRigidBody = asteroid.AddComponent<Rigidbody>(); // Add the rigidbody.
    this.asteroidRigidBody.mass = 5; // Set the asteroid's mass to 5 via the Rigidbody.
    this.asteroidRigidBody.useGravity = false;
    this.asteroidRigidBody.constraints = RigidbodyConstraints.FreezePositionZ;

    this.asteroidRigidBody.position = new Vector3(this.pos.x, this.pos.y);
    this.asteroidRigidBody.velocity = new Vector3(this.vel.x, this.vel.y);
  }
  
  // Update is called once per frame
  void Update () {

  }

  public void init (string initId, Vector2 initPos, Vector2 initVel) {
    this.id = initId;
    this.pos = initPos;
    this.vel = initVel;
  }
}
