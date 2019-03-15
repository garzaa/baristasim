using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TamperController : MonoBehaviour {

    private int currentAnimationFrame = 1;
    public float stepDistance = .3f;

    //these need to be set in the editor
    public Sprite f1, f2, f3, f4, f5, f6;
    private Sprite[] sprites;

    private float lastCursorPos;

    public GameObject filter;
    public bool hasSnappedFilter;

    public bool frozen = false;

    public bool ground = false;


    void Start()
    {
        sprites = new Sprite[] { f1, f2, f3, f4, f5, f6 };
        if (gameObject.GetComponent<BoxCollider2D>() == null) gameObject.AddComponent<BoxCollider2D>();
    }

    //fires once on click
    void OnMouseDown()
    {
        if ((filter = GetSnappedFilter()) == null) return;
        lastCursorPos = GetCursorY();
    }

    void OnMouseDrag()
    {
        if (filter == null || frozen) return;
        if (!filter.GetComponent<FilterController>().locked) return;
        if (!ground) return;
        if (GetCursorDistance() < stepDistance * -1)
        {
            lastCursorPos = GetCursorY();
            if (currentAnimationFrame >= sprites.Length - 1)
            {
                SetAnimationFrame(sprites.Length);
                filter.GetComponent<FilterController>().TampGrounds();
                this.frozen = true;
                this.ground = false;
                return;
            } else
            {
                SetAnimationFrame(currentAnimationFrame + 1);
            }
        }
        else if (GetCursorDistance() > stepDistance)
        {
            if (currentAnimationFrame <= 2)
            {
                SetAnimationFrame(1);
                return;
            }
            else
            {
                SetAnimationFrame(currentAnimationFrame - 1);
            }
        }
    }

    void OnMouseUp()
    {
        SetAnimationFrame(1);
        this.frozen = false;
    }

    public float GetCursorY()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
    }

    public float GetCursorDistance()
    {
        return this.GetCursorY() - this.lastCursorPos;
    }

    public void SetAnimationFrame(int frameNo)
    {
        this.currentAnimationFrame = frameNo;
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[frameNo - 1];
    }

    public GameObject GetSnappedFilter()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "filter") return child.gameObject;
        }
        return null;
    }

    void Update()
    {
        hasSnappedFilter = (GetSnappedFilter() != null);
    }

    void log(string s)
    {
        Debug.Log(s);
    }

    public void AddFilterGrounds()
    {
        if (hasSnappedFilter)
        {
            GetSnappedFilter().GetComponent<FilterController>().AddGrounds();
        }
    }
}
