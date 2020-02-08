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
        public void SetCardImage()
        {
            GameObject newCardImage = Instantiate(Game.Ctx.VfxOperator.cardImagePrefab, transform);
            
            MetaData meta = GetComponent<MetaData>();
            
            // Set strategy color
            var bgSprite = newCardImage.GetComponent<SpriteRenderer>();
            bgSprite.color = VfxManager.strategyColors[meta.strategy];
            
            // Set attribute sprite
            // Missing
            
            // Set name
            newCardImage.transform.Find("CardName").GetComponent<TextMeshProUGUI>().text = meta.title;
            
            Game.Ctx.VfxOperator.MoveCardToSomePosition(newCardImage.transform);
            
            // // Set text
            // var cardTextObject = newCardImage.transform.Find("CardText");
            // if (cardTextObject == null)
            //     Debug.Log("Could not find card text");
            // else
            // {
            //     var text = cardTextObject.GetComponent<TextMeshProUGUI>();
            //     if (text == null)
            //         Debugger.Log("Could not find card text");
            //     else
            //         text.text = newCard.GetComponent<Ability>().Text();
            // }
        }
    }
}