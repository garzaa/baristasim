using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchView : MonoBehaviour {

    public Camera sceneCamera;

    bool switched = false;
    public float distance;

    float size, width;

    public bool dragHotZone = true;

    FridgeController fridgeScript;

    void Start()
    {
        size = sceneCamera.orthographicSize * 2;
        width = size * (float)Screen.width / Screen.height;
 
        distance = width;

        if (!dragHotZone) GetComponent<BoxCollider2D>().enabled = false;

        fridgeScript = GameObject.Find("Fridge").GetComponent<FridgeController>();
    }

	void OnMouseEnter()
    {
        if (Input.GetMouseButton(0))
        {
            SwitchScreen();
        }
    }

    public void SwitchScreen()
    {
        if (!switched) MoveLeft();
        else MoveRight();
    }

    //nav with arrows/AD
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }
    }

    public void MoveLeft()
    {
        if (switched) return;
        fridgeScript.CloseFridge();
        sceneCamera.transform.position = new Vector3(
            sceneCamera.transform.position.x - distance,
            sceneCamera.transform.position.y,
            sceneCamera.transform.position.z
            );
        switched = true;
    }

    void MoveRight()
    {
        if (!switched) return;
        fridgeScript.CloseFridge();
        sceneCamera.transform.position = new Vector3(
            sceneCamera.transform.position.x + distance,
            sceneCamera.transform.position.y,
            sceneCamera.transform.position.z
            );
        switched = false;
    }
}
