using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public int attack, health, money;
    public int maxHealth;
    public DamageableCharacter damageableCharacter;
    [SerializeField]
    private TMP_Text attackText, healthText, moneyText;
    [SerializeField]
    private TMP_Text attackPreText, healthPreText, moneyPreText;
    [SerializeField]
    private Image previewImage;
    [SerializeField]
    private GameObject selectedItemStats;
    [SerializeField]
    private GameObject selectedItemImage;

    // Start is called before the first frame update
    void Start()
    {
        damageableCharacter = GameObject.Find("Player").GetComponent<DamageableCharacter>();
        UpdateEquipmentStats();
    }
    
    public void UpdateEquipmentStats()
    {
        attackText.text = attack.ToString();
        healthText.text = health.ToString();
        moneyText.text = money.ToString();
        maxHealth = health;
        damageableCharacter.UpdateMaxHealth(maxHealth);
    }
    public void PreviewEquipmentStats(int attack, int health, int money, Sprite itemSprite)
    {
        attackPreText.text=attack.ToString();
        healthPreText.text = health.ToString();
        moneyPreText.text = money.ToString();
        previewImage.sprite = itemSprite;
        selectedItemImage.SetActive(true);
        selectedItemStats.SetActive(true);
        UpdateEquipmentStats();
        
    }
    public void TurnOffPreviewStats()
    {
        selectedItemImage.SetActive(false);
        selectedItemStats.SetActive(false);
        damageableCharacter.UpdateMaxHealth(maxHealth);
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateEquipmentStats();
    }
}
