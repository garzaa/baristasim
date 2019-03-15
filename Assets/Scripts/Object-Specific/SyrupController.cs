using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyrupController : MonoBehaviour {

    public string ingredient;

    public Sprite f1, f2, f3, f4, f5, f6;

    GameObject pour;

    private Sprite[] sprites;

    private float lastCursorPos;

    public GameObject syrupContainer;
    public GameObject cup;
    public GameObject pourObj;

    private int currentAnimationFrame = 1;
    public float stepDistance = .1f;

    public bool frozen = false;

    public float pourLength = .1f;

    void Awake()
    {
        sprites = new Sprite[] { f1, f2, f3, f4, f5, f6 };
        syrupContainer = this.transform.parent.gameObject;
        if (gameObject.GetComponent<BoxCollider2D>() == null) gameObject.AddComponent<BoxCollider2D>();
    }

    //fires once on click
    void OnMouseDown()
    {
        if ((cup = GetSnappedCup()) == null) return;
        lastCursorPos = GetCursorY();
    }

    void OnMouseDrag()
    {
        if (cup == null || frozen) return;
        if (GetCursorDistance() < stepDistance * -1)
        {
            lastCursorPos = GetCursorY();
            if (currentAnimationFrame >= sprites.Length - 1)
            {
                SetAnimationFrame(sprites.Length);
                this.frozen = true;
                StartPour();
                return;
            }
            else
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


    GameObject GetSnappedCup()
    {
        foreach (Transform child in transform.parent)
        {
            if (child.tag == "cup" || child.name == "blender-pitcher")
            {
                return child.gameObject;
            }
        }
        return null;
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

    IEnumerator EndPour(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        pourObj.GetComponent<SpriteRenderer>().enabled = false;
        if (cup.GetComponent<CupController>() != null)
        {
            cup.GetComponent<CupController>().drink.AddSyrup(this.ingredient);
        } else
        {
            cup.GetComponent<DragIngredient>().AddIngredient(this.ingredient);
        }
        this.OnMouseUp();
    }

    void StartPour()
    {
        pourObj.GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(EndPour(pourLength));
    }
}
