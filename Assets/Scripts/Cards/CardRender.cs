using System;
using System.Collections;
using System.Collections.Generic;
using _Editor;
using UnityEngine;
using TMPro;

using Managers;
using UnityEngine.Rendering;

namespace Cards
{
    public class CardRender : MonoBehaviour
    {
        public bool visible = true;
        
        // public float moveSpeed = 0.1f;

        private SpriteRenderer borderSprite, bgSprite, attRenderer, strRenderer;
        private TextMeshProUGUI cardText;
        EffectDescription effectDescription = new EffectDescription();
        BuffDescription buffDescription = new BuffDescription();

        private float onSelectZoomScale = 1.35f;

        public void Start()
        {
            MetaData meta = GetComponent<MetaData>();
            
            borderSprite = transform.Find("CardBorder").GetComponent<SpriteRenderer>();
            bgSprite = transform.Find("CardBackground").GetComponent<SpriteRenderer>();
            attRenderer = transform.Find("AttributeSprite").GetComponent<SpriteRenderer>();
            strRenderer = transform.Find("StrategySprite").GetComponent<SpriteRenderer>();
            
            // Set strategy color
            borderSprite.color = VfxManager.strategyColors[meta.strategy];

            // Set attribute 
            attRenderer.sprite = Resources.Load<Sprite>(VfxManager.AttributeSpritePaths[meta.attribute]);
            
            // Set strategy
            strRenderer.sprite = Resources.Load<Sprite>(VfxManager.StrategySpritePaths[meta.strategy]);

            // Set name
            transform.Find("CardName").GetComponent<TextMeshProUGUI>().text = meta.title;

            // Set rules text
            cardText = transform.Find("CardText").GetComponent<TextMeshProUGUI>();
            cardText.text = GetComponent<MetaData>().description;

            SetOrder();
        }
        


        public void OnSelectZoom()
        {
            Vector3 newLocalScale = transform.localScale *  onSelectZoomScale;
            transform.localScale = newLocalScale;
        }

        public void SetAvailability(bool availability)
        {
            if (availability)
            {
                bgSprite.color = Game.Ctx.VfxOperator.isAvailableColor;
            }
            else
            {
                bgSprite.color = Game.Ctx.VfxOperator.notAvailableColor;
            }
        }

        private void Update()
        {
            var ability = GetComponent<MetaData>().ability;
            effectDescription.Update(ability.effectList,
                Game.Ctx.CardOperator.pilePlay.Count(),
                Game.Ctx.CardOperator.pileDiscard
                    .GetStrategyTypeCards(StrategyType.Deceiver).Count,
                Game.Ctx.CardOperator.pileHand.Count());

            buffDescription.Update(ability.buffEffectList,
                Game.Ctx.CardOperator.pilePlay.Count());
        }

        private void LateUpdate()
        {
            cardText.text = Text();
        }

        public void SetOrder()
        {
            SortOrderManager soM = Game.Ctx.SortOrderOperator;
            
            int sortOrder = soM.GetSortOrder();
            
            
            var canvas = GetComponent<Canvas>();
            var sg = GetComponent<SortingGroup>();
            
            canvas.sortingLayerName = "Card";
            canvas.sortingOrder = sortOrder;

            sg.sortingLayerName = "Card";
            sg.sortingOrder = sortOrder;
            
            // bgSprite.sortingOrder = sortOrder;
            
            soM.GetSortOrder();
            borderSprite.sortingOrder = sortOrder;
            
            // sortOrder = Game.Ctx.VfxOperator.GetSortOrder();
            // attRenderer.sortingOrder = sortOrder;
            // strRenderer.sortingOrder = sortOrder;
        }
        
        public void Hide()
        {
            visible = false;
            TextMeshProUGUI[] tmPros = GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var r in tmPros) {
                r.enabled = false;
            }
            SpriteRenderer[] spRenderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in spRenderers) {
                r.enabled = false;
            }
        }
    
        public void Show()
        {
            visible = true;
            TextMeshProUGUI[] tmPros = GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var r in tmPros) {
                r.enabled = true;
            }
            SpriteRenderer[] spRenderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in spRenderers) {
                r.enabled = true;
            }
        }

        public string EffectPreview()
        {
            return effectDescription.ToString();
        }

        public string BuffPreview()
        {
            return buffDescription.ToString();
        }

        public string Preview()
        {
            return effectDescription.ToString() + "\n" +
                     buffDescription.ToString();
        }


        private string Text()
        {
            var playPile = Game.Ctx.CardOperator.pilePlay;
            if (playPile.Count() > 0 && GetComponent<Card>() ==
                playPile.Get(playPile.Count() - 1))
            {
                return Preview();
            }

            else
            {
                return GetComponent<MetaData>().description;
            }

        }
    }
    
}