using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _interactText;
    [SerializeField] private Image _crosshair;

    [SerializeField] private List<Image> _crosshairHits;

    private void Start()
    {
        _crosshair.GetComponentsInChildren(true, _crosshairHits);
        _crosshairHits.RemoveAt(0);
    }
    public Image Crosshair { get => _crosshair; }

    public void UpdateText(string text)
    {
        _interactText.text = text;
    }

    public async UniTask CrosshairHit()
    {
        await UniTask.WhenAll(_crosshairHits.Select(hit => SetActiveAsync(hit, true)));

        await UniTask.Delay(TimeSpan.FromSeconds(0.3f), DelayType.DeltaTime, PlayerLoopTiming.Update);

        await UniTask.WhenAll(_crosshairHits.Select(hit => SetActiveAsync(hit, false)));

/*
        foreach (var hit in _crosshairHits)
        {
            hit.gameObject.SetActive(true);
        }*/
    }
    private async UniTask SetActiveAsync(Image hit, bool isFadeIn)
    {
        Color empty = new Color(255, 255, 255, 0);
        hit.DOColor(isFadeIn ? Color.white : empty, isFadeIn ? 0f : 0.3f);
        await UniTask.Yield();
    }

    public void ChangeCrosshairColor(Color color)
    {
        _crosshair.color = color;
    }
}
