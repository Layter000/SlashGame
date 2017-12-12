using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUnit : MonoBehaviour {

    protected Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public virtual void Damaged()
    {
        
    }

    public virtual void Die()
    {
        anim.SetTrigger("Die");
        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length + 1);
    }
}
