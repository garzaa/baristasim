using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupController : MonoBehaviour {

	public int size;
	public int espresso;


    //drink making
    public bool filled = false;
    public bool hasLid = false;
    public bool hasSleeve = false;

    AcceptIngredient ingredients;

    private DragObject cupDragScript;
    private BoxCollider2D cupCollider;

    public GameObject steamAnimation;
    SpriteRenderer steamSprite;

    public Drink drink;

	// Use this for initialization
	void Start () {
		espresso = 0;

        cupDragScript = GetComponent<DragObject>();
        foreach (Transform child in transform)
        {
            if (child.name == "ingredient-snapper")
            {
                ingredients = child.GetComponent<AcceptIngredient>();
                break;
            }
        }
        cupCollider = GetComponent<BoxCollider2D>();
        steamSprite = steamAnimation.GetComponent<SpriteRenderer>();
        steamSprite.enabled = false;

        drink = new Drink();
		drink.size = this.size;
	}

	public void Update()
	{
/*		GameObject man = GameObject.Find("man");
		if (cupCollider != null &&
            cupCollider.bounds.Intersects(man.GetComponent<BoxCollider2D>().bounds))
		{
			bool value = Verify(man.GetComponent<CustomerController>().order);
			print(value);
		}*/
	}

	public bool Verify(Drink other)
	{
		print(drink.ToString());
		return drink.Compare(other);
	}

    //enables the filled sprite to indicate it's been interacted with
    public void Fill()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "coffeefilled")
            {
                child.GetComponent<SpriteRenderer>().enabled = true;
                this.filled = true;

                //and then make it persist on a failed snap
                cupDragScript.destroyOnFail = false;

                //end the lookup here
                Sounds.PourSound();
                return;
            }
        }
        Debug.LogWarning("No CoffeeFilled sprite to display!");
        
    }



    //check for lid, everything else after ingredients have snapped
    public void LateUpdate()
    {
        //on mouseup, check for ingredients
        if (Input.GetMouseButtonUp(0))
        {
            this.hasLid = false;
            foreach (Transform child in this.transform)
            {
                if (child.name == "Lid")
                {
                    this.hasLid = true;
                } else if (child.name == "Sleeve")
                {
                    this.hasSleeve = true;
                }
            }

            //look through the ingredients and modify the drink accordingly
            if (!ingredients.IsEmpty())
            {
                foreach (string itemName in ingredients.Enumerate())
                {
                    if (itemName.Contains("milk")) {
                        drink.AddMilk(itemName);
                    } else if (itemName.Equals("blended"))
                    {
                        Fill();
                        drink.AddBlended();
                    } else if (itemName.Contains("coffee"))
                    {
                        drink.AddCoffee(itemName);
                        Fill();
                    }
                    else if (itemName.Contains("ice"))
                    {
                        drink.AddIce();
                    }
                }
            }
            //then clear the list
            ingredients.ClearIngredients();
        }
    }

    public bool hasMilk()
    {
        return (drink.milks.Count != 0);
    }

    public bool OnlyMilk()
    {
        return (this.hasMilk() && !(drink.espresso > 0 || drink.iced || drink.coffee || drink.blended 
            || drink.syrups.Count > 0));
    }

    //this is called from the steamerButtonController script
    public void Steam(float seconds)
    {
        steamSprite.enabled = true;
        cupCollider.enabled = false;
        StartCoroutine(EndSteam(seconds));
    }

    IEnumerator EndSteam(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        steamSprite.enabled = false;
        cupCollider.enabled = true;
        drink.Steam();
    }
}
