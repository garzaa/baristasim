using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour {

	public GameObject textBox;
	public Text theText;
	public Queue<string> renderQueue;
	public Button button;

	void Awake()
	{
		textBox = GameObject.Find("TextBox");
	}
		
	void Start () 
	{
		renderQueue = new Queue<string>();
		button.onClick.AddListener(Next);
		Open();
	}
		
	void Update () 
	{
		
	}

	void Next()
	{
		renderQueue.Dequeue();
	}

	public void Add(string str)
	{
		theText.text = str;
	}

	public void Add(string[] arr)
	{
		foreach (string str in arr)
		{
			renderQueue.Enqueue(str);
		}
	}

	public void Open()
	{
		textBox.SetActive(true);
	}

	public void Close()
	{
		textBox.SetActive(false);
	}

	public void Render(string inputText)
	{
		Open();
		theText.text = inputText;
	}

	public void PrintOrder() {

	}
}
