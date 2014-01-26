using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidManager : MonoBehaviour {

  // Use this for initialization
  public List<GameObject> asteroidPrefabs;

  private Dictionary<int, Asteroid> asteroids;

  private int AsteroidCount = 25;

  void Start () {

    Debug.Log("AsteroidManagerStarted");

    asteroids = new Dictionary<int, Asteroid>();

	//newAsteroids();
    
  }
  
  // Update is called once per frame
  void Update () {

  }

  public void newAsteroids () {
    for(int i = 0; i < AsteroidCount; i++) {
      spawnRandomAsteroid(i);
    }
  }

  private void spawnRandomAsteroid (int id) {

    Vector3 position = new Vector3(Random.Range(-55.0F,55.0F), Random.Range(-55.0F,55.0F));
	//Vector3 rotation = new Vector3(Random.Range(0.0F, 90.0F), Random.Range(0.0F, 90.0F));
		GameObject newInstance = Instantiate(asteroidPrefabs[(int)Random.Range(1, asteroidPrefabs.Count)]) as GameObject;

    Asteroid a = newInstance.GetComponent(typeof(Asteroid)) as Asteroid;
    //a.init(id, new Vector2(Random.Range(-55.0F, 55.0F), Random.Range(-55.0F, 55.0F)), new Vector2(Random.Range(-2.0F, 2.0F), Random.Range(-2.0F, 2.0F)));
    //asteroids.Add(id, a);
  }
}
