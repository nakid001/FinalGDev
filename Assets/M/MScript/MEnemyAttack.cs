﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MEnemyAttack : NetworkBehaviour
{
    public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
    public int attackDamage = 10;               // The amount of health taken away per attack.


    //Animator anim;                              // Reference to the animator component.
    GameObject player;                          // Reference to the player GameObject.
    GameObject playerarm;
    MPlayerController playerctl;               // Reference to the player's health.
    MLionController lionctl;
    MBossController bossctrl;
    bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
    float timer;                                // Timer for counting up to the next attack.
    GameObject myobject;


    void Start()
    {
        player = GameObject.Find("MPlayer(Clone)");
        playerarm = GameObject.Find("PlayerArm");
        lionctl = GetComponent<MLionController>();
        bossctrl = GetComponent<MBossController>();
        //anim = GetComponent<Animator>();
    }

 

    void OnTriggerEnter(Collider other)
    {
        // If the entering collider is the player...
        if (other.gameObject.tag =="Player")
        {
            // ... the player is in range.
            Debug.Log("Lion hitting " + other.gameObject);
            myobject = other.gameObject;
            playerInRange = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        // If the exiting collider is the player...
        if (other.gameObject.tag == "Player")
        {
            // ... the player is no longer in range.
            playerInRange = false;
        }
    }


    void Update()
    {
       // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if (timer >= timeBetweenAttacks && playerInRange && lionctl.hp > 0)
        {
            // ... attack.
            Attack();
        }
    }
    
    void Attack()
    {
        // Reset the timer.
        timer = 0f;

        // If the player has health to lose...
        if (!MPlayerController.dying)
        {
            // ... damage the player.
            myobject.GetComponent<MPlayerController>().TakeDamage(attackDamage);
        }
    }
}