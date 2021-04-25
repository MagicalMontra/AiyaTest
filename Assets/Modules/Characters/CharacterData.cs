using System;
using UnityEngine;

[Serializable]
public class CharacterData
{
    public CharacterType type;
    public Sprite sprite;
    public string name;
    public int hp;
    public int attack;
    public int defence;
    public float speed = 1f;

    public CharacterData()
    {
        
    }

    public CharacterData(CharacterData data)
    {
        type = data.type;
        sprite = data.sprite;
        name = data.name;
        hp = data.hp;
        attack = data.attack;
        defence = data.defence;
        speed = data.speed;
    }
}