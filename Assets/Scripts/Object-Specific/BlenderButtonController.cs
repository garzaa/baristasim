using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlenderButtonController : MonoBehaviour
{
    public GameObject blender, animatedBlending;

    SpriteRenderer blendAnimation;
    AcceptIngredient ingredients;
    DragIngredient dragIngredients;
    BlenderPitcherController pitcher;

    SpriteRenderer pitcherSprite;

    public float blendLength = .7f;

    bool blending = false;

    void Start()
    {
        blendAnimation = animatedBlending.GetComponent<SpriteRenderer>();
        blendAnimation.enabled = false;
        pitcher = GameObject.Find("blender-pitcher").GetComponent<BlenderPitcherController>();
        pitcherSprite = pitcher.gameObject.GetComponent<SpriteRenderer>();
        dragIngredients = pitcher.GetComponent<DragIngredient>();
    }
    void OnMouseDown()
    {
        if (!(blending || pitcher.blent) &&
            pitcher.HasAll()){
                 Blend();
                 
            }
    }

    void Blend()
    {
        Sounds.BlendSound();
        this.blending = true;
        pitcherSprite.enabled = false;
        blendAnimation.enabled = true;
        dragIngredients.AddIngredient("blended");
        StartCoroutine(EndAnimation(blendLength));
    }

    IEnumerator EndAnimation(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        blendAnimation.enabled = false;
        pitcherSprite.enabled = true;
        this.blending = false;
        pitcher.blent = true;
        pitcher.ChangePitcher(pitcher.blended);
    }

    void ChangePitcher(Sprite toChange)
    {
        pitcher.GetComponent<SpriteRenderer>().sprite = toChange;
    }
}
