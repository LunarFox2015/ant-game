using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ant : MonoBehaviour
{
    // Public Static Variables
    public static AntOrder OrderToGive;

    public int gimmieAnOrder;

    // Public Variables
    public GameObject antPrefab;
    public GameObject stopPrefab;
    public GameObject climbPrefab;
    public GameObject tunnelPrefab;
    public GameObject burrowPrefab;
    public float Speed;

    // Private Variables
    private TerrainDestroyer destroyer;
    private Rigidbody2D antRB;
    private Vector2 antPos;
    private AntOrder currentOrder = AntOrder.Walk;
    private bool isClimbing;
    [SerializeField]
    private int moveDir;

    // Properties
    //public int MoveDirection { get => moveDir; private set => moveDir = value; }
    // Enumerations
    public enum AntOrder
    {
        Walk, Stop, Climb, Tunnel, Burrow
    }

    // Methods
    public void OrderAnts(AntOrder order)
    {
        Debug.Log($"OrderAnts() has been called, the order to give is {order}");
        switch (order)
        {
            case AntOrder.Walk:
                Debug.Log("The ant has been given an order. Spawning new ant");
                AntDefault(order);
                break;
            case AntOrder.Stop:
                Debug.Log("The ant has been given an order. Spawning new ant");
                AntStopper(order);
                break;
            case AntOrder.Climb:
                Debug.Log("The ant has been given an order. Spawning new ant");
                AntClimber(order);
                break;
            case AntOrder.Burrow:
                Debug.Log("The ant has been given an order. Spawning new ant");
                AntBurrower(order);
                break;
            case AntOrder.Tunnel:
                Debug.Log("The ant has been given an order. Spawning new ant");
                AntTunneller(order);
                break;
            default:
                Debug.Log("No order has been given.");
                break;
        }
        /// this method checks which currentOrder the ant is about to be given and runs the specifed function for the given currentOrder.
    }
    private void AntMove(Collision2D collision)
    {
        if (currentOrder == AntOrder.Climb)
        {
            if (AntIsTouching(collision, "Wall"))
            {
                Climb();
                return;
            }
        }
        
        else if ((currentOrder == AntOrder.Burrow) || (currentOrder == AntOrder.Tunnel))
        {
           if (AntIsTouching(collision, "Dirt"))
           {
               Vector3 tile = collision.transform.position;
               if (currentOrder == AntOrder.Burrow)
               {
                   Burrow(tile);
                   return;
               }
               else
               {
                   Tunnel(tile);
                   Walk();
                   return;
               }
           }
        }

        if (AntIsTouching(collision, "Floor") || AntIsTouching(collision, "Dirt"))
        {
            Walk();
        }
    }
    private bool AntIsTouching(Collision2D collision, string tag)
    {
        if (collision.gameObject.CompareTag($"{tag}")) return true;
        else return false;
    }
    private void AntStopper(AntOrder order)
    {
        Vector3 newAntPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        GameObject newAnt = Instantiate(stopPrefab, newAntPos, Quaternion.identity);
        Destroy(gameObject);
    }
    private void AntClimber(AntOrder order)
    {
        Debug.Log("Climber Ant being Spawned");
        Vector3 newAntPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        GameObject newAnt = Instantiate(climbPrefab, newAntPos, Quaternion.identity);
        var ant = newAnt.GetComponent<Ant>();
        ant.moveDir = moveDir;
        ant.currentOrder = order;
        Destroy(gameObject);
        Debug.Log("Climber Ant has been spawned");
        //this method turns the selected ant into a climber
    }
    private void AntBurrower(AntOrder order)
    {
        Vector3 newAntPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        GameObject newAnt = Instantiate(burrowPrefab, newAntPos, Quaternion.identity);
        var ant = newAnt.GetComponent<Ant>();
        ant.destroyer = ant.GetComponent<TerrainDestroyer>();
        ant.destroyer.terrain = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        ant.moveDir = moveDir;
        ant.currentOrder = order;
        Destroy(gameObject);
        //this method turns the ant into a burrower
    }
    private void AntTunneller(AntOrder order)
    {
        Vector3 newAntPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        GameObject newAnt = Instantiate(tunnelPrefab, newAntPos, Quaternion.identity);
        var ant = newAnt.GetComponent<Ant>();
        ant.destroyer = ant.GetComponent<TerrainDestroyer>();
        ant.destroyer.terrain = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        ant.moveDir = moveDir;
        ant.currentOrder = order;
        Destroy(gameObject);
        //this method turns the ant into a tunneller
    }
    private void AntDefault(AntOrder order)
    {
        Vector3 newAntPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        GameObject newAnt = Instantiate(antPrefab, newAntPos, Quaternion.identity);
        var ant = newAnt.GetComponent<Ant>();
        ant.moveDir = moveDir;
        ant.currentOrder = order;
        Destroy(gameObject);
        //this method turns the ant into a regular ant
    }
    private void Walk()
    {
        var p = transform.position;
        transform.position = new Vector2(p.x + (moveDir * Time.deltaTime * Speed), p.y);

        //moves ant
    }
    private void Climb()
    {
        var p = transform.position;
        transform.position = new Vector2(p.x, p.y + (1 * Time.deltaTime * Speed));

        // moves ant up walls
    }
    private void TurnAround()
    {
        moveDir *= -1;
        //changes movement direction to left (-1) or right (1)
    }
    private void Burrow(Vector3 tile)
    {
        destroyer.DestroyTerrain(tile);
    }
    private void Tunnel(Vector3 tile)
    {
        destroyer.DestroyTerrain(tile);
    }


    // Start is called before the first frame update
    void Start()
    {
        isClimbing = false;
        antRB = gameObject.GetComponent<Rigidbody2D>();
        //switch(gimmieAnOrder)
        //{
        //    case 1: OrderToGive = AntOrder.Walk;
        //        break;
        //    case 2: OrderToGive = AntOrder.Stop;
        //        break;
        //    case 3: OrderToGive = AntOrder.Climb;
        //        break;
        //    case 4: OrderToGive = AntOrder.Tunnel;
        //        break;
        //    case 5: OrderToGive = AntOrder.Burrow;
        //        break;
        //    default: OrderToGive = AntOrder.Walk;
        //        break;
        //}
        //Debug.Log($"The current ant order is {OrderToGive}");
}

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (antRB.gravityScale == 0)
        {
            isClimbing = true;
        }
    }


    // OnMouseDown is called the frame the player left-clicks the mouse
    void OnMouseDown()
    {
        Debug.Log("An Ant Has Been Clicked");
        OrderAnts(OrderToGive);
    }

    // OnCollissionEnter2D is called on the frame that a collider starts touching another collider
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentOrder == AntOrder.Climb)
        {
            if (AntIsTouching(collision, "Wall") || AntIsTouching(collision, "Dirt"))
            {
                Debug.Log("Climber has touched wall, setting gravity to zero");
                antRB.gravityScale = 0;
                Climb();
                return;
            }
            else if (AntIsTouching(collision, "Floor"))
            {
                if (isClimbing)
                {
                    Debug.Log("Climber has touched a floor, turning gravity back on");
                    antRB.gravityScale = 1;
                    Debug.Log("Reverting to default ant");
                    AntDefault(AntOrder.Walk);
                }
                else Walk();
            }
        }

        else if (currentOrder == AntOrder.Tunnel || currentOrder == AntOrder.Burrow)
        {
            int order = (int)currentOrder;
            if (AntIsTouching(collision, "Dirt"))
            {
                var tile = collision.gameObject.transform.position;
                if (order == 3)
                {
                    Tunnel(tile);
                }
                else if (order == 4)
                {
                    Burrow(tile);
                }
                else Debug.LogWarning("Ant.cs Line 259: order conversion failed." +
                    "\nCheck that currentOrder is equal to Tunnel or Burrow," +
                    "\nand that it is properly converted to an int.");
            }
        }

        if (AntIsTouching(collision, "Wall"))
        {
            TurnAround();
        }
        else if (AntIsTouching(collision, "Floor"))
        {
            Walk();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        AntMove(collision);
    }
}