using UnityEngine;

public class GameAreaSpawner
{
    public Vector2 RandomPointInArea(Vector2 center, Vector2 size)
    {
        var x = (Random.value - 0.5f) * (size.x * 0.7f);
        var y = (Random.value - 0.5f) * (size.y * 0.7f);
        return center + new Vector2(x, y);
    }
}