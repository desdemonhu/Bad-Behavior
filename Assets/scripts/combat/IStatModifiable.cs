using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatModifiable {
    int StatModifierValue { get; }

    void AddModifiers(RPGStatModifier mod);
    void RemoveStatModifier(RPGStatModifier mod);
    void ClearModifiers();
    void UpdateModifiers();
	
}
