using Zenject;

public class GameplayController
{
    [Inject] private GameplaySettings _settings;
    [Inject] private GameModeHandler _gameModeHandler;
    [Inject] private GameBoardController _gameBoardController;

    private bool _isPlaying;
    private GameplayData _gameplayData;
    
    public void OnAllyAcquire(HeroAllyAcquireSignal signal)
    {
        _gameplayData.heroCount += 1;
    }
    public void OnEnemyKilled(EnemyKilledSignal signal)
    {
        _gameplayData.killCount += 1;
    }
    public void OnGameModeSelect(GameMode mode)
    {
        _settings.selectionPanel.SetActive(false);
        _gameModeHandler.HandleGameMode(mode.name);
        _gameBoardController.BoardSetup();
        _gameplayData = new GameplayData();
    }
    public void OnGameOver(GameOverSignal signal)
    {
        _gameBoardController.BoardCancel();
        _settings.gameOverPanel.GameOver(_gameplayData);
    }
}