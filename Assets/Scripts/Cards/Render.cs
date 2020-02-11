using System.Collections;
using System.Collections.Generic;
using _Editor;
using UnityEngine;
using TMPro;

using Effects;
using Managers;
using UnityEngine.SocialPlatforms.GameCenter;

namespace Cards
{
    public class Render : MonoBehaviour
    {
        private static int sortOrder;

        public void SetCardImage()
        {
            GameObject newCardImage = Instantiate(Game.Ctx.VfxOperator.cardImagePrefab, transform);
            newCardImage.GetComponent<CardUI>()?.SetCard(GetComponent<Card>());
            var canvas = newCardImage.GetComponent<Canvas>();
            canvas.sortingLayerName = "Card";
            canvas.sortingOrder = sortOrder;
            

            MetaData meta = GetComponent<MetaData>();
            
            // Set strategy color
            var bgSprite = newCardImage.GetComponent<SpriteRenderer>();
            bgSprite.color = VfxManager.strategyColors[meta.strategy];
            bgSprite.sortingOrder = sortOrder;

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


            Game.Ctx.VfxOperator.MoveCardToRandomPosition(newCardImage.transform);
        }
    }
}