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
    public GameObject playerStatus;
    private Transform statusBar;
    private Transform willpowerBar;
    private Transform staminaBar;

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

        ///Set player status
        playerStatus = GameObject.Find("PartyMember-" + gameObject.name);
        statusBar = playerStatus.transform.Find("HealthBars").transform.Find("Bar-Health");
        willpowerBar = playerStatus.transform.Find("WillPower").transform.Find("Bar-Will");
        staminaBar = playerStatus.transform.Find("Stamina").transform.Find("Bar-Stamina");

        ///Modifiy base stats
        health.StatBaseValue = health.StatBaseValue + 10;
        health.SetCurrentValueToMax();
        attackDie.StatBaseValue = DieTypes.GetAttackDie(DieTypes.DieType.D4);
        dex.StatBaseValue = dex.StatBaseValue + 2;
        dex.UpdateLinkers();
        Debug.Log("Player health is: " + health.StatBaseValue);
        Debug.Log("Extroversion: " + socialStats.GetStat(SocialStats.SocialStatType.Extroversion).StatBaseValue);

        ///Tests
        socialStats.SocialStatChange(socialStats.GetStat(SocialStats.SocialStatType.Extroversion));
        Debug.Log("Extroversion after increment: " + socialStats.GetStat(SocialStats.SocialStatType.Extroversion).StatValue);
    }
	
	// Update is called once per frame
	void Update () {
        StatusBars();
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
}
