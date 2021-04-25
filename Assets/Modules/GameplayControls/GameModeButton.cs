using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class GameModeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform _content;
    [SerializeField] private GameMode _mode;
    [Inject] private SignalBus _signalBus;

    private Tween _tween;
    
    public void Select()
    {
        _signalBus.Fire(_mode);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _tween?.Kill();
        _tween = _content.DOScale(_content.localScale * 1.1f, 0.4f).SetEase(Ease.InOutCirc);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _tween?.Kill();
        _tween = _content.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutCirc);
    }
}