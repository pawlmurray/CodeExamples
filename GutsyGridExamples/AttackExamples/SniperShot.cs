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
    class SniperShot : SimpleProjectile
    {
        int startPosition;
        public SniperShot(Point pixelLocation, Texture2D texture)
            : base(new Point(CardConsts.SNIPERSHOT_X_SPEED, CardConsts.SNIPERSHOT_Y_SPEED), 
                   CardConsts.SNIPERSHOT_DRAW_WIDTH, CardConsts.SNIPERSHOT_DRAW_HEIGHT, pixelLocation, texture)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.sourceRectangle = CardConsts.SNIPERSHOT_SOURCE;
            this.startPosition = this.currentGridPosition.X;
        }

        public override void ApplyAttack(Actor target, BattleScene battleScene)
        {
            int tileDifference = Math.Abs(target.GetFrontTile().GetXLocation() - startPosition); 
            int damage = CardConsts.SNIPERSHOT_DAMAGE + tileDifference * CardConsts.SNIPERSHOT_DAMAGE_PER_TILE;
            int exReward = CardConsts.SNIPERSHOT_REWARD_BASE + tileDifference * CardConsts.SNIPERSHOT_REWARD_PER_TILE;
            base.ApplyAttack(target, damage, exReward, true, battleScene);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }

    class SniperShotEx : SimpleProjectile
    {
        int startPosition;
        double multiplier;
        List<Actor> targetsHit;
        public SniperShotEx(Point pixelLocation, Texture2D texture)
             : base(new Point(CardConsts.SNIPERSHOT_EX_X_SPEED, CardConsts.SNIPERSHOT_Y_SPEED), 
                   CardConsts.SNIPERSHOT_DRAW_WIDTH, CardConsts.SNIPERSHOT_DRAW_HEIGHT, pixelLocation, texture)
        {
            Initialize();
        }

        private void Initialize()
        {
            //TODO: Update this once the art is done.
            this.sourceRectangle = CardConsts.SNIPERSHOT_SOURCE;
            this.startPosition = this.currentGridPosition.X;
            this.multiplier = 1;
            targetsHit = new List<Actor>();
        }

        public override void ApplyAttack(Actor target, BattleScene battleScene)
        {
            if (!targetsHit.Contains(target))
            {
                int damage = (int) (CardConsts.SNIPERSHOT_EX_DAMAGE  + 
                             Math.Abs(target.GetFrontTile().GetXLocation() - startPosition) * CardConsts.SNIPERSHOT_EX_DAMAGE_PER_TILE * multiplier);
                multiplier *= CardConsts.SNIPERSHOT_EX_DAMAGE_MULT;
                targetsHit.Add(target);
                base.ApplyAttack(target, damage, 0, false, battleScene);
            }
              
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
    
}
