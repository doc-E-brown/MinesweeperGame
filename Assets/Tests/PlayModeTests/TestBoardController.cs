using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements.Experimental;
using GameObject = UnityEngine.GameObject;

public class TestBoardController
{
    private GameObject _parent;
    private BoardController _controller;
    private (int, int) _initialSelection;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
         // _parent = new GameObject();
         GameObject boardPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Board.prefab");
         GameObject board = (GameObject) Object.Instantiate(boardPrefab);
         if (board)
         {
              _controller = board.GetComponent<BoardController>();
              _controller.boardSize = 3;
              _controller.numberOfMines = 2;
              _initialSelection = (1, 1);
         }
    }

    [SetUp]
    public void Setup()
    {
        _controller.NewGame();
    }

    [TearDown]
    public void TearDown()
    {
        // Remove existing tiles in the parent
        foreach (Transform tile in _controller.transform)
        {
            GameObject.Destroy(tile.gameObject);
        }
    }

    [UnityTest]
    public IEnumerator TestCreateBlankMap()
    {
        Assert.AreEqual(9, _controller.transform.childCount);
        foreach (Transform child in _controller.transform)
        {
            Tile tile = child.GetComponent<Tile>();
            Assert.That(!tile.isMine);
            Assert.That(tile.x >= 0 && tile.x < _controller.boardSize);
            Assert.That(tile.y >= 0 && tile.y < _controller.boardSize);
            
        }
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator TestGenerateMineLocations()
    {
        _controller.GenerateMineLocations(_initialSelection);

        int mineCount = 0;
        foreach (Transform child in _controller.transform)
        {
            Tile tile = child.GetComponent<Tile>();
            if (tile.isMine)
            {
                mineCount++;
                Assert.That(_initialSelection != (tile.x, tile.y));
            }
            
        }
        Assert.That(mineCount == _controller.numberOfMines);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestComputeTileScores()
    {
        _controller.GenerateMineLocations(_initialSelection);
        Tile tile = _controller.GetTile(1, 1);
        _controller.ComputeTileScore(tile);
        
        Assert.AreEqual(2, tile.Value);
        yield return null;
    }
}
