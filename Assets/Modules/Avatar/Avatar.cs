using System;
using DG.Tweening;
using UnityEngine;

public class Avatar : IAttackable, IHealth, IMovable
{
    public string name => _data.name;
    public bool IsOnCombat;
    public bool IsDead => _currentHp <= 0;
    public int CurrentHp => _currentHp;
    public int MaxHp => _data.hp;
    public string Type => _data.type.value;
    public string instanceId => _instanceId;

    public Sprite sprite => _data.sprite;

    public Vector2 position => _transform == null ? Vector3.zero : _transform.position;
    public Vector3 currentDir;

    private int _currentHp;
    private string _instanceId;
    private Transform _transform;
    private CharacterData _data;
    private CharacterSpriteWorker _spriteWorker;
    
    public Avatar(string instanceId, Transform transform, CharacterData data)
    {
        _instanceId = instanceId;
        _currentHp = data.hp;
        _data = new CharacterData(data);
        _transform = transform;
        _spriteWorker = new CharacterSpriteWorker(transform);
    }
    public void TransferAvatar(string instanceId, Transform transform)
    {
        _instanceId = instanceId;
        _transform = transform;
        _spriteWorker = new CharacterSpriteWorker(transform);
    }
    public int Attack(string otherType)
    {
        var damage = _data.type.value == otherType ? _data.attack * 2 : _data.attack;
        return Mathf.CeilToInt(damage * -1);
    }
    public void Modify(int modifyValue)
    {
        if (modifyValue < 0)
        {
            modifyValue += _data.defence;

            if (modifyValue >= 0)
                modifyValue = -1;
        }

        _currentHp = (_currentHp + modifyValue) < 0 ? 0 : _currentHp + modifyValue;
    }

    public void InstantDeath()
    {
        _currentHp = 0;
    }
    public void SetPos(Vector3 vector)
    {
        if (_transform == null)
            return;
        
        _spriteWorker.HandleSprite(currentDir.x);
        _transform.position = vector;
    }
    public void Move(float speedMultiplier, Vector3 vector)
    {
        if (_transform == null)
            return;
        
        if (IsOnCombat)
            return;

        currentDir = vector;
        _spriteWorker.HandleSprite(currentDir.x);
        _transform.Translate(vector * speedMultiplier * Time.deltaTime);
    }
    public PlayerStatData GetStat()
    {
        var stat = new PlayerStatData();
        stat.sprite = sprite;
        stat.attackDmg = _data.attack.ToString();
        stat.def = _data.defence.ToString();
        stat.name = _data.name;
        return stat;
    }
}