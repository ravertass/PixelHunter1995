using Microsoft.Xna.Framework.Audio;

namespace PixelHunter1995
{
    class Song
    {
        private SoundEffectInstance introInstance;
        private SoundEffectInstance loopInstance;
        private SoundEffectInstance currentInstance;

        public Song(SoundEffect intro, SoundEffect loop)
        {
            introInstance = intro.CreateInstance();
            loopInstance = loop.CreateInstance();
            PlayIntro();
        }

        public Song(SoundEffect loop)
        {
            loopInstance = loop.CreateInstance();
            PlayLoop();
        }

        public void Update()
        {
            if (introInstance != null && introInstance.State == SoundState.Stopped)
            {
                PlayLoop();
            }
        }

        private void PlayIntro()
        {
            currentInstance = introInstance;
            currentInstance.IsLooped = false;
        }

        private void PlayLoop()
        {
            currentInstance = loopInstance;
            currentInstance.IsLooped = true;
        }

        public void Start(float volume)
        {
            currentInstance.Volume = volume;
            currentInstance.Play();
        }

        public void Stop()
        {
            currentInstance.Stop();
        }

        public void SetVolume(float volume)
        {
            currentInstance.Volume = volume;
        }
    }
}
