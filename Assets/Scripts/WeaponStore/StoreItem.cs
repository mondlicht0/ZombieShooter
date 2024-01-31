using UnityEngine;

public abstract class StoreItem : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
    public int Price;

    protected abstract void Purchase();

    public void PurchaseItem()
    {
        if (PlayerData.Instance.Money >= Price)
        {
            if (FindObjectOfType<MoneyDisplayer>() != null)
            {
                PlayerData.Instance.Money -= Price;

                FindObjectOfType<MoneyDisplayer>().UpdateMoneyText(PlayerData.Instance.Money);
                Purchase();
            }
        }

    }
}