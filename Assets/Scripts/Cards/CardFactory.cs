using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


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

        public GameObject MakeCard(string name, StrategyType strategy, AttributeType attr, Ability ability)
        {
            var newCard = GameObject.Instantiate(cardPrefab);
            var meta = newCard.GetComponent<MetaData>();
            meta.strategy = strategy;
            meta.attribute = attr;
            meta.title = name;
            var card_ability = newCard.GetComponent<Ability>();
            foreach (var effect in ability.EffectList){
                card_ability.AddEffect(effect);
            } 


            setCardImage(newCard);
            return newCard;
        }

        void setCardImage(GameObject newCard)
        {
            var newCardImage = GameObject.Instantiate(cardImagePrefab);
            var meta = newCard.GetComponent<MetaData>();
            // Set strategy color

            var bgSprite = newCardImage.GetComponent<SpriteRenderer>();
            bgSprite.color = StrategyColors[meta.strategy];
            // Set attribute sprite

            // Set name
            var cardNameObject = newCard.transform.Find("CardName");
            cardNameObject.GetComponent<TextMeshProUGUI>().text = meta.title;
            // Set text
            var cardTextObject = newCard.transform.Find("CardText");
            cardTextObject.GetComponent<TextMeshProUGUI>().text = newCard.GetComponent<Ability>().Text();
        }
    }
}
