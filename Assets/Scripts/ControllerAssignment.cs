using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
public class ControllerAssignment : MonoBehaviour // this  script is temporary for testing purposes // add a script later with similar utility + disconnect and reconnect functionality
{   //add key based character selection, so that we can skip the setup phase
    //
    // rework the selection system later, and add in menus
    InputDevice p1Controller;
    InputDevice p2Controller;
    List<PlayerAction> selectors = new List<PlayerAction>();
    [SerializeField] PlayManager playManager;
    [SerializeField] Canvas connectionCanvas;
    [SerializeField] TextMeshProUGUI connectionText;
    // Start is called before the first frame update
    // add a set up for players later
    void Start()
    {
        connectionCanvas.enabled = true;
        connectionText.text = "Connect Controller 1";
        foreach (InputDevice device in InputSystem.devices)
        {
            try
            {
                Keyboard keyboard = (Keyboard)device;
                CreateSelector(device);
            }
            catch (System.InvalidCastException)
            {
                try
                {
                    Gamepad gamepad = (Gamepad)device;
                    CreateSelector(device);
                }
                catch (System.InvalidCastException) { }
            }
        }
    }
    void CreateSelector(InputDevice device)
    {
        PlayerAction playerAction = new PlayerAction();
        playerAction.Enable();
        playerAction.devices = new InputDevice[] { device };
        playerAction.Controller.Selection.performed += Selection;
        playerAction.Controller.Debug.performed += DebugSelect;
        selectors.Add(playerAction);
    }
    PlayerAction FindSelector(InputDevice device)
    {
        for (int i = 0; i < selectors.Count; ++i)
        {
            InputDevice sDevice = new List<InputDevice>(selectors[i].devices)[0];
            if (sDevice.deviceId == device.deviceId)
                return selectors[i];
        }
        return null;
    }
    void Selection(InputAction.CallbackContext ctx) { Selection(ctx.control.device); }
    void Selection(InputDevice device)
    {
        if (p1Controller == null)
        {
            p1Controller = device;
            PlayerAction playerAction = FindSelector(device);
            playerAction.Controller.Selection.performed -= Selection;
            playerAction.Controller.Debug.performed -= DebugSelect;
            playerAction.devices = new InputDevice[] { };
            //playerAction.Disable();
            selectors.Remove(playerAction); // make sure this removes what you want it to
            connectionText.text = "Connect Controller 2";
        }
        else if (p1Controller != null && p2Controller == null)
        {
            p2Controller = device;
            foreach (PlayerAction playerAction in selectors)
            {
                playerAction.Controller.Selection.performed -= Selection;
                playerAction.Controller.Debug.performed -= DebugSelect;
                playerAction.devices = new InputDevice[] { };
            } 
            selectors.Clear();
            playManager.SetUpPlayers(p1Controller, p2Controller);
            //playersManager.SetUpPlayers(Keyboard.current, Gamepad.current);
            //playersManager.SetUpTraining(p2Controller);
            connectionText.text = "";
            connectionCanvas.enabled = false;
        }
    }
    void DebugSelect(InputAction.CallbackContext ctx) { DebugSelect(ctx.control.device); }
    void DebugSelect(InputDevice device)
    {
        p1Controller = device;
        foreach (PlayerAction playerAction in selectors)
        {
            playerAction.Controller.Selection.performed -= Selection;
            playerAction.Controller.Debug.performed -= DebugSelect;
            playerAction.devices = new InputDevice[] { };
        }
        selectors.Clear();
        playManager.SetUpTraining(p1Controller);
        connectionText.text = "";
        connectionCanvas.enabled = false;

    }
    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
