using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.LoveHate;

public class maxRelationshipCheck : MonoBehaviour {
    string[] names = {"Tao Chace" };

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void IsRelationshipMax()
    {
        foreach(string name in names) {
            var affinity = GetComponent<FactionManager>().GetAffinity(name, "Player");
            print(affinity);

            var temperment = GameObject.Find(name).GetComponent<FactionMember>().pad.GetTemperament();
            print(temperment);
        }
    }

    public virtual bool RelationshipTest(GameObject target)
    {
        var affinity = GetComponent<FactionManager>().GetAffinity(target.name, "Player");
        if(affinity >= 100)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public virtual string CheckAffinity(GameObject target)
    {
        var affinity = GetComponent<FactionManager>().GetAffinity(target.name, "Player");
        if(affinity >= 100)
        {
            return "max";
        } else if(affinity >=50 && affinity < 100)
        {
            return "medium";
        } else
        {
            return "low";
        }
    }
}
