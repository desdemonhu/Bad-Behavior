using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttacksEnemy : MonoBehaviour {

    private enum EnemyType
    {
        Guard = 0,
        EliteGuard = 1,
    }

    private AllAttacks attackDic;
    private EnemyType enemy;
    public AttackOptions[] Attacks;

    // Use this for initialization
    void Start()
    {
        attackDic = gameObject.AddComponent<AllAttacks>();
        enemy = SetEnemyType(gameObject.name);
        Attacks = new AttackOptions[] {
            AttackOptions.Attack,
            AttackOptions.Defend
        };
    }

    // Update is called once per frame
    void Update () {
		
	}

    private EnemyType SetEnemyType(string name)
    {
        if (name.StartsWith("EliteGuard"))
        {
            return EnemyType.EliteGuard;
        } else
        {
            return EnemyType.Guard;
        }
    }

    public AttackOptions AttackPattern(float affinity)
    {
        if(enemy == EnemyType.Guard)
        {
            if (affinity <= 10)
            {
                return AttackOptions.Attack;
            }
            else
            {
                return AttackOptions.Defend;
            }
        }
       else
        {
            return AttackOptions.Attack;
        }

    }
}
