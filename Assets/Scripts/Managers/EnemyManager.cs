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

            curEnemy = null;
        }

        public bool IsAllEnemyDead()
        {
            bool ret = true;
            
            foreach (Enemy enemy in EnemyList)
                if (!enemy.GetComponent<Health>().IsDead())
                {
                    ret = false;
                    break;
                }

            return ret;
        }

        private IEnumerator NextEnemy()
        {
            foreach (Enemy enemy in EnemyList)
                if (!enemy.GetComponent<Health>().IsDead())
                {
                    curEnemy = enemy;
                    yield return null;
                }

            curEnemy = null;
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