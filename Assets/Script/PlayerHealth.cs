using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public float startingHealth = 100;
    public float currentHealth;
    public Slider healthSlider;

    private Animator anim;
    private AudioSource playerAudio;
    private PlayerController playerController;

    private bool isDead = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerController = GetComponentInChildren<PlayerController>();

        currentHealth = startingHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        healthSlider.value -= Mathf.LerpUnclamped(0f, startingHealth, amount / 10000f);

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
