using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using Cards;
using Effects;
using UI;

namespace Managers
 {
     public class VfxManager : MonoBehaviour
     {
         public static Dictionary<StrategyType, Color> strategyColors = new Dictionary<StrategyType, Color>(){
             {StrategyType.None, Color.white },
             {StrategyType.Detriment, new Color(0f, 0.3f, 0.2f)},
             {StrategyType.Berserker, new Color(0.9f, 0.35f, 0.2f) },
             {StrategyType.Craftsman, new Color(0.4f, 0.33f, 0.2f) },
             {StrategyType.Knight,    new Color(0.65f, 0.73f, 0.80f)},
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
         [SerializeField] private int sortOrder = 0;

         public GameObject turnToMoveBoardPrefab;
         
         private void Awake()
         { 
             GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();
         }

         public void Start()
         {
             turnToMoveBoardPrefab = (GameObject)Resources.Load("Prefabs/TurnToMoveBoard");
         }

         public void ShowTurnText(string text)
         {
             GameObject newTurnBoardObj = Instantiate(turnToMoveBoardPrefab, Game.Ctx.UICanvas.transform);
             TurnToMoveBoard board = newTurnBoardObj.GetComponent<TurnToMoveBoard>();

             board.SetText(text);
             board.StartAnimation();
         }

         public void SetAllSortOrders()
         {
             ResetSortOrder();
             Game.Ctx.CardOperator.pileDiscard.SetSortOrders();
             Game.Ctx.CardOperator.pileDeck.SetSortOrders();
             Game.Ctx.CardOperator.pileHand.SetSortOrders();
             Game.Ctx.CardOperator.pilePlay.SetSortOrders();
         }

         public void ResetSortOrder()
         {
             sortOrder = 0;
         }

         public int GetSortOrder()
         {
             int ret = sortOrder;
             sortOrder += 1;
             return ret;
         }
        
         public IEnumerator MoveCardTo(Card card, Vector3 pos, float time)
         {
            float t = 0f;
            Vector3 init = card.transform.position;
            
            while (t < time) 
            {
                float i = t / time;
                card.transform.position = i * pos + (1f - i) * init;
                t += Time.deltaTime;
                yield return null;
            }
         }
     }
 }