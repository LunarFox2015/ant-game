using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject title;
    public GameObject info;
    public GameObject gameOverScreen;

    private GameController gameController;
    
    public void ReturnTitle()
    {
        info.gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(false);
        title.gameObject.SetActive(true);
    }

    public void LoadInfo()
    {
        title.gameObject.SetActive(false);
        info.gameObject.SetActive(true);
    }

    public void StartButton()
    {
        title.gameObject.SetActive(false);
        gameController.StartGame();
    }

    public void LoadGameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
    }


    // Start is called before the first frame update
    void Start()
    {
        gameController = gameObject.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
