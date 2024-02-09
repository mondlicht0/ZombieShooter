using UnityEngine;
using UnityHFSM;

public class ZombieTransition : TransitionBase
{
    public ZombieTransition(string from, string to, bool forceInstantly = false) : base(from, to, forceInstantly)
    {
    }

    public override bool ShouldTransition() { return true; }

    public override void Init() { }

    public override void BeforeTransition() { }
    public override void AfterTransition() { }
}
