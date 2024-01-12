using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Transform> m_blueTeamTowerTransform;
    [SerializeField] private AssetReference m_blueCastleAsset;
    [SerializeField] private AssetReference m_blueTowereAsset;

    [SerializeField] private GameObject BlueCastle;
    [SerializeField] private GameObject BlueTower;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        GameManager.Instance.AssetBundleHandler.InstantiateAssetBundle(m_blueCastleAsset, OnBlueCastleInstantiateComplete);
        GameManager.Instance.AssetBundleHandler.InstantiateAssetBundle(m_blueTowereAsset, OnBlueTowerInstantiateComplete);
    }
    private void OnBlueCastleInstantiateComplete(GameObject @object)
    {
        BlueTower = @object;
        BlueTower.transform.position = m_blueTeamTowerTransform[0].transform.position;
    }
    private void OnBlueTowerInstantiateComplete(GameObject @object)
    {
        BlueCastle = @object;
        BlueCastle.transform.position = m_blueTeamTowerTransform[1].transform.position;
    }
}
