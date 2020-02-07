using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Effects;

namespace Cards
{
    // Note: there
    public class CardFactory : MonoBehaviour
    {

        static CardFactory cardFactory = null;
        static GameObject cardPrefab;
        static GameObject cardImagePrefab;
        static Dictionary<StrategyType, Color> StrategyColors = new Dictionary<StrategyType, Color>(){
            {StrategyType.None, Color.white },
            {StrategyType.Detriment, new Color(0f, 0.3f, 0.2f)},
            {StrategyType.Berserker, new Color(0.9f, 0.35f, 0.2f) },
            {StrategyType.Craftsman, new Color(0.4f, 0.33f, 0.2f) },
            {StrategyType.Knight,    new Color(0.65f, 0.73f, 0.80f)},
            {StrategyType.Sorcerer,  new Color(0.6f, 0.4f, 0.9f)  }
        };
                

        private void Start()
        {
            if (cardFactory == null){
                cardFactory = this;
                cardPrefab = Resources.Load("Prefabs/Card") as GameObject;
                cardImagePrefab = Resources.Load("Prefabs/CardTemplate") as GameObject;
            }
        }

        public GameObject MakeCard(string title, StrategyType strategy, AttributeType attr, List<Effect> effects)
        {
            GameObject newCard = Instantiate(cardPrefab);
            
            MetaData meta = newCard.GetComponent<MetaData>();
            meta.title = title;
            meta.strategy = strategy;
            meta.attribute = attr;
            
            var cardAbility = newCard.GetComponent<Ability>();
            foreach (var effect in effects){
                if (effect != null)
                {
                    cardAbility.AddEffect(effect);
                }
            }
            
            setCardImage(newCard);
            return newCard;
        }

        void setCardImage(GameObject newCard)
        {
            var newCardImage = Instantiate(cardImagePrefab);
            newCardImage.transform.SetParent(newCard.transform);
            var meta = newCard.GetComponent<MetaData>();
            // Set strategy color

            var bgSprite = newCardImage.GetComponent<SpriteRenderer>();
            bgSprite.color = StrategyColors[meta.strategy];
            // Set attribute sprite

            // Set name
            var cardNameObject = newCardImage.transform.Find("CardName");
            if (cardNameObject == null) Debug.Log("Could not find card name");
            else
            {
                var text = cardNameObject.GetComponent<TextMeshProUGUI>();
                if (text == null) Debug.Log("Could not find card name text");
                else text.text = meta.title;
            }
            // Set text
            // Set name
            var cardTextObject = newCardImage.transform.Find("CardText");
            if (cardTextObject == null) Debug.Log("Could not find card text");
            else
            {
                var text = cardTextObject.GetComponent<TextMeshProUGUI>();
                if (text == null) Debug.Log("Could not find card text text");
                else text.text = newCard.GetComponent<Ability>().Text();
            }
        }
    }
}
