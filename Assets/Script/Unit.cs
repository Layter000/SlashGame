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

    private PlayerController playerController;
    private basicAI basicAi;


    private bool isDead = false;

    public enum Belonging
    {
        PLAYER,
        ENEMY
    }

    public Belonging belonging;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        currentHealth = startingHealth;

        if (gameObject.tag == "Player")
        {
            belonging = Unit.Belonging.PLAYER;
            playerController = GetComponent<PlayerController>();
        }
        else
        {
            basicAi = GetComponent<basicAI>();
            belonging = Unit.Belonging.ENEMY;
        }
        
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
        else if (belonging == Unit.Belonging.PLAYER)
        {
            playerController.Damaded();
            healthSlider.value -= amount / startingHealth;
        }
        else if (belonging == Unit.Belonging.ENEMY)
        {

            basicAi.Damaded();
        }
    }

    void Death()
    {
        if (belonging == Unit.Belonging.PLAYER)
        {
            playerController.Die();
            
        }
        else
        {
            basicAi.Die();
        }
    }
}
