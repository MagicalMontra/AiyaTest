using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using nanoid;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Object = UnityEngine.Object;

public class PlayerAvatarWorker : IDisposable, ITickable
{
    [Inject] private SignalBus _signalBus;
    [Inject] private PlayerSettings _settings;
    [Inject] private PlayerMoveInputWorker _moveInputWorker;
    [Inject] private PlayerSwapInputWorker _swapInputWorker;
    [Inject] private PlayerMoveRuleWorker _moveRuleWorker;

    private Vector2 _lastValue;
    private Avatar _head;
    private List<Avatar> _avatars = new List<Avatar>();

    private AvatarPlaceHolder _headPlaceholder;
    private List<AvatarPlaceHolder> _allyPlaceHolders = new List<AvatarPlaceHolder>();

    private bool _isInitialized;
    private bool _impairedControl;
    private bool _isSwapping;
    private float _speed;
    private int _qSwapCache;

    public async void OnWallHit(WallHitSignal signal)
    {
        if (_avatars.Count <= 0)
        {
            _head.InstantDeath();
            _signalBus.Fire(new GameOverSignal());
            return;
        }
        
        _avatars.RemoveAt(_avatars.Count - 1);
        Object.Destroy(_allyPlaceHolders[_allyPlaceHolders.Count - 1].gameObject);
        _allyPlaceHolders.RemoveAt(_allyPlaceHolders.Count - 1);
        
        _impairedControl = true;
        var newDir = signal.gameWall.ReverseDirection(_lastValue);
        _head.currentDir = newDir;
        _lastValue = newDir;
        await Task.Delay(100);
        _impairedControl = false;
    }
    public void IncreaseSpeed(EnemyKilledSignal signal)
    {
        _speed *= 1.1f;
    }
    public void OnHeadDead(string enemyAvatarId)
    {
        if (!_head.IsDead)
            return;
        
        if (_avatars.Count <= 0)
        {
            _signalBus.Fire(new GameOverSignal());
            return;
        }

        _head = _avatars[0];
        _head.IsOnCombat = true;
        _head.TransferAvatar(_headPlaceholder.instanceId,_headPlaceholder.Init(_headPlaceholder.instanceId, _head.sprite, new AvatarTag("Player")));

        for (int i = 0; i < _avatars.Count; i++)
        {
            if (i + 1 >= _avatars.Count)
                break;
            
            _avatars[i] = _avatars[i + 1];
            _avatars[i].TransferAvatar
                (_allyPlaceHolders[i].instanceId, _allyPlaceHolders[i].Init(_allyPlaceHolders[i].instanceId, _avatars[i].sprite, new AvatarTag("Ally")));
        }
        
        _avatars.RemoveAt(_avatars.Count - 1);
        Object.Destroy(_allyPlaceHolders[_allyPlaceHolders.Count - 1].gameObject);
        _allyPlaceHolders.RemoveAt(_allyPlaceHolders.Count - 1);
        
        _settings.playerAvatarUI.UpdateUI(_speed,_head.GetStat());
        
        _signalBus.Fire(new PlayerHeadChangedSignal(_head));
        _signalBus.Fire(new EncounterSignal(enemyAvatarId));
    }
    public void AddHero(Avatar newAllyData, AvatarPlaceHolder newAllyPlaceHolder)
    {
        var changedInstance = newAllyPlaceHolder;
        changedInstance.Init(newAllyPlaceHolder.instanceId, newAllyData.sprite, new AvatarTag("Ally"));
        _allyPlaceHolders.Add(changedInstance);
        _avatars.Add(newAllyData);
        changedInstance.gameObject.name = $"player_ally_{_allyPlaceHolders.Count}";
    }

    private void Swap(float value)
    {
        if (value == 0f)
            return;

        _isSwapping = true;

        if (value > 0)
        {
            var newList = new List<Avatar>();
            var cacheHead = _head;
            
            _head = _avatars[0];
            _head.TransferAvatar(_headPlaceholder.instanceId,_headPlaceholder.Init(_headPlaceholder.instanceId, _head.sprite, new AvatarTag("Player")));
            
            for (int i = 1; i < _avatars.Count; i++)
                newList.Add(_avatars[i]);
            
            newList.Add(cacheHead);

            for (int i = 0; i < newList.Count; i++)
            {
                newList[i].TransferAvatar
                (_allyPlaceHolders[i].instanceId, 
                    _allyPlaceHolders[i].Init(_allyPlaceHolders[i].instanceId, newList[i].sprite, new AvatarTag("Ally")));
            }
            
            _avatars = newList;

        }
        else if (value < 0)
        {
            var newList = new List<Avatar>();
            var cacheHead = _head;
            
            if (_qSwapCache > _avatars.Count - 1)
            {
                _head = _avatars[_avatars.Count - 1];
                _head.TransferAvatar(_headPlaceholder.instanceId,_headPlaceholder.Init(_headPlaceholder.instanceId, _head.sprite, new AvatarTag("Player")));

                _avatars[_avatars.Count - 1] = cacheHead;
                _avatars[_avatars.Count - 1].TransferAvatar(
                    _allyPlaceHolders[_avatars.Count - 1].instanceId, 
                    _allyPlaceHolders[_avatars.Count - 1].Init(
                        _allyPlaceHolders[_avatars.Count - 1].instanceId, _avatars[_avatars.Count - 1].sprite, new AvatarTag("Ally")));
                
                _qSwapCache = 0;
            }
            else
            {
                newList.Add(cacheHead);

                for (int i = 0; i < _avatars.Count; i++)
                {
                    if (i == _qSwapCache)
                        continue;
                
                    newList.Add(_avatars[i]);
                }
            
                _head = _avatars[_qSwapCache];
                _head.TransferAvatar(_headPlaceholder.instanceId,_headPlaceholder.Init(_headPlaceholder.instanceId, _head.sprite, new AvatarTag("Player")));

            
                for (int i = 0; i < newList.Count; i++)
                {
                    newList[i].TransferAvatar
                    (_allyPlaceHolders[i].instanceId, 
                        _allyPlaceHolders[i].Init(_allyPlaceHolders[i].instanceId, newList[i].sprite, new AvatarTag("Ally")));
                }
            
                _avatars = newList;

                _qSwapCache++;
            }
        }

        _settings.playerAvatarUI.UpdateUI(_speed,_head.GetStat());
        _signalBus.Fire(new PlayerHeadChangedSignal(_head));
        _isSwapping = false;
    }
    private void ReadSwapValue(InputAction.CallbackContext context)
    {
        if (_avatars.Count <= 0)
            return;
        
        if (_impairedControl)
            return;
        
        if (_head.IsOnCombat)
            return;
        
        var value = context.ReadValue<float>();
        Swap(value);
    }
    private void ReadMoveValue(InputAction.CallbackContext context)
    {
        if (_impairedControl)
            return;
        
        var value = context.ReadValue<Vector2>().normalized;
        
        if (value.x != 0 && value.y != 0)
            return;

        _lastValue = value;
        _settings.indicatorProcessor.ProcessMovementIndicator(_lastValue);
    }
    public void Initialize(CharacterData data)
    {
        _moveInputWorker.Initialize(ReadMoveValue);
        _swapInputWorker.Initialize(ReadSwapValue);
        var headPlaceholder = Object.Instantiate(_settings.avatarPlaceHolderPrefab, _settings.playerStartPoint.transform.position, Quaternion.identity);
        var instanceId = NanoId.Generate(8);
        var tag = new AvatarTag("Player");
        _head = new Avatar(instanceId, headPlaceholder.Init(instanceId, data.sprite, tag), data);
        _headPlaceholder = headPlaceholder;
        _isInitialized = true;
        _speed = data.speed;
        _settings.playerAvatarUI.UpdateUI(_speed, _head.GetStat());
        _signalBus.Fire(new PlayerHeadChangedSignal(_head));
    }
    public void Dispose()
    {
        _isInitialized = false;
        _moveInputWorker.Dispose();
        _qSwapCache = 0;
    }
    public void Tick()
    {
        if (!_isInitialized)
            return;

        if (_head.IsOnCombat)
            return;
        
        if (_head.IsDead)
            return;
        
        if (_moveRuleWorker.IsMovable(_head.currentDir, _lastValue, _head, _avatars.ToArray()))
            _head.Move(_speed, _lastValue);
        else
            _head.Move(_speed, _head.currentDir);
        
        _signalBus.Fire(new UpdatePlayerPositionSignal(_head.position));

        if (_avatars.Count > 0)
        {
            for (int i = 0; i < _avatars.Count; i++)
            {
                if (_isSwapping)
                    return;
                
                var currentAvatar = _avatars[i];
                var prevAvatar = i == 0 ? _head : _avatars[i - 1];
                
                Vector3 newPos = prevAvatar.position;
                Vector3 pos = currentAvatar.position;

                pos.x = Mathf.Lerp(pos.x, newPos.x, Time.deltaTime * (_speed * 2));
                pos.y = Mathf.Lerp(pos.y, newPos.y, Time.deltaTime * (_speed * 2));

                currentAvatar.currentDir = _head.currentDir;
                currentAvatar.SetPos(pos);
            }
        }
    }
}