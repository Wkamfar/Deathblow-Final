using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class FrameStatesManager : MonoBehaviour
{
    //Add a rollback system here
    int gameFrame;
    List<Player> players = new List<Player>();
    List<Dictionary<int, Vector2>> playerPositions = new List<Dictionary<int, Vector2>>() { new Dictionary<int, Vector2>(), new Dictionary<int, Vector2>() };
    public void Setup(Player p1, Player p2)
    {
        players.Add(p1);
        players.Add(p2);
    }
    public void LinkedUpdate(int curFrame, int deltaFrame)
    {
        SaveStates(curFrame);
        LoadStates(curFrame);
    }
    void SaveStates(int curFrame)
    {
        for (int i = 0; i < players.Count; ++i)
            playerPositions[i].Add(curFrame, players[i].Movements.PlayerPosition);
    }
    void LoadStates(int curFrame)
    {
        for (int i = 0; i < players.Count; ++i) // you can make a set function, I just think this is easier, but it will probably cause problems later
            players[i].Movements.PlayerPosition = playerPositions[i][curFrame];
    }

    public List<Dictionary<int, Vector2>> PlayerPositions
    {
        get => playerPositions;
    }
}
