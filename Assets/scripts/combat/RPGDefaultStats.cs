using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGDefaultStats : RPGStatCollection
{
    protected override void ConfigureStats()
    {
        var level = CreateOrGetStat<RPGAttribute>(RPGStatType.Level);
        level.StatName = "Level";
        level.StatBaseValue = 1;

        var dieType = CreateOrGetStat<RPGAttribute>(RPGStatType.DieType);
        level.StatName = "Die Type";
        level.StatBaseValue = 4;

        var stamina = CreateOrGetStat<RPGVital>(RPGStatType.Stamina);
        stamina.StatName = "Stamina";
        stamina.StatBaseValue = 300;
        stamina.SetCurrentValueToMax();

        var health = CreateOrGetStat<RPGVital>(RPGStatType.Health);
        health.StatName = "Health";
        health.StatBaseValue = 100;
        health.SetCurrentValueToMax();

        var attack = CreateOrGetStat<RPGAttribute>(RPGStatType.Attack);
        attack.StatName = "Attack";
        attack.StatBaseValue = 1;

        var will = CreateOrGetStat<RPGAttribute>(RPGStatType.Will);
        will.StatName = "Will";
        will.StatBaseValue = 1;

        var defense = CreateOrGetStat<RPGAttribute>(RPGStatType.Defense);
        defense.StatName = "Defense";
        defense.StatBaseValue = 1;

        var dexterity = CreateOrGetStat<RPGAttribute>(RPGStatType.Dexterity);
        dexterity.StatName = "Dexterity";
        dexterity.StatBaseValue = 1;

    ///Linked Stats
        var speed = CreateOrGetStat<RPGAttribute>(RPGStatType.Speed);
        speed.StatName = "Speed";
        speed.StatBaseValue = 0;
        speed.AddLinker(new RPGStatLinkerBasic(CreateOrGetStat<RPGAttribute>(RPGStatType.Dexterity), 1f));
        speed.UpdateLinkers();

        var evasion = CreateOrGetStat<RPGAttribute>(RPGStatType.Evasion);
        evasion.StatName = "Evasion";
        evasion.StatBaseValue = 0;
        evasion.AddLinker(new RPGStatLinkerBasic(CreateOrGetStat<RPGAttribute>(RPGStatType.Dexterity), 1f));
        evasion.UpdateLinkers();

        var mana = CreateOrGetStat<RPGVital>(RPGStatType.Mana);
        mana.StatName = "Mana";
        mana.StatBaseValue = 0;
        mana.AddLinker(new RPGStatLinkerBasic(CreateOrGetStat<RPGAttribute>(RPGStatType.Will), 10f));
        mana.UpdateLinkers();
        mana.SetCurrentValueToMax();
    }

}
