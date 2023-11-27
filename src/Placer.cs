using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placer : MonoBehaviour
{
    public GameObject selection; 
    private Color _original;
    private SpriteRenderer _spriteRenderer;

    void Start(){
        InputDetection EventList = GetComponent<InputDetection>();
        EventList.CursorMoved += OnCursorMoved;
        EventList.KeyPressed += OnKeyPress;
    }

    public void PlaceRoadMap(){
        LinkRoadWithGrid();

        if (_spriteRenderer != null)
            _spriteRenderer.color = _original;
        
        GameObject clonedObject = Instantiate(selection);
        selection = null;
        OnRoadSelected(clonedObject);
    }

    public void LinkRoadWithGrid(){

    }

    public void OnCursorMoved(Vector3 Position){
        if (selection != null){
            float x = Mathf.RoundToInt(Position.x / consts.GRID_SIZE) * consts.GRID_SIZE;
            float y = Mathf.RoundToInt(Position.y / consts.GRID_SIZE) * consts.GRID_SIZE;
            Vector3 NewPosition = new Vector3(x, y, 0f);
            selection.transform.position = NewPosition;
        }
    }

    public void OnRoadSelected(GameObject Selection){
        selection = Selection;
        _spriteRenderer = selection.GetComponent<SpriteRenderer>();
        _original = _spriteRenderer.color;

        _spriteRenderer.color = new Color(_original.r, 255, _original.b, 0.5f);
    }

    public void OnKeyPress(KeyCode key){
        if (key == KeyCode.E && selection != null)
            PlaceRoadMap();
        else if (key == KeyCode.Q && selection != null){
            Destroy(selection); 
            selection = null;
        }
        
    }
}

            

