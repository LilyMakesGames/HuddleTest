using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardObject : MonoBehaviour, IPointerClickHandler{

    public Card card;

    public int ID;
    public Sprite backSprite, sprite;
    public bool revealed;
    GameManager manager;



    // Use this for initialization
    void Start () {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ID = card.ID;
        backSprite = card.backSprite;
        sprite = card.sprite;
		
	}
	
	// Update is called once per frame
	void Update () {
        if (revealed)
        {
            GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = backSprite;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!revealed && manager.canClick)
        {
            revealed = true;
            manager.AddToList(gameObject);
        }
    }

}
