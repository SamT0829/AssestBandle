using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class AssetBundleHandler : MonoBehaviour
{
    public void InstantiateAssetBundle(AssetReference assetReference, Action<GameObject> onInstanteFinish)
    {
        StartCoroutine(OnInstantiateAssestBundle(assetReference, onInstanteFinish));
    }

    public void LoadAssetBundle<TObject>(AssetReference assetReference, Action<TObject> onLoadFinish)
    {
        StartCoroutine(OnLoadAssestBundle(assetReference, onLoadFinish));
    }

    public void LoadAssetScene(AssetReference assetReference, Action onLoadSceneFinish)
    {
        StartCoroutine(OnLoadAssetScene(assetReference, onLoadSceneFinish));
    }

    public void DownloadAssetScene(AssetReference assetReference, Action onDownloadAssetFinish)
    {
        StartCoroutine(OnDownloadAssetScene(assetReference, onDownloadAssetFinish));
    }

    public void GetDownloadSize(AssetReference assetReference)
    {
        Addressables.GetDownloadSizeAsync(assetReference.RuntimeKey);
    }

    public void ClearAssestOjb(AssetReference addressNameStr)
    {
        Addressables.ClearDependencyCacheAsync(addressNameStr.RuntimeKey);
    }

    // Instantiate
    private IEnumerator OnInstantiateAssestBundle(AssetReference assetReference, Action<GameObject> onInstanteFinish)
    {
        var handle = assetReference.InstantiateAsync();
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

    // Load object
    private IEnumerator OnLoadAssestBundle<TObject>(AssetReference assetReference, Action<TObject> onLoadFinish)
    {
        var handle = Addressables.LoadAssetAsync<TObject>(assetReference);
        handle.Completed += e => OnLoadAssestObjLoaded(e, onLoadFinish);

        while (!handle.IsDone)
        {
            var status = handle.GetDownloadStatus();
            float progress = status.Percent;
            Debug.Log(progress);
            yield return null;
        }
    }
    private void OnLoadAssestObjLoaded<TObject>(AsyncOperationHandle<TObject> handle, Action<TObject> onLoadFinish)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            onLoadFinish.Invoke(handle.Result);
        }
        else
            Debug.LogError("Loading Asset Failed");
    }

    // Scene
    private IEnumerator OnLoadAssetScene(AssetReference assetReference, Action onLoadSceneFinish)
    {
        var handle = Addressables.LoadSceneAsync(assetReference.RuntimeKey, LoadSceneMode.Additive);
        handle.Completed += e => OnLoadSceneLoaded(e, onLoadSceneFinish);

        while (!handle.IsDone)
        {
            var status = handle.GetDownloadStatus();
            float progress = status.Percent;
            Debug.Log(progress);
            yield return null;
        }
    }

    private void OnLoadSceneLoaded(AsyncOperationHandle<SceneInstance> handle, Action onLoadSceneFinish)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            onLoadSceneFinish.Invoke();
        }
        else
            Debug.LogError("Loading Asset Failed");
    }

    private IEnumerator OnDownloadAssetScene(AssetReference assetReference, Action onDownloadAssetFinish)
    {
        var handle = Addressables.DownloadDependenciesAsync(assetReference.RuntimeKey);
        handle.Completed += e => OnDownloadSceneComplete(e, onDownloadAssetFinish);

        while (!handle.IsDone)
        {
            var status = handle.GetDownloadStatus();
            float progress = status.Percent;
            Debug.Log(progress);
            yield return null;
        }
    }

    private void OnDownloadSceneComplete(AsyncOperationHandle handle, Action onInstanteFinish)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            onInstanteFinish.Invoke();
        }
        else
            Debug.LogError("Loading Asset Failed");
    }
}
