using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspressoButtonController : MonoBehaviour {

    GameObject dispenserParent;
    GameObject filter;
    FilterController filterScript;
    GameObject cup;
    CupController cupScript;


    string[] sizes = new string[] { "Small", "Med", "Tall" };

	void Start () {
        //first, establish the parent dispenser object
        dispenserParent = transform.parent.gameObject;
        if (gameObject.GetComponent<BoxCollider2D>() == null) gameObject.AddComponent<BoxCollider2D>();
    }

    void OnMouseDown()
    {
        //check to see if it's got the required components:
        //filter
        if ((filter = GetFilter()) == null) return;
        //script
        if ((filterScript = filter.GetComponent<FilterController>()) == null) return;
        //if the filter is locked
        if (!filterScript.locked) return;
        //if there's a cup attached to the bottom
        if ((cup = GetCup()) == null) return;
        //if the cup has a script
        if ((cupScript = cup.GetComponent<CupController>()) == null) return;
        //if the cup has a lid on it
        if (cupScript.hasLid) return;
        //if it's in the middle of a pour animation already
        if (filterScript.pouring) return;
        //make sure it has coffee grounds in it
        if (!filterScript.hasGrounds) return;

        //and now enable the pour animation
        filterScript.cupScript = this.cupScript;
        filterScript.AnimatePour(sizes[cupScript.size]);
        //and change the cup contents accordingly
		if (filterScript.doubleShot) cupScript.drink.AddEspressoShots(2);
		else cupScript.drink.AddEspressoShots(1);

        Sounds.PourSound();

        //remove the espresso grounds from the filter so they have to be added again
        filterScript.RemoveGrounds();
    }

    public GameObject GetFilter()
    {
        foreach (Transform child in dispenserParent.transform)
        {
            if (child.name.Contains("Filter"))
            {
                return child.gameObject;
            }
        }

        return null;
    }

    public GameObject GetCup()
    {
        foreach (Transform child in dispenserParent.transform)
        {
            if (child.name.Contains("Cup"))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    void log(string str)
    {
        Debug.Log(str);
    }
}

