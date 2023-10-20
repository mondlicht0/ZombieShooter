using Cinemachine;
using System.Collections;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [SerializeField] private InputHandler _input;

    private CinemachineVirtualCamera virtualCamera;

    [Header("Camera Damage Shake")]
    public float _shakeIntensity;
    public float _shakeTime;
    public NoiseSettings _headBob;
    public NoiseSettings _sixD;

    private float _timer;

    [Header("Head Bobbing")]
    public float idleBobFrequency = 1f;
    public float idleBobAmplitude = 0.1f;

    public float walkBobFrequency = 1.5f;
    public float walkBobAmplitude = 0.8f;

    public float AimIdleBobFrequency = 0.8f;
    public float AimIdleBobAmplitude = 0.1f;

    public float AimWalkBobFrequency = 1.25f;
    public float AimWalkBobAmplitude = 0.4f;


    private CinemachineBasicMultiChannelPerlin noise;
    private float lerpSpeed = 5f;
    private bool isWalking;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        float targetFrequency = 0;
        float targetAmplitude = 0;

        if (_input.IsAim)
        {
            targetFrequency = isWalking ? AimWalkBobFrequency : AimIdleBobFrequency;
            targetAmplitude = isWalking ? AimWalkBobAmplitude : AimIdleBobAmplitude;
        }

        else
        {
            targetFrequency = isWalking ? walkBobFrequency : idleBobFrequency;
            targetAmplitude = isWalking ? walkBobAmplitude : idleBobAmplitude;
        } 


        noise.m_FrequencyGain = Mathf.Lerp(noise.m_FrequencyGain, targetFrequency, Time.deltaTime * lerpSpeed);
        noise.m_AmplitudeGain = Mathf.Lerp(noise.m_AmplitudeGain, targetAmplitude, Time.deltaTime * lerpSpeed);
    }

    public void SetIsWalking(bool walking)
    {
        isWalking = walking;
    }

    public void ShakeCamera()
    {
        noise.m_NoiseProfile = _sixD;
        noise.m_AmplitudeGain = _shakeIntensity;

        _timer = _shakeTime;
    }

    public IEnumerator StopShake()
    {
        Debug.Log("Stop");
        noise.m_AmplitudeGain = Mathf.Lerp(noise.m_AmplitudeGain, 0.1f, Time.deltaTime * 5f);

        yield return new WaitForSeconds(0.7f);
        noise.m_NoiseProfile = _headBob;
        
        _timer = 0f;
    }

    public IEnumerator StartShake()
    {
        ShakeCamera();

        yield return new WaitForSeconds(_shakeTime);

        StartCoroutine(StopShake());
    }
}
