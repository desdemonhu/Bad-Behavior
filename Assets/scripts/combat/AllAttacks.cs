using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using PixelCrushers.LoveHate;

public class AllAttacks : MonoBehaviour {
    private Dictionary<AttackOptions, Func<GameObject, GameObject, bool>> _allAttacks;
    
    public Dictionary<AttackOptions, Func<GameObject, GameObject, bool>> AttackDic
    {
        get {
            if (_allAttacks == null)
            {
                _allAttacks = new Dictionary<AttackOptions, Func<GameObject, GameObject, bool>>
                {
                    { AttackOptions.Attack, BasicAttack },
                    { AttackOptions.Defend, Defending },
                    { AttackOptions.Negotiate, Negotiate },
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

    public Func<GameObject,GameObject, bool> GetAttack(string attack)
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
            case "Negotiate":
                return AttackOptions.Negotiate;
            case "None":
                return AttackOptions.None;
            default: return AttackOptions.None;
        }
    }

    ///TODO: All of the enemies go here with their specific component name
    public AttackOptions[] GetTargetAttacks(GameObject target)
    {
        switch (target.name)
        {
            case "Player":
                return target.GetComponent<AttacksPlayer>().Attacks;
            case "Chace":
                return target.GetComponent<AttacksChace>().Attacks;
            case "Guard":
                return target.GetComponent<AttacksEnemy>().Attacks;
            default: return null;
        }
    }

    private bool BasicAttack(GameObject player, GameObject target)
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

        var tactic = "Attack";
        if(player.tag == "party") { AdjustRelationship(tactic, target); }
        

        return true;
    }

    private bool Defending(GameObject player, GameObject target)
    {
        Debug.Log(player.name + " is Defending");
        return true;
    }

    private bool Negotiate(GameObject player, GameObject target)
    {
        Flowchart flowchart = gameObject.GetComponent<AttacksPlayer>().negotiation;
        flowchart.SetBooleanVariable("negotiating", true);
        flowchart.ExecuteBlock(target.name);

        StartCoroutine(NegotiationTactic(flowchart, target));
        
        return true;
    }


    private float CalculateRelationship(string tactic, GameObject target)
    {
             ///TODO Add social stats to negotiation calculation
        var factionDatabase = gameObject.GetComponent<FactionMember>().factionManager.factionDatabase;
        var factionID = factionDatabase.GetFactionID(target.name);
        var socialStats = GameObject.Find("Player").GetComponent<SocialStats>();
        var mad = 0;
        var sad = 1;
        var joy = 2;
        float newValue;

        switch (tactic)
        {
            case "Flatter":
                newValue = factionDatabase.GetPersonalityTrait(factionID, joy) + 40 + socialStats.GetStat(SocialStats.SocialStatType.Extroversion).StatValue;
                if(newValue >= 80) { newValue = 100 ; }
                factionDatabase.SetPersonalityTrait(target.name, joy, newValue);
                factionDatabase.SetPersonalityTrait(target.name, sad, 0);
                break;
            case "Attack":
                newValue = factionDatabase.GetPersonalityTrait(factionID, mad) + 40;
                if(newValue >= 80) { newValue = 100; }
                factionDatabase.SetPersonalityTrait(target.name, mad, newValue);
                factionDatabase.SetPersonalityTrait(target.name, sad, 0);
                factionDatabase.SetPersonalityTrait(target.name, joy, 0);
                break;
            default:
                newValue = 0;
                break;

        }

        return factionDatabase.GetPersonalityTrait(factionID, joy) - (factionDatabase.GetPersonalityTrait(factionID, mad) + factionDatabase.GetPersonalityTrait(factionID, sad));
    }

    private void AdjustRelationship(string tactic, GameObject target)
    {
        var scale = CalculateRelationship(tactic, target);
        GameObject.Find("Player").GetComponent<Deeds>().Negotiate(target, scale);
    }

    IEnumerator NegotiationTactic(Flowchart flowchart, GameObject target)
    {
        yield return new WaitUntil(() => flowchart.GetBooleanVariable("negotiating") == false);
        var tactic = flowchart.GetStringVariable("tactic");
        Debug.Log("Tactic: " + tactic);
        AdjustRelationship(tactic, target);
    }

}
