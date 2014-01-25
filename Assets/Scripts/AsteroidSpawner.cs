using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

	// Use this for initialization
  public Transform asteroidPrefab;
	void Start () {
	  Debug.Log("hello world");
    //GameObject instance = Instantiate(Resources.Load("Asteroid", typeof(GameObject)));
    //instance.init(Vector2(0,0), Vector2(0,0));
    Instantiate(asteroidPrefab, new Vector3(0, 0, 0), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	 
	}

  void spawnAsteroid (Vector2 pos, Vector2 vel) {
    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    cube.transform.position = new Vector3(pos.x, pos.y, 0);
  }
}
