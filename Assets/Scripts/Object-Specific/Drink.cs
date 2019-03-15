using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink {

	public string[] sizes = {"small", "medium", "large"};
	public string[] foamLevels = {"no steam", "steamed milk", "a little foam", "lots of foam"};
	public string drinkName = "";
	public int size;
	public int espresso;
	public bool iced;
	public bool coffee;
    public bool decaf;
    public bool caf;
	public List<string> milks;
	public List<string> syrups;
    public bool blended;
    public int steamLevel;

    public Drink()
    {
        milks = new List<string>();
        syrups = new List<string>();
    }

    public override string ToString()
    {
        string str = "";

        str += sizes[size] + " ";
        if (blended)
        {
            str += "blended ";
        }

        //if it's supposed to be hot and the milk wasn't steamed, bad!
        if (!iced && milks.Count > 0 && steamLevel == 0 &&
            (!coffee || drinkName.Equals("cafe au lait")))
        {
            str += "cold ";
        }

        if (iced && !blended)
        {
            str += "iced ";
        }
        if (decaf && !caf)
        {
            str += "decaf ";
        }
        else if (decaf && caf)
        {
            str += "half-caf ";
        }
        if (coffee || espresso > 0)
        {
            str += "coffee";
        } else
        {
            str += "cup";
        }

        //if this is the last one, put " with"
        //else, nothing
        if (milks.Count > 0 || syrups.Count > 0 || steamLevel > 0 || espresso > 0)
        str += " with";

        if (espresso > 1)
        {
            str += " " + espresso + " shots of espresso";
        }
        else if (espresso == 1)
        {
            str += " a shot of espresso";
        }

        //if this next one is the last one, put "and"
        //else ", "
        if ((syrups.Count > 1 || steamLevel > 1 || milks.Count > 1) && espresso > 0)
        {
            str += ",";
        } else if (syrups.Count == 0 && milks.Count == 0)
        {
            return str;
        } else if ((syrups.Count == 0 && steamLevel == 0 || milks.Count > 0) && espresso > 0)
        {
            str += ", and";
        }

        //if there's nothing else but milks
        if (syrups.Count == 0 && steamLevel == 0 && milks.Count > 0)
        {
            if (espresso == 0)
            {
                str += " with";
            }
        }


        milks.Sort();
        str += FormatList(milks);

        //again, if the milks were the last element
        if (syrups.Count == 0 && steamLevel <= 1)
        {
            return str;
        }
        
        //if there were milks and there's something left here
        if ((syrups.Count > 1 || steamLevel > 1) && milks.Count > 0)
        {
            str += ",";
        } else if (syrups.Count > 0 && steamLevel == 0 && milks.Count > 0)
        {
            str += ", and";
        } else if (syrups.Count > 0 && steamLevel == 0)
        {
            str += " and";
        }

        syrups.Sort();
        str += FormatList(syrups, "syrup");

        //if steamLevel == 0 skip this
        //if there's anything before this, you need ", and "
        if (steamLevel > 1)
        {
            str += " and " + foamLevels[steamLevel];
        }
        return str;
    }

    #region Add Ingredient

    public void AddEspressoShots(int shots)
	{
		espresso += shots;
		Debug.Log(espresso + " shot(s) added");
	}
	public void AddIce()
	{
		iced = true;
		Debug.Log("ice added");
	}
	public void AddCoffee(string coffee)
	{
		this.coffee = true;
        if (coffee.Contains("decaf"))
        {
            this.decaf = true;
        }
        else
        {
            this.caf = true;
        }
            Debug.Log(coffee + " added");
	}
	public void AddSyrup(string syrup)
	{
        if (!syrups.Contains(syrup))
        {
            syrups.Add(syrup);
            Debug.Log(syrup + " added");
        }
	}
	public void AddMilk(string milk)
	{
        if (!milks.Contains(milk))
        {
            milks.Add(milk);
            Debug.Log(milk + " added");
        }
	}

    public void AddBlended()
    {
        this.blended = true;
        this.steamLevel = 0;
        this.coffee = true;
    }

    #endregion

    public bool Compare(Drink order)
	{
        Debug.Log("this order: " + this.ToString());
        Debug.Log("other order: " + order.ToString());
        return this.ToString().Equals(order.ToString());    //:^)
	}

	public bool ListEquals(List<string> l1, List<string> l2)
	{
		if (l1.Count != l2.Count)
			return false;
		for (int i = 0; i < l1.Count; i++) {
			if (l1[i] != l2[i])
				return false;
		}
		return true;
	}

    public void Steam()
    {
        if (steamLevel < 3) steamLevel++;
        //steam melts ice
        iced = false;
    }

    string FormatList(List<string> list)
    {
        string str = "";
        if (list.Count > 0) str += " ";

        for (int i=0; i<list.Count; i++)
        {
            //last word
            if (i == list.Count - 1)
            {
                str += list[i];
            } else if (i == list.Count - 2)
            //second to last word
            {
                str += list[i] + " and ";
            } else
            //all the other words
            {
                str += list[i] + ", ";
            }
        }

        /*most elements: "element, "
        second to last element: element and
        last element: element
        */


        return str;
    }

    string FormatList(List<string> list, string modifier) //e.g. "_____ syrup"
    {
        string str = "";
        modifier = " " + modifier;
        if (list.Count > 0) str += " ";

        for (int i = 0; i < list.Count; i++)
        {
            //last word
            if (i == list.Count - 1)
            {
                str += list[i] + modifier;
            }
            else if (i == list.Count - 2)
            //second to last word
            {
                str += list[i] + modifier + " and ";
            }
            else
            //all the other words
            {
                str += list[i] + modifier + ", ";
            }
        }

        /*most elements: "element, "
        second to last element: element and
        last element: element
        */


        return str;
    }

}


