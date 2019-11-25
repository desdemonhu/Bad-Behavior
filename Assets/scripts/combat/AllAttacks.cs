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
                    { AttackOptions.Willpower, Willpower },
                    { AttackOptions.Distract, Distract }
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
                return AttackOptions.Willpower;
            case "Negotiate":
                return AttackOptions.Negotiate;
            case "Distract":
                return AttackOptions.Distract;
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

    public AttackOptions[] GetTargetWillpowers(GameObject target)
    {
        switch (target.name)
        {
            case "Player":
                return target.GetComponent<AttacksPlayer>().Willpowers;
            case "Chace":
                return target.GetComponent<AttacksChace>().Willpowers;
            default: return null;
        }
    }

    private bool BasicAttack(GameObject player, GameObject target)
    {
        Debug.Log("Basic Attack against: " + target.name);

        var attack = player.GetComponent<ModifyStats>().GetRPGVitalStat(RPGStatType.Attack).Statvalue;
        var dieType = player.GetComponent<ModifyStats>().GetRPGAttributeStat(RPGStatType.DieType).StatValue;
        var rng = UnityEngine.Random.Range(attack, (attack + dieType)) + 1;
        var newHealth = target.GetComponent<ModifyStats>().GetRPGVitalStat(RPGStatType.Health).StatCurrentValue - rng;

        target.GetComponent<ModifyStats>().ModifyTheStat(RPGStatType.Health, -rng);

        if (target.GetComponent<ModifyStats>().GetRPGVitalStat(RPGStatType.Health).StatCurrentValue < 1)
        {
            Debug.Log(target.name + " is Unconcious");
        } else
        {
            Debug.Log(player.name + " did " + rng + " damage to " + target.name);
            Debug.Log(target.name + " is at " + target.GetComponent<ModifyStats>().GetRPGVitalStat(RPGStatType.Health).StatCurrentValue + " health");
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

    private bool Willpower(GameObject player, GameObject target)
    {
        Debug.Log("Willpower selected");
        ///Activate AttackPanelSub
        var willpowermenu = GameObject.Find("AttackPanelSub");

        bool wait = true;
        
        while (wait)
        {
            ///Wait till option is chosen
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
                if (hit.collider)
                {
                    var selection = hit.collider.gameObject;
                    var method = player.GetComponent<AllAttacks>().GetAttack(selection.name);
                    Destroy(willpowermenu.gameObject);
                    var done = false;

                    while (!done)
                    {
                        done = method(player, target);
                        if (done)
                        {
                            wait = false;
                        }

                    }
                }
            }
        }
        return true;
    }


    ///Relationship stuff
    private float CalculateRelationship(string tactic, GameObject target)
    {
             ///TODO Add social stats to negotiation calculation
        var factionDatabase = gameObject.GetComponent<FactionMember>().factionManager.factionDatabase;
        var factionID = factionDatabase.GetFactionID(target.name);
        var socialStats = GameObject.Find("Player").GetComponent<SocialStats>();
        int neutral = 1;
        int angry = 2;
        int afraid = 3;
        int charmed = 4;
        int confused = 5;
        float newValue;

        switch (tactic)
        {
            case "Flatter":
                newValue = factionDatabase.GetRelationshipTrait(factionID, 0, charmed) + 1;
                if(newValue < 6) factionDatabase.SetPersonalRelationshipTrait(factionID, 0, charmed, newValue);
                else factionDatabase.SetPersonalRelationshipTrait(factionID, 0, charmed, 5);
                break;
            case "Attack":
                newValue = factionDatabase.GetRelationshipTrait(factionID, 0, angry) + 1;
                if (newValue < 6) factionDatabase.SetPersonalRelationshipTrait(factionID, 0, angry, newValue);
                else factionDatabase.SetPersonalRelationshipTrait(factionID, 0, angry, 5);
                break;
            default:
                newValue = 0;
                break;

        }

        return newValue;
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

    private bool Distract(GameObject player, GameObject target)
    {
        int statMod = -200;
        target.GetComponent<ModifyStats>().ModifyTheStat(RPGStatType.Stamina, statMod);
        Debug.Log("distraction!");
        return true;
    }

}
