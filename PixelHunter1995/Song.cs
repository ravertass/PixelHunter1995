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
        }

        public Song(SoundEffect loop)
        {
            loopInstance = loop.CreateInstance();
        }

        public void Update()
        {
            if (introInstance != null && introInstance.State == SoundState.Stopped)
            {
                SetLoop();
                currentInstance.Play();
            }
        }

        private void SetIntro()
        {
            currentInstance = introInstance;
            currentInstance.IsLooped = false;
        }

        private void SetLoop()
        {
            currentInstance = loopInstance;
            currentInstance.IsLooped = true;
        }

        public void Start(float volume)
        {
            if (introInstance != null)
            {
                SetIntro();
            }
            else
            {
                SetLoop();
            }

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
