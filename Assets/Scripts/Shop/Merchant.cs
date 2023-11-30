using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Merchant : MonoBehaviour
{
    [SerializeField] private SkillType[] merchantSkillsId;
    [SerializeField] private Canvas shopMenuCanvas;
    [SerializeField] private GameObject onRangeFeedback;
    [SerializeField] private int numberOfDialogues;
    internal bool MenuToggled { get; private set; }
    internal bool OnRange{ get; private set; }
    private int activeSkillIndex;

    private void Awake()
    {
        activeSkillIndex = 0;
        MenuToggled = false;
        OnRange = false;
    }

    private void Start()
    {
        ChangeShopMenuSkill();
    }
    
    internal void ProximityChange(bool onRange)
    {
        onRangeFeedback.SetActive(onRange);
        OnRange = onRange;

        if (MenuToggled)
        {
            ToggleShopMenu(false);
        }
    }

    public void ToggleShopMenu(bool active)
    {
        onRangeFeedback.SetActive(!active && OnRange);

        int dialogueTrigger = active ? 0 : Random.Range(1, numberOfDialogues + 1);
        transform.Find("Dialogue").GetComponent<Animator>().SetTrigger("" + dialogueTrigger);

        MenuToggled = active;
        for (int i = 0; i < shopMenuCanvas.transform.childCount; i++)
        {
            shopMenuCanvas.transform.GetChild(i).gameObject.SetActive(active);
        }

        if(merchantSkillsId.Length == 1)
        {
            shopMenuCanvas.transform.Find("Arrow").gameObject.SetActive(false);
        }
    }

    private void ChangeShopMenuSkill()
    {
        Image skillSprite = shopMenuCanvas.transform.Find("Skill Sprite").GetComponent<Image>();

        skillSprite.sprite = Inventory.instance.GetAttributesBySkillType(merchantSkillsId[activeSkillIndex]).icon;
        skillSprite.GetComponent<RectTransform>().sizeDelta = Inventory.instance.GetAttributesBySkillType(merchantSkillsId[activeSkillIndex]).icon.rect.size;

        UpdateTexts();
    }

    public void Upgrade()
    {
        int upgradePrice = Inventory.instance.GetAttributesBySkillType(merchantSkillsId[activeSkillIndex]).basePrice * Inventory.instance.SkillsLevel[(int)merchantSkillsId[activeSkillIndex]];
        if (Inventory.instance.Money >= upgradePrice)
        {
            Inventory.instance.Money -= upgradePrice;
            Inventory.instance.SkillsLevel[(int)merchantSkillsId[activeSkillIndex]] += 1;
            UpdateTexts();
            Money.instance.MoneyToApply -= upgradePrice;
        }
    }

    private void UpdateTexts()
    {
        TextMeshProUGUI moneyText = shopMenuCanvas.transform.Find("Money Text").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI levelText = shopMenuCanvas.transform.Find("Skill Text").GetComponent<TextMeshProUGUI>();

        moneyText.text = (Inventory.instance.GetAttributesBySkillType(merchantSkillsId[activeSkillIndex]).basePrice * Inventory.instance.SkillsLevel[(int)merchantSkillsId[activeSkillIndex]]).ToString();
        levelText.text = Inventory.instance.SkillsLevel[(int)merchantSkillsId[activeSkillIndex]].ToString();
    }

    public void ChangeToNextSkill()
    {
        activeSkillIndex++;
        if(activeSkillIndex == merchantSkillsId.Length)
        {
            activeSkillIndex = 0;
        }

        ChangeShopMenuSkill();
    }
}
