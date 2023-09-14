using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollisions : MonoBehaviour
{
    //Rework the push boxes and hitboxes



    // make collider manager separate bc otherwise they won't update at the same time, or you can make it so that it iterates through both
    //!IMPORTANT! : Save all game states

    // different types of hitbox:
    // 0 = Hitbox 
    // 1 = Hurtbox
    // 2 = push box
    // 3 = shield box
    // 4 = parry box
    // 5 = counter box
    // 6 = grab box
    // 

    // make hitbox interpolation
    // make an over lap hitbox
    // return contact point return

    // add game states later

    //Player player;
    //Player enemy;
    // all collider holders // list or separate iterations
    //GameObject hitboxHolder; 
    //GameObject hurtboxHolder;
    //GameObject pushBoxHolder;
    //GameObject shieldBoxHolder;
    //GameObject parryBoxHolder;
    //GameObject counterBoxHolder;
    //GameObject grabBoxHolder;
    //
    //List<GameObject> colliderHolders = new List<GameObject>();
    // change if it gets too spagetti, but it should be pretty symetrical, you probably only need to iterate through one for some
    List<Player> players = new List<Player>();
    List<GameObject> p1Holders = new List<GameObject>();
    List<GameObject> p2Holders = new List<GameObject>();
    // combined, makes things easier
    List<List<GameObject>> playerHolders = new List<List<GameObject>>();
    //
    List<List<GameObject>> p1Colliders = new List<List<GameObject>>();
    List<List<GameObject>> p2Colliders = new List<List<GameObject>>();

    FrameStatesManager frameStates;
    //combined
    //List<List<List<GameObject>>> playerColliders = new List<List<List<GameObject>>>();
    //List<List<GameObject>>
    //
    public void Setup(Player p1, Player p2, FrameStatesManager _frameStates)
    {
        //CharacterCollisions collisions = player.enemy.GetComponent<Player>().Collisions;
        //enemyHolders = collisions.playerHolders;
        //enemyColliders = collisions.playerColliders;
        //GameObject cols = player.collidersHolder;
        //for (int i = 0; i < cols.transform.childCount; ++i)
        //    playerHolders.Add(cols.transform.GetChild(i).gameObject);
        //GameObject c1 = p1.collidersHolder;
        //GameObject c2 = p2.collidersHolder;
        frameStates = _frameStates;
        players.Add(p1); 
        players.Add(p2);
        GameObject[] colHolders = new GameObject[] { p1.collidersHolder, p2.collidersHolder};
        for (int i = 0; i < colHolders.Length; ++i)
        {
            List<GameObject> pHolders = new List<GameObject>();
            for (int j = 0; j < colHolders[i].transform.childCount; ++j) // might have to switch how these operations are done, could add them directly to an array on creation, would be easier, and less intensive
            {
                pHolders.Add(colHolders[i].transform.GetChild(j).gameObject);
            }
            playerHolders.Add(pHolders);    
        }
    }
    public void LinkedUpdate(int curFrame, int deltaFrame)
    {
        //for (int i = 0; i < playerHolders.Count; ++i)
        //{

        //}
        //if ()
        //Debug.Log("Testing colliders: " + playerColliders[2][0]);
        CheckCollisionBoxes(curFrame);
    }
    public void CheckCollisionBoxes(int curFrame) // make them simple for now // one push box
    {
        // too many iterations // try to limit it
        List<List<BoxCollider2D>> pushBoxes = new List<List<BoxCollider2D>>();
        for (int i = 0; i < playerHolders.Count; ++i)
        {
            List<BoxCollider2D> playerBoxes = new List<BoxCollider2D>();
            for (int j = 0; j < playerHolders[i][(int)ColliderType.collisionBox].transform.childCount; ++j)
            {
                playerBoxes.Add(playerHolders[i][(int)ColliderType.collisionBox].transform.GetChild(j).gameObject.GetComponent<BoxCollider2D>());
            }
            pushBoxes.Add(playerBoxes);
        }
        for (int i = 0; i < pushBoxes[0].Count; ++i) // make the collision detection system, and everything different, just in case, and optimize later
        {
            BoxCollider2D col1 = pushBoxes[0][i];
            for (int j = 0; j < pushBoxes[1].Count; ++j) // don't include interpolation
            {
                BoxCollider2D col2 = pushBoxes[1][j];
                if (col1.bounds.Intersects(col2.bounds)) // make different interactions // work more on this later // make it based on stats // rework interactions later // interpolate between colliders
                {
                    float p1Speed = players[0].movements.PlayerVelocity.x; // get the total velocity later if I change the mechanics of this game
                    float p2Speed = players[1].movements.PlayerVelocity.x;
                    if (players[0].movements.MovingForward && players[1].movements.MovingForward)
                    {
                        float xMean = (col1.transform.position.x + col2.transform.position.x) / 2;
                        float x1 = xMean - players[0].inputs.Orientation * col1.size.x / 2;
                        float x2 = xMean - players[1].inputs.Orientation * col2.size.x / 2;
                        float y1 = players[0].movements.PlayerPosition.y;
                        float y2 = players[1].movements.PlayerPosition.y;

                        players[0].movements.PlayerPosition = new Vector2(x1, y1);
                        players[1].movements.PlayerPosition = new Vector2(x2, y2);
                    }
                    else if (players[0].movements.MovingForward && !players[1].movements.MovingForward)
                    {
                        
                        float x = col2.transform.position.x - players[0].inputs.Orientation * (col1.size.x / 2 + col2.size.x / 2);
                        float y = players[0].movements.PlayerPosition.y;
                        players[0].movements.PlayerPosition = new Vector2(x, y);
                    }
                    else if (!players[0].movements.MovingForward && players[1].movements.MovingForward)
                    {
                        float x = col1.transform.position.x - players[1].inputs.Orientation * (col1.size.x / 2 + col2.size.x / 2);
                        float y = players[1].movements.PlayerPosition.y;
                        players[1].movements.PlayerPosition = new Vector2(x, y);
                    }
                }
            }
        }
        // Make a running force system + weight system // linking hitboxes // do all this work later
    }
    public void DetectCollision(GameObject obj1, GameObject obj2)
    {

    }
    public void InterpolateDetectCollision()// save the location of the hitboxes last frame // do before move
    {

    }

    public (float, float, float) DetermineStats(Vector2 statPoint)
    {
        Vector2 speedPoint = new Vector2(0, 0);
        Vector2 strengthPoint = new Vector2(1, 0);
        Vector2 weightPoint = new Vector2(0.5f, Mathf.Sqrt(3)/2f);
        statPoint = new Vector2(0.5f,0.5f); // always normalize the statPoint


        //Constrain statPoint
        float speedDist = 1f - Vector2.Distance(statPoint, speedPoint);
        float strengthDist = 1f - Vector2.Distance(statPoint, strengthPoint);
        float weightDist = 1f - Vector2.Distance(statPoint, weightPoint);

        //if(speedDist > 1f)
        //{
        //    statPoint = (speedPoint - statPoint).normalized;
        //}
        //float StrengthDist = 
        //Determine Stat Values
        //float speedStat = 
        return (speedDist, strengthDist, weightDist);
    }
}
enum ColliderType
{
    hitbox = 0,
    hurtbox = 1,
    collisionBox = 2,
    shieldBox = 3,
    parryBox = 4,
    counterBox = 5,
    grabBox = 6
}
