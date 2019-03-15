using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour {

	private Vector3 screenPoint;
	private Vector3 offset;
    public GameObject snappedObject = null;
    private SpriteRenderer spr;
    private SpriteOutline outline = null;
    //to be removed to make it lock onto a snapped object
    public bool draggable = true;
    //whether it's locked in place, like as in a filter
    public bool locked = false;
    //this if not snapped and locked to a parent
    public GameObject target;
    public DragObject targetScript;
    //whether it's following the mouse or being dragged
    public bool dragged = false;
	public bool useSpriteBounds;
    public bool snapOnStart = false;
    public GameObject toSnapTo;

    //for despawning unless already snapped, in which case return to that position
    private bool snappedOnce = false;

    public float threshold = .5f;
    public bool followingMouse = false;
    public bool lockOnSnap = false;
    public bool destroyOnFail = false;
    public bool returnOnFail = false;
    public bool snapExclusive = false;
    public string snapToNameTag;

    //for returning
    public GameObject lastSnappedTo;

    //adds a box collider to make things draggable if they aren't already
    void Start()
    {
        if (gameObject.GetComponent<BoxCollider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }

        if (this.snapOnStart && toSnapTo != null)
        {
            this.target = this.gameObject;
            this.targetScript = target.GetComponent<DragObject>();
            SnapTo(toSnapTo);
        }

        //if it's not nested, the move target is this
        if (transform.parent != null && 
            transform.parent.gameObject.GetComponent<DragObject>() != null)
            target = gameObject.transform.parent.transform.gameObject;
        else target = gameObject;
      
        //deactivate the outline shader
        outline = GetComponent<SpriteOutline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<SpriteOutline>();
        }
        outline.enabled = false;
        outline.color = new Color(0x00, 0x41, 0xFF, 0xFF); //blue
    }

	//Drag the object
	public void OnMouseDown()
	{
        if (locked) return;

        //this is for when the object's parent should be dragged instead
        targetScript = target.GetComponent<DragObject>();
        targetScript.dragged = true;
        //if it's draggable
        if (draggable) target = this.gameObject;
        //if not, if it has a draggable parent
        else if (gameObject.transform.parent != null &&
                gameObject.transform.parent.gameObject.GetComponent<DragObject>() != null &&
                !draggable)
        {
            target = gameObject.transform.parent.gameObject;
        }
        //so if it's not and neither is the parent
        else {
        }

        //keep track of snapped object
        if (targetScript.snappedObject != null)
        {
            SnapTo snappedTo = targetScript.snappedObject.GetComponent<SnapTo>();
            snappedTo.holdingObject = false;
            snappedObject = null;
        }
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
	void OnMouseDrag()
	{
        if (locked) return;
        target.transform.parent = null;
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
        target.transform.position = cursorPosition;
	}


	//Snap object to available snappable objects
	void OnMouseUp()
	{
        if (locked) return;

        targetScript = target.GetComponent<DragObject>();
        if (targetScript.snappedObject != null)
            targetScript.snappedObject.GetComponent<SnapTo>().holdingObject = false;
        targetScript.dragged = false;
        spr = target.GetComponent<SpriteRenderer>();
        GameObject nearestObject = targetScript.getNearestSnappable();

        Sounds.SnapSound();

        //on a failed snap
        if (nearestObject == null)
        {
            target.transform.parent = null;
            targetScript.snappedObject = null;
            if (targetScript.destroyOnFail && targetScript.returnOnFail && snappedOnce)
            {
                targetScript.SnapTo(targetScript.lastSnappedTo);
            }
            else if (targetScript.destroyOnFail)
            { 
                Destroy(target);
            }
            else if (targetScript.returnOnFail)
            {
                targetScript.SnapTo(targetScript.lastSnappedTo);
            }
            return;
        }

        targetScript.SnapTo(nearestObject);

    }

    //used for following the mouse after being instantiated
    void Update()
    {
        DragObject targetScript = target.GetComponent<DragObject>();

        //make it follow the mouse if spawned by a prefab spawner
        if (followingMouse)
        {
            targetScript.dragged = true;
            Vector3 temp = Input.mousePosition;
            temp.z = this.transform.position.z+10; // Set this to be the distance you want the object to be placed in front of the camera.
            this.transform.position = Camera.main.ScreenToWorldPoint(temp);
            if (!Input.GetMouseButton(0))
            {
                targetScript.dragged = false;
                followingMouse = false;
                this.OnMouseUp();
            }
        }

        //highlight if it's in a close snap zone and being dragged
        if (outline == null) return;
        if (Input.GetMouseButton(0) && targetScript != null &&
            targetScript.dragged &&
            targetScript.getNearestSnappable() != null)
        {
            targetScript.SetOutline(true);
        }
        else if (targetScript != null) {
            targetScript.SetOutline(false);
        }
    }

    public GameObject getNearestSnappable()
    {
        DragObject targetScript = target.GetComponent<DragObject>();
        GameObject[] snapToObjects = GameObject.FindGameObjectsWithTag("snap");
        GameObject nearestObject = null;
        double largestDistance = double.PositiveInfinity;
        //get the nearest snappable object within its threshold
        for (int i = 0; i < snapToObjects.Length; i++)
        {
            GameObject g = snapToObjects[i];
            SnapTo gSnap = g.GetComponent<SnapTo>();
            Vector2 newPos = gameObject.transform.position;

            //if the target wants to snap to the bottom, calculate distance based on the bottom of the sprite
            //so it'll feel more accurate to the player
            if (gSnap.snapBottom)
            {
                SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer>();
                newPos = new Vector2(gameObject.transform.position.x, spr.bounds.min.y);
            }

            //check this.threshold and the other object's threshold to find the closest
            float d = GetDistance(newPos, g.transform.position);

            //various error checking
            if ((d < threshold || d < g.GetComponent<SnapTo>().threshold) &&
                d < largestDistance &&
                (!targetScript.snapExclusive || targetScript.snapToNameTag.Contains(g.name) || targetScript.snapToNameTag.Contains(g.tag)) &&
                g != gameObject &&
                g != target &&
                g != null &&
                (!gSnap.exclusive || gSnap.acceptNameTag.Contains(target.name) || gSnap.acceptNameTag.Contains(target.tag)) &&
                !gSnap.holdingObject)
            {
                nearestObject = g;
            }
        }
        return nearestObject;
    }

	//Return the distance between two positions
	private float GetDistance(Vector3 a, Vector3 b)
	{
		return Mathf.Sqrt( Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.x - b.x, 2) );
	}

    //recursively outline draggable parents and children
    public void OutlineSelf(bool input)
    {
        gameObject.GetComponent<SpriteOutline>().enabled = input;
    }

    public void SetOutline(bool input)
    {
        OutlineSelf(input);
        OutlineChild(input);
        if (!snappedObject) OutlineParent(input);
    }

    public void OutlineParent(bool input)
    {
        if (transform.parent != null)
        {
            if (transform.parent.gameObject.GetComponent<DragObject>() != null)
            {
                transform.parent.gameObject.GetComponent<SpriteOutline>().enabled = input;
                transform.parent.gameObject.GetComponent<DragObject>().OutlineParent(input);
            }
        }
    }
    public void OutlineChild(bool input)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<DragObject>() != null)
            {
                child.gameObject.GetComponent<SpriteOutline>().enabled = input; //this biNch
                child.gameObject.GetComponent<DragObject>().OutlineChild(input);
            }
        }
    }

    public bool IsSnapped()
    {
        return snappedObject != null;
    }

    public void SnapTo(GameObject nearestObject)
    {

        //else, continue as normal
        snappedOnce = true;
        targetScript.snappedObject = nearestObject;
        SnapTo snappedTo = nearestObject.GetComponent<SnapTo>();
        //future proof :^)
        snappedTo.Accept(target);
        if (snappedTo.destroyOnSnap) return;

        //try to nest them for moving and remove the drag ability (optional)
        if (targetScript.snappedObject.transform.parent != null) target.transform.parent = targetScript.snappedObject.transform.parent;
        //otherwise, just nest directly to the parent
        else target.transform.parent = targetScript.snappedObject.transform;

        spr = target.GetComponent<SpriteRenderer>();

        //snap to the bottom
        if (snappedTo.snapBottom)
        {
            float xPos = nearestObject.transform.position.x;
            float bottomY = nearestObject.transform.position.y -
                nearestObject.GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
            float offset = spr.bounds.extents.y;
            float yPos = bottomY + offset;
            target.transform.position = new Vector2(xPos, yPos);
        }
        else
        //or just the target transform
        {
            target.transform.Translate(nearestObject.transform.position.x - target.transform.position.x, nearestObject.transform.position.y - target.transform.position.y, 0);
        }
        //edit the snapped object properties
        nearestObject.GetComponent<SnapTo>().holdingObject = true;


        if (lockOnSnap) draggable = false; //now if this is dragged it'll drag the parent


        //update the drag target
        //if it's draggable
        //make the target the parent on mouseUp for highlighting fixes
        if (gameObject.transform.parent != null &&
                gameObject.transform.parent.gameObject.GetComponent<DragObject>() != null)
        {
            target = gameObject.transform.parent.gameObject; //THIS FIXES THE HIGHLIGHT ISSUE FOR SOME REASON
        }
        else if (draggable) target = gameObject;
        //if not, if it has a draggable parent
        else if (gameObject.transform.parent != null &&
                gameObject.transform.parent.gameObject.GetComponent<DragObject>() != null &&
                !draggable)
        {
            target = gameObject.transform.parent.gameObject;
        }

        lastSnappedTo = nearestObject;
    }
}