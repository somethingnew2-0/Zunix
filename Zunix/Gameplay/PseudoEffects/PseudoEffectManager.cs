using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Zunix.Gameplay.PseudoEffects
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
        /// Active particle effects.
        /// </summary>
        private BatchRemovalCollection<PseudoEffect> activePseudoEffects =
            new BatchRemovalCollection<PseudoEffect>();


        #endregion


        #region Graphics Data


        /// <summary>
        /// The content manager used to load textures in the particle systems.
        /// </summary>
        private ContentManager contentManager;


        #endregion


        #region Initialization Methods


        /// <summary>
        /// Construct a new particle-effect manager.
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
        /// Update the particle-effect manager.
        /// </summary>
        /// <param name="elapsedTime">The amount of elapsed time, in seconds.</param>
        public void Update(float elapsedTime)
        {
            for (int i = 0; i < activePseudoEffects.Count; ++i)
            {
                if (activePseudoEffects[i].Active)
                {
                    activePseudoEffects[i].Update(elapsedTime);
                    if (!activePseudoEffects[i].Active)
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
        /// Draw all of the particle effects in the manager.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch object used to draw.</param>
        /// <param name="blendMode">Filters the systems drawn in this pass.</param>
        public virtual void Draw(SpriteBatch spriteBatch, SpriteBlendMode blendMode)
        {
            for (int i = 0; i < activePseudoEffects.Count; ++i)
            {
                if (activePseudoEffects[i].Active)
                {
                    activePseudoEffects[i].Draw(spriteBatch, blendMode);
                }
            }
        }


        #endregion


        #region Particle-Effect Creation Methods


        /// <summary>
        /// Spawn a new particle effect at a given location
        /// </summary>
        /// <param name="effectType">The effect in question.</param>
        /// <param name="position">The position of the effect.</param>
        /// <returns>The new particle effect.</returns>
        public PseudoEffect SpawnEffect(PseudoEffectType effectType,
            Vector2 position)
        {
            return SpawnEffect(effectType, position, null);
        }


        /// <summary>
        /// Spawn a new particle effect at a the position of a given gameplay object
        /// </summary>
        /// <param name="effectType">The effect in question.</param>
        /// <param name="actor">The gameplay object.</param>
        /// <returns>The new particle effect.</returns>
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
        /// Spawn a new particle effect at a given location and gameplay object
        /// </summary>
        /// <param name="effectType">The effect in question.</param>
        /// <param name="position">The position of the effect.</param>
        /// <param name="actor">The gameplay object.</param>
        /// <returns>The new particle effect.</returns>
        public PseudoEffect SpawnEffect(PseudoEffectType effectType,
            Vector2 position, GameObject gameplayObject)
        {
            PseudoEffect particleEffect = null;

            if (particleEffectCache.ContainsKey(effectType) == true)
            {
                List<PseudoEffect> availableSystems = particleEffectCache[effectType];

                for (int i = 0; i < availableSystems.Count; ++i)
                {
                    if (availableSystems[i].Active == false)
                    {
                        particleEffect = availableSystems[i];
                        break;
                    }
                }

                if (particleEffect == null)
                {
                    particleEffect = availableSystems[0].Clone();
                    particleEffect.Initialize(contentManager);
                    availableSystems.Add(particleEffect);
                }
            }

            if (particleEffect != null)
            {
                particleEffect.Reset();
                particleEffect.GameplayObject = gameplayObject;
                particleEffect.Position = position;
                activePseudoEffects.Add(particleEffect);
            }

            return particleEffect;
        }


        #endregion


    }
}