﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MEnemyLeft : MonoBehaviour {

    public static int CountEnemy;


    Text text;


    void Start()
    {
        text = GetComponent<Text>();
        CountEnemy = 0;
    }


    void Update()
    {
        text.text = "Enemy left " + (GameObject.FindGameObjectsWithTag("PigTag").Length
            + GameObject.FindGameObjectsWithTag("LionTag").Length
            + GameObject.FindGameObjectsWithTag("BossTag").Length);
    }
}
