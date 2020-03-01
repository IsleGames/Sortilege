using System.Collections;
using System.Collections.Generic;

using Data;
using Units;
using Units.Enemies;
using UnityEngine;

namespace Managers
{
    public class EnemyManager : MonoBehaviour
    {
        public List<Enemy> EnemyList;
        
        public List<EnemyData> EnemyInitData;
        [SerializeField]
        private Enemy curEnemy;
        
        private IEnumerator AttackSeq;

        public GameObject enemyPrefab;
    
        private void Start()
        {
            enemyPrefab = (GameObject)Resources.Load("Prefabs/Enemy");

            foreach (EnemyData cardData in EnemyInitData)
			{
				GameObject newEnemyObj = Instantiate(enemyPrefab, transform);
				Enemy newEnemy = newEnemyObj.GetComponent<Enemy>();
				
				newEnemy.Initialize(cardData);

                EnemyList.Add(newEnemy);
            }
        }

        private IEnumerator NextEnemy()
        {
            yield return null;
        }

        public void InitEnemy()
        {
            AttackSeq = NextEnemy();
        }
        
        public Enemy GetNextEnemy()
        {
            AttackSeq.MoveNext();
            return curEnemy;
        }

    }
}