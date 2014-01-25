using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
	 Debug.Log("hello world");
   
	}
	
	// Update is called once per frame
	void Update () {
	 
	}

  void spawnAsteroid (Vector2 pos, Vector2 vel) {
    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    cube.transform.position = new Vector3(pos.x, pos.y, 0);

  }
}
