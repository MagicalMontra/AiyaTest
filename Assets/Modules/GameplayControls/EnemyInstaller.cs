using UnityEngine;
using Zenject;

public class EnemyInstaller : MonoInstaller<EnemyInstaller>
{
    [SerializeField] private EnemySettings _settings;
    public override void InstallBindings()
    {
        Container.Bind<EnemyController>().AsSingle();
        Container.Bind<EnemySettings>().FromInstance(_settings).AsSingle();

        Container.DeclareSignal<EnemySpawnSignal>();
        Container.DeclareSignal<EnemyKilledSignal>();

        Container.BindSignal<EnemySpawnSignal>().ToMethod<EnemyController>(c => c.SpawnEnemy).FromResolve();
        Container.BindSignal<EncounterSignal>().ToMethod<EnemyController>(c => c.OnEncounter).FromResolve();
        Container.BindSignal<EnemyKilledSignal>().ToMethod<EnemyController>(c => c.OnEnemyKilled).FromResolve();
    }
}