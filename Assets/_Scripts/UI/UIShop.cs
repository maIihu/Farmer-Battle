using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIShop : MonoBehaviour
{
    [SerializeField] private GameObject shop1;
    [SerializeField] private GameObject shop2;
    [SerializeField] private GameObject shopItemTemplate;
    [SerializeField] private ItemEffectManager itemEffectManager;
    
    private List<GameObject> _shopItem1;
    private int _index1;

    private GameObject _characterInShop;
    
    private void Start()
    {
        shop1.SetActive(false);
        _shopItem1 = new List<GameObject>();
        CreateShop();
    }

    private void CreateShop()
    {
        CreateItemShop(Item.GetName(Item.ItemType.Shield),0, 0);
        CreateItemShop(Item.GetName(Item.ItemType.Rain),0, 1);
        CreateItemShop(Item.GetName(Item.ItemType.Exit),0, 2);

        CreateItemShop(Item.GetName(Item.ItemType.Tsunami),1, 0);
        CreateItemShop(Item.GetName(Item.ItemType.Nutty),1, 1);
        CreateItemShop(Item.GetName(Item.ItemType.Thunder),1, 2);
    }
    
    private void CreateItemShop(string text, int x, int y)
    {
        GameObject itemClone = Instantiate(shopItemTemplate, shop1.transform);
        itemClone.name = text;
        RectTransform rectTransform = itemClone.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(-40 + 75 * x, 100 - 75 * y);
        itemClone.GetComponentInChildren<TextMeshProUGUI>().text = text;
        itemClone.transform.GetChild(1).gameObject.SetActive(false);
        
        _shopItem1.Add(itemClone);
    }

    private void Update()
    {
        if (shop1.activeSelf) // Player 1
        {
            if (Input.GetKeyDown(KeyCode.W)) MoveSelection(-1); 
            if (Input.GetKeyDown(KeyCode.S)) MoveSelection(1);  
            if (Input.GetKeyDown(KeyCode.A)) MoveSelection(-3); 
            if (Input.GetKeyDown(KeyCode.D)) MoveSelection(3);
            
            HighlightItem(_index1, _shopItem1);
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ApplyItem(1, _index1);
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            int item = Random.Range(0, 5);
            ApplyItem(2, item);   
        }
    }

    private void ApplyItem(int player, int indexItem)
    {
        if (_shopItem1[indexItem].name == "Exit")
        {
            Exit();
            return;
        }
        
        itemEffectManager.GetEffect(_shopItem1[indexItem].name, player);

    }

    private void Exit()
    {
        _characterInShop.gameObject.GetComponent<PlayerController>().isShopping = false;
        foreach (var item in _shopItem1)
        {
            item.GetComponentInChildren<Image>().color = Color.white;
            item.transform.GetChild(1).gameObject.SetActive(false);
        }
        shop1.SetActive(false);
    }

    private void MoveSelection(int direction)
    {
        int row = _index1 % 3; // 0 1 2
        int col = _index1 / 3; // 0 1

        int newRow = row;
        int newCol = col;

        if (direction == -1 && row > 0) newRow--;
        if (direction == 1 && row < 2) newRow++;
        if (direction == -3 && col > 0) newCol--;
        if (direction == 3 && col < 1) newCol++;

        int newIndex = newCol * 3 + newRow;
        
        _index1 = newIndex;
    }

    private void HighlightItem(int index, List<GameObject> shopItem)
    {
        foreach (var item in shopItem)
        {
            item.GetComponentInChildren<Image>().color = Color.white;
            item.transform.GetChild(1).gameObject.SetActive(false);
        }
        
        shopItem[index].GetComponentInChildren<Image>().color = Color.yellow;
        shopItem[index].transform.GetChild(1).gameObject.SetActive(true);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _characterInShop = other.gameObject;
            _characterInShop.GetComponent<PlayerController>().isShopping = true;
            shop1.SetActive(true);
            _index1 = 0;
            HighlightItem(0, _shopItem1);
        }
    }

}
