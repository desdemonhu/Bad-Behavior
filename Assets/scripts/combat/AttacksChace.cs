using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttacksChace : MonoBehaviour {

    private AllAttacks attackDic;
    public AttackOptions[] Attacks;
    public AttackOptions[] Willpowers;


    // Use this for initialization
    void Start()
    {
        attackDic = gameObject.AddComponent<AllAttacks>();
        Attacks = new AttackOptions[] {
            AttackOptions.Attack,
            AttackOptions.Defend,
            AttackOptions.Willpower,
        };
        Willpowers = new AttackOptions[]
        {
            AttackOptions.Distract
        };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
