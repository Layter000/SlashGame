using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour {

    public Transform a;
    public Transform b;
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float angel = Vector3.Angle(a.position - transform.position, b.position - transform.position);
        print(angel);
	}
}
