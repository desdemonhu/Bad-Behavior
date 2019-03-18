using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatStates {
    None = 0,
    StaminaFilling = 1,
    CharacterTurn = 2,
    SelectAction = 3,
    SelectTarget = 4,
    Attacking = 5,
    CheckingStamina = 6,
    Negotiating = 7
}
