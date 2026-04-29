using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoGameTemplate.Engine.Components;

namespace MonoGameTemplate.Managers
{
    public class AudioManager : ManagerBase
    {
        private MainGame _game;
        private readonly Dictionary<string, SoundEffect> _loadedSounds = new();
        private readonly AudioListener _listener = new();

        public float MasterVolume
        {
            get => SoundEffect.MasterVolume;
            set => SoundEffect.MasterVolume = MathHelper.Clamp(value, 0f, 1f);
        }

        public override void OnInit()
        {
            _game = MainGame.Instance ?? throw new System.InvalidOperationException("MainGame.Instance is not initialized.");
        }

        public SoundEffect GetOrLoadSound(string assetName)
        {
            if (string.IsNullOrWhiteSpace(assetName))
                throw new System.ArgumentException("Asset name cannot be null or empty.", nameof(assetName));

            if (_loadedSounds.TryGetValue(assetName, out SoundEffect soundEffect))
                return soundEffect;

            soundEffect = _game.Content.Load<SoundEffect>(assetName);
            _loadedSounds[assetName] = soundEffect;
            return soundEffect;
        }

        public SoundEffectInstance CreateInstance(string assetName, bool isLooped)
        {
            SoundEffect sound = GetOrLoadSound(assetName);
            SoundEffectInstance instance = sound.CreateInstance();
            instance.IsLooped = isLooped;
            return instance;
        }

        public void Update3D(AudioSource source)
        {
            if (source == null)
                throw new System.ArgumentNullException(nameof(source));
            if (!source.Enable3D)
                return;
                
            AudioEmitter emitter = source.Emitter;
            emitter.Position = source.Entity.GetWorldMatrix().Translation;
            emitter.Forward = Vector3.Transform(Vector3.Forward, source.Transform.Rotation);
            emitter.Up = Vector3.Transform(Vector3.Up, source.Transform.Rotation);
            source.Instance.Apply3D(_listener, emitter);
        }
    }
}
