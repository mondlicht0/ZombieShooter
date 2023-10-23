using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class HealthScreen : MonoBehaviour
{
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private Image _bloodScreen;
    [SerializeField] private Volume _cameraVolume;
    [SerializeField] private Vignette _vignette;

    [SerializeField] private float _hurtTimer = 0.1f;

    [SerializeField] private AudioClip _hurtSound = null;

    private AudioSource _audioSource;

    private float _currentHealth;
    private float _intensity;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator HurtFlash()
    {
        if (_cameraVolume.sharedProfile.TryGet(out _vignette))
        {

            _intensity = 0.4f + 1 - _health.CurrentHealth / _health.MaxHealth;

            _vignette.intensity.value = _intensity;
            //_audioSource.PlayOneShot(_hurtSound);
            yield return new WaitForSeconds(_hurtTimer);

            while (_intensity > 0)
            {
                _intensity -= 0.05f;

                if (_intensity <= _health.CurrentHealth / (_health.MaxHealth * 2)) _intensity = 1 - _health.CurrentHealth / _health.MaxHealth;
                _vignette.intensity.value = _intensity;

                yield return new WaitForSeconds(0.5f);
            }
            
        }
    }

    public void HealFlash()
    {
        if (_cameraVolume.sharedProfile.TryGet(out _vignette))
        {
            _vignette.color.Override(new Color(0, 255, 0));
            _intensity = 0.7f;


            _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, _intensity, 0.5f);
            //_audioSource.PlayOneShot(_hurtSound);
            _vignette.color.Override(new Color(152, 0, 0));
        }
    }

    public void UpdateHealth()
    {
        Color splatterAlpha = _bloodScreen.color;
        splatterAlpha.a = 1 - (_health.CurrentHealth / _health.MaxHealth);

        _bloodScreen.color = splatterAlpha;

        if (_health.CurrentHealth == _health.MaxHealth - 0.01f) _bloodScreen.color = new Color(_bloodScreen.color.r, _bloodScreen.color.g, _bloodScreen.color.b, 0);
    }

    public void SetHealthAlpha0()
    {
        Color splatterAlpha = _bloodScreen.color;
        splatterAlpha.a = 0;

        _bloodScreen.color = splatterAlpha;
    }

    private void OnDisable()
    {
        if (_cameraVolume.sharedProfile.TryGet(out _vignette))
        {
            _vignette.intensity.value = 0f;
        }
    }
}
