using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionOnClick : MonoBehaviour {

    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DetectAction()
    {
        CurrentState actionScript = GameObject.Find("Player").GetComponent<CurrentState>();

        actionScript.Action = gameObject.name.ToString();
        Debug.Log("Action: " + actionScript.Action);
    }
}
