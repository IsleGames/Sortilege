using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using Cards;
using Effects;
using UI;
using Units.Enemies;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Managers
 {
     public class VfxManager : MonoBehaviour
     {
         public static Dictionary<StrategyType, Color> strategyColors = new Dictionary<StrategyType, Color>(){
             {StrategyType.None, Color.white },
             {StrategyType.Detriment, new Color(0f, 0.3f, 0.2f)},
             {StrategyType.Berserker, new Color(0.9f, 0.35f, 0.2f) },
             {StrategyType.Craftsman, new Color(0.4f, 0.1f, 0.6f) },
             {StrategyType.Knight,    new Color(0.25f, 0.73f, 0.4f)},
             {StrategyType.Sorcerer,  new Color(0.529f, 0.808f, 0.922f)  },
             {StrategyType.Deceiver,  new Color(0.6f, 0.4f, 0.9f)  }
         };
        
         public static Dictionary<AttributeType, string> AttributeSpritePaths = new Dictionary<AttributeType, string>()
         {
             {AttributeType.None,     "Icons/icon-transparent"},
             {AttributeType.Infernal, "Icons/icon-fire"},
             {AttributeType.Storm,    "Icons/icons8-air-100"},
             {AttributeType.Thunder,  "Icons/icon-lightning"},
             {AttributeType.Venom,    "Icons/icon-skull"}
         };
        
         public static Dictionary<StrategyType, string> StrategySpritePaths = new Dictionary<StrategyType, string>()
         {
             {StrategyType.None,         "Icons/icon-transparent"},
             {StrategyType.Detriment,    "Icons/icon-none"},
             {StrategyType.Berserker,    "Icons/icon-sword"},
             {StrategyType.Craftsman,    "Icons/icon-sword"},
             {StrategyType.Knight,       "Icons/icon-arrow"},
             {StrategyType.Sorcerer,     "Icons/icon-wand" },
             {StrategyType.Deceiver,     "Icons/icon-wand" }
         };
         
         public static Dictionary<BuffType, string> BuffSpritePaths = new Dictionary<BuffType, string>()
         {
             {BuffType.Block,  "Icons/icons8-shield-64"},
             {BuffType.Forge,  "Icons/icons8-cauldron-64"},
             {BuffType.Thorns, "Icons/icons8-crown-of-thorns-100"},
             {BuffType.Plague, "Icons/icons8-skull-64"},
             {BuffType.Flinch, "Icons/icon-fear"},
             {BuffType.Voodoo, "Icons/icons8-cauldron-64"},
             {BuffType.Breeze, "Icons/icons8-air-100"},
         };
         
         [NonSerialized]
         public Color isAvailableColor = new Color(0.8196f, 0.8196f, 0.8196f);
         [NonSerialized]
         public Color notAvailableColor = new Color(0.5f, 0.5f, 0.5f);
         
         public Card draggedCard;

         public GameObject turnToMoveBoardPrefab;
         public Image DarkenMask;

         private void Awake()
         { 
             GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();
         }

         public void Start()
         {
             turnToMoveBoardPrefab = (GameObject)Resources.Load("Prefabs/TurnToMoveBoard");

             DarkenMask = transform.Find("RaycasterScreen").GetComponent<Image>();
         }

         public void ShowTurnText(string text, bool dontDisappear = false)
         {
             GameObject newTurnBoardObj = Instantiate(turnToMoveBoardPrefab, Game.Ctx.UICanvas.transform);
             TurnToMoveBoard board = newTurnBoardObj.GetComponent<TurnToMoveBoard>();

             board.SetText(text);
             board.StartAnimation(dontDisappear);
         }

         public void SetAllSortOrders()
         {
             Game.Ctx.SortOrderOperator.ResetSortOrder();
             Game.Ctx.CardOperator.pileDiscard.SetSortOrders();
             Game.Ctx.CardOperator.pileDeck.SetSortOrders();
             Game.Ctx.CardOperator.pileHand.SetSortOrders();
             Game.Ctx.CardOperator.pilePlay.SetSortOrders();
         }

         public void SetAllBrightnessInAimMode(float alpha, bool isEnemyAtFront)
         {
             SetMaskBrightness(alpha);
             string layerName = "";
             if (isEnemyAtFront)
                 layerName = "HighlightedObjects";
             else
                 layerName = "Default";

             foreach (Enemy enemy in Game.Ctx.EnemyOperator.EnemyList)
             {
                 var sg = enemy.GetComponent<SortingGroup>();
                 var cv = enemy.GetComponent<Canvas>();
                 
                 sg.sortingLayerName = layerName;
                 cv.sortingLayerName = layerName;
             }

             var bt = Game.Ctx.transform.GetComponentInChildren<CommandButton>().GetComponent<Canvas>();
             bt.sortingLayerName = layerName;
         }
         
         public void SetMaskBrightness(float alpha)
         {
             Color newColor = DarkenMask.color;
             newColor.a = alpha;
             DarkenMask.color = newColor;
         }
     }
 }