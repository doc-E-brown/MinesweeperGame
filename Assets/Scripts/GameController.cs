using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public int numberOfTilesClicked = 0;
    
    [FormerlySerializedAs("BoardPrefab")] public GameObject boardPrefab;

    private BoardController _boardController;

    private bool _gameOver;
    public bool isRunning = false;
    
    
    [FormerlySerializedAs("label")] public TextMeshProUGUI timer;
    private float _runTime;

    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple game controllers in scene");
            return;
        }
        Instance = this;
        isRunning = false;
        timer.text = "00.00s";
        NewGame();
    }
    
    void Update()
    {
        if (GameController.Instance.isRunning)
        {
            _runTime += Time.deltaTime;
            timer.text = $"{_runTime:00.00}s";
        }
    }

    public void RegisterClick(Tile location)
    {
        // If this is the first click on a board
        // it needs to be a 'safe' click
        if (numberOfTilesClicked == 0)
        {
            _boardController.GenerateMap(location);
        }
        numberOfTilesClicked++;
    }

    public void NewGame()
    {
        numberOfTilesClicked = 0;
        _gameOver = false;
        isRunning = !_gameOver;
        
        GameObject _board = Instantiate(boardPrefab);

        _boardController = _board.GetComponent<BoardController>();
        _boardController.NewGame();
    }

    public bool IsGameFinished()
    {
        if (_gameOver)
        {
            return true;
        }
        var tileMap = _boardController.TileMap();
        foreach (var row in tileMap)
        {
            foreach (var tile in row)
            {
                // The tile is not a mine and it isnt revealed
                // unclicked blank tiles are ok 
                if (!tile.isMine && !tile.isRevealed && !tile.isBlank)
                {
                    Debug.Log("Game is continuing");
                    isRunning = true;
                    return false;
                }
            }
        }
        
        // If we have reached this far the game is over and the player won
        Debug.Log("Game is over, you won!");
        isRunning = false;
        return true;
    }

    public void LoseGame()
    {
        Debug.Log("Game is lost");
        _gameOver = true;
        isRunning = !_gameOver;

    }
    
}
