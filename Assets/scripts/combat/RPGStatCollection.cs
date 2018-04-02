using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGStatCollection {
    private Dictionary<RPGStatType, RPGStat> _statDict;

    public RPGStatCollection()
    {
        _statDict = new Dictionary<RPGStatType, RPGStat>();
        ConfigureStats();
    }

    protected virtual void ConfigureStats()
    {

    }

    public bool Contains(RPGStatType statType)
    {
        return _statDict.ContainsKey(statType);
    }

    public RPGStat GetStat(RPGStatType statType)
    {
        if (Contains(statType))
        {
            return _statDict[statType];
        }
        return null;
    }

    protected RPGStat CreateStat(RPGStatType statType)
    {
        RPGStat stat = new RPGStat();
        _statDict.Add(statType, stat);
        return stat;
    }

    protected RPGStat CreateOrGetStat(RPGStatType statType)
    {
        RPGStat stat = GetStat(statType);
        if (stat == null)
        {
            stat = CreateStat(statType);
        }
        return stat;
    }
}
