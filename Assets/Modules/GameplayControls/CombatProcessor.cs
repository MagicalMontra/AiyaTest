using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class CombatProcessor
{
    [Inject] private SignalBus _signalBus;
    [Inject] private CombatSettings _settings;
    
    private Avatar _attacker;
    private Avatar _defender;
    
    private Avatar _playerHeadCache;

    private AvatarHealthUI _attackerHealthbar;
    private AvatarHealthUI _defenderHealthbar;

    public async void StartCombat(CombatStartSignal signal)
    {
        Avatar cacheAvatar;
        AvatarHealthUI cacheHealthBar;
        _defender = signal.data;
        _attacker.IsOnCombat = _defender.IsOnCombat = true;
        
        if (_attackerHealthbar == null)
            _attackerHealthbar = Object.Instantiate(_settings.healthbarPrefab, _settings.hpCanvas);
        else
            _attackerHealthbar.gameObject.SetActive(true);
        
        Vector3 ViewportPosition = Camera.main.WorldToScreenPoint(new Vector3(_attacker.position.x, _attacker.position.y + 0.4f, 1));
        _attackerHealthbar.transform.position = ViewportPosition;

        if (_defenderHealthbar == null)
            _defenderHealthbar = Object.Instantiate(_settings.healthbarPrefab, _settings.hpCanvas);

        else
            _defenderHealthbar.gameObject.SetActive(true);
        
        ViewportPosition = Camera.main.WorldToScreenPoint(new Vector3(_defender.position.x, _defender.position.y + 0.4f, 1));
        _defenderHealthbar.transform.position = ViewportPosition;
        
        while (!_attacker.IsDead && !_defender.IsDead)
        {
            var attackDmg = _attacker.Attack(_defender.Type);
            _defender.Modify(attackDmg);
            _attackerHealthbar.UpdateHealthText(_attacker.CurrentHp, _attacker.MaxHp);
            _defenderHealthbar.UpdateHealthText(_defender.CurrentHp, _defender.MaxHp);

            cacheAvatar = _defender;
            _defender = _attacker;
            _attacker = cacheAvatar;

            cacheHealthBar = _defenderHealthbar;
            _defenderHealthbar = _attackerHealthbar;
            _attackerHealthbar = cacheHealthBar;
            
            await Task.Delay(500);
        }
        
        _attackerHealthbar.gameObject.SetActive(false);
        _defenderHealthbar.gameObject.SetActive(false);
        
        _attacker.IsOnCombat = _defender.IsOnCombat = false;
        _attacker = _playerHeadCache;
        
        if (_playerHeadCache.IsDead)
            _signalBus.Fire(new SwapHeroSignal(signal.data.instanceId));
        else
            _signalBus.Fire(new EnemyKilledSignal(signal.data));
    }
    public void OnHeadChanged(PlayerHeadChangedSignal changedSignal)
    {
        _attacker = changedSignal.head;
        _playerHeadCache = changedSignal.head;
    }
}

[Serializable]
public class CombatSettings
{
    public Transform hpCanvas;
    public AvatarHealthUI healthbarPrefab;
}

public class SwapHeroSignal
{
    public string enemyAvatarId => _enemyAvatarId;
    private string _enemyAvatarId;

    public SwapHeroSignal(string enemyAvatarId)
    {
        _enemyAvatarId = enemyAvatarId;
    }
}