using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGStatModBaseAdd : RPGStatModifier
{
    public RPGStatModBaseAdd(float value) : base(value){}

    public RPGStatModBaseAdd(float value, bool stacks) : base(value, stacks){}

    public override int Order
    {
        get
        {
            return 2;
        }
    }

    public override int ApplyModifier(int statValue, float modValue)
    {
        return (int)(modValue);
    }
}
