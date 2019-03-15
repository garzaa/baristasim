using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterController : MonoBehaviour {

    public bool locked = false;
    private bool locking = false;
    private bool unlocking = false;
    private int currentAnimationFrame = 1;
    public float stepDistance = .2f;
    //these need to be set in the editor
    public Sprite f1, f2, f3, f4, f5;
    private Sprite[] sprites;
    private DragObject dragScript;

    //save for mouseUp
    bool toUnlock = false;
    bool frozen = false;

    //the x coordinate of the cursor
    private float lastCursorPos;

    //pouring animation length
    public float pourLength = .7f;
    public bool pouring = false;

    //changing cup settings
    public bool doubleShot = false;

    //filling with coffee grounds
    public bool hasGrounds = true;

    public CupController cupScript;

    //sprites for coffee grounds
    SpriteRenderer coneGrounds, tampedGrounds;

    ForegroundOnDrag fgScript;

    // Use this for initialization
    void Start () {
        this.locked = false;
        sprites = new Sprite[] { f1, f2, f3, f4, f5 };
        dragScript = gameObject.GetComponent<DragObject>();
        fgScript = GetComponent<ForegroundOnDrag>();

        foreach (Transform child in transform)
        {
            if (child.name == "cone-grounds")
            {
                coneGrounds = child.gameObject.GetComponent<SpriteRenderer>();
                coneGrounds.enabled = false;
            }
            else if (child.name == "tamped-grounds")
            {
                tampedGrounds = child.gameObject.GetComponent<SpriteRenderer>();
                tampedGrounds.enabled = false;
            }
        }
	}

    //fires once on click
    void OnMouseDown() {
        //only fire if it's locked to something
        if (dragScript.IsSnapped())
        {
            dragScript.locked = true;
            lastCursorPos = this.GetCursorX();
            //if it's not twisted to lock to the machine
        }  //otherwise, let it be dragged like normal
    }
    
    //keep the espresso grounds from showing up during the locking animation
    void Update()
    {
        if (dragScript.locked)
        {
            fgScript.fgEnabled = false;
        } else
        {
            fgScript.fgEnabled = true;
        }
    }

    void OnMouseDrag()
    {
        //if it's not snapped to something, don't do anything
        if (!dragScript.IsSnapped() || this.frozen || dragScript.dragged) return;

        //set locking/unlocking
        if (GetCursorDistance() > 0) locking = true;
        else unlocking = true;
        //if it's unlocked and being dragged right to lock
        if (!locked && GetCursorDistance() > stepDistance)
        {
            //reset the step calculations
            lastCursorPos = GetCursorX();
            //if it's almost at the end of the animation frame
            if (currentAnimationFrame >= sprites.Length - 1)
            {
                SetAnimationFrame(sprites.Length);
                this.locked = true;
                this.locking = false;
                this.frozen = true;
                dragScript.locked = true;
            } else //otherwise, increment animation like normal
            {
                SetAnimationFrame(currentAnimationFrame + 1);
            }
        //multiply by -1 because the cursor is moving to the left, the negative step distance
        } else if (locked && GetCursorDistance() < stepDistance * -1)
        {
            lastCursorPos = GetCursorX();
            if (currentAnimationFrame <= 2)
            {
                SetAnimationFrame(1);
                this.locked = false;
                this.unlocking = false;
                dragScript.locked = false;
                fgScript.fgEnabled = true;
                this.frozen = true;
            } else
            {
                SetAnimationFrame(currentAnimationFrame - 1);
            }
        }

        //if it's being dragged away from the right direction, and not in the process
        //of locking or unlocking, then unclip it from the parent and let dragObject do its thing
        if ((!locked && GetCursorDistance() < 0) ||
            (locked && GetCursorDistance() > 0) &&
            !(locking || unlocking))
        {
            toUnlock = true;
        }
    }

    void OnMouseUp()
    {
        //do nothing on a single click
        if (GetCursorDistance() == 0f) return;
        this.frozen = false;
        dragScript.locked = dragScript.IsSnapped();

        if (toUnlock)
        {
            dragScript.locked = false;
            dragScript.OnMouseDown();
            toUnlock = false;
        }

        if (locking)
        {
            if (!locked) SetAnimationFrame(1);
            else
            {
                this.locked = false;
            }
        }
        else if (unlocking) {
            if (locked) SetAnimationFrame(sprites.Length);
            else
            {
                this.locked = true;
            }
        }
    }

    public float GetCursorX()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
    }

    public float GetCursorDistance()
    {
        return this.GetCursorX() - this.lastCursorPos;
    }

    public void SetAnimationFrame(int frameNo)
    {
        this.currentAnimationFrame = frameNo;
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[frameNo - 1];
    }

    void log(string s)
    {
        Debug.Log(s);
    }

    public void AnimatePour(string pourSize)
    {
        bool foundSprite = false;

        foreach (Transform child in transform)
        {
            //find a pouring sprite of the requested length (tall, med, short)
            if (child.name.Contains("pour") )
            {
                foundSprite = true;
                SpriteRenderer spr = child.gameObject.GetComponent<SpriteRenderer>();
                spr.enabled = true;
                this.pouring = true;
                //and then disable after the pouring animation has finished
                StartCoroutine(EndPour(spr));
            }
        }
        if (!foundSprite) Debug.LogWarning("No pour sprite found!");
    }

    private IEnumerator EndPour(SpriteRenderer sp)
    {
        yield return new WaitForSeconds(this.pourLength);
        sp.enabled = false;
        this.pouring = false;
        //no fill animation for espresso shots
    }

    public void AddGrounds()
    {
        coneGrounds.enabled = true;
        tampedGrounds.enabled = false;
    }

    public void TampGrounds()
    {
        this.hasGrounds = true;
        coneGrounds.enabled = false;
        tampedGrounds.enabled = true;
    }
    
    public void RemoveGrounds() {
        this.hasGrounds = false;
        coneGrounds.enabled = false;
        tampedGrounds.enabled = false;
    }
}
