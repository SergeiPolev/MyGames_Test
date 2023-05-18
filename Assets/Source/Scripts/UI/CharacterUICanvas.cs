using DG.Tweening;
using TMPro;
using UnityEngine;

public class CharacterUICanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private RectTransform rectTransform;

    public RectTransform RectTransform => rectTransform;
    
    public void InitCanvas(bool isPlayer)
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1f, 1f);
        levelText.gameObject.SetActive(!isPlayer);
        healthBar.Init(isPlayer);
    }
    
    public void UpdateHealthBar(float current, float max)
    {
        var rounded = Mathf.Ceil(current);
        healthBar.UpdateBar(rounded / max);
        healthText.text = $"{rounded}";
    }
    public void UpdateLevelText(int value)
    {
        levelText.text = $"{value}";
    }
}