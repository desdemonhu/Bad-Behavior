using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGAttribute : RPGStatModifiable, IStatScaleable, IStatLinkable
{
    private int _statLevelValue;
    private int _statLinkerValue;
    private List<RPGStatLinker> _statLinkers;

    public int StatLevelValue
    {
        get { return _statLevelValue; }
    }

    public int StatLinkerValue
    {
        get
        {
            UpdateLinkers();
            return _statLinkerValue;
        }
    }

    public override int StatBaseValue
    {
        get
        {
            return base.StatBaseValue + StatLevelValue + StatLinkerValue; ///This calls UpdateLinkers infinitly. Why?
        }
    }

    public virtual void ScaleStat(int level)
    {
        ///Change this to scale what happens at each level for bonus to stats
        _statLevelValue = level;

    }

    public void AddLinker(RPGStatLinker linker)
    {
        _statLinkers.Add(linker);
    }

    public void ClearLinkers()
    {
        _statLinkers.Clear();
    }

    public void UpdateLinkers()
    {
        _statLinkerValue = 0;
        foreach (RPGStatLinker link in _statLinkers)
        {
             _statLinkerValue = link.Value;
        }
    }

    

    public RPGAttribute()
    {
        _statLinkers = new List<RPGStatLinker>();
    }
}
