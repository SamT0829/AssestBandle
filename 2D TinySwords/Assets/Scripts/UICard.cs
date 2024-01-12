using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    [SerializeField] private Button m_button;
    [SerializeField] private Image m_image;
    [SerializeField] private AssetReference m_assetReference;
    [SerializeField] GameObject UICardObject;
    [SerializeField] Transform m_spawnPosition;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Addressables.ClearDependencyCacheAsync(m_assetReference.RuntimeKey);

        GameManager.Instance.AssetBundleHandler.LoadAssetBundle<GameObject>(m_assetReference, OnLoadFinish);
        m_button.onClick.AddListener(OnUICardClick);
    }
    private void OnLoadFinish(GameObject @object)
    {
        m_image.sprite = @object.GetComponent<SpriteRenderer>().sprite;
    }

    private void OnUICardClick()
    {
        GameManager.Instance.AssetBundleHandler.InstantiateAssetBundle(m_assetReference, OnInstanteFinish);
    }

    private void OnInstanteFinish(GameObject @object)
    {
        if (GameManager.Instance.changeObject != null)
            Destroy(GameManager.Instance.changeObject);

        GameManager.Instance.changeObject = @object;
        GameManager.Instance.changeObject.GetComponent<Transform>().position = m_spawnPosition.position;
    }
}
