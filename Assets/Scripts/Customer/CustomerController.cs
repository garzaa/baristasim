using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour {

	public Transform target;
	public float speed;

	public float minimum = 0.9f;
	public float maximum = 1f;
	public float duration = .01f;
	private float startTime;
	public SpriteRenderer sprite;
    Sprite happy;
    public Sprite angry;

	CupController cup;
    public SnapTo cupSnapper;
	TextBoxManager textBoxManager;

	bool enableText;

	public Drink order;

	int streak;
	public Text streakText;
	float tips;
	public Text tipsText;

	void Start () {
		speed = 6.0f;
		order = DrinkGenerator.Generate();
		target = GameObject.Find("customerPosition").transform;
		startTime = Time.time;
		sprite = GetComponent<SpriteRenderer>();
		enableText = true;
        happy = sprite.sprite;
		textBoxManager = GameObject.Find("TextBoxManager").GetComponent<TextBoxManager>();
		//print(order.ToString());
	}
		

	void Update() {
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, target.position, step);
		if (transform.position.Equals(target.position) && enableText)
		{
			enableText = false;
            textBoxManager.Add("Give me a " + order.ToString() + ".");
        }

		float t = ((Time.time - startTime) * 10f) / duration;
		sprite.color = new Color(1f,1f,1f,Mathf.Lerp(minimum, maximum, t));

        //looking for a snapped cup
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "cup")
            {
                VerifyCup(child.GetComponent<CupController>());
                Destroy(child.gameObject);
                cupSnapper.holdingObject = false;
                return;
            }
        }
	}

    void VerifyCup(CupController cup)
    {
        if (order.Compare(cup.drink))
        {
            this.order = DrinkGenerator.Generate();
			textBoxManager.Add("HMM MM GOOD.\nTIP FOR YOU.\nNow, give me a " + order.ToString() + ".");
            sprite.sprite = happy;
			Sounds.EatSound();
			Sounds.ChangeSound();
			streak++;
			streakText.text = "Current Streak: " + streak;
			Tip();
        }
        else
        {
			textBoxManager.Add("NO! You gave me a " +cup.drink.ToString()+".\nGive me a " + order.ToString() + ".");
            if (angry != null) sprite.sprite = angry;
			Sounds.WrongSound();
			streak = 0;
			streakText.text = "Current Streak: " + streak;
        }
    }

	void Tip() {
		float f = 1f + Random.Range(0f, 2f);
		tips += f;
		tipsText.text = "Tips: $" + tips.ToString("F2");
		print(tips);
		print("Tips: $" + tips.ToString("F2"));
	}
}
