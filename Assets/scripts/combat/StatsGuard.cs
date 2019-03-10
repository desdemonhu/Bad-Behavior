using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsGuard : MonoBehaviour {

    private RPGStatCollection stats;
    private RPGVital health;
    private RPGAttribute attackDie;

    // Use this for initialization
    void Start()
    {
        stats = gameObject.AddComponent<RPGDefaultStats>();
        health = stats.GetStat<RPGVital>(RPGStatType.Health);
        attackDie = stats.GetStat<RPGAttribute>(RPGStatType.DieType);

        ///Modifiy base stats
        health.StatBaseValue = health.StatBaseValue;
        attackDie.StatBaseValue = DieTypes.GetAttackDie(DieTypes.DieType.D6);
        Debug.Log("Guard health is: " + health.StatBaseValue);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
