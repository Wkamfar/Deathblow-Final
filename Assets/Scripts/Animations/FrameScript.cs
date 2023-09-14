using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameScript : MonoBehaviour
{
    public List<GameObject> playerModels = new List<GameObject>();
    public List<GameObject> hitboxes = new List<GameObject>();
    public List<GameObject> hurtboxes = new List<GameObject>();
    public List<GameObject> collisionboxes = new List<GameObject>(); // collision boxes
    public List<GameObject> shieldboxes = new List<GameObject>();
    public List<GameObject> parryboxes = new List<GameObject>();
    public List<GameObject> counterboxes = new List<GameObject>();
    public List<GameObject> grabboxes = new List<GameObject>();
}
