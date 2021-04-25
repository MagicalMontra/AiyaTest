using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class GameBoardController : IDisposable
{
    [Inject] private SignalBus _signalBus;
    [Inject] private GameplaySettings _settings;
    [Inject] private GameAreaSpawner _areaSpawner;
    
    private List<Transform> _spawnPoints = new List<Transform>();
    private List<Transform> _occupiedPoints = new List<Transform>();
    private Vector3 _playerPos;
    private bool _isSpawning;

    public void GetPlayerPosition(UpdatePlayerPositionSignal signal)
    {
        _playerPos = signal.position;
    }
    public void OnAllyAcquire(HeroAllyAcquireSignal signal)
    {
        var index = _occupiedPoints.FindIndex(op => Vector2.Distance(signal.placeholder.transform.position, op.position) < 1f);
        _spawnPoints.Add(_occupiedPoints[index]);
        _occupiedPoints.RemoveAt(index);
    }
    public void OnEnemyKilled(EnemyKilledSignal signal)
    {
        var index = _occupiedPoints.FindIndex(op => Vector2.Distance(signal.enemyKilled.position, op.position) < 1f);
        _spawnPoints.Add(_occupiedPoints[index]);
        _occupiedPoints.RemoveAt(index);
    }
    public void BoardSetup()
    {
        for (int i = 0; i < 4; i++)
        {
            var transform = new GameObject().transform;
            transform.position = _areaSpawner.RandomPointInArea(_settings.gameBoardArea.position, _settings.gameBoardArea.localScale);
            _spawnPoints.Add(transform);
        }

        Spawn();
    }
    public void BoardCancel()
    {
        _isSpawning = false;
    }
    private async void Spawn()
    {
        _isSpawning = true;
        var enemyIncreaseThreshold = 0;
        var heroDecreaseThreshold = 75;
        while (_isSpawning)
        {
            if (_spawnPoints.Count <= 0)
            {
                await Task.Delay(100);
                continue;
            }
            
            var index = Random.Range(0, _spawnPoints.Count);
            var compareIndex = index;

            while (index == compareIndex && _spawnPoints.All(sp => sp != null))
            {
                compareIndex = _spawnPoints.FindIndex(sp => Vector2.Distance(_playerPos, sp.position) < 4f);
                await Task.Delay(10);
            }
            
            var randomAvatar = Random.Range(0, _settings.heroPool.Count);
            var heroChance = Random.Range(heroDecreaseThreshold, 100);
            var enemyChance = Random.Range(enemyIncreaseThreshold, 100);
            _occupiedPoints.Add(_spawnPoints[index]);

            if (enemyChance >= heroChance)
            {
                if (enemyIncreaseThreshold < 75)
                    enemyIncreaseThreshold += 3;
                
                _signalBus.Fire(new EnemySpawnSignal(_settings.heroPool[randomAvatar], _spawnPoints[index].position));
            }
            else
            {
                if (heroDecreaseThreshold > 0)
                    heroDecreaseThreshold -= 15;
                
                _signalBus.Fire(new HeroSpawnSignal(_settings.heroPool[randomAvatar], _spawnPoints[index].position));
            }
            
            _spawnPoints.RemoveAt(index);

            await Task.Delay(Random.Range(2, 5) * 1000);
        }
    }

    public void Dispose()
    {
        _isSpawning = false;
    }
}