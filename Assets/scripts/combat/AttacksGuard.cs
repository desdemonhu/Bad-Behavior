using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttacksGuard : MonoBehaviour {

    private AllAttacks attackDic;
    public AttackOptions[] Attacks;

    // Use this for initialization
    void Start()
    {
        attackDic = gameObject.AddComponent<AllAttacks>();
        Attacks = new AttackOptions[] {
            AttackOptions.Attack,
            AttackOptions.Defend
        };
    }

    // Update is called once per frame
    void Update () {
		
	}
}
