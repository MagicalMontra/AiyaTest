using UnityEngine;
using Zenject;

public class GameModeHandler
{
    [Inject] private SignalBus _signalBus;
    [Inject] private GameplaySettings _settings;
    
    public void HandleGameMode(string modeName)
    {
        switch (modeName)
        {
            case "Classic":
            {
                _signalBus.Fire(new GameStartSignal(_settings.defaultPlayer));
                break;
            }
            case "Random":
            {
                var random = Random.Range(0, _settings.heroPool.Count);
                _signalBus.Fire(new GameStartSignal(_settings.heroPool[random]));
                break;
            }
        }
    }
}

public class GameStartSignal
{
    public CharacterData data => _data;
    private CharacterData _data;

    public GameStartSignal(CharacterData data)
    {
        _data = data;
    }
}