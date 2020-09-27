using Microsoft.Xna.Framework.Audio;

namespace PixelHunter1995
{
    class Song
    {
        private SoundEffectInstance introInstance;
        private SoundEffectInstance loopInstance;
        private bool hasIntro;

        public Song(SoundEffect intro, SoundEffect loop)
        {
            introInstance = intro.CreateInstance();
            loopInstance = loop.CreateInstance();
            hasIntro = true;
        }

        public Song(SoundEffect loop)
        {
            loopInstance = loop.CreateInstance();
            hasIntro = false;
        }

        public void Update()
        {
            if (hasIntro && introInstance.State == SoundState.Stopped)
            {
                PlayLoop();
            }
        }

        public void Start()
        {
            if (hasIntro)
            {
                PlayIntro();
            }
            else
            {
                PlayLoop();
            }
        }

        private void PlayIntro()
        {
            introInstance.Volume = 0.1f;
            introInstance.IsLooped = false;
            introInstance.Play();
        }

        private void PlayLoop()
        {
            loopInstance.Volume = 0.1f;
            loopInstance.IsLooped = true;
            loopInstance.Play();
        }

        public void Stop()
        {
            loopInstance.Stop();
        }
    }
}
