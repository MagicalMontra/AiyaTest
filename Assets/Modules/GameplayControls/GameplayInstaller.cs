using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller<GameplayInstaller>
{
    [SerializeField] private GameplaySettings _settings;
    public override void InstallBindings()
    {
        Container.Bind<GameplayController>().AsSingle();
        Container.Bind<GameModeHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameBoardController>().AsSingle();
        Container.Bind<GameAreaSpawner>().AsSingle();
        Container.Bind<GameplaySettings>().FromInstance(_settings).AsSingle();

        Container.DeclareSignal<GameMode>();
        Container.DeclareSignal<GameStartSignal>();
        Container.DeclareSignal<GameOverSignal>();
        Container.DeclareSignal<EncounterSignal>();

        Container.BindSignal<GameMode>().ToMethod<GameplayController>(c => c.OnGameModeSelect).FromResolve();
        Container.BindSignal<GameOverSignal>().ToMethod<GameplayController>(c => c.OnGameOver).FromResolve();
        
        Container.BindSignal<HeroAllyAcquireSignal>().ToMethod<GameplayController>(c => c.OnAllyAcquire).FromResolve();
        Container.BindSignal<HeroAllyAcquireSignal>().ToMethod<GameBoardController>(c => c.OnAllyAcquire).FromResolve();
        
        Container.BindSignal<UpdatePlayerPositionSignal>().ToMethod<GameBoardController>(c => c.GetPlayerPosition).FromResolve();
        
        Container.BindSignal<EnemyKilledSignal>().ToMethod<GameplayController>(c => c.OnEnemyKilled).FromResolve();
        Container.BindSignal<EnemyKilledSignal>().ToMethod<GameBoardController>(c => c.OnEnemyKilled).FromResolve();
    }
}