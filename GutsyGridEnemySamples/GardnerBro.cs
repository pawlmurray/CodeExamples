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
    class GardnerBro : Enemy
    {
        int movesUntilNextPlant;
        int plantsDown;
        public GardnerBro(Tile startingTile, ContentManager content)
            : base(startingTile, EnemyConsts.GB_HP, EnemyConsts.GB_HP)
        {
            Initialize(content);
        }

        private void Initialize(ContentManager content)
        {
            movesUntilNextPlant = EnemyConsts.GB_INITIAL_PLANT_MOVES_REQ;
            this.plantsDown = 0;
            texture = content.Load<Texture2D>("duckface");
            Rectangle sourceRectangle = EnemyConsts.GB_SOURCE;
            SetBattleSprite(sourceRectangle, EnemyConsts.GB_X_BUF, EnemyConsts.GB_Y_BUF,
                            EnemyConsts.GB_DRAW_WIDTH, EnemyConsts.GB_DRAW_HEIGHT);
            this.currentState = new GBThinkingState(this);
        }

        public override void LaunchAttack(BattleScene battleScene)
        {
            if (plantsDown == EnemyConsts.GB_MAX_PLANTS)
            {
            }
            else
            {
                battleScene.AddEnemy(new FlowerFiend(this, this.GetFrontTile(), battleScene.GetContent()));
                plantsDown++;
            }
        }

        public override void FullTileMove(Tile newTile)
        {
            base.FullTileMove(newTile, EnemyConsts.GB_X_BUF, EnemyConsts.GB_Y_BUF);
        }

        public override void Think(BattleScene battleScene)
        {
            BattleGrid grid = battleScene.GetGrid();
            List<Tile> frontCol = grid.GetFrontEnemyEmptyTiles();
            List<Tile> nonFrontCol = grid.GetNonFrontEnemyEmptyTiles();
            List<Tile> allEmpties = grid.GetEnemyEmptyTiles();
            if (allEmpties.Count > 0)
            {
                if (movesUntilNextPlant == 0)
                {
                    LaunchAttack(battleScene);
                    if (nonFrontCol.Count != 0)
                        this.FullTileMove(grid.GetRandomNonFirstColEnemyTile());
                    else
                        this.FullTileMove(grid.GetRandomEnemyEmptyTile());
                    movesUntilNextPlant = EnemyConsts.GB_MOVES_BEFORE_PLANTS;
                }
                else if (movesUntilNextPlant == 1 && plantsDown != EnemyConsts.GB_MAX_PLANTS)
                {
                    if (frontCol.Count != 0)
                        this.FullTileMove(grid.GetRandomFirstColEnemyTile());
                    else
                        this.FullTileMove(grid.GetRandomEnemyEmptyTile());
                    movesUntilNextPlant--;
                }
                else
                {
                    if (nonFrontCol.Count != 0)
                        this.FullTileMove(grid.GetRandomNonFirstColEnemyTile());
                    else
                        this.FullTileMove(grid.GetRandomEnemyEmptyTile());
                    movesUntilNextPlant--;
                }
            }
        }

        public void AwknowledgePetDeath()
        {
            plantsDown--;
        }
    }

    class GBThinkingState : EnemyState
    {
        int framesUntilNextMove;
        public GBThinkingState(Enemy link)
            : base(link)
        {
            framesUntilNextMove = EnemyConsts.GB_FRAMES_TO_MOVE;
        }

        public override void Update(BattleScene battleScene)
        {
            if (framesUntilNextMove <= 0)
            {
                enemy.Think(battleScene);
                framesUntilNextMove = EnemyConsts.GB_FRAMES_TO_MOVE;
            }
            else
            {
                framesUntilNextMove--;
            }
        }


    }
}
