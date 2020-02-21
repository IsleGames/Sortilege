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
        public void Start()
        {
            //cardui.setCallbacks();
            int sortOrder;

            MetaData meta = GetComponent<MetaData>();
            
            // Set strategy color
            var borderSprite = transform.Find("CardBorder").GetComponent<SpriteRenderer>();
            borderSprite.color = VfxManager.strategyColors[meta.strategy];

            var bgSprite = transform.Find("CardBackground").GetComponent<SpriteRenderer>();

            // Set attribute 
            var attRenderer = transform.Find("AttributeSprite").GetComponent<SpriteRenderer>();
            attRenderer.sprite = Resources.Load<Sprite>(VfxManager.AttributeSpritePaths[meta.attribute]);
            
            // Set strategy
            var strRenderer = transform.Find("StrategySprite").GetComponent<SpriteRenderer>();
            strRenderer.sprite = Resources.Load<Sprite>(VfxManager.StrategySpritePaths[meta.strategy]);

            sortOrder = Game.Ctx.VfxOperator.GetSortOrder();
            var canvas = GetComponent<Canvas>();
            canvas.sortingLayerName = "Card";
            canvas.sortingOrder = sortOrder;
            bgSprite.sortingOrder = sortOrder;
            sortOrder = Game.Ctx.VfxOperator.GetSortOrder();
            borderSprite.sortingOrder = sortOrder;
            sortOrder = Game.Ctx.VfxOperator.GetSortOrder();
            attRenderer.sortingOrder = sortOrder;
            strRenderer.sortingOrder = sortOrder;

            // Set name
            transform.Find("CardName").GetComponent<TextMeshProUGUI>().text = meta.title;
            
            // Set rules text
            transform.Find("CardText").GetComponent<TextMeshProUGUI>().text = GetComponent<MetaData>().description;
        }
    }
}