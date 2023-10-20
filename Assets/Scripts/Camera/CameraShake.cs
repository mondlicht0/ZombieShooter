using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;

    [SerializeField] private float _shakeIntensity;
    [SerializeField] private float _shakeTime;
    [SerializeField] private NoiseSettings _headBob;
    [SerializeField] private NoiseSettings _sixD;

    private float _timer;
    private CinemachineBasicMultiChannelPerlin _channels;

    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
        _channels = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera()
    {
        _channels.m_NoiseProfile = _sixD;
        _channels.m_AmplitudeGain = _shakeIntensity;

        _timer = _shakeTime;
    }

    public void StopShake()
    {
        Debug.Log("Stop");
        _channels.m_NoiseProfile = _headBob;
        _channels.m_AmplitudeGain = 0.1f;
        _timer = 0f;
    }

    public IEnumerator StartShake()
    {
        ShakeCamera();

        yield return new WaitForSeconds(_shakeTime);

        StopShake();
    }
}
