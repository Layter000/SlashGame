using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerController : MonoBehaviour {

    public Camera cam;
    public float speed;

    public Animation AnimationPlayer;
    private Rigidbody rb;

    public Transform SpawnShotPosition;
    public GameObject shot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 rot = transform.eulerAngles;
            transform.LookAt(hit.point);
            transform.eulerAngles = new Vector3(rot.x, transform.eulerAngles.y, rot.z);
        }

        Atack();
        if (AnimationPlayer.IsPlaying("Hit"))
        {
            ///print("+");
        }
    }

    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        if (Input.GetKey("w")){
            //transform.Translate(new Vector3(0, 0, 1 * Time.deltaTime * speed));
            //rb.AddForce(new Vector3(0, 0, 1 * Time.deltaTime * speed* 100));
            rb.velocity = new Vector3(0, 0, 1 * speed);
        }
        if (Input.GetKey("s"))
        {
            rb.velocity = new Vector3(0, 0, 1 * -speed);
        }
        if (Input.GetKey("a"))
        {
            rb.velocity = new Vector3(1 * -speed, 0, 0);
        }
        if (Input.GetKey("d"))
        {
            rb.velocity = new Vector3(1 * speed, 0, 0);
        }
    }
    void Atack() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            AnimationPlayer.CrossFade ("Hit");
        }
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            Instantiate(shot, SpawnShotPosition.position, SpawnShotPosition.rotation);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hitter")
        {
            Destroy(gameObject);
        }
    }

}
