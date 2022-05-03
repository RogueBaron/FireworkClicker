using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace FireworkClicker
{
    public class StarsParticleSystem : ParticleSystem
    {
        Rectangle _source;

        public bool IsRaining { get; set; } = true;

        public StarsParticleSystem(Game game, Rectangle source) : base(game, 5000)
        {
            _source = source;
        }

        protected override void InitializeConstants()
        {
            textureFilename = "circle";
            minNumParticles = 0;
            maxNumParticles = 3;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            p.Initialize(where, Vector2.UnitY * 20, Vector2.Zero, Color.White, scale: RandomHelper.NextFloat(0.02f, 0.15f), lifetime: 5);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(IsRaining) AddParticles(_source);
        }
    }
}
