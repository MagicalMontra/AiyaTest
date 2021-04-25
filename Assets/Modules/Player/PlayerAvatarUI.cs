using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAvatarUI : MonoBehaviour
{
    [SerializeField] private Image _characterImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _attackText;
    [SerializeField] private TextMeshProUGUI _defenceText;
    [SerializeField] private TextMeshProUGUI _moveSpeedText;

    public void UpdateUI(float playerCurrentMs, PlayerStatData data)
    {
        gameObject.SetActive(true);
        _characterImage.sprite = Instantiate(data.sprite);
        _nameText.text = data.name;
        _attackText.text = data.attackDmg;
        _defenceText.text = data.def;
        _moveSpeedText.text = playerCurrentMs.ToString("#.##");
    }
}