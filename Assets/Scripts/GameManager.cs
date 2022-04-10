using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameGrid grid;
    [SerializeField] private CircleFactory circleFactory;

    private Circle selectedCircle;
    private Tile selectedTile;
    private void Update()
    {
        var currentTile = grid.GetTile(Input.mousePosition);
        if (currentTile == null)
            return;
        
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Spawn");
            circleFactory.SpawnAt(currentTile);
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(1))
        {
            Debug.Log("Remove");
            circleFactory.RemoveAt(currentTile);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Select");
            var shape = currentTile.Shape;
            if (shape == null)
                return;
            if (shape is Circle circle)
            {
                selectedCircle = circle;
                selectedTile = currentTile;
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Move");
            if (selectedCircle == null)
                return;
            selectedCircle.MoveAlong(grid.GetPath(selectedTile, currentTile));
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Test Path");
            var path = grid.GetPath(grid.GetTile(0, 0), currentTile);
            if (path == null)
                return;
            for (var i = 0; i < path.Length - 1; i++)
            {
                Debug.DrawLine(
                    path[i].transform.position + path[i].transform.localScale * 0.5f,
                    path[i+1].transform.position+  path[i].transform.localScale * 0.5f,
                    Color.green, 10f, false);
            }
        }
    }
}
