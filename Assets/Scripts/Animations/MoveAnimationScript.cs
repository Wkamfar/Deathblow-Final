using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimationScript : MonoBehaviour
{
    public List<GameObject> entryFrames = new List<GameObject>();
    public List<GameObject> frames = new List<GameObject>();
    public List<GameObject> exitFrames = new List<GameObject>();
    public bool loop;
    public GameObject entryAnim;
    public GameObject exitAnim;
}
