using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DrinkGenerator {

	static string[] syrups = {"vanilla", "chocolate", "hazelnut", "pumpkin", "peppermint", "chai", "white chocolate"};
	static string[] milks = {"almond milk", "soy milk", "2% milk", "skim milk"};


	public static Drink Generate()
	{
        System.Random rnd = new System.Random();

		Drink drink = new Drink();

        //rnd.next picks a random int where 0 <= int < arg1
		drink.size = rnd.Next(3);
		drink.espresso = rnd.Next(3);
		drink.iced = (Random.value < 0.3f);
		drink.blended = (Random.value < 0.2f);

		int numSyrups = (int)(Random.value * syrups.Length);
        if (Random.value < .7) numSyrups = 0;
		while (numSyrups > 0)
		{
            //add a random syrup, repeats are fine because we don't want too many
            drink.AddSyrup(syrups[rnd.Next(syrups.Length)]);
			numSyrups--;
		}

		int numMilks = (int)(Random.value * milks.Length);

        //if blended, gotta have at least one milk
        if (drink.blended)
        {
            numMilks = (int)(Mathf.Ceil(Random.value * milks.Length));
        }

        if (numMilks > 0 && !drink.iced)
		{
			drink.steamLevel = rnd.Next(1, drink.foamLevels.Length);
		}
		else
		{
			drink.steamLevel = 0;
		}

        //don't want to have too many milks if there are milks
        //just lower the likelihood of having a lot
        if (Random.value < .8 && numMilks > 0) numMilks = 1;
        while (numMilks > 0)
		{
            drink.AddMilk(milks[rnd.Next(milks.Length)]);
			numMilks--;
		}

        drink.caf = true;
        drink.coffee = true;
        if (Random.value < .2)
        {
            drink.decaf = true;
            if (Random.value < .7)
            {
                drink.caf = false;
                drink.espresso = 0;
            }
        }

        //if blended, you can't steam it (it's cold)
        if (drink.blended)
        {
            drink.steamLevel = 0;
            drink.iced = true;
            drink.decaf = false;
            drink.espresso = 0;
        }
		return drink;
	}
	

}
