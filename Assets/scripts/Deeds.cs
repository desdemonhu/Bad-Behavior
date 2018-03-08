using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.LoveHate;

public class Deeds : MonoBehaviour {

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void Disappointed(GameObject target)
    {
        GetComponent<DeedReporter>().ReportDeed("disappointed", target.GetComponent<FactionMember>());

        var temperament = target.GetComponent<FactionMember>().pad.GetTemperament();        print(temperament);
    }

    public virtual void Flatter(GameObject target)
    {
        GetComponent<DeedReporter>().ReportDeed("flatter", target.GetComponent<FactionMember>());
    }
}
