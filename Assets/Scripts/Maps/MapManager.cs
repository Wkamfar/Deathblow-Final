using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // load json here with all the map data, control map animations, and other map related things from here
    // just manually input the data for now
    // rework how this works later

    [SerializeField] float[] mapXRange = new float[2]; // write a getter function

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float[] MapXRange // inefficient, could be optimized
    {
        get => new float[] { mapXRange[0], mapXRange[1]};
    }
}
