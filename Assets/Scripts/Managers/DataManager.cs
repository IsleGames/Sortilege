using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using _Editor;
using Cards;
using Data;
using UnityEngine;

namespace Managers
{
    public class DataManager : MonoBehaviour
    {
        protected Dictionary<string, CardData> cardDataScript = new Dictionary<string, CardData>();
        protected Dictionary<string, EnemyData> enemyDataScript = new Dictionary<string, EnemyData>();

        private void Start()
        {
            LoadAll();
        }

        public void LoadAll()
        {
            var allCards = Resources.LoadAll("Card", typeof(CardData));
            foreach (CardData card in allCards){
                cardDataScript[card.title] = card;
                // Debugger.Log("Add " + card.title);
            }
            
            var allEnemies = Resources.LoadAll("Enemy", typeof(EnemyData));
            foreach (EnemyData enemy in allEnemies){
                enemyDataScript[enemy.title] = enemy;
            }
        }

        public CardData GetCard(string title)
        {
            return cardDataScript[title];
        }        
        
        public EnemyData GetEnemy(string title)
        {
            return enemyDataScript[title];
        }
    }
}