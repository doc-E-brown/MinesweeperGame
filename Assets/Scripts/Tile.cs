using System;
using UnityEngine;


public class Tile : MonoBehaviour
{
    public Color hoverColour;
    public bool isMine;

    public static readonly float offset = 0.8f;
    private Color _startColour;
    private Renderer _rend;

    public int Value = 0;

    public int x;
    public int y;

    public bool isFlagProtected = false;
    
    private void OnMouseOver()
    {
        _rend.material.color = hoverColour;
    }

    private void OnMouseExit()
    {
        _rend.material.color = _startColour;
    }

    private void OnMouseDown()
    {
        GameController.Instance.RegisterClick(this);
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

    public void makeMine()
    {
        isMine = true;
    }
    
    public void makeClear()
    {
        isMine = false;
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
