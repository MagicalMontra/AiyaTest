using UnityEngine;
using Zenject;

public class HeroInstaller : MonoInstaller<HeroInstaller>
{
    [SerializeField] private HeroSettings _settings;
    public override void InstallBindings()
    {
        Container.Bind<HeroController>().AsSingle();
        Container.Bind<HeroSettings>().FromInstance(_settings).AsSingle();

        Container.DeclareSignal<HeroSpawnSignal>();

        Container.BindSignal<HeroSpawnSignal>().ToMethod<HeroController>(c => c.SpawnHero).FromResolve();
        Container.BindSignal<EncounterSignal>().ToMethod<HeroController>(c => c.OnEncounter).FromResolve();
    }
}