using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BoardController : MonoBehaviour
{
    public int numberOfMines = 3;
    public int boardSize = 9;
    public int randomSeed = 0;
    
    [FormerlySerializedAs("TilePrefab")] public GameObject tilePrefab;

    private List<List<Tile>> _tileMap;

    public List<List<Tile>> TileMap()
    {
        return _tileMap;
    }
    
    void Start()
    {
        Random.InitState(randomSeed);
    }

    public void NewGame()
    {
        CreateBlankMap();
    }

    public void CreateBlankMap()
    {
        _tileMap = new List<List<Tile>>();
        for (int y = 0; y < boardSize; y++)
        {
            List<Tile> row = new List<Tile>();
            for (int x = 0; x < boardSize; x++)
            {
                GameObject newTile = Instantiate(tilePrefab, Tile.GetTilePosition(x, y, new Vector3(2, 2, 1)),
                    Quaternion.identity, transform);
                Tile tile = newTile.GetComponent<Tile>();
                tile.Controller = this;
                tile.x = x;
                tile.y = y;
                tile.makeClear();
                row.Add(tile);
            }
            _tileMap.Add(row);
        }
    }

    public void GenerateMap(Tile initialSelection)
    {
        // Generate mines
        GenerateMineLocations((initialSelection.x, initialSelection.y));
        ComputeAllTileScores();
    }

    public void ComputeAllTileScores()
    {
         foreach (var row in _tileMap)
         {
             foreach (var cell in row)
             {
                 if (!cell.isMine)
                 {
                 ComputeTileScore(cell);
                 }
             }
         }       
    }

    public void ComputeTileScore(Tile location)
    {
        var score = 0;
        var xMin = Math.Clamp(location.x - 1, 0, boardSize - 1);
        var xMax = Math.Clamp(location.x + 1, 0, boardSize - 1);
        var yMin = Math.Clamp(location.y - 1, 0, boardSize - 1);
        var yMax = Math.Clamp(location.y + 1, 0, boardSize - 1);
        
        for (int i = yMin; i <= yMax; i++)
        {
            for (int j = xMin; j <= xMax; j++)
            {
                Tile target = GetTile(j, i);
                if (target != location && target.isMine)
                {
                    score++;
                }
            }
        }

        location.SetValue(score);
    }

    public Tile GetTile(int x, int y)
    {
        return _tileMap[y][x];
    }

    public void GenerateMineLocations((int, int) initialSelection)
    {

        List<(int, int)> mineLocations = new List<(int, int)>();

        while (mineLocations.Count < numberOfMines)
        {
            int xPos = (int)Random.Range(0, (float)boardSize - 1);
            int yPos = (int)Random.Range(0, (float)boardSize - 1);
            (int, int) pos = (xPos, yPos);

            if (!mineLocations.Contains(pos) && pos != initialSelection)
            {
                mineLocations.Add(pos);
                Tile tile = GetTile(xPos, yPos);
                tile.makeMine();
                Debug.Log($"Mine location: {xPos}, {yPos}");
            }

        }
    }
}
