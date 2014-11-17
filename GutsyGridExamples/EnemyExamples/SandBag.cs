using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using GutsyGridAlpha.Battle.BattleConstants;
using GutsyGridAlpha.Battle.Sprites;
using GutsyGridAlpha.Battle.Stage;
using GutsyGridAlpha.Scenes;


namespace GutsyGridAlpha.Battle.Actors.Enemies
{
    class SandBag : Enemy
    {
        public SandBag(Tile startingTile, ContentManager content)
            : base(startingTile, EnemyConsts.SANDBAG_HP, EnemyConsts.SANDBAG_HP)
        {
            Initialize(content);
        }

        private void Initialize(ContentManager content)
        {
            texture = content.Load<Texture2D>("SandBag");
            Rectangle sourceRectangle = new Rectangle(0, 0, EnemyConsts.SANDBAG_SOURCE_WIDTH, EnemyConsts.SANDBAG_SOURCE_HEIGHT);
            LoadBattleSprite(sourceRectangle, EnemyConsts.SANDBAG_X_BUFFER, EnemyConsts.SANDBAG_Y_BUFFER, 
                             EnemyConsts.SANDBAG_WIDTH, EnemyConsts.SANDBAG_HEIGHT);
        }

        public override void MoveToTile(Tile newTile)
        {
            base.MoveToTile(newTile, EnemyConsts.SANDBAG_X_BUFFER, EnemyConsts.SANDBAG_Y_BUFFER);
        }

        public override void Update(BattleScene battleScene)
        {
            ConditionCheck(battleScene);
        }

        // SAND BAG AINT GOT NO TIME TO THINK
        public override void Think(BattleScene battleScene)
        {
        }

    }


}
