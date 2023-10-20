using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 _currentRotation;
    private Vector3 _targetRotation;

    public InputHandler Input;

    [Header("Basic Values")]
    public float RecoilX;
    public float RecoilY;
    public float RecoilZ;

    [Header("Aim Values")]
    public float AimRecoilX;
    public float AimRecoilY;
    public float AimRecoilZ;

    [Header("Other Values")]
    public float Snapinness;
    public float ReturnSpeed;
    public float Kickback;

    [Header("Transforms")]
    public Transform WeaponHolder;
    public Vector3 OrigPosition;

    private bool _isAiming;

    private void Update()
    {
        _isAiming = Input.IsAim;

        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, ReturnSpeed * Time.deltaTime);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, Snapinness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(_currentRotation);
        WeaponHolder.localPosition = Vector3.Lerp(WeaponHolder.localPosition, OrigPosition, Time.deltaTime * 3f);
    }

    public void RecoilFire()
    {
        if (_isAiming) _targetRotation += new Vector3(AimRecoilX, Random.Range(-AimRecoilY, AimRecoilY), Random.Range(-AimRecoilZ, AimRecoilZ));
        else _targetRotation += new Vector3(RecoilX, Random.Range(-RecoilY, RecoilY), Random.Range(-RecoilZ, RecoilZ));

        WeaponHolder.transform.position -= WeaponHolder.transform.forward * Kickback * Time.deltaTime;
    }
}
