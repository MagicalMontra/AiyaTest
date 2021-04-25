using UnityEngine;
using Zenject;

namespace Gamespace.Project
{
    [CreateAssetMenu(menuName = "Create ProjectInstaller", fileName = "ProjectInstaller", order = 0)]
    public class ProjectInstaller : ScriptableObjectInstaller<ProjectInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
        }
    }
}