using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate () {
        if(player != null)
        transform.position = new Vector3(player.transform.position.x, 20f, player.transform.position.z - 12f);
	}
}
