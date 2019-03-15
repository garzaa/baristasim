using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlenderPitcherController : MonoBehaviour {

    public Sprite empty, mix, milk, ice, blended;
    SpriteRenderer spr;
    AcceptIngredient placedIngredients;
    DragIngredient dragIngredients;

    public bool blent = false;
    
    void Start()
    {
        foreach (Transform child in this.transform)
        {
            if (child.tag == "snapingredient") placedIngredients = child.gameObject.GetComponent<AcceptIngredient>();
        }
        dragIngredients = this.GetComponent<DragIngredient>();
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //clear the pitcher if it's been dragged to a cup
            if (dragIngredients.dumpedIngredients)
            {
                ChangePitcher(empty);
                dragIngredients.dumpedIngredients = false;
                blent = false;
                dragIngredients.ingredientNames.Clear();
                placedIngredients.ingredients.Clear();
            }

            //add milk, this is long because there can be more than one milk type
            if (HasMilk())
            {
                dragIngredients.ingredientNames.AddRange(placedIngredients.GetContaining("milk"));
            }

            //if mix, it has to be empty
            if (HasMix() && !(HasMilk() || HasIce()))
            {
                ChangePitcher(mix);
            }
            else if (HasMix() && HasMilk() && !HasIce())
            {
                ChangePitcher(milk);
            }
            else if (HasMix() && HasMilk() && HasIce() && !blent)
            {
                ChangePitcher(ice);
            }
        }
    }

    void OnMouseDown()
    {
        //update the list
        dragIngredients.ingredientNames.Clear();
        foreach (string i in placedIngredients.ingredients)
        {
            dragIngredients.AddIngredient(i);
        }
    }

    public void ChangePitcher(Sprite spr)
    {
        this.GetComponent<SpriteRenderer>().sprite = spr;
    }

    bool HasMilk()
    {
        foreach (string name in placedIngredients.ingredients)
        {
            if (name.Contains("milk")) return true;
        }
        return false;
    }

    bool HasMix()
    {
        foreach (string name in placedIngredients.ingredients)
        {
            if (name.Contains("mix")) return true;
        }
        return false;
    }

    bool HasIce()
    {
        foreach (string name in placedIngredients.ingredients)
        {
            if (name.Contains("ice")) return true;
        }
        return false;
    }

    public bool HasAll()
    {
        return HasMix() && HasMilk() && HasIce();
    }
}
