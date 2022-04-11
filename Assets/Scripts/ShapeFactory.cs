using System.Collections.Generic;
using UnityEngine;

public class ShapeFactory<T> where T : Shape
{
    private readonly T prefab;
    private readonly Transform parent;
    private readonly Stack<T> shapePool;

    public ShapeFactory(T shapePrefab, Transform parent)
    {
        prefab = shapePrefab;
        this.parent = parent;
        shapePool = new Stack<T>();
    }

    public T SpawnAt(Tile tile, Color? color)
    {
        if (tile == null)
        {
            Debug.LogError($"Can't spawn at null tile.");
            return null;
        }
        if (tile.Shape != null)
        {
            Debug.LogError($"Can't spawn {typeof(T)} at {tile}. Tile is occupied by {tile.Shape}.");
            return null;
        }

        var tileTransform = tile.transform;
        T shape;
        if (shapePool.Count > 0)
        {
            shape = shapePool.Pop();
            shape.gameObject.SetActive(true);
        }
        else
        {
            shape = Object.Instantiate(prefab, parent, true);
            shape.transform.localScale = tileTransform.localScale;
        }

        shape.transform.position = tile.transform.position;
        tile.Shape = shape;
        shape.Tile = tile;
        shape.Color = color;
        shape.Clear();
        return shape;
    }

    public void RemoveAt(Tile tile)
    {
        if (tile.Shape == null)
        {
            Debug.LogError($"Can't remove at {tile}. There is no shape");
            return;
        }

        if (!(tile.Shape is T))
        {
            Debug.LogError($"Can't remove at {tile}. Shape is not {typeof(T)}, use appropriate factory.");
            return;
        }
        
        tile.Shape.gameObject.SetActive(false);
        shapePool.Push((T)tile.Shape);
        tile.Shape = null;
    }

    public void Reclaim(T shape)
    {
        shapePool.Push(shape);
        shape.gameObject.SetActive(false);
    }

}
