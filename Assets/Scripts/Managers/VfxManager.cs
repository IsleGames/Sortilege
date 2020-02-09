using System;
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


         }
         
         // Temporary Method
         public void MoveCardToSomePosition(Transform trans)
         {
             trans.position = new Vector3(Random.Range(-150f, 150f), Random.Range(-150f, 150f), 0);
         }
     }
 }