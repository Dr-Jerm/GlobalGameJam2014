using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

  private int id;
  private GameObject asteroid;
  private Vector2 pos;
  private Vector2 vel;
  private Rigidbody asteroidRigidBody;

  // Use this for initialization
  void Start () {
    Debug.Log("AsteroidSpawned");
    //this.asteroid = GameObject.CreatePrimitive(PrimitiveType.Sphere);
	  //this.asteroid.transform.localScale = new Vector3(3f,3f,3f);
//    this.asteroidRigidBody = asteroid.AddComponent<Rigidbody>(); // Add the rigidbody.
    //this.asteroidRigidBody.mass = 5; // Set the asteroid's mass to 5 via the Rigidbody.
    //this.asteroidRigidBody.useGravity = false;
    //this.asteroidRigidBody = gameObject.GetComponent<Rigidbody>();

    //this.asteroidRigidBody.position = new Vector3(Random.Range(-55.0F,55.0F), Random.Range(-55.0F,55.0F));
		gameObject.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-2.0F,2.0F), Random.Range(-2.0F,2.0F));
		gameObject.transform.Rotate(new Vector3(Random.Range(0.0F,90.0F), Random.Range(0.0F,90.0F)));
  }
  
  // Update is called once per frame
  void Update () {
    
  }
}
