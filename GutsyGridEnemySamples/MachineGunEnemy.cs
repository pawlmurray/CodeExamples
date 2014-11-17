using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using GutsyGridAlpha.Battle.BattleConstants;
using GutsyGridAlpha.Battle.EnemyAttacks;
using GutsyGridAlpha.Battle.Sprites;
using GutsyGridAlpha.Battle.Stage;
using GutsyGridAlpha.Scenes;

namespace GutsyGridAlpha.Battle.Actors.Enemies
{
    class MachineGunEnemy : Enemy
    {
        public MachineGunEnemy(Tile startingTile, ContentManager content)
            : base(startingTile, EnemyConsts.MAE_HP, EnemyConsts.MAE_HP)
        {
            Initialize(content);
        }

        private void Initialize(ContentManager content)
        {
            texture = content.Load<Texture2D>("gumball");
            Rectangle sourceRectangle = EnemyConsts.MAE_SOURCE;
            SetBattleSprite(sourceRectangle, EnemyConsts.MAE_X_BUF, EnemyConsts.MAE_Y_BUF,
                            EnemyConsts.MAE_DRAW_WIDTH, EnemyConsts.MAE_DRAW_HEIGHT);
            this.currentState = new MGEIdle(this);
            this.attackTexture = content.Load<Texture2D>("gumballShot");
        }

        public override void LaunchAttack(BattleScene battleScene)
        {
            Point launchPoint = new Point(BattleGrid.GetStartPixelsOfTile(GetFrontTile()).X + EnemyConsts.GUMBALLSHOT_SHOT_X_BUF,
                                  BattleGrid.GetStartPixelsOfTile(GetFrontTile()).Y);
            launchPoint.Y += EnemyConsts.GUMBALLSHOT_Y_BUF + GutsyMain.rand.Next(EnemyConsts.GUMBALLSHOT_Y_VARIANCE);
            battleScene.AddEnemyAttack(new MAEShot(GetFrontTile().GetYLocation(), launchPoint, attackTexture));
        }

        public override void Think(BattleScene battleScene)
        {
        }
    }

    class MGEIdle : EnemyState
    {
        int framesTillNextShot;
        public MGEIdle(Enemy link)
            : base(link)
        {
            this.framesTillNextShot = EnemyConsts.MAE_COOLDOWN_FRAMES;
        }

        public override void Update(BattleScene battleScene)
        {
            if (framesTillNextShot == 0)
            {
                enemy.ChangeState(new MGEAttacking(enemy));
            }
            else
                framesTillNextShot--;
        }
    }

    class MGEAttacking : EnemyState
    {
        int remainingAttackingFrames;
        public MGEAttacking(Enemy link)
            : base(link)
        {
            remainingAttackingFrames = EnemyConsts.MAE_TOTAL_SHOTS_FIRED * EnemyConsts.MAE_FRAMES_BETWEEN_ATTACK;
        }

        public override void Update(BattleScene battleScene)
        {
            if (remainingAttackingFrames <= 0)
                enemy.ChangeState(new MGEIdle(enemy));
            else if (remainingAttackingFrames % EnemyConsts.MAE_FRAMES_BETWEEN_ATTACK == 0)
            {
                enemy.LaunchAttack(battleScene);
                enemy.StartFlash();
            }
            remainingAttackingFrames--;
        }
    }


    class MAEShot : BasicEnemyProjectile
    {
        public MAEShot(int row, Point pixelLocation, Texture2D texture)
            : base(new Point(EnemyConsts.MAE_SHOT_X_SPEED, EnemyConsts.MAE_SHOT_Y_SPEED), row,
                             EnemyConsts.MAE_SHOT_DRAW_WIDTH, EnemyConsts.MAE_SHOT_DRAW_HEIGHT, pixelLocation, texture)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.sourceRectangle = EnemyConsts.GET_GUMBALL_SHOT_SOURCE();
            hasCollided = false;
        }

        public override void ApplyAttack(Actor target, BattleScene battleScene)
        {
            target.TakeAttackDamage(EnemyConsts.MAE_SHOT_DAMAGE, battleScene);
            hasCollided = true;
        }
    }

}
