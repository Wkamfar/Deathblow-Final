using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour // make a two part camera system, an orthographic camera, and a perspective camera
{
    // make camera zoom in and out
    // follows center
    // prevents players from moving past the max edge
    // make map ends
    // Start is called before the first frame update
    //Players // make a function for this

    //rework how camera works

    // make a manager for map variables such as map size
    [SerializeField] Camera orthoCam;
    [SerializeField] Camera perspectiveCam;
    private Player[] players;// = new Player[2];
    //Camera variables
    //[SerializeField] float[] camXRange = new float[2];//set this with a function
    [SerializeField] float yOffset;
    //add travel speed, scale speed, then add a tuple that contains the size, and the point at which it would scale
    [SerializeField] float camMoveSpeed; // don't use these for now
    [SerializeField] float camScaleSpeed; 
    [SerializeField] float[] minScaleDist; // 0 = dist, 1 = scale
    [SerializeField] float[] maxScaleDist;
    private float scale; // might be redundant for now
    private Vector2 camPos; // deprecated / not in use
    //private float make current boundaries

    // Make a cinematic Camera Later
    public void GameSetup()
    {

    }
    public void GameLinkedUpdate(int curFrame, int deltaFrame, MapManager _mapManager) // include some code to make wall connection more smooth, I guess // might not need it
    {
        float x1 = players[0].transform.position.x;
        float x2 = players[1].transform.position.x;
        //float camX1 = camXRange[0];
        //float camX2 = camXRange[1];
        float[] mapXRange = _mapManager.MapXRange;
        float xDist = Mathf.Abs(x1 - x2);
        float midX = (x1 + x2) / 2;
        float normScale = Mathf.Max(0, Mathf.Min(1, (xDist - minScaleDist[0]) / (maxScaleDist[0] - minScaleDist[0])));
        ScaleGameCameras((maxScaleDist[1] - minScaleDist[1]) * normScale + minScaleDist[1]);
        float height = orthoCam.orthographicSize * 2;
        float aspect = orthoCam.aspect;
        float width = aspect * height;
        if (Mathf.Abs(mapXRange[0] - midX) < width / 2)
        {
            midX = mapXRange[0] + width / 2;
        }
        else if (Mathf.Abs(mapXRange[1] - midX) < width / 2)
        {
            midX = mapXRange[1] - width / 2;
        }
        MoveGameCameras(midX); // fix camera controller later
    }
    void ScaleGameCameras(float scale)
    {
        float height = scale / orthoCam.aspect;
        orthoCam.orthographicSize = height / 2;
    }
    void MoveGameCameras(float xPos)
    {
        orthoCam.transform.position = new Vector3(xPos, yOffset +  orthoCam.orthographicSize, -10);
        //perspectiveCam //do this later same value
    }
    public void SetPlayers(Player p1, Player p2)
    {
        players = new Player[] { p1, p2 };
    }
    public float[] CamXBounds // make this return a value later
    {
        get
        {
            float maxDist = maxScaleDist[1];
            float camX = orthoCam.transform.position.x;
            return new float[2] {camX - maxDist / 2, camX + maxDist / 2 };
        }
    }
}
