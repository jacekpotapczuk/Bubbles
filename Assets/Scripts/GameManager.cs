using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameGrid grid;
    [SerializeField] private CircleFactory circleFactory;
    
    private void Update()
    {
        var currentTile = grid.GetTile(Input.mousePosition);
        if (currentTile == null)
            return;
        
        if (Input.GetMouseButtonDown(0))
        {
            circleFactory.SpawnAt(currentTile);
        }
        else if (Input.GetMouseButtonDown(1))
        {

            circleFactory.RemoveAt(currentTile);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
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
