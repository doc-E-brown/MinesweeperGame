using System;
using TMPro;
using UnityEngine;


public class Tile : MonoBehaviour
{
    [Header("Colours")]
    public Color hoverColour;
    public Color flaggedColour;
    
    [Header("Tile States")]
    public bool isFlagProtected = false;
    public bool isRevealed = false;
    public bool isMine;
    public bool isBlank;
    
    [Header("Position")]
    public BoardController Controller;
    public int x;
    public int y;
 
    [Header("Value")]
    public int Value = 0;
    public TextMeshPro label;

   
    public static readonly float offset = 20f;
    private Color _startColour;
    private Renderer _rend;
    
    
    private void OnMouseOver()
    {
        _rend.material.color = hoverColour;
        if (Input.GetMouseButtonDown(0))
        {
            Reveal();
        } else if (Input.GetMouseButtonDown(1))
        {
            isFlagProtected = !isFlagProtected;
            if (isFlagProtected)
            {
                _rend.material.color = flaggedColour;
            }
        }
    }

    private void OnMouseExit()
    {
        if (isFlagProtected)
        {
            _rend.material.color = flaggedColour;
        }
        else
        {
            _rend.material.color = _startColour;
        }
    }

    public void ExplodeBlanks()
    {
         // Revealing a blank cell explodes connected
         // blank cells
         for (int i = Math.Clamp(y - 1, 0, Controller.boardSize); i <= Math.Clamp(y + 1, 0, Controller.boardSize - 1); i++)
         {
             for (int j = Math.Clamp(x - 1, 0, Controller.boardSize); j <= Math.Clamp(x + 1, 0, Controller.boardSize - 1); j++)
             {
                 var connectedTile = Controller.GetTile(j, i);
                 if (connectedTile.isBlank)
                 {
                     connectedTile.Reveal();
                 }
             }
         }
    }

    public void Reveal()
    {
        GameController.Instance.RegisterClick(this);
        if (isRevealed) {
            return;
        }
        
        // If then revealed tile is a mine, lose the game 
        if (isMine)
        {
            GameController.Instance.LoseGame();
        }
        
        // Show the value
        label.enabled = true;
        isRevealed = true;
        
        // Check the game state
        GameController.Instance.IsGameFinished();
        
        // Explode connected blanks
        if (isBlank)
        {
            ExplodeBlanks();
        }
        

    }

    // Start is called before the first frame update
    void Start()
    {
        _rend = GetComponent<Renderer>();
        if (_rend)
        {
            _startColour = _rend.material.color;
        }
    }

    private void Awake()
    {
        label.enabled = false;
    }

    public void makeMine()
    {
        isMine = true;
        isBlank = false;
        label.text = "M";
    }
    
    public void makeClear()
    {
        isMine = false;
        isBlank = true;
    }

    public void SetValue(int value)
    {
        Value = value;
        label.text = Value.ToString();
        isBlank = (value == 0);
    }

    public static Vector3 GetTilePosition(int x, int y, Vector3 scale)
    {
        Vector3 position = new Vector3(0, 0, 0)
        {
            x = x * (scale.x + offset),
            y = y * (scale.y + offset),
            z = 0
        };
        return position;

    }
}
