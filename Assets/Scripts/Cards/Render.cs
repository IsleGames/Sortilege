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
            var attRenderer = newCardImage.transform.Find("AttributeSprite").GetComponent<SpriteRenderer>();
            attRenderer.sprite = Resources.Load<Sprite>(VfxManager.AttributeSpritePaths[meta.attribute]);
            attRenderer.sortingOrder = sortOrder + 1;
            // Set strategy
            var strRenderer = newCardImage.transform.Find("StrategySprite").GetComponent<SpriteRenderer>();
            strRenderer.sprite = Resources.Load<Sprite>(VfxManager.StrategySpritePaths[meta.strategy]);
            strRenderer.sortingOrder = sortOrder + 1;

            // Set name
            newCardImage.transform.Find("CardName").GetComponent<TextMeshProUGUI>().text = meta.title;
            
            // Set rules text
            newCardImage.transform.Find("CardText").GetComponent<TextMeshProUGUI>().text = GetComponent<MetaData>().description;
            sortOrder += 2;

            cardui.Hide();
            Game.Ctx.VfxOperator.MoveCardToRandomPosition(newCardImage.transform);
        }
    }
}