using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGDefaultStats : RPGStatCollection
{
    protected override void ConfigureStats()
    {
        RPGStat health = CreateOrGetStat(RPGStatType.Health);
        health.StatName = "Health";
        health.StatValue = 100;

        RPGStat mana = CreateOrGetStat(RPGStatType.Mana);
        mana.StatName = "Mana";
        mana.StatValue = 100;

        RPGStat attack = CreateOrGetStat(RPGStatType.Attack);
        attack.StatName = "Attack";
        attack.StatValue = 1;

        RPGStat will = CreateOrGetStat(RPGStatType.Will);
        will.StatName = "Will";
        will.StatValue = 1;

        RPGStat defense = CreateOrGetStat(RPGStatType.Defense);
        defense.StatName = "Defense";
        defense.StatValue = 1;

        RPGStat dexterity = CreateOrGetStat(RPGStatType.Dexterity);
        dexterity.StatName = "Dexterity";
        dexterity.StatValue = 1;

        RPGStat speed = CreateOrGetStat(RPGStatType.Speed);
        speed.StatName = "Speed";
        speed.StatValue = dexterity.StatValue;

        RPGStat evasion = CreateOrGetStat(RPGStatType.Evasion);
        evasion.StatName = "Evasion";
        evasion.StatValue = dexterity.StatValue * 10;
    }

}
