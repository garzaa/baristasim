using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour {

	bool skipEnabled;
	void Start() {
        skipEnabled = false;
		StartCoroutine(Wait());
	}

	IEnumerator Wait() {
		yield return new WaitForSeconds(1);
        skipEnabled = true;
	}

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space) && skipEnabled)
		{
			SceneManager.LoadScene("Coffee Shop");
		}
		if (Input.GetKey("escape"))
		{
			Application.Quit();
		}
	}


}
