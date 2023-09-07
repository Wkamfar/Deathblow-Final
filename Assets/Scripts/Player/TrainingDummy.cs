using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class TrainingDummy : Player
{
    public override void Setup(InputDevice _controller, GameObject _enemy, CameraController _camController, MapManager _mapManager) //add in the different stuff later
    {
        inputs = GetComponent<CharacterInputManager>();
        movements = GetComponent<CharacterMovement>();
        inputs.Setup(this, _controller);
        movements.Setup(this);
    }
    public override void LinkedUpdate(int curFrame, int deltaFrame)
    {
        
    }
}
