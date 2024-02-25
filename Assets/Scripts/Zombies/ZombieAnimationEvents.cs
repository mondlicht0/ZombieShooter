using UnityEngine;

public class ZombieAnimationEvents : MonoBehaviour
{
    private Zombie _zombie;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
    }

    public void StartLeftDealDamage()
    {
        Debug.Log("dfdf");
        _zombie.LeftDealer.StartDealDamage();
    }

    public void EndLeftDealDamage()
    {
        _zombie.LeftDealer.EndDealDamage();
    }

    public void StartRightDealDamage()
    {
        _zombie.RightDealer.StartDealDamage();
    }

    public void EndRightDealDamage()
    {
        _zombie.RightDealer.EndDealDamage();
    }
}
