using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ant : MonoBehaviour
{
    // Public Static Variables
    public static AntOrder OrderToGive;
    public static int deadAnts;

    // Public Variables
    public GameObject antPrefab;
    public GameObject stopPrefab;
    public GameObject climbPrefab;
    public GameObject tunnelPrefab;
    public GameObject burrowPrefab;
    public float Speed;

    // Private Variables
    private Rigidbody2D antRB;
    [SerializeField]
    private int moveDir;
    private AntOrder currentOrder;
    private GameObject dirt;
    private GameObject dirtFloor;
    private Tilemap dirtTilemap;
    private Tilemap dirtFloorTile;
    private bool burrowing;
    
    // Enumerations
    public enum AntOrder
    {
        Walk, Stop, Climb, Tunnel, Burrow
    }

    // Methods
    public void OrderAnts(AntOrder order)
    {
        switch (order)
        {
            case AntOrder.Walk:
                AntDefault();
                break;
            case AntOrder.Stop:
                AntStopper();
                break;
            case AntOrder.Climb:
                AntClimber();
                break;
            case AntOrder.Burrow:
                AntBurrower();
                break;
            case AntOrder.Tunnel:
                AntTunneller();
                break;
            default:
                Debug.LogWarning("No valid order was given." + 
                    "\nEnsure this function is passed valid inputs");
                break;
        }
        /// this method checks which order the ant is about to be given 
        /// from the game controller and runs the specifed function for the given order.
        /// If the order is invalid or otherwise does not show up correctly then it prints a message
        /// and does nothing
    }
    
    private void AntMove(Collision2D collision)
    {
        /// This method is responsible for moving ants during the OnColission2DStay event

        if (currentOrder == AntOrder.Climb)
        {
            int[] climbLayerCheck = new int[] { 6, 8 };
            if (TouchingAnyLayers(collision, climbLayerCheck))
            { 
                Climb();
                return;
            }
        }
            /// It first checks if the ant is a climber. If so, it checks if it's touching the Walls or Dirt layers.
            /// if it is, it climbs, and stops the method. Otherwise it proceeds to the next check.
        
        else if (currentOrder == AntOrder.Burrow 
            || currentOrder == AntOrder.Tunnel)
        {
            if (AntTouchingTag(collision,"Dirt"))
            {
                if (currentOrder == AntOrder.Tunnel)
                {
                    antRB.gravityScale = 0;
                    Tunnel(collision);
                    return;
                }
                else
                {
                    burrowing = false;
                    Burrow(collision);
                    return;
                }
            }
        }

            /// If it is either a burrower or a tunneller then it checks if the ant is colliding with dirt
            /// if it is then it runs the tunnel method if the ant is a tunneller, otherwise it runs the burrow method.
            /// If either method is called then it ends the method and prevents further checks, otherwise
            /// it proceeds to the final check.

        int[] moveCheckLayers = new int[] { 7, 8 };

        if(TouchingAnyLayers(collision, moveCheckLayers))
        {
            Walk();
        }

            /// This last expression checks if the ant is touching the floor or dirt layer.
            /// If it is it runs the walk method.
        
        /// TODO: 
        ///     * Currently the ants are only able to use layers to tell the difference between floors and walls
        ///      and dirt is on its own layer so ants behave strangely when touching dirt. I need to find a way
        ///      for ants to tell when the dirt or stone they have touched is below them, above them, or to the side 
        ///      in worldspace to determine how to act. I believe that the code in the tunnel and burrow functions
        ///      may be helpful in accomplishing this
    }
    
    private bool AntTouchingTag(Collision2D collision, string tag)
    {
        /// This method checks the tag of the game object collided with and returns true if it is the same
        /// as the argument string provided. This method is currently unused but may be needed later.
        if (collision.gameObject.CompareTag($"{tag}")) return true;
        else return false;
    }

    private bool TouchingAnyTags(Collision2D collision, string[] tags)
    {
        /// This method checks the tag of the game object collided with and returns true if it is the same
        /// as the any of the strings in the argument string[] array provided. 
        /// This method is currently unused but may be needed later.
        bool a = false;
        foreach(string t in tags)
        {
            if (collision.gameObject.CompareTag($"{t}")) a = true;
        }
        return a;
    }

    private bool AntTouchingLayer(Collision2D collision, int layer)
    {
        /// This method checks if the game object collided with is on the argument layer provided
        if (collision.gameObject.layer == layer) return true;
        else return false;
    }

    private bool TouchingAnyLayers(Collision2D collision, int[] layers)
    {
        /// This method checks if the game object collided with is on any of the argument layers provided.
        bool a = false;
        foreach(int i in layers)
        {
            if (collision.gameObject.layer == i) a = true;
        }
        return a;
    }

    private void AntStopper()
    {
        /// This method gets the position of the current ant, instantiates a new Stopper ant at that position,
        /// sets the order for that new ant, and then destroys the ant ordered.
        Vector3 newAntPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        GameObject newAnt = Instantiate(stopPrefab, newAntPos, Quaternion.identity);
        var antScript = newAnt.GetComponent<Ant>();
        antScript.currentOrder = AntOrder.Stop;
        Destroy(gameObject);
    }
    
    private void AntClimber()
    {
        /// This method gets the position of the current ant, instantiates a new Burrower ant at that position,
        /// sets the move direction and order for that new ant, and then destroys the ant ordered.
        Vector3 newAntPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        GameObject newAnt = Instantiate(climbPrefab, newAntPos, Quaternion.identity);
        var antScript = newAnt.GetComponent<Ant>();
        antScript.moveDir = moveDir;
        antScript.currentOrder = AntOrder.Climb;
        Destroy(gameObject);
        //this method turns the selected ant into a climber
    }
    
    private void AntBurrower()
    {
        /// This method gets the position of the current ant, instantiates a new Burrower ant at that position,
        /// sets the move direction and order for that new ant, and then destroys the ant ordered.
        Vector3 newAntPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        GameObject newAnt = Instantiate(burrowPrefab, newAntPos, Quaternion.identity);
        var antScript = newAnt.GetComponent<Ant>();
        antScript.moveDir = moveDir;
        antScript.currentOrder = AntOrder.Burrow;
        Destroy(gameObject);
    }
    
    private void AntTunneller()
    {
        /// This method gets the position of the current ant, instantiates a new Tunneller ant at that position,
        /// sets the move direction and order for that new ant, and then destroys the ant ordered.
        Vector3 newAntPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        GameObject newAnt = Instantiate(tunnelPrefab, newAntPos, Quaternion.identity);
        var antScript = newAnt.GetComponent<Ant>();
        antScript.moveDir = moveDir;
        antScript.currentOrder = AntOrder.Tunnel;
        Destroy(gameObject);
    }
    
    private void AntDefault()
    {
        /// This method gets the position of the current ant, instantiates a new default ant at that position,
        /// sets the move direction and order for that new ant, and then destroys the ant ordered.
        Vector3 newAntPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        GameObject newAnt = Instantiate(antPrefab, newAntPos, Quaternion.identity);
        var antScript = newAnt.GetComponent<Ant>();
        antScript.moveDir = moveDir;
        antScript.currentOrder = AntOrder.Walk;
        Destroy(gameObject);
    }

    private void AntDefault(Vector3 newAntPos)
    {
        GameObject newAnt = Instantiate(antPrefab, newAntPos, Quaternion.identity);
        var antScript = newAnt.GetComponent<Ant>();
        antScript.moveDir = moveDir;
        antScript.currentOrder = AntOrder.Walk;
        Destroy(gameObject);
    }
    
    private void Walk()
    {
        /// Moves ant left or right based on the move direction (+ or - x) by translating the game object.
        var p = transform.position;
        transform.position = new Vector2(p.x + (moveDir * Time.deltaTime * Speed), p.y);
    }
    
    private void Climb()
    {
        /// Moves ant up walls by translating the game object.
        var p = transform.position;
        transform.position = new Vector2(p.x, p.y + (1 * Time.deltaTime * Speed));
    }
    
    private void TurnAround()
    {
        /// Changes the move direction for the ant. Left is -1, right is 1, and it simply flips the sign.
        moveDir *= -1;
    }
    
    private void Burrow(Collision2D collision)
    {
        /// Disables all tiles the ant is touching to the left, right, and below.
        Vector3 touchPos = Vector3.zero;
        foreach (ContactPoint2D touch in collision.contacts)
        {
            touchPos.x = touch.point.x - 0.01f * touch.normal.x;
            touchPos.y = touch.point.y - 0.01f * touch.normal.y;
            dirtTilemap.SetTile(dirtTilemap.WorldToCell(touchPos), null);
            dirtFloorTile.SetTile(dirtFloorTile.WorldToCell(touchPos), null);
        }
    }
    
    private void Tunnel(Collision2D collision)
    {
        /// Disables all tiles the ant is touching to the left, right, and above
        Vector3 touchPos = Vector3.zero;
        foreach (ContactPoint2D touch in collision.contacts)
        {            
            touchPos.x = touch.point.x - 0.01f * touch.normal.x;
            touchPos.y = touch.point.y - 0.01f * touch.normal.y;
            dirtTilemap.SetTile(dirtTilemap.WorldToCell(touchPos), null);
            dirtFloorTile.SetTile(dirtFloorTile.WorldToCell(touchPos), null);
        }

        /// TODO: 
        ///     * This function and Burrow are functionally identical, 
        ///     can they be merged into the same thing?
    }

    // Start is called before the first frame update
    private void Start()
    {
        dirt = GameObject.Find("Dirt");
        dirtFloor = GameObject.Find("Dirt Floor");
        dirtTilemap = dirt.GetComponent<Tilemap>();
        dirtFloorTile = dirtFloor.GetComponent<Tilemap>();
        antRB = gameObject.GetComponent<Rigidbody2D>();
        burrowing = false;
    }

    private void Update()
    {
        if (!GameController.gameOver)
        {
            if (currentOrder == AntOrder.Tunnel
                && antRB.gravityScale == 0)
            {
                Walk();
            }
        }
        else
        {
            deadAnts++;
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        /// When a player clicks on an ant, this script checks if the ant is a Stopper ant.
        /// If it is, it tells the player that the ant cannot be given new orders.
        /// Otherwise it calls the OrderAnts method using the order passed into the script
        /// from the game controller.        
        if(currentOrder == AntOrder.Stop)
        {
            Debug.Log("This ant cannot be given any more orders");
        }
        
        else OrderAnts(OrderToGive);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(currentOrder == AntOrder.Climb)
        {
            var edge = collision.transform.position;
            var climbStopPoint = new Vector3(edge.x + 0.75f, edge.y + 0.8f);
            AntDefault(climbStopPoint);
        }     
        if(collision.gameObject.layer == 9)
        {
            Destroy(gameObject);
            Debug.Log("Ant has reached exit");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /// When an ant collides with anything for the first time, a series of checks is run.

        if (currentOrder == AntOrder.Climb)
        {
            /// First, if the ant is a climber, it checks if it's touching walls or dirt.
            /// Otherwise, if it's touching the floor and the gravity scale is set to 0
            /// then it returns to a normal ant.
            /// In either situation the script returns to prevent further checks.

            if (AntTouchingLayer(collision, 6))
            {
                antRB.gravityScale = 0;
                Climb();
                return;
            }
            else if (AntTouchingLayer(collision, 7)
                && antRB.gravityScale == 0)
            {
                antRB.gravityScale = 1;
                AntDefault();
                return;
            }
        }

        if (currentOrder == AntOrder.Tunnel 
            || currentOrder == AntOrder.Burrow)
        {
            /// If the ant is a tunneller or a burrower, then it checks if the ant is colliding with dirt.
            /// If it is, it checks which of the two the ant is, actually: burrower or tunneller.
            /// If it's a tunneller it sets gravity scale to zero and calls Tunnel.
            /// If it's a burrower it sets burrowing to true and calls Burrow.
            /// In either case the script then returns.

            if (AntTouchingTag(collision,"Dirt"))
            {
                if (currentOrder == AntOrder.Tunnel)
                {
                    antRB.gravityScale = 0;
                    Tunnel(collision);
                    return;
                }
                else
                {
                    Burrow(collision);
                    return;
                }
            }

            /// If the ant is a tunneller or burrower and touches a floor or wall, however,
            /// then it checks if the gravity scale is zero: if it is, it resets it.
            /// Otherwise if the ant is burrowing, it sets burrowing to false.
            /// In either case the ant is then returned to a normal ant
            /// and then the script returns.



            if (AntTouchingTag(collision,"Stone"))
            {
                if (antRB.gravityScale == 0)
                {
                    antRB.gravityScale = 1;
                    AntDefault();
                    return;
                }
                else if (burrowing)
                {
                    AntDefault();
                    return;
                }
            }
        }
       
        /// TODO:
        ///     * Right now the ants do not move correctly while digging.
        ///       Tunneller ants will dig into the dirt and then stop moving forward because they are not
        ///       colliding with anything. This has been temporarily fixed by setting gravity scale to zero
        ///       and having the ant walk every frame while gravity scale is zero.
        ///       Burrower ants will begin burrowing but once they are on dirt they will dig straight down.
        ///       I've decided I'd rathey they walk forward while digging down. When they touch the dirt below them
        ///       as they fall due to gravity, the delete it, and the only move a tiny bit during that frame, so
        ///       they seem to not move at all. This has been temporarily fixed by setting a bool to true when
        ///       the burrower ants are digging, and moving them on every frame if it is true.
        ///       Ideally both of these situations will be fixed and improved somehow
        ///       so that the ants move correctly without manually setting these values.

        //bool grounded = IsAntOnGround(collision);
        //int[] _groundcheck = new int[] { 6, 7 };

        //if(TouchingAnyLayers(collision, _groundcheck))
        //{
        //    if(grounded)
        //    {
        //        Walk();
        //    }
        //    else
        //    {
        //        TurnAround();
        //        Walk();
        //    }
        //}

        if (AntTouchingLayer(collision, 6))
        {
            /// If the ants touch the walls layer then they turn around.
            TurnAround();
            Walk();
        }

        if (AntTouchingLayer(collision, 7))
        {
            /// If the ants touch the floor layer then they walk.
            Walk();
        }

        if (AntTouchingTag(collision,"Spike"))
        {
            /// If the ants touch a spike then they die. :(
            deadAnts++;
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        /// On every frame the ant is still touching a collision the AntMove function is called in order
        /// to interpret how it should proceed to move during that frame.
        AntMove(collision);
    }
}