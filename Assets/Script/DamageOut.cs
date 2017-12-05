using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOut : MonoBehaviour {

    public float damageOut;

    public enum Belonging
    {
        PLAYER,
        ENEMY
    }

    public Belonging belonging;

    void Awake()
    {
        if (gameObject.tag == "PlayerSword")
        {
            belonging = DamageOut.Belonging.PLAYER;
        }
        else
        {
            belonging = DamageOut.Belonging.ENEMY;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && belonging == DamageOut.Belonging.ENEMY)
        {
            if (other is CapsuleCollider)
            {
                other.gameObject.GetComponent<Unit>().TakeDamage(damageOut);
            }
        }
        if (other.tag == "Enemy" && belonging == DamageOut.Belonging.PLAYER)
        {
            if (other is CapsuleCollider)
            {
                other.gameObject.GetComponent<Unit>().TakeDamage(damageOut);
            }
        }
    }

    IEnumerator DisableObjectEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        gameObject.SetActive(false);
    }

}
