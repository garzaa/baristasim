using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeController : MonoBehaviour {

	BoxCollider2D col;
	public bool open;
	GameObject closedDoor;
	GameObject openDoor; 
	GameObject openInside;
	// Use this for initialization
	void Start () 
	{
		open = false;
		closedDoor = transform.Find("closed-door").gameObject;
		openDoor = transform.Find("open-door").gameObject;
		openInside = transform.Find("open-inside").gameObject;
		col = GetComponent<BoxCollider2D>();
	}

	void Update () 
	{
	}

	void OpenFridge()
	{
		open = true;
        col.offset = new Vector2(-5f, 0.4f);
        closedDoor.SetActive(false);
		openDoor.SetActive(true);
		openInside.SetActive(true);
	}

	public void CloseFridge()
	{
		open = false;
        col.offset = new Vector2(1.1f, 0.4f);
        closedDoor.SetActive(true);
		openDoor.SetActive(false);
		openInside.SetActive(false);
    }

	void OnMouseDown()
	{
		if (!open)
		{
			OpenFridge();

		}
		else
		{
			CloseFridge();
		}
	}
}
