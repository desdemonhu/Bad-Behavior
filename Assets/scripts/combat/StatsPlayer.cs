using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsPlayer : MonoBehaviour {
    public RPGStatCollection stats;
    private RPGVital health;
    private RPGAttribute dex;
    private RPGAttribute attackDie;

    // Use this for initialization
    void Start () {
        var dieDic = DieTypes.AttackDie;
        stats = gameObject.AddComponent<RPGDefaultStats>();
        health = stats.GetStat<RPGVital>(RPGStatType.Health);
        dex = stats.GetStat<RPGAttribute>(RPGStatType.Dexterity);
        attackDie = stats.GetStat<RPGAttribute>(RPGStatType.DieType);

        ///Modifiy base stats
        health.StatBaseValue = health.StatBaseValue + 50;
        attackDie.StatBaseValue = DieTypes.GetAttackDie(DieTypes.DieType.D4);
        dex.StatBaseValue = dex.StatBaseValue + 2;
        dex.UpdateLinkers();
        Debug.Log("Player health is: " + health.StatBaseValue);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
