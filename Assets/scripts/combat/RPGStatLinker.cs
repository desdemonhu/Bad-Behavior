using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RPGStatLinker {
    private RPGStat _stat;

    public RPGStatLinker(RPGStat stat)
    {
        _stat = stat;
    }

    public RPGStat Stat
    {
        get { return _stat; }
    }

    public abstract int Value { get; }
}
