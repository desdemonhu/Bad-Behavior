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

    public AttackOptions AttackPattern(Dictionary<Emotions, float> affinity)
    {
        List<Emotions> primaryEmotions = new List<Emotions>();
        float neutral = affinity[Emotions.Neutral];
        float angry = affinity[Emotions.Angry];
        float afraid = affinity[Emotions.Afraid];
        float charmed = affinity[Emotions.Charmed];
        float confused = affinity[Emotions.Confused];

        if (enemy == EnemyType.Guard)
        {
            ///set what counts as affinity
            ///
            if (angry > 0) primaryEmotions.Add(Emotions.Angry);
            if (afraid > 2) primaryEmotions.Add(Emotions.Afraid);
            if (charmed > 2) primaryEmotions.Add(Emotions.Charmed);
            if (confused > 2) primaryEmotions.Add(Emotions.Confused);
            if (neutral > -1) primaryEmotions.Add(Emotions.Neutral);

            ///Find highest and second highest

            /// return corosponding attack
            /// 
            if (primaryEmotions.Contains(Emotions.Angry)) return AttackOptions.Attack;
            else if (primaryEmotions.Contains(Emotions.Afraid)) return AttackOptions.Defend;
            else return AttackOptions.Attack;
        }

        return AttackOptions.None;

    }
}
