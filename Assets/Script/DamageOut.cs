using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOut : MonoBehaviour {

    public float damageOut;



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != gameObject.tag)
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
