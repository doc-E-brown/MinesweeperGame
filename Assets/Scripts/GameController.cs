using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public int numberOfTilesClicked = 0;
    public GameObject BoardPrefab;

    private BoardController _boardController;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple game controllers in scene");
            return;
        }
        Instance = this;
        NewGame();
    }

    public bool isFirstClick()
    {
        return numberOfTilesClicked == 0;
    }

    public void RegisterClick(Tile location)
    {
        // If this is the first click on a board
        // it needs to be a 'safe' click
        if (numberOfTilesClicked == 0)
        {
        }
        numberOfTilesClicked++;
    }

    public void NewGame()
    {
        numberOfTilesClicked = 0;
        GameObject _board = Instantiate(BoardPrefab);

        _boardController = _board.GetComponent<BoardController>();
        _boardController.NewGame();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
