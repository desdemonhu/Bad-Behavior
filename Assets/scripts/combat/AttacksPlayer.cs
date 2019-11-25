using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class AttacksPlayer : MonoBehaviour {
    private AllAttacks attackDic;
    public AttackOptions[] Attacks;
    public AttackOptions[] Willpowers;
    public Flowchart negotiation;


	// Use this for initialization
	void Start () {
        attackDic = gameObject.AddComponent<AllAttacks>();
        Attacks = new AttackOptions[] {
            AttackOptions.Attack,
            AttackOptions.Defend,
            AttackOptions.Negotiate,
            AttackOptions.Willpower,
        };

        Willpowers = new AttackOptions[]
        {
            AttackOptions.Distract
        };

    }
	
	// Update is called once per frame
	void Update () {

	}


}
