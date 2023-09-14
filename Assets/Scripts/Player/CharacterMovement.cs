using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterMovement : MonoBehaviour
{
    // make everything smooth using curves
    // use procedure running animation from this video // https://www.youtube.com/watch?v=PcpkBzcRdSU
    // learn inverse kinematics

    //rework the character movement!
    //player
    Player player;
    // maybe make reference to the player Model
    CharacterInputManager inputs;
    CameraController camController;
    MapManager mapManager;
    CharacterAnimations animations;
    [SerializeField] float forwardSpeed; // set speeds based on the value
    [SerializeField] float backwardSpeed;
    //player Velocity
    Vector2 playerVelocity = new Vector2();

    // player position // splitting the position from the object itself to implement rollback  
    Vector2 playerPosition = new Vector2();
    // Start is called before the first frame update

    int deltaFrame;

    //crouch
    //block
    //parry
    //jump
    //dash

    //make a coroutine for the dash
    public void Setup(Player _player) // add input parameter later?
    {
        player = _player;
        inputs = player.inputs;
        camController = player.camController;
        mapManager = player.mapManager;
        animations = player.animations;
    }
    public void LinkedUpdate(int _curFrame, int _deltaFrame)
    {
        deltaFrame = _deltaFrame;
        Orientation();
        BasicMovement();
        CommitMove();
    }
    // add dash later
    void Orientation() // make the ability to lock orientation for moves // during an animation it is only visual shift
    {
        int orientation = inputs.Orientation;
        transform.eulerAngles = new Vector3(0, orientation == 1 ? 0 : 180);
    }
    void BasicMovement() //integrate the value enums if it gets too confusing
    {
        int direction = inputs.Direction;
        int orientation = inputs.Orientation;
        //playerVelocity = new Vector2(direction == 0 ? 0 : direction > 0 ? forwardSpeed : -backwardSpeed, playerVelocity.y); // changes in velocity // maybe not cancel out velocity // add acceleration?
        //playerVelocity = new Vector2(direction == 0 ? 0 : direction * orientation > 0 ? forwardSpeed * orientation : -backwardSpeed * orientation, 0); // add acceleration / deceleration
        //if not doing another action
        if (direction == 6)// add additional logic later // rework later
        {
            playerVelocity = new Vector2(forwardSpeed * orientation, playerVelocity.y);
            if (animations.curAnimObj != player.walk)
                animations.PlayAnimation(player.walk);
            
        }        
        else if (direction == 4)
        {
            playerVelocity = new Vector2(-backwardSpeed * orientation, playerVelocity.y);
            if (animations.curAnimObj != player.backWalk)
                animations.PlayAnimation(player.backWalk);
        }     
        else if (direction == 5)
        {
            playerVelocity = new Vector2(0, 0);
            if (animations.curAnimObj != player.idle)
                animations.PlayAnimation(player.idle);
        }   

    } // make negative ground force + make ground acceleration
    void CommitMove()
    {
        //transform.Translate(playerVelocity * Time.deltaTime); // change to delta time in frames or make it distance per frame
        float x = transform.position.x + playerVelocity.x / 60 * deltaFrame; // add y axis movement
        //limit x movement
        float[] mapXRange = mapManager.MapXRange;
        float[] camXBounds = camController.CamXBounds;
        float width = player.transform.lossyScale.x; // width, manual implementation, temporary
        mapXRange[0] += width / 2;
        mapXRange[1] -= width / 2;
        camXBounds[0] += width / 2;
        camXBounds[1] -= width / 2;
        x = Mathf.Min(Mathf.Min(Mathf.Max(Mathf.Max(x, mapXRange[0]), camXBounds[0]), mapXRange[1]), camXBounds[1]); // change this // make a physical barrier that can be interacted with
        float y = transform.position.y + playerVelocity.y / 60 * deltaFrame;
        PlayerPosition = new Vector2(x, y);
    }
    public Vector2 PlayerPosition // Change this later (probably after collisions)
    {
        get => playerPosition;
        
        set
        {
            playerPosition = value;
            transform.position = playerPosition;
        }
    }
    public Vector2 PlayerVelocity
    {
        get => playerVelocity;
    }
    public bool MovingForward // move this to input manager // add a bunch more states
    {
        get => inputs.Orientation * playerVelocity.x > 0;
    }
}
