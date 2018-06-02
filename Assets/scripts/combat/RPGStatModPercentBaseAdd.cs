using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGStatModPercentBaseAdd : RPGStatModifier
{
    public override int Order
    {
        get
        {
            return 1;
        }
    }

    public override int ApplyModifier(int statValue, float modValue)
    {
        return (int)(statValue * modValue);
    }

    public RPGStatModPercentBaseAdd(float value) : base(value) {}
    public RPGStatModPercentBaseAdd(float value, bool stacks) : base(value, stacks) {}
}
