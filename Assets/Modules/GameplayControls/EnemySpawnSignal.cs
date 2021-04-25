using UnityEngine;

public class EnemySpawnSignal
{
    public CharacterData data => _data;
    public Vector2 position => _position;
    private CharacterData _data;
    private Vector2 _position;

    public EnemySpawnSignal(CharacterData data, Vector2 position)
    {
        _data = data;
        _position = position;
    }
}