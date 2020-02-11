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
         public GameObject cardImagePrefab;
        public GameObject cardCanvas;
         
         public static Dictionary<StrategyType, Color> strategyColors = new Dictionary<StrategyType, Color>(){
             {StrategyType.None, Color.white },
             {StrategyType.Detriment, new Color(0f, 0.3f, 0.2f)},
             {StrategyType.Berserker, new Color(0.9f, 0.35f, 0.2f) },
             {StrategyType.Craftsman, new Color(0.4f, 0.33f, 0.2f) },
             {StrategyType.Knight,    new Color(0.65f, 0.73f, 0.80f)},
             {StrategyType.Sorcerer,  new Color(0.6f, 0.4f, 0.9f)  }
         };

        public static Dictionary<AttributeType, string> attributeSpritePaths = new Dictionary<AttributeType, string>()
        {
            { AttributeType.Infernal, "Sprites/Icons/icon-fire"      },
            { AttributeType.Storm,    "Sprites/Icons/icon-snowflake" },
            { AttributeType.Thunder,  "Sprites/Icons/icon-lightning" },
            { AttributeType.Venom,    "Sprites/Icons/icon-skull"     },
            { AttributeType.None,    "Sprites/Icons/icon-none"       },
        };

         private void Start()
         {
             cardImagePrefab = Resources.Load("Prefabs/CardTemplate") as GameObject;
            cardCanvas = GameObject.Find("CardCanvas");

         }
         
         // Temporary Method
         public void MoveCardToRandomPosition(Transform trans)
         {
             trans.position = new Vector3(Random.Range(-500f, 500f), -190f, 0);
         }

        public void SetCardPosition(Card card, Vector3 pos)
        {
            card.transform.position = pos;
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