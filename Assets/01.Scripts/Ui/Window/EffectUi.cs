using TMPro;
using UnityEngine.UI;

public class EffectUi : UiBase
{
    private Slider slider;
    private TMP_Text sliderText;

    public override void Init()
    {
        slider = this.TryGetComponent<Slider>();
        sliderText = this.TryGetChildComponent<TMP_Text>(StringMap.Count);

        slider.value = 0.5f;
        slider.onValueChanged.AddListener(OnSlider);

        OnSlider(slider.value);
    }

    private void OnSlider(float _value)
    {
        slider.value = _value;

        var valueText = _value * 100f;
        sliderText.text = $"{valueText.ToString("F0")}%";

        //사운드 조절 메서드
    }
}
