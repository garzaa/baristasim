using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamerButtonController : MonoBehaviour {

    /*
        on button press, check if the snapped cup exists and has a lid
        if it does, don't do anything
        if it doesn't, and if the cup has milk (a function in CupController)
        then tell CupController to run Steam(): 
            lock the cup in place (disable the DragObject script)
            play the steam animation 
            when it's done:
            edit the drink settings accordingly
            re-enable the script

        need variables for:
        snapped cup
        cupcontroller
    */

    GameObject snappedCup = null;
    CupController cupScript = null;
    bool foaming;
    
    public float steamLength = 1.0f;

    void OnMouseDown()
    {
        //look at sibling objects for a snapped cup
        foreach (Transform child in transform.parent)
        {
            if (child.tag == "cup")
            {
                snappedCup = child.gameObject;
                cupScript = snappedCup.GetComponent<CupController>();
                break;
            }
        }

        //if there's a cup with milk, and it doesn't have a lid, steam it up fam
        if (snappedCup != null && !cupScript.hasLid && cupScript.OnlyMilk() && !foaming)
        {
            cupScript.Steam(steamLength);
            foaming = true;
            StartCoroutine(StopFoaming(steamLength));
        }
    }

    IEnumerator StopFoaming(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        foaming = false;
    }
}
