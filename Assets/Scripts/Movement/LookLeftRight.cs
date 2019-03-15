using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookLeftRight : MonoBehaviour {

    public Camera sceneCamera;

    public float distance;
    public bool Left;

    float size, width;

    public GameObject viewSwitcher;

    void Start()
    {
        size = sceneCamera.orthographicSize * 2;
        width = size * (float)Screen.width / Screen.height;
        distance = width;
    }

    void OnMouseUp()
    {
        viewSwitcher.GetComponent<SwitchView>().SwitchScreen();
    }

    void MoveLeft()
    {
        sceneCamera.transform.position = new Vector3(
            sceneCamera.transform.position.x - distance,
            sceneCamera.transform.position.y,
            sceneCamera.transform.position.z
            );
    }

    void MoveRight()
    {
        sceneCamera.transform.position = new Vector3(
            sceneCamera.transform.position.x + distance,
            sceneCamera.transform.position.y,
            sceneCamera.transform.position.z
            );
    }
}
