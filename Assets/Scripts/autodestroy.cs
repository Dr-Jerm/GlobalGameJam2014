using UnityEngine;
using System.Collections;

public class autodestroy : MonoBehaviour {

	public float delay = 4.0f; 

	// Use this for initialization
	void Start () {
		Destroy (gameObject, delay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
