using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    // save the frame by frame animations as states, and then draw the states\
    // make deathblow a cinematic animation
    //player
    Player player;
    GameObject playerModel;
    GameObject collidersHolder;
    //animation stats
    //root anim
    public GameObject rootAnimObj { get; private set; }
    public MoveAnimationScript rootAnim { get; private set; }
    //cur Anim
    public GameObject curAnimObj { get; private set; }
    public MoveAnimationScript curAnim { get; private set; }
    int curAnimFrame; // add some kind of interpolation if you are missing frames
    public bool animComplete;


    public void Setup(Player _player)
    {
        player = _player;
        playerModel = player.playerModel;
        collidersHolder = player.collidersHolder;
    }
    public void LinkedUpdate(int curFrame, int deltaFrame)
    {
        if (curAnim != null && !animComplete && deltaFrame >= 1)
        {
            curAnimFrame += deltaFrame;
            if (curAnim.loop && curAnimFrame >= curAnim.frames.Count) //this could cause issues with the animation system // look here if there is a problem 
                curAnimFrame -= curAnim.frames.Count;
            else if (curAnimFrame >= curAnim.frames.Count)
            {
                curAnimFrame = curAnim.frames.Count - 1;
                animComplete = true;
            }
            SetFrame(curAnim.frames);
        }
    }

    public void PlayRootAnimation(GameObject Move) { PlayRootAnimation(Move.GetComponent<MoveAnimationScript>()); curAnimObj = Move; rootAnimObj = Move; } // make it so you can play entry, root, or exit frames
    public void PlayRootAnimation(MoveAnimationScript anim) // change this to playing the root animation vs entry vs exit animation 
    {
        animComplete = false;
        rootAnim = anim; // this system might be more confusing
        curAnim = anim;
        curAnimFrame = 0;
        SetFrame(curAnim.frames);
    }
    public void PlayEntryAnimation(GameObject Move, GameObject Root) { PlayEntryAnimation(Move.GetComponent<MoveAnimationScript>(), Root.GetComponent<MoveAnimationScript>()); curAnimObj = Move; rootAnimObj = Root; } //make this simple for now (change this later)
    public void PlayEntryAnimation(MoveAnimationScript anim, MoveAnimationScript root)  
    {
        animComplete = false;
        rootAnim = root;
        curAnim = anim;
        curAnimFrame = 0;
        SetFrame(curAnim.frames); // curAnim.entryFrames
    }
    public void PlayExitAnimation(GameObject Move, GameObject Root) { PlayExitAnimation(Move.GetComponent<MoveAnimationScript>(), Root.GetComponent<MoveAnimationScript>()); curAnimObj = Move; rootAnimObj = Root; } 
    public void PlayExitAnimation(MoveAnimationScript anim, MoveAnimationScript root) 
    {
        animComplete = false;
        rootAnim = root;
        curAnim = anim;
        curAnimFrame = 0;
        SetFrame(curAnim.frames); // curAnim.exitFrames
    }

    private void SetFrame(List<GameObject> frames)
    {
        if (frames[curAnimFrame] != null) 
        {
            FrameScript frame = frames[curAnimFrame].GetComponent<FrameScript>();
            //player model
            for (int i = playerModel.transform.childCount - 1; i >= 0; --i)
                Destroy(playerModel.transform.GetChild(i).gameObject);
            for (int i = 0; i < frame.playerModels.Count; ++i)
                Instantiate(frame.playerModels[i], playerModel.transform);
            //colliders // finish this later
            List<GameObject>[] colliderList = new List<GameObject>[7] { frame.hitboxes, frame.hurtboxes, frame.collisionboxes, frame.shieldboxes, frame.parryboxes, frame.counterboxes, frame.grabboxes };
            for (int i = 0; i < collidersHolder.transform.childCount; ++i)
            {
                if (colliderList[i].Count > 0)
                {
                    for (int j = playerModel.transform.childCount - 1; j >= 0; --j)
                        Destroy(collidersHolder.transform.GetChild(i).GetChild(j).gameObject);
                    for (int j = 0; j < colliderList[i].Count; ++j)
                        Instantiate(colliderList[i][j], collidersHolder.transform.GetChild(i));
                }
            }
        }
    }

}
