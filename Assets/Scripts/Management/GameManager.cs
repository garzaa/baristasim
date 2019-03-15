using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


	GameObject phone;
	GameObject textBox;
	TextBoxManager textBoxManager;

	void Awake()
	{
		phone = GameObject.Find("Phone");
		textBox = GameObject.Find("TextBox");
		textBoxManager = GameObject.Find("TextBoxManager").GetComponent<TextBoxManager>();

		phone.SetActive(false);
	}

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Q))
		{
			ToggleMenu();
		}
	}

	void Exit()
	{
		SceneManager.LoadScene("Start");
	}

	void Save()
	{
		print("Game Saved!");
	}

	void ToggleMenu()
	{
		if (phone.activeSelf)
		{
			phone.SetActive(false);
		}
		else
		{
			phone.SetActive(true);
		}
	}



}
