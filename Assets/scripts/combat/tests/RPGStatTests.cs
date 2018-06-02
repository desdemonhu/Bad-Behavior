using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGStatTests : MonoBehaviour {
    private RPGStatCollection stats;
    private RPGVital health;

    // Use this for initialization
    void Start()
    {
        stats = new RPGDefaultStats();
        health = stats.GetStat<RPGVital>(RPGStatType.Health);
        health.OnCurrentValueChange += OnStatValueChange;

        DisplayStatValues();
        Debug.Log(string.Format("_____________________"));
        HealthTest();
        DisplayStatValues();
    }

    private void DisplayStatValues()
    {
        ForEachEnum<RPGStatType>((statType) =>
        {
            RPGStat stat = stats.GetStat((RPGStatType)statType);
            if (stat != null)
            {
                RPGVital vital = stat as RPGVital;
                if(vital != null)
                {
                    Debug.Log(string.Format("Stat {0}'s value is {1}/{2}", stat.StatName, vital.StatCurrentValue, stat.StatValue));
                }
                else
                {
                    Debug.Log(string.Format("Stat {0}'s value is {1}", stat.StatName, stat.StatValue));
                }

            }
        });
    }

    private void ForEachEnum<T>(Action<T> action)
    {
        if(action != null)
        {
            var statTypes = Enum.GetValues(typeof(T));
            foreach(var statType in statTypes)
            {
                action((T)statType);
            }
        }
    }

    void OnStatValueChange(object sender, EventArgs args)
    {
        RPGVital vital = (RPGVital)sender;
        if(vital != null)
        {
            Debug.Log(string.Format("Vital {0} OnStatValueChange event was triggered", vital.StatName));
        }
    }

    private void HealthTest()
    {
        health.AddModifiers(new RPGStatModBaseAdd(50f));
        health.AddModifiers(new RPGStatModPercentBaseAdd(1.0f, false));
        health.AddModifiers(new RPGStatModTotalAdd(15f));
        health.AddModifiers(new RPGStatModPercentTotalAdd(1.0f));
        health.UpdateModifiers();

        var attack = stats.GetStat<RPGAttribute>(RPGStatType.Attack);
        attack.ScaleStat(16);
        var dexterity = stats.GetStat<RPGAttribute>(RPGStatType.Dexterity);
        dexterity.ScaleStat(16);

        health.StatCurrentValue = health.StatValue - 75;
    }

}
