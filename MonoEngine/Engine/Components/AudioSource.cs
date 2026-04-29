using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoGameTemplate.Managers;

namespace MonoGameTemplate.Engine.Components
{
    public class AudioSource : Component
    {
        private AudioManager _audioManager;
        private SoundEffectInstance _instance;
        private string _clipAssetName;

        public string ClipAssetName => _clipAssetName;
        public bool PlayOnStart { get; set; }
        public bool IsLooped { get; set; }
        public bool Enable3D { get; set; } = true;
        public float Volume { get; set; } = 1f;
        public float Pitch { get; set; }
        public float Pan { get; set; }
        public AudioEmitter Emitter { get; } = new AudioEmitter();
        public SoundEffectInstance Instance => _instance;

        public override void OnAdded()
        {
            _audioManager = MainGame.Instance?.GetManager<AudioManager>() ??
                            throw new System.InvalidOperationException("AudioManager must be registered before using AudioSource.");
            if (PlayOnStart && !string.IsNullOrWhiteSpace(_clipAssetName))
                Play();
        }

        public override void OnRemoved()
        {
            DisposeInstance();
            _audioManager = null;
        }

        public override void OnUpdate(GameTime gameTime)
        {
            if (_instance == null)
                return;

            _instance.Volume = MathHelper.Clamp(Volume, 0f, 1f);
            _instance.Pitch = MathHelper.Clamp(Pitch, -1f, 1f);
            _instance.Pan = MathHelper.Clamp(Pan, -1f, 1f);
            _instance.IsLooped = IsLooped;

            if (Enable3D && _instance.State != SoundState.Stopped)
                _audioManager.Update3D(this);
        }

        public void SetClip(string clipAssetName)
        {
            if (string.IsNullOrWhiteSpace(clipAssetName))
                throw new System.ArgumentException("Clip asset name cannot be null or empty.", nameof(clipAssetName));

            bool wasPlaying = _instance != null && _instance.State == SoundState.Playing;
            _clipAssetName = clipAssetName;
            DisposeInstance();

            if (wasPlaying)
                Play();
        }

        public void Play()
        {
            EnsureClipSet();

            if (_instance == null || _instance.IsDisposed)
            {
                _instance = _audioManager.CreateInstance(_clipAssetName, IsLooped);
            }
            else if (_instance.State == SoundState.Playing)
            {
                _instance.Stop();
            }

            _instance.Volume = MathHelper.Clamp(Volume, 0f, 1f);
            _instance.Pitch = MathHelper.Clamp(Pitch, -1f, 1f);
            _instance.Pan = MathHelper.Clamp(Pan, -1f, 1f);
            _instance.IsLooped = IsLooped;
            _instance.Play();

            if (Enable3D)
                _audioManager.Update3D(this);
        }

        public void Pause()
        {
            if (_instance?.State == SoundState.Playing)
                _instance.Pause();
        }

        public void Resume()
        {
            if (_instance?.State == SoundState.Paused)
                _instance.Resume();
        }

        public void Stop()
        {
            if (_instance != null && !_instance.IsDisposed)
                _instance.Stop();
        }

        private void EnsureClipSet()
        {
            if (string.IsNullOrWhiteSpace(_clipAssetName))
                throw new System.InvalidOperationException("No clip set. Call SetClip before Play.");
        }

        private void DisposeInstance()
        {
            if (_instance == null)
                return;
            if (!_instance.IsDisposed)
                _instance.Dispose();
            _instance = null;
        }
    }
}
