using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialStats : MonoBehaviour {

    public enum SocialStatType
    {
        None = 0,
        Introversion = 1,
        Extroversion = 2,
        Sense = 3,
        Intuition = 4,
        Thinking = 5,
        Feeling = 6,
        Judge = 7,
        Percieve = 9,
    }

    private Dictionary<SocialStatType, SocialStat> _socialStatDict;

    public Dictionary<SocialStatType, SocialStat> SocialStatDic
    {
        get
        {
            if (_socialStatDict == null)
            {
                _socialStatDict = new Dictionary<SocialStatType, SocialStat>();
            }
            return _socialStatDict;
        }
    }


    // Use this for initialization
    private void Awake () {
        ConfigureSocialStats();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    ///set values
    public class SocialStat
    {
        private string _statName;
        private int _statBaseValue;
        private int _statMaxValue;
        private SocialStatType _statLink;

        public string StatName
        {
            get { return _statName; }
            set { _statName = value; }
        }

        public virtual int StatValue
        {
            get { return StatBaseValue; }
        }

        public virtual int StatBaseValue
        {
            get { return _statBaseValue; }
            set { _statBaseValue = value; }
        }

        public virtual int StatMaxValue
        {
            get { return _statMaxValue; }
            set { _statMaxValue = value; }
        }

        public virtual SocialStatType Link
        {
            get { return _statLink; }
            set { _statLink = value; }
        }

        public SocialStat()
        {
            this.StatName = string.Empty;
            this.StatBaseValue = 0;
            this.StatMaxValue = 3;
            this.Link = SocialStatType.None;
        }

        public SocialStat(string name, int value, int max, SocialStatType link)
        {
            this.StatName = name;
            this.StatBaseValue = value;
            this.StatMaxValue = max;
            this.Link = link;
        }
    }

    public bool ContainStat(SocialStatType statType)
    {
        return SocialStatDic.ContainsKey(statType);
    }

    public SocialStat GetStat(SocialStatType statType)
    {
        if (ContainStat(statType))
        {
            return SocialStatDic[statType];
        }
        return null;
    }

    public T GetStat<T>(SocialStatType type) where T : SocialStat
    {
        return GetStat(type) as T;
    }

    protected T CreateStat<T>(SocialStatType statType) where T : SocialStat
    {
        T stat = System.Activator.CreateInstance<T>();
        SocialStatDic.Add(statType, stat);
        return stat;
    }

    protected T CreateOrGetStat<T>(SocialStatType statType) where T : SocialStat
    {
        T stat = GetStat<T>(statType);
        if (stat == null)
        {
            stat = CreateStat<T>(statType);
        }
        return stat;
    }

    protected void ConfigureSocialStats()
    {
        ///World
        var introversion = CreateOrGetStat<SocialStat>(SocialStatType.Introversion);
        introversion.StatName = "Introversion";
        introversion.StatBaseValue = 0;
        introversion.Link = SocialStatType.Extroversion;

        var extroversion = CreateOrGetStat<SocialStat>(SocialStatType.Extroversion);
        extroversion.StatName = "Extroversion";
        extroversion.StatBaseValue = 0;
        extroversion.Link = SocialStatType.Introversion;

        ///Information
        var sense = CreateOrGetStat<SocialStat>(SocialStatType.Sense);
        sense.StatName = "Sense";
        sense.StatBaseValue = 0;
        sense.Link = SocialStatType.Intuition;

        var intuition = CreateOrGetStat<SocialStat>(SocialStatType.Intuition);
        intuition.StatName = "Intuition";
        intuition.StatBaseValue = 0;
        intuition.Link = SocialStatType.Sense;

        ///Decisions
        var thinking = CreateOrGetStat<SocialStat>(SocialStatType.Thinking);
        thinking.StatName = "Thinking";
        thinking.StatBaseValue = 0;
        thinking.Link = SocialStatType.Feeling;

        var feeling = CreateOrGetStat<SocialStat>(SocialStatType.Feeling);
        feeling.StatName = "Feeling";
        feeling.StatBaseValue = 0;
        feeling.Link = SocialStatType.Thinking;

        ///Structure
        var judge = CreateOrGetStat<SocialStat>(SocialStatType.Judge);
        judge.StatName = "Judging";
        judge.StatBaseValue = 0;
        judge.Link = SocialStatType.Percieve;

        var percieve = CreateOrGetStat<SocialStat>(SocialStatType.Percieve);
        percieve.StatName = "Percieve";
        percieve.StatBaseValue = 0;
        percieve.Link = SocialStatType.Judge;

    }

    ///modify stats based on input (arrow that each click increments it by one)
    public void SocialStatChange(SocialStat target)
    {
        if(target.StatBaseValue < target.StatMaxValue)
        {
            target.StatBaseValue += 1;
            GetStat(target.Link).StatBaseValue = -(target.StatBaseValue);

            ///Modify combat stats based on current stat
        }
        else
        {
            target.StatBaseValue = target.StatMaxValue;
        }
    }
    


}
