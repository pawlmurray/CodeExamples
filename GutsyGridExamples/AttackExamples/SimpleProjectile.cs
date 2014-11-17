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


using GutsyGridAlpha.TesterLibrary;
using GutsyGridAlpha.Scenes;

namespace GutsyGridAlpha.Battle.CardAttacks
{
    abstract class SimpleProjectile : CardAttack
    {
        protected Point pixelLocation;
        protected Point speed;
        public Point currentGridPosition;
        protected int drawWidth;
        protected int drawHeight;
        protected int exReward;
        protected Boolean hasCollided;
        protected Boolean offScreen;
        protected Texture2D attackTexture;
        protected Rectangle sourceRectangle;
        public Rectangle destinationRectangle;

        public SimpleProjectile(Point speed, int drawWidth, int drawHeight, Point pixelLocation, Texture2D attackTexture)
        {
            this.speed = speed;
            this.pixelLocation = pixelLocation;
            this.drawWidth = drawWidth;
            this.drawHeight = drawHeight;
            this.currentGridPosition = BattleGrid.GetTileFromPixels(new Point(pixelLocation.X + drawWidth / 2, pixelLocation.Y + drawHeight / 2));
            this.exReward = 0;
            this.attackTexture = attackTexture;
            this.hasCollided = false;
            this.offScreen = false;
            this.destinationRectangle =  new Rectangle(pixelLocation.X, pixelLocation.Y, drawWidth, drawHeight);
        }


        public override Boolean CollisionCheck(Actor actor)
        {
            int enemyY = actor.GetFrontTile().GetYLocation();
            return  actor.IsHittable() &&
                    currentGridPosition.Y == enemyY &&
                    pixelLocation.X + drawWidth >= actor.GetCollisionBox().X &&
                    pixelLocation.X <= actor.GetCollisionBox().X + actor.GetCollisionBox().Width;
        }

        protected void ApplyAttack(Actor target, int damage, double exReward, Boolean isNonPiercing, BattleScene battleScene)
        {
            target.TakeAttackDamage(damage, battleScene);
            hasCollided = isNonPiercing;
            battleScene.AddToMeter(exReward);
        }


        public override void Update(BattleScene battleScene)
        {
            pixelLocation.X += speed.X;
            pixelLocation.Y += speed.Y;
            destinationRectangle.X = pixelLocation.X;
            destinationRectangle.Y = pixelLocation.Y;
            offScreen = pixelLocation.X > StageConsts.SCREEN_WIDTH;
            currentGridPosition = BattleGrid.GetTileFromPixels(new Point(pixelLocation.X + drawWidth / 2 , pixelLocation.Y + drawHeight / 2 ));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(attackTexture, destinationRectangle, sourceRectangle, Color.White, 0f,
                             new Vector2(0, 0), SpriteEffects.None, Layers.LayerValue(currentGridPosition, Layers.ATTACK_LAYER));
            DrawCollisionBox(spriteBatch);
        }

        private void DrawCollisionBox(SpriteBatch spriteBatch)
        {
            Debug.DrawCollisionBox(destinationRectangle, spriteBatch);
        }

        public override Boolean RemovalCheck()
        {
            return hasCollided || offScreen;
        }
    }
}
