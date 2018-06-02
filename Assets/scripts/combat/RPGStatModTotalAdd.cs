using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGStatModTotalAdd : RPGStatModifier
{
    public RPGStatModTotalAdd(float value) : base(value){}

    public RPGStatModTotalAdd(float value, bool stacks) : base(value, stacks){}

    public override int Order
    {
        get
        {
            return 4;
        }
    }

    public override int ApplyModifier(int statValue, float modValue)
    {
        return (int)(modValue);
    }
}
