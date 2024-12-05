using System;
using System.Threading.Tasks;
using com.appidea.MiniGamePlatform.CommunicationAPI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MatchColorFrogsEntryPoint : BaseMiniGameEntryPoint, IMiniGameLoadingProgressHandler
{
    private AsyncOperationHandle<GameObject> handle;
    
    protected override Task LoadInternal()
    {
        return Task.CompletedTask;
    }

    protected override Task UnloadInternal()
    {
        Addressables.Release(handle);
        return Task.CompletedTask;
    }

    public float Progress { get; private set; }
    public event Action<float> ProgressChanged;
}
