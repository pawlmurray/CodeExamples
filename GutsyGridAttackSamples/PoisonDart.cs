using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using GutsyGridAlpha.Battle.Actors;
using GutsyGridAlpha.Battle.Actors.Enemies;
using GutsyGridAlpha.Battle.BattleConstants;
using GutsyGridAlpha.Battle.Cards;
using GutsyGridAlpha.Battle.Stage;

using GutsyGridAlpha.Scenes;

namespace GutsyGridAlpha.Battle.CardAttacks
{
    class PoisonDart : SimpleProjectile
    {
        public PoisonDart(Point pixelLocation, Texture2D texture)
            : base(new Point(CardConsts.POISONDART_X_SPEED, CardConsts.POISONDART_Y_SPEED), 
                             CardConsts.POISONDART_DRAW_WIDTH, CardConsts.POISONDART_DRAW_HEIGHT, pixelLocation, texture)
        {
            Initialize(texture);
        }

        private void Initialize(Texture2D texture)
        {
            this.sourceRectangle = CardConsts.POISONDART_SOURCE;
        }

        public override void ApplyAttack(Actor target, BattleScene battleScene)
        {
            target.AddStatus(StatusCondition.Poison1, battleScene);
            base.ApplyAttack(target, CardConsts.POISONDART_DAMAGE, CardConsts.POISONDART_REWARD, true, battleScene);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }

    class PoisonDartEx : SimpleProjectile
    {
        public PoisonDartEx(Point pixelLocation, Texture2D texture)
            : base(new Point(CardConsts.POISONDART_X_SPEED, CardConsts.POISONDART_Y_SPEED), 
                             CardConsts.POISONDART_DRAW_WIDTH, CardConsts.POISONDART_DRAW_HEIGHT, pixelLocation, texture)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.sourceRectangle = CardConsts.POISONDART_SOURCE;
        }

        public override void ApplyAttack(Actor target, BattleScene battleScene)
        {
            int damage = CardConsts.POISONDART_EX_DAMAGE_0;
            if (target.HasCondition(StatusCondition.Poison1))
            {
                damage = CardConsts.POISONDART_EX_DAMAGE_1;
                target.ConditionCleanse(StatusCondition.Poison1);
            }
            else if (target.HasCondition(StatusCondition.Poison2))
            {
                damage = CardConsts.POISONDART_EX_DAMAGE_2;
                target.ConditionCleanse(StatusCondition.Poison2);
            }
            else if (target.HasCondition(StatusCondition.Poison3))
            {
                damage = CardConsts.POISONDART_EX_DAMAGE_3;
                target.ConditionCleanse(StatusCondition.Poison3);
            }
            base.ApplyAttack(target, damage, 0, true, battleScene);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
