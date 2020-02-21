using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using Cards;

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
             {StrategyType.Sorcerer,  new Color(0.6f, 0.4f, 0.9f)  }
         };
        
         public static Dictionary<AttributeType, string> AttributeSpritePaths = new Dictionary<AttributeType, string>()
         {
             {AttributeType.None, "Sprites/Icons/icon-transparent"},
             {AttributeType.Infernal, "Sprites/Icons/icon-fire"},
             {AttributeType.Storm, "Sprites/Icons/icon-snowflake"},
             {AttributeType.Thunder, "Sprites/Icons/icon-lightning"},
             {AttributeType.Venom, "Sprites/Icons/icon-skull"}
         };
        
         public static Dictionary<StrategyType, string> StrategySpritePaths = new Dictionary<StrategyType, string>()
         {
             {StrategyType.None,         "Sprites/Icons/icon-transparent"},
             {StrategyType.Detriment,    "Sprites/Icons/icon-none"},
             {StrategyType.Berserker,    "Sprites/Icons/icon-sword"},
             {StrategyType.Craftsman,    "Sprites/Icons/icon-sword"},
             {StrategyType.Knight,       "Sprites/Icons/icon-arrow"},
             {StrategyType.Sorcerer,     "Sprites/Icons/icon-wand" },
         }; 
         
         public Card draggedCard;
         [SerializeField] private int sortOrder = 0;

         private void Awake()
         { 
            GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();
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