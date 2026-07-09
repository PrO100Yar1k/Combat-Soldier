using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private List<MonoBehaviour> _managersOnScene = new List<MonoBehaviour>();

    public override void InstallBindings()
    {
        Container.Bind<CoroutineStarter>()
            .FromNewComponentOnNewGameObject()
            .AsSingle();

        Container.Bind<TroopModelManager>().AsSingle();

        Container.Bind<RepositoryManager>().AsSingle();
        Container.Bind<GameEvents>().AsSingle();

        InitializeAllSceneManagers();
    }

    private void InitializeAllSceneManagers()
    {
        foreach (var manager in _managersOnScene)
        {
            if (manager is IInitializable)
            {
                Container.Bind<IInitializable>()
                    .FromInstance(manager as IInitializable)
                    .AsCached();
            }

            Container.Bind(manager.GetType())
                .FromInstance(manager)
                .AsSingle();
        }
    }
}