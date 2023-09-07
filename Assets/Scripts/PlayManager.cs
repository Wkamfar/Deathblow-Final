using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayManager : MonoBehaviour // make this a frame manager, make it so that every game state is saved to a certain point // online save until the players connects
{
    // Ideas! //
    // make it so that the character, the weapon, the special weapon, and grab are all seperate
    // characters have different stats that affect moves and how they play
    // character class to store all of the data that gets read on creation // make it into a txt file, and then just read it
    // make each frame managed by a frame manager class
    // make a replay after a kill
    // make opening cinematics
    // change input system
    // make grounded check
    //Roadmap
    // 1. Special inputs update / the ability to customize your special inputs
    // 2. grab update, allows for more grab options
    // 3. cosmetic update / skins
    // 4. character + weapon update

    // DEBUG COLORS
    [SerializeField] Material player1Color;
    [SerializeField] Material player2Color;

    //maybe make things reliant on the start
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject dummyPrefab;
    [SerializeField] GameObject[] spawnPoints = new GameObject[2];
    GameObject player1Obj;
    Player player1; // I don't think we need to reference the objects themselves
    GameObject player2Obj;
    Player player2;
    int curFrame;
    int deltaFrame;
    float deltaFrameRemainder; // you can base this system off of a different time system later
    //if active
    bool activated = false;
    bool training; // make different properties if in training mode

    // Map
    [SerializeField] GameObject mapManagerObj;
    MapManager mapManager;
    // Camera Controller
    [SerializeField] GameObject camControllerObj;
    CameraController camController;
    // make a get bounds function and get active
    // add deltaFrames later
    //calculate deltatime in frames
    // Character Collisions
    [SerializeField] GameObject collisionsObj;
    CharacterCollisions collisions;
    //Frame States Manager
    [SerializeField] GameObject frameStatesObj;
    FrameStatesManager frameStates;
    //second test // in case I want to check the frame rate
    //float secondTimer;
    //int runAmount;

    private void Start()
    {
        camController = camControllerObj.GetComponent<CameraController>();
        mapManager = mapManagerObj.GetComponent<MapManager>();
        collisions = collisionsObj.GetComponent<CharacterCollisions>();
        frameStates = frameStatesObj.GetComponent<FrameStatesManager>();
    }
    private void Update()
    { 
        deltaFrameRemainder += Time.deltaTime * 60; // account for pause time, don't use real time, use 
        //secondTimer += Time.deltaTime;
        if (deltaFrameRemainder >= 1)
        {
            deltaFrame = Mathf.FloorToInt(deltaFrameRemainder);
            deltaFrameRemainder -= deltaFrame;
            curFrame += deltaFrame;
            if (activated) // make an in game variable
            {
                player1.LinkedUpdate(curFrame, deltaFrame);
                player2.LinkedUpdate(curFrame, deltaFrame);
                collisions.LinkedUpdate(curFrame, deltaFrame);
                frameStates.LinkedUpdate(curFrame, deltaFrame);
                camController.GameLinkedUpdate(curFrame, deltaFrame, mapManager);// rework how this is passed later, maybe reverse the roles
                //runAmount++;
            }
        }
            //if (secondTimer >= 1)
            //{
            //    secondTimer = 0;
            //    Debug.Log("runAmount is: " + runAmount);
            //    runAmount = 0;
            //}
    }
    public float curTime
    {
        get => Time.time;
    }
    public int CurFrame
    {
        get => (int)Mathf.Floor(Time.time * 60);
    }
    public void SetUpPlayers(InputDevice c1, InputDevice c2) // rework the set up
    {
        Vector2 spawn1 = spawnPoints[0].transform.position, spawn2 = spawnPoints[1].transform.position;
        player1Obj = Instantiate(playerPrefab, spawn1, Quaternion.identity); // I don't expect to have to rotate 
        player2Obj = Instantiate(playerPrefab, spawn2, Quaternion.identity);
        player1 = player1Obj.GetComponent<Player>();
        player2 = player2Obj.GetComponent<Player>();
        player1.Setup(c1, player2Obj, camController, mapManager);
        player2.Setup(c2, player1Obj, camController, mapManager);
        //DEBUG LINE, SETS COLORS OF PLAYERS
        player1.playerModel.transform.GetChild(0).GetComponent<MeshRenderer>().material = player1Color;
        player2.playerModel.transform.GetChild(0).GetComponent<MeshRenderer>().material = player2Color;
        camController.SetPlayers(player1, player2);
        collisions.Setup(player1, player2, frameStates);

        activated = true;
    }
    public void SetUpTraining(InputDevice controller)//
    {
        Vector2 spawn1 = spawnPoints[0].transform.position, spawn2 = spawnPoints[1].transform.position;
        player1Obj = Instantiate(playerPrefab, spawn1, Quaternion.identity); // I don't expect to have to rotate 
        player2Obj = Instantiate(dummyPrefab, spawn2, Quaternion.identity);
        player1 = player1Obj.GetComponent<Player>();
        player2 = player2Obj.GetComponent<TrainingDummy>();
        player1.Setup(controller, player2Obj, camController, mapManager);
        player2.Setup(controller, player1Obj, camController, mapManager);
        //DEBUG LINE, SETS COLORS OF PLAYERS
        player1.playerModel.transform.GetChild(0).GetComponent<MeshRenderer>().material = player1Color;
        player2.playerModel.transform.GetChild(0).GetComponent<MeshRenderer>().material = player2Color;
        camController.SetPlayers(player1, player2);
        collisions.Setup(player1, player2, frameStates);
        activated = true;
    }
}
