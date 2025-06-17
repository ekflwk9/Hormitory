using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 슬롯 전용 열거형 데이터
/// </summary>
public enum SlotSizeType
{
    NoneIconSize,
    NoneSelectSize,
    IconSize,
    SelectSize,
}

public class SlotUi : UiBase
{
    private Image icon;
    private Image select;
    private Sprite none;

    private RectTransform iconScale;
    private RectTransform selectScale;

    private Dictionary<SlotSizeType, Vector2> size = new()
    {
        [SlotSizeType.SelectSize] = new Vector2(155f, 155f),
        [SlotSizeType.IconSize] = new Vector2(80f, 80f),
    };

    public override void Init()
    {
        select = this.TryGetComponent<Image>();
        selectScale = this.TryGetComponent<RectTransform>();
        select.color = Color.clear;

        icon = this.TryGetChildComponent<Image>(StringMap.Icon);
        iconScale = icon.TryGetComponent<RectTransform>();
        icon.color = Color.clear;
        none = icon.sprite;

        size.Add(SlotSizeType.NoneIconSize, icon.TryGetComponent<RectTransform>().sizeDelta);
        size.Add(SlotSizeType.NoneSelectSize, this.TryGetComponent<RectTransform>().sizeDelta);
    }

    public void SetSlotView(int _itemId)
    {
        if (_itemId != 0)
        {
            icon.sprite = ItemManager.instance.Getitem(_itemId).Icon;
        }

        else
        {
            icon.sprite = none;
        }
    }

    public override void Show(bool _isActive)
    {
        icon.DOKill();
        select.DOKill();

        icon.color = Color.white;
        select.color = Color.white;

        if (_isActive)
        {
            select.color = new Color(0.278f, 0.764f, 1f, 1f);

            iconScale.sizeDelta = size[SlotSizeType.IconSize];
            selectScale.sizeDelta = size[SlotSizeType.SelectSize];
        }

        else
        {
            iconScale.sizeDelta = size[SlotSizeType.NoneIconSize];
            selectScale.sizeDelta = size[SlotSizeType.NoneSelectSize];
        }

        icon.DOFade(0, 2f);
        select.DOFade(0, 2f);
    }
}
