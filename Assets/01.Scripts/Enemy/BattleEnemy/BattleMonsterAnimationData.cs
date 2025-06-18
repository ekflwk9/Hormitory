using System;
using UnityEngine;

[Serializable]
public class BattleMonsterAnimationData
{
    [SerializeField] private string deathParameterName = "Death";
    [SerializeField] private string flyingLandingParameterName = "FlyingLanding";
    [SerializeField] private string flyingRoarParameterName = "FlyingRoar";
    [SerializeField] private string biteAttackParameterName = "BiteAttack";
    [SerializeField] private string flyParameterName = "Fly";
    [SerializeField] private string takeOffParameterName = "TakeOff";
    [SerializeField] private string roarParameterName = "Roar";
    [SerializeField] private string groggyParameterName = "Groggy";

    public int DeathParameterHash { get; private set; }
    public int FlyingLandingParameterHash { get; private set; }
    public int FlyingRoarParameterHash { get; private set; }
    public int BiteAttackParameterHash { get; private set; }
    public int FlyParameterHash { get; private set; }
    public int TakeOffParameterHash { get; private set; }
    public int RoarParameterHash { get; private set; }
    public int GroggyParameterHash { get; private set; }

    public void Initialize()
    {
        DeathParameterHash = Animator.StringToHash(deathParameterName);
        FlyingLandingParameterHash = Animator.StringToHash(flyingLandingParameterName);
        FlyingRoarParameterHash = Animator.StringToHash(flyingRoarParameterName);
        BiteAttackParameterHash = Animator.StringToHash(biteAttackParameterName);
        FlyParameterHash = Animator.StringToHash(flyParameterName);
        TakeOffParameterHash = Animator.StringToHash(takeOffParameterName);
        RoarParameterHash = Animator.StringToHash(roarParameterName);
        GroggyParameterHash = Animator.StringToHash(groggyParameterName);
    }
}