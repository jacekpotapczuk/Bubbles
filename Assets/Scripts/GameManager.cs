using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameGrid grid;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var clickedTile = grid.GetTile(Input.mousePosition);
            if (clickedTile == null)
                return;

            var path = grid.GetPath(grid.GetTile(0, 0), clickedTile);
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
        if (Input.GetMouseButtonDown(1))
        {
            var clickedTile = grid.GetTile(Input.mousePosition);
            if (clickedTile == null)
                return;
                
            grid.SpawnAt(clickedTile);
        }
    }
}
