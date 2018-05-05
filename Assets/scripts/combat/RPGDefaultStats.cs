using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGDefaultStats : RPGStatCollection
{
    protected override void ConfigureStats()
    {
        var health = CreateOrGetStat<RPGStatModifiable>(RPGStatType.Health);
        health.StatName = "Health";
        health.StatBaseValue = 100;

        var attack = CreateOrGetStat<RPGStatModifiable>(RPGStatType.Attack);
        attack.StatName = "Attack";
        attack.StatBaseValue = 1;

        var will = CreateOrGetStat<RPGStatModifiable>(RPGStatType.Will);
        will.StatName = "Will";
        will.StatBaseValue = 1;

        var defense = CreateOrGetStat<RPGStatModifiable>(RPGStatType.Defense);
        defense.StatName = "Defense";
        defense.StatBaseValue = 1;

        var dexterity = CreateOrGetStat<RPGStatModifiable>(RPGStatType.Dexterity);
        dexterity.StatName = "Dexterity";
        dexterity.StatBaseValue = 1;

        var speed = CreateOrGetStat<RPGStatModifiable>(RPGStatType.Speed);
        speed.StatName = "Speed";
        speed.StatBaseValue = dexterity.StatValue;

        var evasion = CreateOrGetStat<RPGStatModifiable>(RPGStatType.Evasion);
        evasion.StatName = "Evasion";
        evasion.StatBaseValue = dexterity.StatValue * 10;

        var mana = CreateOrGetStat<RPGStatModifiable>(RPGStatType.Mana);
        mana.StatName = "Mana";
        mana.StatBaseValue = will.StatValue * 10;
    }

}
