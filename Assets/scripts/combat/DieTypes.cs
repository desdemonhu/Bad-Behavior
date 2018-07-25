using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieTypes {
    public enum DieType {
        D4 = 0,
        D6 = 1,
        D8 = 2,
        D10 = 3,
        D12 = 4,
        D20 = 5
    }

    private static Dictionary<DieType, int> _attackDie;

    public static Dictionary<DieType, int> AttackDie
    {
        get
        {
            if(_attackDie == null)
            {
                _attackDie = new Dictionary<DieType, int> {
                    { DieType.D4, 4 },
                    { DieType.D6, 6 },
                    { DieType.D8, 8 },
                    { DieType.D10, 10 },
                    { DieType.D12, 12 },
                    { DieType.D20, 20 },
                };
            }
            return _attackDie;
        }
    }

    public static int GetAttackDie(DieType dieType)
    {
        return _attackDie[dieType];
    }
}
