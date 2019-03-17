using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllAttacks : MonoBehaviour {
    private Dictionary<AttackOptions, Action<GameObject, GameObject>> _allAttacks;

    public Dictionary<AttackOptions, Action<GameObject, GameObject>> AttackDic
    {
        get {
            if (_allAttacks == null)
            {
                _allAttacks = new Dictionary<AttackOptions, Action<GameObject, GameObject>>
                {
                    { AttackOptions.Attack, BasicAttack },
                    { AttackOptions.Defend, Defending }
                };
            }
            return _allAttacks;
        }
    }

    private void Awake()
    {
        ConfigureAttacks();
    }

    protected virtual void ConfigureAttacks()
    {

    }

    public bool ContainAttack(AttackOptions attack)
    {
        return AttackDic.ContainsKey(attack);
    }

    public Action<GameObject,GameObject> GetAttack(string attack)
    {
        Debug.Log(attack);
        AttackOptions attackType = GetAttackOption(attack);
        Debug.Log("Attack Type: " + attackType);
        if (ContainAttack(attackType))
        {
            return AttackDic[attackType];
        }
        return null;
    }

    //ToDo make this a dictionary
    private AttackOptions GetAttackOption(string name)
    {
        switch (name)
        {
            case "Attack":
                return AttackOptions.Attack;
            case "Defend":
                return AttackOptions.Defend;
            case "Surrender":
                return AttackOptions.Surrender;
            case "Willpower":
                return AttackOptions.WillPower;
            case "None":
                return AttackOptions.None;
            default: return AttackOptions.None;
        }
    }

    ///All of the enemies go here with their specific component name
    public AttackOptions[] GetTargetAttacks(GameObject target)
    {
        switch (target.name)
        {
            case "Player":
                return target.GetComponent<AttacksPlayer>().Attacks;
            case "Guard":
                return target.GetComponent<AttacksEnemy>().Attacks;
            default: return null;
        }
    }

    private void BasicAttack(GameObject player, GameObject target)
    {
        Debug.Log("Basic Attack against: " + target.name);
        var attack = player.GetComponent<RPGDefaultStats>().GetStat<RPGAttribute>(RPGStatType.Attack).StatValue;
        var dieType = player.GetComponent<RPGDefaultStats>().GetStat<RPGAttribute>(RPGStatType.DieType).StatValue;
        var rng = UnityEngine.Random.Range(attack, (attack + dieType));

        target.GetComponent<RPGDefaultStats>().GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue -= rng;

        if(target.GetComponent<RPGDefaultStats>().GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue < 1)
        {
            Debug.Log(target.name + " is Unconcious");
        } else
        {
            Debug.Log(player.name + " did " + rng + " damage to " + target.name);
        }
    }

    private void Defending(GameObject player, GameObject target)
    {
        Debug.Log(player.name + " is Defending");
    }
}
