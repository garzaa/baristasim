using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragIngredient : MonoBehaviour {

    public float threshold = .5f;
    public bool snapExclusive;
    public string snapToNameTag;
    public List<string> ingredientNames;
    public List<string> originalIngredients;

    public bool followingMouse = false;
    private Vector2 lastPos;
    private Vector3 screenPoint;
    private Vector3 offset;
    public bool dragged = false;

    public bool dumpedIngredients = false;

    public bool locked;

    SpriteOutline outline;

    //for returning
    public Transform lastParent;

    void Awake()
    {
        if (gameObject.GetComponent<BoxCollider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
        //deactivate the outline shader
        outline = GetComponent<SpriteOutline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<SpriteOutline>();
        }
        outline.enabled = false;
        outline.color = new Color(0x00, 0x41, 0xFF, 0xFF); //blue

        lastParent = gameObject.transform.parent;
        originalIngredients.AddRange(ingredientNames);
    }

    public void OnMouseDown()
    {
        if (locked) return;
        this.dragged = true;
        this.dumpedIngredients = false;
        lastPos = this.transform.position;
    screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = this.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        if (locked) return;
        this.transform.parent = null;
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
        this.transform.position = cursorPosition;
        this.dragged = true;
    }

    public void OnMouseUp()
    {
        /*if the nearest snappable object isn't null, then add this.ingredient to its ingredientlist
        so get the component, then add to the list via a function
        */
        AcceptIngredient receptacle;
        if ((getNearestSnappable()) != null)
        {
            receptacle = getNearestSnappable().GetComponent<AcceptIngredient>();
            //add everything to the receptacle
            receptacle.AddIngredient(ingredientNames);
            this.dumpedIngredients = true;
            ingredientNames.Clear();
            ingredientNames.AddRange(originalIngredients);
        }
        this.transform.parent = this.lastParent;
        this.transform.position = lastPos;
        this.dragged = false;
    }

    void Update()
    {
        if (followingMouse)
        {
            this.dragged = true;
            Vector3 temp = Input.mousePosition;
            temp.z = this.transform.position.z + 10; // Set this to be the distance you want the object to be placed in front of the camera.
            this.transform.position = Camera.main.ScreenToWorldPoint(temp);
            if (!Input.GetMouseButton(0))
            {
                followingMouse = false;
                this.OnMouseUp();
            }
        }
        //highlight if it's in a close snap zone and being dragged
        if (outline == null) return;
        if (Input.GetMouseButton(0) &&
            (this.dragged || this.followingMouse) &&
            this.getNearestSnappable() != null)
        {
            this.OutlineSelf(true);
        }
        else {
            this.OutlineSelf(false);
        }
    }

    public GameObject getNearestSnappable()
    {

        GameObject[] snapToObjects = GameObject.FindGameObjectsWithTag("snapingredient");
        GameObject nearestObject = null;
        double largestDistance = double.PositiveInfinity;
        //get the nearest snappable object within its threshold
        for (int i = 0; i < snapToObjects.Length; i++)
        {
            GameObject g = snapToObjects[i];
            AcceptIngredient gSnap = g.GetComponent<AcceptIngredient>();
            Vector2 newPos = gameObject.transform.position;

            //check this.threshold and the other object's threshold to find the closest
            float d = GetDistance(newPos, g.transform.position);

            //various error checking
            if ((d < threshold || d < g.GetComponent<AcceptIngredient>().threshold) &&
                d < largestDistance &&
                (!this.snapExclusive || this.snapToNameTag.Contains(g.name) || this.snapToNameTag.Contains(g.tag)) &&
                g != gameObject &&
                g != null &&
                (!gSnap.exclusive || gSnap.acceptNameTag.Contains(this.gameObject.name) || gSnap.acceptNameTag.Contains(this.tag))
                && (g.transform.parent != null && g.transform.parent.name != this.name))
            {
                nearestObject = g;
            }
        }
        return nearestObject;
    }

    //Return the distance between two positions
    private float GetDistance(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt(Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.x - b.x, 2));
    }

    //recursively outline draggable parents and children
    public void OutlineSelf(bool input)
    {
        gameObject.GetComponent<SpriteOutline>().enabled = input;
    }

    public void AddIngredient(string toAdd)
    {
        if (!ingredientNames.Contains(toAdd)) ingredientNames.Add(toAdd);
    }
}
