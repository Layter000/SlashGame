using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;

public class PlayerController : MonoBehaviour{

    public float speed;
    public float forceBlink;

    

    public Camera cam;
    public GameObject swordDamageOut;
    public GameObject SwordTrailRenderer;

    private Animator anim;
    private Rigidbody rb;
    private CapsuleCollider cc;

    private AnalogGlitch analogGlitch;

    private bool isCooldownDash = false;

    private bool isAttack1 = false;
    private bool isAttack2 = false;

    private Vector3 movement;

    private Transform target;

    private Vector3 toBlinkPoint;
    private Vector3 moveVector;
    private Vector3 rateVector;

    private int TerrainMask;

    string verticalString;
    string horizontalString;

    private int tempTest;


    public enum State
    {
        READY,
        MOVE,
        DASH,
        ATTACK,
        DAMADED,
        DIE
    }

    public State state;

    void Start()
    {

    }

    private void Awake()
    {
        TerrainMask = LayerMask.GetMask("Terrain");
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        cc = GetComponent<CapsuleCollider>();
        analogGlitch = GameObject.Find("CameraPlayer").GetComponent<AnalogGlitch>();
    }

    void Update()
    {
        if (state == PlayerController.State.READY || state == PlayerController.State.MOVE  || state == PlayerController.State.ATTACK)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isCooldownDash == false)
            {
                Turning();
                StartCoroutine(Dash());
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!isAttack1 && !isAttack2)
                {
                    Turning();
                    StartCoroutine(Attack(1));
                }
                else if (isAttack1 && !isAttack2)
                {
                    StopCoroutine(Attack(1));
                    Turning();
                    StartCoroutine(Attack(2));
                }
            }
            else
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                if ((state == PlayerController.State.READY || state == PlayerController.State.MOVE) )
                {
                    if (h != 0 || v != 0)
                    {
                        Move(h, v);
                    }
                    else
                    {
                        anim.SetBool("IsRunning", false);
                        rb.velocity = new Vector3(0, 0, 0);
                        state = PlayerController.State.READY;
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        
    }

    void Move(float h, float v)
    {

        state = PlayerController.State.MOVE;
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
        Quaternion newRotation = Quaternion.LookRotation(movement - Vector3.zero);
        rb.MoveRotation(newRotation);
        anim.SetBool("IsRunning", true);
    }

    void Turning() {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, 100f, TerrainMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            rb.MoveRotation(newRotation);
            toBlinkPoint = floorHit.point;
        }
    }

    public void Die()
    {
        state = PlayerController.State.DIE;
        anim.SetTrigger("Die");
        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length +1);
    }

    public void Damaded()
    {
        StartCoroutine(DamadedAnim());
    }

    IEnumerator DamadedAnim()
    {
        state = PlayerController.State.DAMADED;
        analogGlitch.colorDrift = 1f;
        anim.SetTrigger("Damaded");
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        analogGlitch.colorDrift = 0f;
        state = PlayerController.State.READY;
    }

    IEnumerator Attack(int nam)
    {
        state = PlayerController.State.ATTACK;

        isAttack1 = true;

        anim.SetTrigger("Attack" + nam.ToString());

        rb.velocity = new Vector3(0, 0, 0);
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        isAttack1 = false;

        state = PlayerController.State.READY;
    }

    IEnumerator Dash()
    {
        isCooldownDash = true;

        state = PlayerController.State.DASH;

        anim.SetTrigger("Dash");

        cc.isTrigger = true;
        analogGlitch.scanLineJitter = 0.5f;
        Vector3 direction = toBlinkPoint - transform.position;
        rb.AddForce(direction.normalized * forceBlink);
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        rb.velocity = new Vector3(0,0,0);
        cc.isTrigger = false;
        analogGlitch.scanLineJitter = 0f;
        state = PlayerController.State.READY;

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        isCooldownDash = false;
    }

}
