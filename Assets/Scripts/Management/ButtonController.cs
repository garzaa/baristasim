using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {

    public Sprite downSprite = null;
    Sprite prevSprite;

	void Start () {
        if (gameObject.GetComponent<BoxCollider2D>() == null) gameObject.AddComponent<BoxCollider2D>();
        prevSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }
	

    void OnMouseDown()
    {
        if (downSprite != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = downSprite;
        }
    }

    void OnMouseUp()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = prevSprite;
    }
}
