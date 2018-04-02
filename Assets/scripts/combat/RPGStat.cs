using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGStat {

    private string _statName;
    private int _statValue;

    public string StatName
    {
        get { return _statName; }
        set { _statName = value; }
    }

    public int StatValue
    {
        get { return _statValue; }
        set { _statValue = value; }
    }

    public RPGStat()
    {
        this.StatName = string.Empty;
        this.StatValue = 0;
    }

    public RPGStat(string name, int value)
    {
        this.StatName = name;
        this.StatValue = value;
    }
}
