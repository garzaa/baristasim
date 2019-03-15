using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTo : MonoBehaviour {

    public float threshold = .5f;
    public bool holdingObject = false;
    public bool snapBottom = false;
    public bool exclusive = false;
    public string acceptNameTag;

    //for trash cans, etc
    public bool destroyOnSnap = false;

	// Use this for initialization
	void Start () {
        gameObject.tag = "snap";
	}
	
    public void Accept(GameObject target)
    {
        if (destroyOnSnap) Destroy(target);
    }
}
