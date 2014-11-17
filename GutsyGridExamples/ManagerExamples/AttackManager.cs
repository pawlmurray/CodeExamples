using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using GutsyGridAlpha.Scenes;
using GutsyGridAlpha.Battle.Actors;
using GutsyGridAlpha.Battle.Actors.Enemies;
using GutsyGridAlpha.Battle.BattleConstants;
using GutsyGridAlpha.Battle.CardAttacks;
using GutsyGridAlpha.Battle.EnemyAttacks;
using GutsyGridAlpha.Battle.Stage;

namespace GutsyGridAlpha.Battle.BattleManagers
{
    class AttackManager
    {
        List<CardAttack> cardAttacks;
        List<CardAttack> attackQueue;
        List<EnemyAttack> enemyAttacks;
        List<EnemyAttack> enemyAttackQueue;
        BattleScene battleScene;

        public AttackManager(List<CardAttack> cardAttacks, List<EnemyAttack> enemyAttacks, BattleScene battleScene)
        {
            this.cardAttacks = cardAttacks;
            this.attackQueue = new List<CardAttack>();
            this.enemyAttackQueue = new List<EnemyAttack>();
            this.enemyAttacks = enemyAttacks;
            this.battleScene = battleScene;
        }
        public void CollisionCheck(List<Enemy> enemies)
        {
            foreach (CardAttack cardAttack in cardAttacks)
            {
                foreach (Enemy enemy in enemies)
                {
                    if (cardAttack.CollisionCheck(enemy))
                    {
                        cardAttack.ApplyAttack(enemy, battleScene);
                    }
                }
            }
        }

        public void PCCollisionCheck(PlayableCharacter pc)
        {
            foreach (EnemyAttack enemyAttack in enemyAttacks)
            {
                if (enemyAttack.CollisionCheck(pc))
                {
                    enemyAttack.ApplyAttack(pc, battleScene);
                }
            }
        }

        public void AddAttack(CardAttack cardAttack)
        {
            cardAttacks.Add(cardAttack);
        }

        private void AddFromAttackQueues()
        {
            foreach (CardAttack cardAttack in attackQueue)
            {
                cardAttacks.Add(cardAttack);
            }
            foreach (EnemyAttack enemyAttack in enemyAttackQueue)
            {
                enemyAttacks.Add(enemyAttack);
            }
            attackQueue.Clear();
            enemyAttackQueue.Clear();
        }

        // The Attack Queue is used specifically for attacks
        // that spawn additional attacks while updating. This ensures
        // that the newly spawned attacks will be added after 
        // the current update cycle, rather than during it.
        public void AddToAttackQueue(CardAttack cardAttack)
        {
            attackQueue.Add(cardAttack);
        }

        public void AddEnemyAttack(EnemyAttack enemyAttack)
        {
            enemyAttacks.Add(enemyAttack);
        }

        public void AddToEnemyAttackQueue(EnemyAttack enemyAttack)
        {
            enemyAttackQueue.Add(enemyAttack);
        }

        public void RemovalCheck()
        {
            cardAttacks.RemoveAll(p => p.RemovalCheck());
            enemyAttacks.RemoveAll(p => p.RemovalCheck());
        }

        public void Update(BattleScene battleScene)
        {
            RemovalCheck();
            UpdateAttacks();
            CollisionCheck(battleScene.GetEnemies());
            PCCollisionCheck(battleScene.GetPlayableCharacter());
            AddFromAttackQueues();
        }

        public void UpdateAttacks()
        {   
            foreach (CardAttack cardAttack in cardAttacks)
            {
                cardAttack.Update(battleScene);
            }
            foreach (EnemyAttack enemyAttack in enemyAttacks)
            {
                enemyAttack.Update(battleScene);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (EnemyAttack enemyAttack in enemyAttacks)
            {
                enemyAttack.Draw(spriteBatch);
            }

            foreach (CardAttack cardAttack in cardAttacks)
            {
                cardAttack.Draw(spriteBatch);
            }
        }
    }
}
