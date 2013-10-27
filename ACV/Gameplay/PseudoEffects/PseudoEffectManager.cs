using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ACV
{
    class PseudoEffectManager
    {
        #region Effect Collection Data

        /// <summary>
        /// Cache of registered pseudo effects.
        /// </summary>
        private Dictionary<PseudoEffectType, List<PseudoEffect>> pseudoEffectCache
            = new Dictionary<PseudoEffectType, List<PseudoEffect>>();

        /// <summary>
        /// Alive pseudoeffects.
        /// </summary>
        private BatchRemovalCollection<PseudoEffect> activePseudoEffects =
            new BatchRemovalCollection<PseudoEffect>();


        #endregion


        #region Graphics Data


        /// <summary>
        /// The content manager used to load textures in the pseudosystems.
        /// </summary>
        private ContentManager contentManager;


        #endregion


        #region Initialization Methods


        /// <summary>
        /// Construct a new pseudo-effect manager.
        /// </summary>
        public PseudoEffectManager(ContentManager contentManager)
        {
            // safety-check the parameters
            if (contentManager == null)
            {
                throw new ArgumentNullException("contentManager");
            }

            this.contentManager = contentManager;
        }


        #endregion


        #region Updating Methods


        /// <summary>
        /// Update the pseudo-effect manager.
        /// </summary>
        /// <param name="elapsedTime">The amount of elapsed time, in seconds.</param>
        public void Update(float elapsedTime)
        {
            for (int i = 0; i < activePseudoEffects.Count; ++i)
            {
                if (activePseudoEffects[i].Alive)
                {
                    activePseudoEffects[i].Update(elapsedTime);
                    if (!activePseudoEffects[i].Alive)
                    {
                        activePseudoEffects.QueuePendingRemoval(
                            activePseudoEffects[i]);
                    }
                }
            }
            activePseudoEffects.ApplyPendingRemovals();
        }


        #endregion


        #region Drawing Methods


        /// <summary>
        /// Draw all of the pseudoeffects in the manager.
        /// </summary>
        /// <param name="elapsedTime">The amount of elapsed time, in seconds.</param>
        /// <param name="spriteBatch">The SpriteBatch object used to draw.</param>
        public virtual void Draw(float elapasedTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < activePseudoEffects.Count; ++i)
            {
                if (activePseudoEffects[i].Alive)
                {
                    // TODO: Maybe code it so the alpha fades when an psuedo effect is killed or created
                    activePseudoEffects[i].Draw(elapasedTime, spriteBatch, activePseudoEffects[i].Sprite, Color.White);
                }
            }
        }


        #endregion


        #region Particle-Effect Creation Methods


        /// <summary>
        /// Spawn a new pseudoeffect at a given location
        /// </summary>
        /// <param name="effectType">The effect in question.</param>
        /// <param name="position">The position of the effect.</param>
        /// <returns>The new pseudoeffect.</returns>
        public PseudoEffect SpawnEffect(PseudoEffectType effectType,
            Vector2 position)
        {
            return SpawnEffect(effectType, position, null);
        }


        /// <summary>
        /// Spawn a new pseudoeffect at a the position of a given gameplay object
        /// </summary>
        /// <param name="effectType">The effect in question.</param>
        /// <param name="actor">The gameplay object.</param>
        /// <returns>The new pseudoeffect.</returns>
        public PseudoEffect SpawnEffect(PseudoEffectType effectType,
            GameObject gameplayObject)
        {
            // safety-check the parameter
            if (gameplayObject == null)
            {
                throw new ArgumentNullException("gameplayObject");
            }

            return SpawnEffect(effectType, gameplayObject.Position, gameplayObject);
        }


        /// <summary>
        /// Spawn a new pseudoeffect at a given location and gameplay object
        /// </summary>
        /// <param name="effectType">The effect in question.</param>
        /// <param name="position">The position of the effect.</param>
        /// <param name="actor">The gameplay object.</param>
        /// <returns>The new pseudoeffect.</returns>
        public PseudoEffect SpawnEffect(PseudoEffectType effectType,
            Vector2 position, GameObject gameplayObject)
        {
            PseudoEffect pseudoEffect = null;


            // TODO: Code it so the effectType set the gameObject.Sprite
            if (pseudoEffectCache.ContainsKey(effectType) == true)
            {
                List<PseudoEffect> availableSystems = pseudoEffectCache[effectType];

                for (int i = 0; i < availableSystems.Count; ++i)
                {
                    if (availableSystems[i].Alive == false)
                    {
                        pseudoEffect = availableSystems[i];
                        break;
                    }
                }

                if (pseudoEffect == null)
                {
                    pseudoEffect = new PseudoEffect();
                    availableSystems.Add(pseudoEffect);
                }
            }

            if (pseudoEffect != null)
            {
                pseudoEffect.GameObject = gameplayObject;
                pseudoEffect.Position = position;
                activePseudoEffects.Add(pseudoEffect);
            }

            return pseudoEffect;
        }


        #endregion


    }
}