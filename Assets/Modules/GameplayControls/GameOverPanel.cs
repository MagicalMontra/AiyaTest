using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameOverPanel : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _heroText;
    [SerializeField] private TextMeshProUGUI _killText;

    [SerializeField] private CanvasGroup _content;
    [SerializeField] private Button _restartButton;

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }

    public void GameOver(GameplayData data)
    {
        var minute = new DateTime(DateTime.Now.Ticks - data.timeStamp).Minute;
        var seconds = new DateTime(DateTime.Now.Ticks - data.timeStamp).Second;
        var millisecond = new DateTime(DateTime.Now.Ticks - data.timeStamp).Millisecond;
        
        _timeText.text = $"Time played: {minute}m {seconds}s {millisecond}ms";
        _heroText.text = $"Hero recruited: {data.heroCount}";
        _killText.text = $"Kills: {data.killCount}";

        _content.alpha = 0;
        _content.gameObject.SetActive(true);
        _content.DOFade(1, 0.5f).SetEase(Ease.InOutCirc).OnComplete(() =>
        {
            _content.interactable = true;
            _content.blocksRaycasts = true;
        });

    }
}