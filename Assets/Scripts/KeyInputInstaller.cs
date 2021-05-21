using UnityEngine;
using Zenject;

public class KeyInputInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<IKeyInput>()
            .To<KeyInput>()
            .FromComponentInHierarchy()
            .AsCached();
    }
}