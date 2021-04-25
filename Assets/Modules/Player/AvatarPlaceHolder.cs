using System;
using UnityEngine;
using Zenject;

public class AvatarPlaceHolder : MonoBehaviour
{
    public string instanceId => _instanceId;
    public string tag => _tag.value;

    [Inject] private SignalBus _signalBus;
    [SerializeField] private LayerMask _avatarLayer;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private SpriteRenderer _renderer;

    private string _instanceId;
    private AvatarTag _tag;

    public Transform Init(string instanceId, Sprite sprite, AvatarTag tag)
    {
        _renderer.sprite = sprite;
        _instanceId = instanceId;
        _tag = new AvatarTag(tag);
        return transform;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (tag != "Player")
            return;
        
        var gameWall = other.GetComponent<GameWall>();

        if (gameWall != null)
        {
            _signalBus.Fire(new WallHitSignal(gameWall));
            return;
        }
        
        var otherAvatar = other.GetComponent<AvatarPlaceHolder>();

        if (otherAvatar != null)
        {
            if (otherAvatar.tag == "Ally")
                return;
            
            _signalBus.Fire(new EncounterSignal(otherAvatar.instanceId));
        }
        
    }
}

[Serializable]
public class AvatarTag
{
    public string value;

    public AvatarTag(string value)
    {
        this.value = value;
    }
    public AvatarTag(AvatarTag tag)
    {
        value = tag.value;
    }
}