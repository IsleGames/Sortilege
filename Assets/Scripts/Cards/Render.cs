using System.Collections;
using System.Collections.Generic;
using _Editor;
using UnityEngine;
using TMPro;

using Managers;

namespace Cards
{
    public class Render : MonoBehaviour
    {
        private static int sortOrder;

        

        public void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            GameObject newCardImage = Instantiate(Game.Ctx.VfxOperator.cardImagePrefab, transform);
            Card c = GetComponent<Card>();
            CardUI cardui = newCardImage.GetComponent<CardUI>();

            cardui.SetCard(c);

            cardui.setCallbacks();
          

            var canvas = newCardImage.GetComponent<Canvas>();
            canvas.sortingLayerName = "Card";
            canvas.sortingOrder = sortOrder;
            

            MetaData meta = GetComponent<MetaData>();
            
            // Set strategy color
            var bordersprite = newCardImage.transform.Find("CardBorder").GetComponent<SpriteRenderer>();
            bordersprite.color = VfxManager.strategyColors[meta.strategy];
            bordersprite.sortingOrder = sortOrder;

            var bgsprite = newCardImage.transform.Find("CardBackground").GetComponent<SpriteRenderer>();
            bgsprite.sortingOrder = sortOrder;

            // Set attribute 
            var renderer = newCardImage.transform.Find("AttributeSprite").GetComponent<SpriteRenderer>();
            renderer.sprite = Resources.Load<Sprite>(
                VfxManager.attributeSpritePaths[meta.attribute]);
            renderer.sortingOrder = sortOrder+1;

            // Set name
            newCardImage.transform.Find("CardName").GetComponent<TextMeshProUGUI>().text = meta.title;
            // Set rules text
            newCardImage.transform.Find("CardText").GetComponent<TextMeshProUGUI>().text = GetComponent<Ability>()?.Text();
            sortOrder += 2;

            cardui.Hide();
            Game.Ctx.VfxOperator.MoveCardToRandomPosition(newCardImage.transform);
        }
    }
}