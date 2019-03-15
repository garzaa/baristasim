using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabOnDrag : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;

    public GameObject spawnThis;

	void Start () {
        if (gameObject.GetComponent<BoxCollider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = this.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;

        GameObject test = (GameObject) Instantiate(spawnThis, cursorPosition, Quaternion.identity);
        test.name = spawnThis.name;
        if (test.GetComponent<DragObject>() != null)
            test.GetComponent<DragObject>().followingMouse = true;
        if (test.GetComponent<DragIngredient>() != null)
        {
            test.GetComponent<DragIngredient>().followingMouse = true;
            test.GetComponent<DragIngredient>().dragged = true;
        }
        test.GetComponent<SpriteOutline>();
        if (test == null) Debug.Log("fug");
    }
}
