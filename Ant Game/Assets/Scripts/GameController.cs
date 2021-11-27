using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static bool gameOver;

    public GameObject ant;
    public GameObject spawner;
    public GameObject level;
    public List<int> Commands;
    public float spawnTime;
    public int antsToSpawn;
    
    // Remeber to remove in final version
    public bool spawnAnts = true;
    
    private List<int> CommandKeys;
    private UIController _UI;
    private int _spawnCount;
    //Coroutine spawn;

    public void StartGame()
    {
        gameOver = false;
        Ant.deadAnts = 0;
        level.gameObject.SetActive(true);
        CommandKeys = Commands;
        StartCoroutine(AntSpawn());
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOver = true;
        _UI = gameObject.GetComponent<UIController>();
        _UI.ReturnTitle();
        _spawnCount = antsToSpawn;
        level.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver) 
        { 
            GetOrder(CommandKeys);
        }
    }

    private void LateUpdate()
    {
        CheckGameEnd();
    }

    IEnumerator AntSpawn()
    {
        while (_spawnCount > 0)
        {
            Instantiate(ant, spawner.transform.position, spawner.transform.rotation);
            _spawnCount--;
            yield return new WaitForSeconds(spawnTime);
        }
        yield break;
    }

    void GetOrder (List<int> keys)
    {
        string input = Input.inputString;
        var orders = new List<int>();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndGame(antsToSpawn);
            return;
        }
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

    public void CheckGameEnd ()
    {
        if (_spawnCount > 0) return;

        var ants = GameObject.FindGameObjectsWithTag("Ant");

        if (ants.Length == 0) 
        {
            EndGame();
        }
    }

    public void EndGame(int scorePenalty = 0)
    {
        gameOver = true;
        StopAllCoroutines();
        Debug.Log("The game is over." +
            "\nYou have won.");
        int score = GameScore(scorePenalty);
        Debug.Log($"You have managed to get {score} ants home." +
            "\nPlay again and go for a higher score!");
        _UI.LoadGameOver();
    }

    private int GameScore(int penalty = 0)
    {
        var stoppers = GameObject.FindGameObjectsWithTag("Stop");
        var score = antsToSpawn - stoppers.Length - Ant.deadAnts - penalty;
        if (score < 0)
        {
            return 0;
        }
        else return score;
    }
}
