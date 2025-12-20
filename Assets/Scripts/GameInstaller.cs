using System.ComponentModel;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameConfig _gameConfig;

    public override void InstallBindings()
    {
        // Bind config as a singleton so it can be injected anywhere
        Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
    }
}
