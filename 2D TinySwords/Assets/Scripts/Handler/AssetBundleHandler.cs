using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetBundleHandler : MonoBehaviour
{
    public void InstantiateAssestBundle(AssetReference addressNameStr, Action<GameObject> onInstanteFinish)
    {
        StartCoroutine(OnInstantiateAssestBundle(addressNameStr, onInstanteFinish));
    }

    public void LoadAssestBundle<TObject>(AssetReference addressNameStr, Action<TObject> onInstanteFinish)
    {
        StartCoroutine(OnLoadAssestBundle<TObject>(addressNameStr, onInstanteFinish));
    }

    public void ClearAssestOjb(AssetReference addressNameStr)
    {
        Addressables.ClearDependencyCacheAsync(addressNameStr.RuntimeKey);
    }

    // Instantiate
    private IEnumerator OnInstantiateAssestBundle(AssetReference addressNameStr, Action<GameObject> onInstanteFinish)
    {
        var handle = addressNameStr.InstantiateAsync();
        handle.Completed += e => OnInstanceAssestObjLoaded(e, onInstanteFinish);

        while (!handle.IsDone)
        {
            var status = handle.GetDownloadStatus();
            float progress = status.Percent;
            Debug.Log(progress);
            yield return null;
        }
    }

    private void OnInstanceAssestObjLoaded(AsyncOperationHandle<GameObject> handle, Action<GameObject> onInstanteFinish)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            onInstanteFinish.Invoke(handle.Result);
        }
        else
            Debug.LogError("Instance Asset Failed");
    }

    // Load
    private IEnumerator OnLoadAssestBundle<TObject>(AssetReference addressNameStr, Action<TObject> onInstanteFinish)
    {
        var handle = Addressables.LoadAssetAsync<TObject>(addressNameStr);
        handle.Completed += e => OnLoadAssestObjLoaded(e, onInstanteFinish);

        while (!handle.IsDone)
        {
            var status = handle.GetDownloadStatus();
            float progress = status.Percent;
            Debug.Log(progress);
            yield return null;
        }
    }
    private void OnLoadAssestObjLoaded<TObject>(AsyncOperationHandle<TObject> handle, Action<TObject> onInstanteFinish)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            onInstanteFinish.Invoke(handle.Result);
        }
        else
            Debug.LogError("Loading Asset Failed");
    }
}
