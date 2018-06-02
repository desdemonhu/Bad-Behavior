using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class RPGStatModifier {

    private float _value;
    public event EventHandler OnValueChange;
    public abstract int Order { get; }
    public bool Stacks { get; set; }
    public float Value
    {
        get { return _value; }
        set {
            if(_value != value)
            {
                _value = value;
                if(OnValueChange != null)
                {
                    OnValueChange(this, null);
                }
            }

        }
    }

    public RPGStatModifier(float value)
    {
        _value = value;
        Stacks = false;
    }

    public RPGStatModifier(float value, bool stacks)
    {
        _value = value;
        Stacks = stacks;
    }

    public abstract int ApplyModifier(int statValue, float modValue);
}
