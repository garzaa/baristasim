using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundOnDrag : MonoBehaviour {

    DragObject dragScript;
    bool startedFollowing;
    int originalPos;
    int newOrder;
    public bool fgEnabled = true;
    
    void Start()
    {
        //add a copy of the script to each sprite child to recursively modify sorting order
        foreach (Transform child in transform)
        {
            if ((child.GetComponent<ForegroundOnDrag>()) == null &&
                child.GetComponent<SpriteRenderer>() != null &&
                !child.name.Contains("snapper"))
            {
                child.gameObject.AddComponent<ForegroundOnDrag>();
            }
        }
        originalPos = GetComponent<SpriteRenderer>().sortingOrder;

        if ((dragScript = gameObject.GetComponent<DragObject>()) != null &&
            dragScript.followingMouse)
        {
            startedFollowing = true;
            IncreaseOrder();
        }
    }

    void OnMouseDrag()
    {
        if (fgEnabled && !startedFollowing) IncreaseOrder();
    }

    void OnMouseUp()
    {
        if (fgEnabled) DecreaseOrder();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && startedFollowing && fgEnabled)
        {
            DecreaseOrder();
            startedFollowing = false;
        }
    }

    public void IncreaseOrder()
    {
        this.GetComponent<SpriteRenderer>().sortingOrder = originalPos + 1000;
        foreach (Transform child in transform)
        {
            if ((child.GetComponent<ForegroundOnDrag>()) != null)
            {
                child.GetComponent<ForegroundOnDrag>().IncreaseOrder();
            }
        }
    }

    public void DecreaseOrder()
    {
        this.GetComponent<SpriteRenderer>().sortingOrder = originalPos;
        foreach (Transform child in transform)
        {
            if ((child.GetComponent<ForegroundOnDrag>()) != null)
            {
                child.GetComponent<ForegroundOnDrag>().DecreaseOrder();
            }
        }
    }
}
