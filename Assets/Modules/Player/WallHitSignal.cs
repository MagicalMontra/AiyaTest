using UnityEngine;

public class WallHitSignal
{
    public GameWall gameWall => _gameWall;
    private GameWall _gameWall;

    public WallHitSignal(GameWall gameWall)
    {
        _gameWall = gameWall;
    }
}