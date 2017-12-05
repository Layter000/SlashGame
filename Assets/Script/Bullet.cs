using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float velocity;


    void Update () {
        transform.Translate(new Vector3(0, 0, 1 * Time.deltaTime * velocity));
    }
}
