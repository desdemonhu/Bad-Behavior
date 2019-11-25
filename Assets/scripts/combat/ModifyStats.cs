using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyStats : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}


    // Update is called once per frame
    void Update () {
		
	}

    void Awake()
    {
    }


    public RPGVital GetRPGVitalStat(RPGStatType stat)
    {
        switch (gameObject.name)
        {
            case "Player":
                return gameObject.GetComponent<StatsPlayer>().GetVitalStat(stat);
            case "Chace":
                return gameObject.GetComponent<StatsChace>().GetVitalStat(stat);
            case "Guard":
                return gameObject.GetComponent<StatsGuard>().GetVitalStat(stat);
            default:
                return gameObject.GetComponent<StatsGuard>().GetVitalStat(stat);
        }
    }

    public RPGAttribute GetRPGAttributeStat(RPGStatType stat)
    {
        switch (gameObject.name)
        {
            case "Player":
                return gameObject.GetComponent<StatsPlayer>().GetAttributeStat(stat);
            case "Chace":
                return gameObject.GetComponent<StatsChace>().GetAttributeStat(stat);
            case "Guard":
                return gameObject.GetComponent<StatsGuard>().GetAttributeStat(stat);
            default:
                return gameObject.GetComponent<StatsGuard>().GetAttributeStat(stat);
        }
    }

    ///Get stats from personal stat script
    ///
    public void ModifyTheStat (RPGStatType stat, int change)
    {
        //RPGAttribute level;
        //RPGAttribute dieType;
        //RPGVital stamina;
        //RPGVital health;
        //RPGAttribute attack;
        //RPGAttribute will ;
        //RPGAttribute defense;
        //RPGAttribute dexterity;
        //RPGAttribute alive ;
        //RPGAttribute speed;
        //RPGAttribute evasion;
        //RPGVital willpower;

        RPGVital statToModify;

        switch (gameObject.name)
        {
            case "Player":
                statToModify = gameObject.GetComponent<StatsPlayer>().GetVitalStat(stat);
                statToModify.StatCurrentValue += change;
                gameObject.GetComponent<StatsPlayer>().UpdateModifiedStat(stat, statToModify.StatCurrentValue);
                //level = target.GetComponent<StatsPlayer>().GetVitalStat(RPGStatType.Level);
                //dieType = target.GetComponent<StatsPlayer>().GetVitalStat(RPGStatType.DieType);
                //stamina = target.GetComponent<StatsPlayer>().GetVitalStat(RPGStatType.Stamina);
                //health = target.GetComponent<StatsPlayer>().GetVitalStat(RPGStatType.Health);
                //attack = target.GetComponent<StatsPlayer>().GetVitalStat(RPGStatType.Attack);
                //will = target.GetComponent<StatsPlayer>().GetVitalStat(RPGStatType.Will);
                //defense = target.GetComponent<StatsPlayer>().GetVitalStat(RPGStatType.Defense);
                //dexterity = target.GetComponent<StatsPlayer>().GetVitalStat(RPGStatType.Dexterity);
                //alive = target.GetComponent<StatsPlayer>().GetVitalStat(RPGStatType.Alive);
                //speed = target.GetComponent<StatsPlayer>().GetVitalStat(RPGStatType.Speed);
                //evasion = target.GetComponent<StatsPlayer>().GetVitalStat(RPGStatType.Evasion);
                //willpower = target.GetComponent<StatsPlayer>().GetVitalStat(RPGStatType.Willpower);
                break;
            case "Chace":
                statToModify = gameObject.GetComponent<StatsChace>().GetVitalStat(stat);
                statToModify.StatCurrentValue += change;
                gameObject.GetComponent<StatsChace>().UpdateModifiedStat(stat, statToModify.StatCurrentValue);
                //level = target.GetComponent<StatsChace>().GetVitalStat(RPGStatType.Level);
                //dieType = target.GetComponent<StatsChace>().GetVitalStat(RPGStatType.DieType);
                //stamina = target.GetComponent<StatsChace>().GetVitalStat(RPGStatType.Stamina);
                //health = target.GetComponent<StatsChace>().GetVitalStat(RPGStatType.Health);
                //attack = target.GetComponent<StatsChace>().GetVitalStat(RPGStatType.Attack);
                //will = target.GetComponent<StatsChace>().GetVitalStat(RPGStatType.Will);
                //defense = target.GetComponent<StatsChace>().GetVitalStat(RPGStatType.Defense);
                //dexterity = target.GetComponent<StatsChace>().GetVitalStat(RPGStatType.Dexterity);
                //alive = target.GetComponent<StatsChace>().GetVitalStat(RPGStatType.Alive);
                //speed = target.GetComponent<StatsChace>().GetVitalStat(RPGStatType.Speed);
                //evasion = target.GetComponent<StatsChace>().GetVitalStat(RPGStatType.Evasion);
                //willpower = target.GetComponent<StatsChace>().GetVitalStat(RPGStatType.Willpower);
                break;
            case "Guard":
                statToModify = gameObject.GetComponent<StatsGuard>().GetVitalStat(stat);
                statToModify.StatCurrentValue += change;
                gameObject.GetComponent<StatsGuard>().UpdateModifiedStat(stat, statToModify.StatCurrentValue);
                //level = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Level);
                //dieType = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.DieType);
                //stamina = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Stamina);
                //health = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Health);
                //attack = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Attack);
                //will = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Will);
                //defense = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Defense);
                //dexterity = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Dexterity);
                //alive = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Alive);
                //speed = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Speed);
                //evasion = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Evasion);
                //willpower = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Willpower);
                break;
            default:
                statToModify = gameObject.GetComponent<StatsGuard>().GetVitalStat(stat);
                //level = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Level);
                //dieType = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.DieType);
                //stamina = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Stamina);
                //health = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Health);
                //attack = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Attack);
                //will = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Will);
                //defense = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Defense);
                //dexterity = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Dexterity);
                //alive = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Alive);
                //speed = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Speed);
                //evasion = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Evasion);
                //willpower = target.GetComponent<StatsGuard>().GetVitalStat(RPGStatType.Willpower);
                break;

        }

    }



}
