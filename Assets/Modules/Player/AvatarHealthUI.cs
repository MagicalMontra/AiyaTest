using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvatarHealthUI : MonoBehaviour
{
    [SerializeField] private Image _hpBar;
    [SerializeField] private TextMeshProUGUI _hpText;

    public void UpdateHealthText(int currentHp, int maxHp)
    {
        var percentageHp = (float)currentHp / (float)maxHp;
        _hpBar.fillAmount = percentageHp;
        _hpText.text = $"{currentHp}/{maxHp}";
    }
}