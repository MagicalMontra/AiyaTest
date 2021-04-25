using UnityEngine;
using Zenject;


public class PlayerController
{
    [Inject] private PlayerAvatarWorker _player;

    private bool _isEnabled;

    public void OnGameStart(GameStartSignal signal)
    {
        _player.Initialize(signal.data);
        _isEnabled = true;
    }
    public void OnGetAlly(HeroAllyAcquireSignal signal)
    {
        if (!_isEnabled)
            return;
        
        _player.AddHero(signal.avatar, signal.placeholder);
    }
    public void OnSwapRequested(SwapHeroSignal signal)
    {
        _player.OnHeadDead(signal.enemyAvatarId);
    }
}

public class UpdatePlayerPositionSignal
{
    public Vector2 position => _position;
    private Vector2 _position;

    public UpdatePlayerPositionSignal(Vector2 position)
    {
        _position = position;
    }
}