using System;
using System.Threading.Tasks;
using com.appidea.MiniGamePlatform.CommunicationAPI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MatchColorFrogsEntryPoint : BaseMiniGameEntryPoint, IMiniGameLoadingProgressHandler
{
    private AsyncOperationHandle<GameObject> handle;
    [SerializeField] private GameObject gameObject;
    
    protected override Task LoadInternal()
    {
        handle = Addressables.LoadAssetAsync<GameObject>("EntryPoint");
        Instantiate(gameObject);
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
