using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class PlayerIK : MonoBehaviour
{
    [SerializeField] private Transform _rightHand;
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _weaponPivot;
    [SerializeField] private MultiParentConstraint _parentConstraint;

    /*    public Transform LeftHandIKTarget;
        public Transform RightHandIKTarget;
        public Transform LeftElbowIKTarget;
        public Transform RightElbowIKTarget;*/

    [Range(0, 1f)]
    public float HandIKAmount = 1f;
    [Range(0, 1f)]
    public float ElbowIKAmount = 1f;

    public AvatarIKGoal RightIKGoal;
    public AvatarIKGoal LeftIKGoal;
    public Animator animator;
    private Transform IKTarget;


    public SO_Gun ActiveGun;

    private void Start()
    {
        ActiveGun = GetComponent<PlayerGunSelector>().ActiveGun;

        
    }

    public void ChangeTargetTransforms(Vector3 rightIKPos, Vector3 rightIKRot, Vector3 leftIKPos, Vector3 leftIKRot)
    {
        /*        _rightHand.localPosition = rightIKPos;
                _leftHand.localPosition = leftIKPos;

                _rightHand.localRotation = Quaternion.Euler(rightIKRot);
                _leftHand.localRotation = Quaternion.Euler(leftIKRot);*/

        animator.SetIKPosition(RightIKGoal, rightIKPos);
        animator.SetIKPosition(LeftIKGoal, rightIKPos);
    }

    /* private void OnAnimatorIK(int layerIndex)
     {
         if (LeftHandIKTarget != null)
         {
             animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, HandIKAmount);
             animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, HandIKAmount);
             animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandIKTarget.position);
             animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandIKTarget.rotation);
         }
         if (RightHandIKTarget != null)
         {
             animator.SetIKPositionWeight(AvatarIKGoal.RightHand, HandIKAmount);
             animator.SetIKRotationWeight(AvatarIKGoal.RightHand, HandIKAmount);
             animator.SetIKRotation(AvatarIKGoal.RightHand, RightHandIKTarget.rotation);
             animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandIKTarget.position);
         }
         if (LeftElbowIKTarget != null)
         {
             animator.SetIKHintPosition(AvatarIKHint.LeftElbow, LeftElbowIKTarget.position);
             animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, ElbowIKAmount);
         }

         if (RightElbowIKTarget != null)
         {
             animator.SetIKHintPosition(AvatarIKHint.RightElbow, RightElbowIKTarget.position);
             animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, ElbowIKAmount);
         }
     }*/

/*    public void SetGunStyle(bool OneHanded)
    {
        animator.SetBool("Is2Handed", !OneHanded);
        animator.SetBool("Is1Handed", OneHanded);
    }*/

    public void Setup(Transform GunParent)
    {
        /*        Transform[] allChildren = GunParent.GetComponentsInChildren<Transform>();
                LeftElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "LeftElbow");
                RightElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "RightElbow");
                LeftHandIKTarget = allChildren.FirstOrDefault(child => child.name == "LeftHand");
                RightHandIKTarget = allChildren.FirstOrDefault(child => child.name == "RightHand");*/
    }

/*    [ContextMenu("Save Weapon pose")]
    private void SaveWeaponPose()
    {
        GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
        recorder.BindComponentsOfType<Transform>(_weaponPivot.gameObject, false);
        recorder.BindComponentsOfType<Transform>(_rightHand.gameObject, false);
        recorder.BindComponentsOfType<Transform>(_leftHand.gameObject, false);
        recorder.TakeSnapshot(0.0f);
        recorder.SaveToClip(ActiveGun.AnimationClip);
        UnityEditor.AssetDatabase.SaveAssets();
    }*/
}
