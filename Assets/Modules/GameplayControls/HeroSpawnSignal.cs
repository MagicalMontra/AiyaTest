using UnityEngine;

public class HeroSpawnSignal
{
    public CharacterData data => _data;
    public Vector2 position => _position;
    private CharacterData _data;
    private Vector2 _position;

    public HeroSpawnSignal(CharacterData data, Vector2 position)
    {
        _data = data;
        _position = position;
    }
}