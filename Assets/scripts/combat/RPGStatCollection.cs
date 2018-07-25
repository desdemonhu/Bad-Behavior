using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGStatCollection : MonoBehaviour {
    private Dictionary<RPGStatType, RPGStat> _statDict;

    public Dictionary<RPGStatType, RPGStat> StatDic
    {
        get
        {
            if(_statDict == null)
            {
                _statDict = new Dictionary<RPGStatType, RPGStat>();
            }
            return _statDict;
        }
    }

    private void Awake()
    {
        ConfigureStats();
    }

    protected virtual void ConfigureStats()
    {

    }

    public bool ContainStat(RPGStatType statType)
    {
        return StatDic.ContainsKey(statType);
    }

    public RPGStat GetStat(RPGStatType statType)
    {
        if (ContainStat(statType))
        {
            return StatDic[statType];
        }
        return null;
    }

    public T GetStat<T>(RPGStatType type) where T: RPGStat
    {
        return GetStat(type) as T;
    }

    protected T CreateStat<T>(RPGStatType statType) where T: RPGStat 
    {
        T stat = System.Activator.CreateInstance<T>();
        StatDic.Add(statType, stat);
        return stat;
    }

    protected T CreateOrGetStat<T>(RPGStatType statType) where T : RPGStat
    {
        T stat = GetStat<T>(statType);
        if (stat == null)
        {
            stat = CreateStat<T>(statType);
        }
        return stat;
    }

    public void AddModifier(RPGStatType target, RPGStatModifier mod)
    {
        AddModifier(target, mod, false);
    }

    public void AddModifier(RPGStatType target, RPGStatModifier mod, bool update)
    {
        if (ContainStat(target))
        {
            var modStat = GetStat(target) as IStatModifiable;
            if(modStat != null)
            {
                modStat.AddModifiers(mod);
                if(update == true)
                {
                    modStat.UpdateModifiers();
                }
            } else
            {
                Debug.Log("[RPGStatCollection] Trying to add Stat Modifier to non modifiable stat \"" + target.ToString() + "\"");
            }
        } else
        {
            Debug.Log("[RPGStatCollection] Trying to add Stat Modifier to \"" + target.ToString() + "\" but RPGStatCollection does not contain that stat.");
        }
    }

    public void RemoveStatModifier(RPGStatType target, RPGStatModifier mod)
    {
        RemoveStatModifier(target, mod, false);
    }

    public void RemoveStatModifier(RPGStatType target, RPGStatModifier mod, bool update)
    {
        if (ContainStat(target))
        {
            var modStat = GetStat(target) as IStatModifiable;
            if (modStat != null)
            {
                modStat.RemoveStatModifier(mod);
                if (update == true)
                {
                    modStat.UpdateModifiers();
                }
            }
            else
            {
                Debug.Log("[RPGStatCollection] Trying to remove Stat Modifier from non modifiable stat \"" + target.ToString() + "\"");
            }
        }
        else
        {
            Debug.Log("[RPGStatCollection] Trying to remove Stat Modifier from \"" + target.ToString() + "\" but RPGStatCollection does not contain that stat.");
        }
    }

    public void ClearAllStatModifiers()
    {
        ClearAllStatModifiers(false);
    }

    public void ClearAllStatModifiers(bool update)
    {
        foreach(var key in StatDic.Keys)
        {
            ClearStatModifier(key, update);
        }
    }

    public void ClearStatModifier(RPGStatType target)
    {
        ClearStatModifier(target, false);
    }
    public void ClearStatModifier(RPGStatType target, bool update)
    {
        if (ContainStat(target))
        {
            var modStat = GetStat(target) as IStatModifiable;
            if (modStat != null)
            {
                modStat.ClearModifiers();
                if (update == true)
                {
                    modStat.UpdateModifiers();
                }
            }
            else
            {
                Debug.Log("[RPGStatCollection] Trying to clear all Stat Modifiers from non modifiable stat \"" + target.ToString() + "\"");
            }
        }
        else
        {
            Debug.Log("[RPGStatCollection] Trying to clear all Stat Modifier from \"" + target.ToString() + "\" but RPGStatCollection does not contain that stat.");
        }
    }

    public void UpdateAllStatModifiers()
    {
        foreach(var key in StatDic.Keys)
        {
            UpdateStatModifier(key);
        }
    }

    public void UpdateStatModifier(RPGStatType target)
    {
        if (ContainStat(target))
        {
            var modStat = GetStat(target) as IStatModifiable;
            if (modStat != null)
            {
                modStat.UpdateModifiers();
            }
            else
            {
                Debug.Log("[RPGStatCollection] Trying to Update a Stat Modifier from non modifiable stat \"" + target.ToString() + "\"");
            }
        }
        else
        {
            Debug.Log("[RPGStatCollection] Trying to Update a Stat Modifier from \"" + target.ToString() + "\" but RPGStatCollection does not contain that stat.");
        }
    }

    public void ScaleStat(RPGStatType target, int level)
    {
        if (ContainStat(target))
        {
            var stat = GetStat(target) as IStatScaleable;
            if (stat != null)
            {
                stat.ScaleStat(level);
            }
            else
            {
                Debug.Log("[RPGStats] Trying to Scale Stat with a non scalable stat \"" + target.ToString() + "\"");
            }
        }
        else
        {
            Debug.Log("[RPGStats] Trying to Scale Stat for \"" + target.ToString() + "\", but RPGStatCollection does not contain that stat");
        }
    }
}
