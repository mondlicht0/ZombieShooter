using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStore : Interactable
{
    [SerializeField] private List<WeaponStoreItem> _weaponItems = new List<WeaponStoreItem>();
    [SerializeField] private List<AmmoStoreItem> _ammoItems = new List<AmmoStoreItem>();
    [SerializeField] private List<ResourceStoreItem> _resourceItems = new List<ResourceStoreItem>();

    [SerializeField] private Transform _storeMenu;
    [SerializeField] private Transform _storeItemTemplate;

    [Header("Shop Categories")]
    [SerializeField] private Transform _weapons;
    [SerializeField] private Transform _resources;
    [SerializeField] private Transform _ammo;

    [Header("Inventory")]
    [SerializeField] private Transform _inventory;
    [SerializeField] private Player _player;

    private void Start()
    {
        _player = FindObjectOfType<Player>();

        InitItems();
    }

    protected override void Interact()
    {
        InitAmmoItems();
        ShowStore();
    }

    private void InitItems()
    {
        InitAmmoItems();

        for (int i = 0; i < _weaponItems.Count; i++)
        {
            CreateStoreItem(_weaponItems[i], _weapons, i);
        }

        for (int i = 0; i < _resourceItems.Count; i++)
        {
            CreateStoreItem(_resourceItems[i], _resources, i);
        }

        //_weaponItems.ForEach(item => { CreateStoreItem(item, _weapons, _weaponItems.IndexOf(item) + 1); });

        //_resourceItems.ForEach(item => { CreateStoreItem(item, _resources, _resourceItems.IndexOf(item) + 1); });

        //_ammoItems.ForEach(item => { CreateStoreItem(item, _ammo, _ammoItems.IndexOf(item) + 1); });
    }

    private void InitAmmoItems()
    {
        InitAmmoStoreItems();

        for (int i = 0; i < _ammoItems.Count; i++)
        {
            CreateStoreItem(_ammoItems[i], _ammo, i);
        }

    }

    private void InitAmmoStoreItems()
    {
        _ammoItems.Clear();

        foreach (var item in _player.GunSelector.GunsSlots)
        {
            AmmoStoreItem ammoItem = new AmmoStoreItem();
            ammoItem.Gun = item;
            ammoItem.Name = item.Name;
            ammoItem.Price = item.Price;
            ammoItem.AmountOfAmmo = 20;
            _ammoItems.Add(ammoItem);
        }
    }

    private void ShowStore()
    {
        _player.Look.LockOnStore();
        ShowInventory();

        _storeMenu.gameObject.SetActive(true);
    }

    public void HideStore()
    {
        _player.Look.LockOffStore();
        _storeMenu.gameObject.SetActive(false);
    }

    private void CreateStoreItem(StoreItem item, Transform category, int positionIndex)
    {
        Transform newStoreItem = Instantiate(_storeItemTemplate, category);
        RectTransform newStoreItemRectTransform = newStoreItem.GetComponent<RectTransform>();

        float storeItemHeight = 200f;
        newStoreItemRectTransform.anchoredPosition = new Vector2(0, -storeItemHeight * positionIndex);

        newStoreItem.Find("Name").GetComponent<TextMeshProUGUI>().SetText(item.Name);
        newStoreItem.Find("Price").GetComponent<TextMeshProUGUI>().SetText(item.Price.ToString());
        newStoreItem.GetComponent<Image>().sprite = item.Sprite;
        newStoreItem.GetComponent<Button>().onClick.AddListener(item.PurchaseItem);
    }

    private void CreateInventoryItem(int positionIndex)
    {
        Transform newStoreItem = Instantiate(_storeItemTemplate, _inventory);
        RectTransform newStoreItemRectTransform = newStoreItem.GetComponent<RectTransform>();

        float storeItemHeight = 30f;
        newStoreItemRectTransform.anchoredPosition = new Vector2(0, -storeItemHeight * positionIndex);
    }

    private void ShowInventory()
    {
        int i = 1;

        foreach (var item in _player.GunSelector.GunsSlots)
        {
            CreateInventoryItem(i);
            i++;
        }
    }
}
