﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MLionController : NetworkBehaviour
{

    UnityEngine.AI.NavMeshAgent nav;

    //float trun = 0;
    [SyncVar] public int hp = 50;
    public static float damageApply = 0;
    bool running = false;
    bool fliping = false;
    float TimetoWalk = -10;
    public float speedup = 0;
    public GameObject meat;
    public float lionspeed = 0.03f;
    int fieldOfViewRange = 45;
    public static bool bosscommand = false;
	public AudioClip hitlion;
	public AudioClip liondeath;
	AudioSource audios;


    // Use this for initialization
    void Start()
    {
		audios = GetComponent<AudioSource>();


        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (running||bosscommand)
        {
            if (CanSeePlayer())
            {
                Debug.Log("Detected!!!");
                running = true;
            }
            else
            {
                running = false;
            }
            run ();
        }
        else
        {
            if (CanSeePlayer())
            {
                Debug.Log("Detected!!!");
                running = true;
            }
            else
            {
                running = false;
            }
            if (TimetoWalk <= 0)
            {
                if (!fliping)
                {
                    Vector3 wayPoint = new Vector3(Random.Range(-100, 100), gameObject.transform.position.y, Random.Range(-100, 100));
                    gameObject.transform.LookAt(wayPoint);
                    fliping = true;
                }


                gameObject.transform.Translate(Vector3.forward * lionspeed * Time.deltaTime*20);
                TimetoWalk -= Time.deltaTime;
                if (TimetoWalk <= -5)
                {
                    fliping = false;
                    TimetoWalk = Random.Range(10, 15);
                }
            }
            else
            {
                TimetoWalk -= Time.deltaTime + speedup;
            }
        }

    }

    public void HpController(int damage)
    {
        Debug.Log("HP CON!");
        running = true;
        fliping = true;
        gameObject.transform.Translate(Vector3.up * Time.deltaTime * 10f);
        hp -= damage;

		if (hp >= 1) 
		{
			audios.PlayOneShot(hitlion, 0.7F);
		}

        if (hp <= 0)
        {
			AudioSource.PlayClipAtPoint(liondeath, transform.position);
            DropItem();
            DropItem();
            DropItem();
            MScoreManager.Score += 10;
			Destroy(gameObject);
        }
    }

    void run()
    {
        Vector3 targetPosition = new Vector3();
        targetPosition = FindClosestEnemy().transform.position;
        nav.SetDestination(targetPosition);
    }
		

    void DropItem()
    {
        Vector3 meatposition = new Vector3(Random.Range(gameObject.transform.position.x + 0.3f, gameObject.transform.position.x - 0.3f), gameObject.transform.position.y, gameObject.transform.position.z);
        GameObject Dmeat = (GameObject)Instantiate(meat, meatposition, Quaternion.identity);
        NetworkServer.Spawn(Dmeat);
    }

    bool CanSeePlayer()
    {
        Vector3 rayDirection=new Vector3() ;
        if (FindClosestEnemy()!=null)
             rayDirection = FindClosestEnemy().transform.position - transform.position;
        RaycastHit hit;
        int layerMask = 1 << 10;
        layerMask = ~layerMask;
        if (Physics.Raycast(transform.position, rayDirection, out hit, 7, layerMask))
        { 
            if (hit.transform.tag == "Player")
            {
                return true;
            }
        }
        if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfViewRange)
        {
            if (Physics.Raycast(transform.position, rayDirection, out hit, 20, layerMask))
            {
                if (hit.transform.tag == "Player")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        if (closest == null)
            return null;
        else if (closest.tag != null)
            return closest;
        else
            return null;

    }

}