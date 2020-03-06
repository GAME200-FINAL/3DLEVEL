using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambience : MonoBehaviour {

    public string AmbienceName;

	// Use this for initialization
	void Start () {
		if(AmbienceName != "")
        {
            AudioManager.PlaySound(AmbienceName, gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
