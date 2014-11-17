using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

using GutsyGridAlpha.Battle.Actors;
using GutsyGridAlpha.Battle.Actors.Enemies;
using GutsyGridAlpha.Battle.Cards;

using GutsyGridAlpha.Scenes;

namespace GutsyGridAlpha.Battle.CardAttacks
{
    abstract class CardAttack
    {
        public abstract Boolean CollisionCheck(Actor actor);
        public abstract void Update(BattleScene battleScene);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void ApplyAttack(Actor target, BattleScene battleScene);
        public abstract Boolean RemovalCheck();
    }
}
