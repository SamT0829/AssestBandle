using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Button m_gameStartButton;
    [SerializeField] AssetReference gameSceneAsset;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        m_gameStartButton.onClick.AddListener(OnGameStartClick);
    }

    private void OnGameStartClick()
    {
        GameManager.Instance.AssetBundleHandler.DownloadAssetScene(gameSceneAsset, OnDownloadAssetFinish);
    }

    private void OnDownloadAssetFinish()
    {
        GameManager.Instance.AssetBundleHandler.LoadAssetScene(gameSceneAsset, OnLoadGameSceneFinish);
    }

    private void OnLoadGameSceneFinish()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        Debug.Log("Game Scene Loaded");
    }
}
