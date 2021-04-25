using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller<PlayerInstaller>
{
    [SerializeField] private PlayerSettings _settings;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerAvatarWorker>().AsSingle();
        Container.Bind<PlayerMoveRuleWorker>().AsSingle();
        Container.Bind<PlayerMoveInputWorker>().AsSingle();
        Container.Bind<PlayerSwapInputWorker>().AsSingle();
        Container.Bind<PlayerControls>().AsSingle();
        Container.Bind<PlayerSettings>().FromInstance(_settings).AsSingle();

        Container.DeclareSignal<HeroAllyAcquireSignal>();
        Container.DeclareSignal<PlayerHeadChangedSignal>();
        Container.DeclareSignal<UpdatePlayerPositionSignal>();
        Container.DeclareSignal<SwapHeroSignal>();
        Container.DeclareSignal<WallHitSignal>();
        
        Container.BindSignal<GameStartSignal>().ToMethod<PlayerController>(c => c.OnGameStart).FromResolve();
        Container.BindSignal<HeroAllyAcquireSignal>().ToMethod<PlayerController>(c => c.OnGetAlly).FromResolve();
        Container.BindSignal<SwapHeroSignal>().ToMethod<PlayerController>(c => c.OnSwapRequested).FromResolve();
        Container.BindSignal<EnemyKilledSignal>().ToMethod<PlayerAvatarWorker>(w => w.IncreaseSpeed).FromResolve();
        Container.BindSignal<WallHitSignal>().ToMethod<PlayerAvatarWorker>(w => w.OnWallHit).FromResolve();
    }
}