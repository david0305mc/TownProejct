using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class IAPManager : MonoBehaviour, IStoreListener
{

    private void Start()
    {
        InitStore();
    }

    void InitStore()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        //builder.AddProduct("test pid", ProductType.Consumable, new IDs { { "a_jewel10", GooglePlay.Name } });
        builder.AddProduct("test pid", ProductType.Consumable);

        // √ ±‚»≠
        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log("ProcessPurchase");
        return PurchaseProcessingResult.Pending;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("OnPurchaseFailed");
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized");
        var googleExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();
        foreach (UnityEngine.Purchasing.Product item in controller.products.all)
        {
            Debug.Log($"item.definition.id {item.definition.id}");
            Debug.Log($"item.definition.storeSpecificId {item.definition.storeSpecificId}");
        }
    }
}
