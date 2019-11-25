using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsPlayer : MonoBehaviour {
    public RPGStatCollection stats;
    public SocialStats socialStats;
    private RPGVital health;
    private RPGVital willpower;
    private RPGVital stamina;
    private RPGAttribute dex;
    private RPGAttribute attackDie;
    private RPGAttribute level;
    private RPGAttribute attack;
    private RPGAttribute will;
    private RPGAttribute defense;
    private RPGAttribute alive;
    private RPGAttribute speed;
    private RPGAttribute evasion;
    private Transform statusBar;
    private Transform willpowerBar;
    private Transform staminaBar;
    public  GameObject playerStatus;




    // Use this for initialization
    void Start () {
        var dieDic = DieTypes.AttackDie;
        stats = gameObject.AddComponent<RPGDefaultStats>();
        socialStats = gameObject.GetComponent<SocialStats>();
        health = stats.GetStat<RPGVital>(RPGStatType.Health);
        willpower = stats.GetStat<RPGVital>(RPGStatType.Willpower);
        stamina = stats.GetStat<RPGVital>(RPGStatType.Stamina);
        dex = stats.GetStat<RPGAttribute>(RPGStatType.Dexterity);
        attackDie = stats.GetStat<RPGAttribute>(RPGStatType.DieType);
        level = stats.GetStat<RPGAttribute>(RPGStatType.Level);
        attack = stats.GetStat<RPGAttribute>(RPGStatType.Attack);
        will = stats.GetStat<RPGAttribute>(RPGStatType.Will);
        defense = stats.GetStat<RPGAttribute>(RPGStatType.Defense);
        alive = stats.GetStat<RPGAttribute>(RPGStatType.Alive);
        speed = stats.GetStat<RPGAttribute>(RPGStatType.Speed);
        evasion = stats.GetStat<RPGAttribute>(RPGStatType.Evasion);

        if (gameObject.GetComponent<CurrentState>().inCombat)
        {
            SetPlayerStatusBars();
        }


        ///Modifiy base stats
        ///
        health.StatBaseValue += 10;
        health.SetCurrentValueToMax();
        attackDie.StatBaseValue = DieTypes.GetAttackDie(DieTypes.DieType.D4);
        dex.StatBaseValue = dex.StatBaseValue + 1;
        dex.UpdateLinkers();
        Debug.Log("Player health is: " + health.StatBaseValue);
        Debug.Log("Extroversion: " + socialStats.GetStat(SocialStats.SocialStatType.Extroversion).StatBaseValue);

        ///Tests
        socialStats.SocialStatChange(socialStats.GetStat(SocialStats.SocialStatType.Extroversion));
        Debug.Log("Extroversion after increment: " + socialStats.GetStat(SocialStats.SocialStatType.Extroversion).StatValue);
    }
	
	// Update is called once per frame
	void Update () {

        if (gameObject.GetComponent<CurrentState>().inCombat)
        {
            StatusBars();
        }
        
	}

    public virtual void SetPlayerStatusBars()
    {
        playerStatus = GameObject.Find("PartyMember-" + gameObject.name);
        statusBar = playerStatus.transform.Find("HealthBars").transform.Find("Bar-Health");
        willpowerBar = playerStatus.transform.Find("WillPower").transform.Find("Bar-Will");
        staminaBar = playerStatus.transform.Find("Stamina").transform.Find("Bar-Stamina");
    }


    private void StatusBars()
    {
        HealthBar();
        WillPowerBar();
        StaminaBar();
    }


    private void StaminaBar()
    {
        
        float calcValue = (float)stamina.StatCurrentValue / (float)stamina.StatBaseValue;
        Vector3 barVector = staminaBar.localScale;
        barVector.x = calcValue;
        staminaBar.transform.localScale = barVector;
    }

    private void WillPowerBar()
    {

        float calcValue = (float)willpower.StatCurrentValue / (float)willpower.StatBaseValue;
        Vector3 barVector = willpowerBar.localScale;
        barVector.x = calcValue;
        willpowerBar.transform.localScale = barVector;
    }

    private void HealthBar()
    {
        float calcValue = (float)health.StatCurrentValue / (float)health.StatBaseValue;
        Vector3 barVector = statusBar.localScale;
        barVector.x = calcValue;
        statusBar.transform.localScale = barVector;
    }

    public virtual void UpdateModifiedStat(RPGStatType stat, int newAmount)
    {
        switch (stat)
        {
            case RPGStatType.Stamina:
                stamina.StatCurrentValue = newAmount;
                break;
            case RPGStatType.Alive:
                alive.StatBaseValue = newAmount;
                break;
            case RPGStatType.Health:
                health.StatCurrentValue = newAmount;
                break;
            case RPGStatType.Willpower:
                willpower.StatCurrentValue = newAmount;
                break;
            default:
                break;
        }
    }

    public virtual RPGVital GetVitalStat(RPGStatType stat)
    {
        switch (stat)
        {
            case RPGStatType.Stamina:
                return stamina;
            case RPGStatType.Health:
                return health;
            case RPGStatType.Willpower:
                return willpower;
            default:
                return stamina; ///TODO what should be the default for this?
        }
    }

    public virtual RPGAttribute GetAttributeStat(RPGStatType stat)
    {
        switch (stat)
        {
            case RPGStatType.Alive:
                return alive;
            case RPGStatType.Speed:
                return speed;
            default:
                return speed; ///TODO what should be the default for this?
        }
    }
}
