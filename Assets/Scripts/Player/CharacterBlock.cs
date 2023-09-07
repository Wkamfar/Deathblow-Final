using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBlock : MonoBehaviour
{
    private Player player; // make singletons later
    private GameObject enemy;
    public void Setup (Player _player)
    {
        player = _player;
        SetEnemy(player.enemy); //redundant -> maybe remove this later
    }
    public void SetEnemy(GameObject _enemy) { enemy = _enemy; }
    public void LinkedUpdate(int _curFrame, int _deltaFrame)
    {

    }
}
