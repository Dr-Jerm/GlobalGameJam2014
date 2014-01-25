using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidManager : MonoBehaviour {

  // Use this for initialization
  public GameObject asteroidPrefab;

  private Dictionary<string, Asteroid> asteroids;

  void Start () {

    Debug.Log("AsteroidManagerStarted");

    asteroids = new Dictionary<string, Asteroid>();

    for(int i = 0; i < 25; i++) {
      spawnRandomAsteroid();
    }
  }
  
  // Update is called once per frame
  void Update () {
   
  }

  void spawnRandomAsteroid () {
    GameObject newInstance = Instantiate(asteroidPrefab) as GameObject;

    Asteroid a = newInstance.GetComponent(typeof(Asteroid)) as Asteroid;
    int intId = Random.Range(0, 1000);
    string id = intId.ToString();
    a.init(id, new Vector2(Random.Range(-15.0F, 15.0F), Random.Range(-15.0F, 15.0F)), new Vector2(Random.Range(-1.0F, 1.0F), Random.Range(-1.0F, 1.0F)));
    asteroids.Add(id, a);
  }
}
