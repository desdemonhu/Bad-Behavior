using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGStatModPercentTotalAdd : RPGStatModifier
{
    public RPGStatModPercentTotalAdd(float value) : base(value){}

    public RPGStatModPercentTotalAdd(float value, bool stacks) : base(value, stacks){}

    public override int Order
    {
        get
        {
            return 3;
        }
    }

    public override int ApplyModifier(int statValue, float modValue)
    {
        return (int)(statValue * modValue);
    }
}
