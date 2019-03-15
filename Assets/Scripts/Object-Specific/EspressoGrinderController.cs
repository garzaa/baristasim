using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspressoGrinderController : MonoBehaviour {

    //grab the tamper gameobject
    TamperController tamperScript;
    SpriteRenderer grindAnimation;
    GameObject grinderObject;

    public bool grinding = false;

    public float grindLength = .7f;

    void Start()
    {
        if (gameObject.GetComponent<BoxCollider2D>() == null) gameObject.AddComponent<BoxCollider2D>();

        grinderObject = transform.parent.gameObject;
        foreach (Transform child in grinderObject.transform)
        {
            if (child.gameObject.name == "tamper")
            {
                tamperScript = child.gameObject.GetComponent<TamperController>();
            } else if (child.gameObject.name == "animation")
            {
                grindAnimation = child.gameObject.GetComponent<SpriteRenderer>();
            }
        }
    }

    void AnimateGrind()
    {
        this.grinding = true;
        grindAnimation.enabled = true;
        Sounds.BlendSound();
        StartCoroutine(EndAnimation(grindLength));
    }

    IEnumerator EndAnimation(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        grindAnimation.enabled = false;
        this.grinding = false;
        tamperScript.ground = true;
        tamperScript.AddFilterGrounds();
    }

    void OnMouseDown()
    {
        if (!grinding && !tamperScript.ground &&
            tamperScript.hasSnappedFilter)
        {
            AnimateGrind();
        }
    }

}
