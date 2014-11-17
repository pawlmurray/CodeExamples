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
using GutsyGridAlpha.Battle.BattleConstants;
using GutsyGridAlpha.Battle.Cards;
using GutsyGridAlpha.Battle.Stage;

using GutsyGridAlpha.TesterLibrary;

namespace GutsyGridAlpha.Battle.BattleManagers
{
    class CardManager
    {
        Rectangle slot1, slot2, slot3, slot4, slot5;
        Rectangle icon1, icon2, icon3, icon4, icon5;
        List<Rectangle> icons;
        BattleScene battleScene;
        List<BattleCard> deck;
        List<BattleCard> graveyard;
        List<BattleCard> hand;
        Random rand;
        int currentCooldown;
        int fullCooldown;
        int cardsUsed;

        Texture2D exSignalTexture;
        Texture2D handSlot;
        Texture2D cooldownShader;

        int signalAnimationFrame;
        int signalAnimationDirection;
        int signalAnimationIndex;

        public CardManager(List<BattleCard> deck, ContentManager content, BattleScene battleScene)
        {
            this.rand = new Random();
            rand.Next();
            slot1 = new Rectangle(CIConsts.SLOT_X_1, CIConsts.SLOT_Y, CIConsts.SLOT_SIZE, CIConsts.SLOT_SIZE);
            slot2 = new Rectangle(CIConsts.SLOT_X_2, CIConsts.SLOT_Y, CIConsts.SLOT_SIZE, CIConsts.SLOT_SIZE);
            slot3 = new Rectangle(CIConsts.SLOT_X_3, CIConsts.SLOT_Y, CIConsts.SLOT_SIZE, CIConsts.SLOT_SIZE);
            slot4 = new Rectangle(CIConsts.SLOT_X_4, CIConsts.SLOT_Y, CIConsts.SLOT_SIZE, CIConsts.SLOT_SIZE);
            slot5 = new Rectangle(CIConsts.SLOT_X_5, CIConsts.SLOT_Y, CIConsts.SLOT_SIZE, CIConsts.SLOT_SIZE);

            icon1 = new Rectangle(CIConsts.SLOT_X_1 + CIConsts.CICON_X_BUFFER, CIConsts.SLOT_Y + CIConsts.CICON_Y_BUFFER, 
                                  CIConsts.CICON_SIZE, CIConsts.CICON_SIZE);

            icon2 = new Rectangle(CIConsts.SLOT_X_2 + CIConsts.CICON_X_BUFFER, CIConsts.SLOT_Y + CIConsts.CICON_Y_BUFFER,
                                  CIConsts.CICON_SIZE, CIConsts.CICON_SIZE);

            icon3 = new Rectangle(CIConsts.SLOT_X_3 + CIConsts.CICON_X_BUFFER, CIConsts.SLOT_Y + CIConsts.CICON_Y_BUFFER,
                                  CIConsts.CICON_SIZE, CIConsts.CICON_SIZE);

            icon4 = new Rectangle(CIConsts.SLOT_X_4 + CIConsts.CICON_X_BUFFER, CIConsts.SLOT_Y + CIConsts.CICON_Y_BUFFER,
                                  CIConsts.CICON_SIZE, CIConsts.CICON_SIZE);

            icon5 = new Rectangle(CIConsts.SLOT_X_5 + CIConsts.CICON_X_BUFFER, CIConsts.SLOT_Y + CIConsts.CICON_Y_BUFFER,
                                  CIConsts.CICON_SIZE, CIConsts.CICON_SIZE);
            icons = new List<Rectangle>();
            icons.Add(icon1);
            icons.Add(icon2);
            icons.Add(icon3);
            icons.Add(icon4);
            icons.Add(icon5);

            this.deck = DeckMaker.MakeTestDeck();
            LoadContent(content);
            this.hand = new List<BattleCard>();
            this.graveyard = new List<BattleCard>();
            ShuffleDeck();
            GetHand();
            this.currentCooldown = 0;
            this.fullCooldown = 1;
            this.cardsUsed = 0;
            this.battleScene = battleScene;
            signalAnimationFrame = 0;
            signalAnimationDirection = 1;
            signalAnimationIndex = 0;
        }

        public void LoadContent(ContentManager content)
        {
            handSlot = content.Load<Texture2D>("cardHolderSheet");
            exSignalTexture = content.Load<Texture2D>("signalPalette");
            cooldownShader = content.Load<Texture2D>("CooldownIndicator1");
            foreach(BattleCard card in deck)
            {
                card.Load(content);
            }   
        }

        public void ShuffleDeck()
        {
            List<BattleCard> shuffledDeck = new List<BattleCard>();
            for (int i = 0; i < 30; i++)
            {
                int randomIndex = rand.Next(30 - i);
                BattleCard randomCard = deck[randomIndex];
                shuffledDeck.Add(randomCard);
                deck.Remove(randomCard);
            }
            deck = shuffledDeck;
        }

        public void ShuffleGraveyardIntoDeck()
        {
            for (int i = 0; i < 30; i++)
            {
                int randomIndex = rand.Next(30 - i);
                BattleCard randomCard = graveyard[randomIndex];
                deck.Add(randomCard);
                graveyard.Remove(randomCard);
            }
        }

        public void GetHand()
        {
            if (deck.Count == 0)
            {
                ShuffleGraveyardIntoDeck    ();
            }
            for (int i = 0; i < 5; i++)
            {
                int randomIndex = rand.Next(30 - graveyard.Count - i);
                BattleCard randomCard = deck[randomIndex];
                randomCard.SetUsable();
                hand.Add(randomCard);
                deck.Remove(randomCard);
            }
        }

        public void DiscardHand()
        {
            foreach (BattleCard card in hand)
            {
                graveyard.Add(card);
            }
            hand.Clear();
        }

        public BattleCard GetCard(int index)
        {
            return hand[index];
        }

        public void SetKeyBinding(int index, Keys keyBinding)
        {
            hand[index].SetKeyBinding(keyBinding);
        }

        public int GetCurrentCooldown()
        {
            return currentCooldown;
        }

        public void SetCurrentCooldown(int cooldown)
        {
            currentCooldown = cooldown;
            fullCooldown = cooldown;
        }

        void UpdateSignals()
        {
            if (signalAnimationFrame % 6 == 0 && signalAnimationFrame != 0)
            {
                signalAnimationIndex += signalAnimationDirection;
                if (signalAnimationIndex == 2 || signalAnimationIndex == 0)
                {
                    signalAnimationDirection *= -1;
                }
               
            }
            signalAnimationFrame++;
        }

        public void Update()
        {
            if (currentCooldown > 0)
            {
                currentCooldown--;
            }
            if (cardsUsed > 2)
            {
                DiscardHand();
                GetHand();
                cardsUsed = 0;
            }

            UpdateSignals();
        }

        public void DrawHandSlots(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(handSlot, slot1, CIConsts.SLOT_SOURCE_1, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, Layers.UI_LAYER3);
            spriteBatch.Draw(handSlot, slot2, CIConsts.SLOT_SOURCE_2, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, Layers.UI_LAYER3);
            spriteBatch.Draw(handSlot, slot3, CIConsts.SLOT_SOURCE_3, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, Layers.UI_LAYER3);
            spriteBatch.Draw(handSlot, slot4, CIConsts.SLOT_SOURCE_4, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, Layers.UI_LAYER3);
            spriteBatch.Draw(handSlot, slot5, CIConsts.SLOT_SOURCE_5, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, Layers.UI_LAYER3);
        }

        public void DrawIcons(SpriteBatch spriteBatch)
        {
            hand[0].DrawIcon(spriteBatch, icon1);
            hand[1].DrawIcon(spriteBatch, icon2);
            hand[2].DrawIcon(spriteBatch, icon3);
            hand[3].DrawIcon(spriteBatch, icon4);
            hand[4].DrawIcon(spriteBatch, icon5);
        }

        private void DrawSignals(SpriteBatch spriteBatch)
        {
            int i = 0;
            foreach (BattleCard card in hand)
            {
                DrawExSignal(card, battleScene.GetExMeter(), i, spriteBatch);
                i++;
            }
        }

        private void DrawExSignal(BattleCard card, double currentEx, int index, SpriteBatch spriteBatch)
        {
            
            int cardCost = card.GetExCost();
            int yIndex = cardCost / 25 -1;
            int indexToDraw;
            if (battleScene.GetExMeter() >= cardCost &&
                card.GetUsability())
            {
                indexToDraw = signalAnimationIndex;
            }
            else
            {
                indexToDraw = 0;
            }
            Rectangle destination = new Rectangle(CIConsts.EX_SIG_START_X + (index * CIConsts.NEXT_SLOT_X),
                                      CIConsts.EX_SIG_Y, 20, 15);
            Rectangle source = new Rectangle(indexToDraw * 20, yIndex * 15, 20, 15);
            spriteBatch.Draw(exSignalTexture, destination, source, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, Layers.UI_LAYER2);
             
        }

        private void DrawCooldownShaders(SpriteBatch spriteBatch)
        {
            if (currentCooldown > 0)
            {
                float percentCooledDown = ((float)(currentCooldown)) / fullCooldown;
                for (int i = 0; i < 5; i++)
                {
                    Rectangle destination = new Rectangle(icons[i].X, icons[i].Y, icons[i].Width, (int)(icons[i].Height * percentCooledDown));
                    spriteBatch.Draw(cooldownShader, destination, new Rectangle(0, 0, 100, 100),
                                 Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, Layers.UI_LAYER4);
                }
            }
        }

        public void FinishCard(BattleCard card, BattleScene battleScene, int cooldown)
        {
            card.SetDead();
            cardsUsed++;
            battleScene.GetCardManager().SetCurrentCooldown(cooldown);
            
        }

        public void UseCard(int index, Boolean exFlag)
        {
            if (exFlag && hand[index].GetExCost() <= battleScene.GetExMeter())
            {
                hand[index].UseExCard(battleScene);
                battleScene.SubtractFromExMeter(hand[index].GetExCost());
            }
            else if (!exFlag)
            {
                hand[index].UseCard(battleScene);
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            DrawIcons(spriteBatch);
            DrawHandSlots(spriteBatch);
            DrawCooldownShaders(spriteBatch);
            DrawSignals(spriteBatch);
        }
    }
}
