using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CharacterInputManager : MonoBehaviour
{
    // 4 attacks, light attack, heavy attack, special move, grab
    // 2D movement
    // add roll and dash (later) // make dashing more simple, just check if you have a press queued up // add a dash button
    // parry
    // vitality / health
    // endurance
    // sword clashing
    // charged attacks
    // seperate classes for all features

    // ! MAKE ALL INPUTS BASED ON THE ORIENTATION YOU ARE FACING AND THEN ACT ACCORDING TO ORIENTATION


    // add special moves later
    //Player 
    Player player;
    //Enemy 
    GameObject enemy;
    // Game Manager
    //GameVariablesManager game;
    //Game Manager
    InputDevice controller;
    PlayerAction playerAction;
    // Start is called before the first frame update
    // orientation
    private int orientation = 1; // -1 = face left, 1 = facing right
    // movement variables
    private int direction = 5; // notepad notation
    private List<(int, int)> directionHistory = new List<(int, int)>(); // 0 = direction, 1 = age // using notepad notation
    [SerializeField] int storeFrames;
    // implement these systems later...
    // (int, int)[] directionCharge = new (int, int)[3]; // 0 = hold time, 1 = duration / time left
    //[SerializeField] int chargeRetain; // value in frames
    // action variables
    int action;
    int[] heldActions = new int[4];

    // check if you can input
    //List<(int, int, bool, int)> directions = new List<(int, int, bool, int)>(); // 0 = dir * orientation, 1 = pressed timestamp, 2 = still held, 3 = released

    //action variables // all time is done in frames // switch to dictionary if too inconveinent
    //List<(int, int, int, bool, int)> actions = new List<(int, int, int, bool, int)>(); // 0 = action (list), 1 = dir * orientation, 2 = pressed timestamp , 3 = still held, 4 = released 

    // make more simple inputs --> no multi-input commands // subject to change
    // make it so that it is first come, first serve for inputs
    // make held inputs and frame inputs
    // make it so that inputs are effected by direction
    // have something that reads the actions, removing them if they were already used
    //activation window
    //[SerializeField] private int storeTime = 3600;
    //[SerializeField] private int maxStored = 30;

    //Time variables
    int curFrame;
    int deltaFrame;

    public void Setup(Player _player, InputDevice _controller) // add input device here
    {
        player = _player;
        SetEnemy(player.enemy);
        controller = _controller;
        playerAction.devices = new InputDevice[] { controller };
        //getting the gameManager script
        //game = player.GameManager.GetComponent<GameVariablesManager>();
        //ask for enemy here
    }
    public void SetEnemy(GameObject _enemy)
    {
        enemy = _enemy;
    }
    private void OnEnable()
    {
        playerAction = new PlayerAction();
        playerAction.Enable();
    }
    public void LinkedUpdate(int _curFrame, int _deltaFrame)// rework this if I need a new system
    {
        //if (playerAction.Gameplay.Movement.IsPressed())
        //{
        //    int dir = (int)playerAction.Gameplay.Movement.ReadValue<float>();
        //    Debug.Log("Movement vector is: " + dir);
        //}
        //else if (playerAction.Gameplay.Light.WasPerformedThisFrame())
        //    Debug.Log("Light Move");
        //else if (playerAction.Gameplay.Heavy.WasPerformedThisFrame())
        //    Debug.Log("Heavy Move");
        //else if (playerAction.Gameplay.Special.WasPerformedThisFrame())
        //    Debug.Log("Special Move");
        //else if (playerAction.Gameplay.Grab.WasPerformedThisFrame())
        //    Debug.Log("Grab Move");
        curFrame = _curFrame;
        deltaFrame = _deltaFrame;
        GetOrientation(enemy);
        CheckDirectionInput();
        DetermineActionInput();
    }
    void GetOrientation(GameObject enemy)
    {
        orientation = enemy.transform.position.x - player.transform.position.x >= 0 ? 1 : -1;
        // do math to get your position versus the player's position
    }
    void CheckDirectionInput()
    {
        //int direction;
        //int[] directionCharge = new int[3]; // all values in frames
        //[SerializeField] int chargeRetainFrames;
        //List<(int, int)> directionHistory = new List<(int, int)>(); // 0 = direction, 1 = age
        //[SerializeField] int maxStored;
        //[SerializeField] int storeFrames;
        InputAction move = playerAction.Gameplay.Movement;
        int lastFrameDir = direction;
        Vector2 rawDir = move.ReadValue<Vector2>();
        direction = 5 + (int)rawDir.x * orientation + (int)rawDir.y * 3;
       /* for (int i = 0; i < directionCharge.Length; ++i)
        {
            if (direction + 1 == i)
            {
                if (lastFrameDir == direction)
                {
                    directionCharge[i] = (directionCharge[i].Item1 + deltaFrame, chargeRetain);
                }
                else
                {
                    directionCharge[i] = (0, chargeRetain);
                }
            }
            else
            {
                (int chargeTime, int duration) = directionCharge[i];
                duration = Mathf.Max(0, duration - deltaFrame);
                directionCharge[i] = (duration == 0 ? 0 : chargeTime, duration);
            }
        }*/
        for (int i = directionHistory.Count - 1; i >= 0; --i)
        {
            (int dir, int age) = directionHistory[i];
            age += deltaFrame;
            if (age > storeFrames)
            {
                directionHistory.RemoveAt(i);
                continue;
            }
            directionHistory[i] = (dir, age);
        }
        if (direction != lastFrameDir && lastFrameDir != 0)
            directionHistory.Add((lastFrameDir, 0));
    }
    void DetermineActionInput()
    {
        InputAction light = playerAction.Gameplay.Light, heavy = playerAction.Gameplay.Heavy, special = playerAction.Gameplay.Special, grab = playerAction.Gameplay.Grab;
        // make a press priority a separate thing later, or keep it here (I don't really care)
        List<(bool, int)> pressedStates = new List<(bool, int)>() { (light.WasPressedThisFrame(), 0),
                                                                    (heavy.WasPressedThisFrame(), 1), 
                                                                    (special.WasPressedThisFrame(), 3), 
                                                                    (grab.WasPressedThisFrame(), 2) }; // 0 = pressed value, 1 = priority 
        int curPriority = -1 ; // make it so that there is always a value to lower to
        for (int i = 0; i < pressedStates.Count; ++i)
        {
            (bool pressed, int priority) = pressedStates[i];
            if (pressed && priority > curPriority)
            {
                action = i;
                curPriority = priority;
                if (curPriority == pressedStates.Count - 1)
                    break;
            }
        }
        bool[] heldStates = new bool[4] { light.IsPressed(), heavy.IsPressed(), special.IsPressed(), grab.IsPressed() };
        for (int i = 0; i < heldActions.Length; ++i)
            heldActions[i] = heldStates[i] ? heldActions[i] + deltaFrame : 0;
    }
    
    public int Orientation
    {
        get => orientation;
    }
    public int Direction
    {
        get => direction;
    }
}
public enum OrientationValues
{
    right = 1,
    left = -1
}
// direction values = numpad notation
public enum ActionValues
{
    light = 0,
    heavy = 1,
    special = 2,
    grab = 3
}