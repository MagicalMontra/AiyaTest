using UnityEngine;

public class GameWall : MonoBehaviour
{
    public Vector2 wallDirection;

    public Vector2 ReverseDirection(Vector2 dir)
    {
        return Vector2.Reflect(dir, wallDirection);
    }
}