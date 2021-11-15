using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public static bool gameOver;
    public TerrainDestroyer destroyer;

    public GameObject ant;
    public GameObject spawner;
    public List<int> Commands;
    public float spawnTime;
    public int antsToSpawn;
    public bool spawnAnts = true;
    
    List<int> CommandKeys;
    Coroutine spawn;
    bool levelEnd;
    
    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        levelEnd = false;
        CommandKeys = Commands;
        if (spawnAnts)
        {
            spawn = StartCoroutine(AntSpawn());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver) 
        { 
            GetOrder(CommandKeys);
        }
        else 
        { 
            StopCoroutine(spawn);
            Debug.Log("The game is over.");
        }
    }

    private void LateUpdate()
    {
        CheckGameEnd();
    }

    IEnumerator AntSpawn()
    {
        while (antsToSpawn > 0)
        {
            Instantiate(ant, spawner.transform.position, spawner.transform.rotation);
            antsToSpawn--;
            yield return new WaitForSeconds(spawnTime);
        }
        yield break;
    }

    void GetOrder (List<int> keys)
    {
        string input = Input.inputString;
        //int orderID = 1;
        var orders = new List<int>();
        for (int i = 1; i <= keys.Count; i++)
        {
            if(input.Contains($"{i}"))
            {
                orders.Add(i);
            }
        }
        if (orders.Count > 0) 
        {
            Debug.Log($"Old Order: {Ant.OrderToGive}");
            switch(orders[0])
            {
                case 1: Ant.OrderToGive = Ant.AntOrder.Walk;
                    break;
                case 2: Ant.OrderToGive = Ant.AntOrder.Stop;
                    break;
                case 3: Ant.OrderToGive = Ant.AntOrder.Climb;
                    break;
                case 4: Ant.OrderToGive = Ant.AntOrder.Tunnel;
                    break;
                case 5: Ant.OrderToGive = Ant.AntOrder.Burrow;
                    break;
                default:
                    break;
            }
            Debug.Log($"New order: {Ant.OrderToGive}");
        }
    }

    void CheckGameEnd ()
    {
        if (antsToSpawn > 0) { return; }

        var ants = GameObject.FindGameObjectsWithTag("Ant");

        if (ants.Length == 0) 
        { 
            levelEnd = true;
            gameOver = true;
        }
    }
}
