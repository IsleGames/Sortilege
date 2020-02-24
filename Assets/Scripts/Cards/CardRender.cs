using System;
using System.Collections;
using System.Collections.Generic;
using _Editor;
using UnityEngine;
using TMPro;

using Managers;
using UI;
using UnityEditor.U2D;

namespace Cards
{
    public class CardRender : MonoBehaviour
    {
        public bool visible = true;
        
        public float moveSpeed = 0.1f;

        private SpriteRenderer borderSprite, bgSprite, attRenderer, strRenderer;

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
            transform.Find("CardText").GetComponent<TextMeshProUGUI>().text = GetComponent<MetaData>().description;
            
            MoveToFront();
        }

        public void MoveToFront()
        {
            int sortOrder = Game.Ctx.VfxOperator.GetSortOrder();
            var canvas = GetComponent<Canvas>();
            canvas.sortingLayerName = "Card";
            canvas.sortingOrder = sortOrder;
            bgSprite.sortingOrder = sortOrder;
            sortOrder = Game.Ctx.VfxOperator.GetSortOrder();
            borderSprite.sortingOrder = sortOrder;
            sortOrder = Game.Ctx.VfxOperator.GetSortOrder();
            attRenderer.sortingOrder = sortOrder;
            strRenderer.sortingOrder = sortOrder;
        }
/*
        IEnumerator MoveCard(Vector3 dest, float delay = 0)
        {
            Vector3 init = new Vector3(transform.position.x, transform.position.y);
            float t = 0f;
            yield return new WaitForSeconds(delay);
            while (t < moveSpeed) {
                float i = t / moveSpeed;
                transform.SetPositionAndRotation(i * dest + (1f - i) * init,
                    transform.rotation);
                t += Time.deltaTime;
                yield return null;
            }
        }
  */  
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
    }
    
}