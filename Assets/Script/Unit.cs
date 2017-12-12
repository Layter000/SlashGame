using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Unit : MonoBehaviour {

    public float startingHealth = 100;
    public float currentHealth;
    public Slider healthSlider;

    private Animator anim;
    private AudioSource playerAudio;

    private BasicUnit basicUnit;


    private bool isDead = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        currentHealth = startingHealth;

        basicUnit = GetComponent<BasicUnit>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
        else
        {
            basicUnit.Damaged();
            if (basicUnit is PlayerController)
                healthSlider.value -= amount / startingHealth;
        }
    }

    void Death()
    {
        basicUnit.Die();
    }
}
