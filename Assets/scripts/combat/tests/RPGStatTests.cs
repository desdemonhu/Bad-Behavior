using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGStatTests : MonoBehaviour {
    private RPGStatCollection stats;

    // Use this for initialization
    void Start()
    {
        stats = new RPGDefaultStats();
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
                Debug.Log(string.Format("Stat {0}'s value is {1}",
                    stat.StatName, stat.StatValue));
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

    private void HealthTest()
    {
        var health = stats.GetStat<RPGStatModifiable>(RPGStatType.Health);
        health.AddModifiers(new RPGStatModifier(RPGStatType.Health, RPGStatModifier.Types.BaseValueAdd, 50f));
        health.AddModifiers(new RPGStatModifier(RPGStatType.Health, RPGStatModifier.Types.BaseValuePercent, 1.0f));
        health.UpdateModifiers();
    }

}
