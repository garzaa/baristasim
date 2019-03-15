using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptIngredient : MonoBehaviour {

    public List<string> ingredients;

    public bool snapBottom = false;
    public bool exclusive = false;
    public float threshold = 0.5f;
    public string acceptNameTag;

    void Awake()
    {
        gameObject.tag = "snapingredient";
    }

    void Start () {
        ingredients = new List<string>();
	}

    public void AddIngredient(string ing)
    {
        if (!ingredients.Contains(ing)) ingredients.Add(ing);
    }

    public void AddIngredient(List<string> ing)
    {
        if (ingredients.Count != 0)
        {
            //use basic for loop because it might get modified by a blenderpitcher
            for (int i=0; i<ingredients.Count; i++)
            {
                foreach (string addedIngredient in ing)
                {
                    if (!addedIngredient.Equals(ingredients[i]))
                    {
                        ingredients.Add(addedIngredient);
                    }
                }
            }
        } else
        {
            ingredients.AddRange(ing);
        }
    }

    public void ClearIngredients()
    {
        ingredients.Clear();
    }

    public string GetIngredient()
    {
        if (ingredients.Count != 0) return ingredients[0];
        return "";
    }

    public bool IsEmpty()
    {
        return ingredients.Count == 0;
    }

    public string[] Enumerate()
    {
        return this.ingredients.ToArray();
    }

    //returns the first element whose name contains the needle
    //useful for getting milks, syrups, etc
    public string GetFirstContaining(string needle)
    {
        for (int i = 0; i < ingredients.Count; i++)
        {
            if (ingredients[i].Contains(needle))
            {
                return ingredients[i];
            }
        }
        return null;
    }

    public List<string> GetContaining(string needle)
    {
        List<string> temp = new List<string>();

        for (int i = 0; i < ingredients.Count; i++)
        {
            if (ingredients[i].Contains(needle))
            {
                temp.Add(ingredients[i]);
            }
        }

        return temp;
    }
}