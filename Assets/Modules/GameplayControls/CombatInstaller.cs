using UnityEngine;
using Zenject;

public class CombatInstaller : MonoInstaller<CombatInstaller>
{
    [SerializeField] private CombatSettings _settings;
    public override void InstallBindings()
    {
        Container.Bind<CombatProcessor>().AsSingle();
        Container.Bind<CombatSettings>().FromInstance(_settings).AsSingle();

        Container.DeclareSignal<CombatStartSignal>();
        Container.BindSignal<CombatStartSignal>().ToMethod<CombatProcessor>(c => c.StartCombat).FromResolve();
        Container.BindSignal<PlayerHeadChangedSignal>().ToMethod<CombatProcessor>(c => c.OnHeadChanged).FromResolve();
    }
}