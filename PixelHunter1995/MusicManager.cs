using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PixelHunter1995.Utilities;

namespace PixelHunter1995
{
    class MusicManager
    {
        private static readonly string SONG_LIST_PATH = Path.Combine("Content", "Music", "songs.json");
        private float VOLUME_DELTA => GlobalSettings.MAX_VOLUME / 60.0F;

        private Song currentSong = null;
        private string currentSongName;
        private IDictionary<string, Song> songs = new Dictionary<string, Song>();

        private string NextSongName;
        private float Volume = GlobalSettings.MAX_VOLUME;
        private bool Transitioning = false;
        private float VolumeDelta;

        public void Update()
        {
            if (currentSong != null)
            {
                currentSong.Update();
                currentSong.SetVolume(Volume);
            }

            if (Transitioning)
            {
                Volume += VolumeDelta;
                if (Volume <= 0.0F)
                {
                    Volume = 0.0F;
                    VolumeDelta = VOLUME_DELTA;
                    StartSong(NextSongName);
                }
                if (Volume >= GlobalSettings.MAX_VOLUME)
                {
                    Volume = GlobalSettings.MAX_VOLUME;
                    Transitioning = false;
                }
            }
        }

        public void ChangeSong(string songName)
        {
            if (songName == currentSongName)
            {
                return;
            }

            if (currentSong == null)
            {
                StartSong(songName);
            }
            else
            {
                NextSongName = songName;
                Transitioning = true;
                VolumeDelta = -VOLUME_DELTA;
            }
        }

        private void StartSong(string songName)
        {
            if (currentSong != null)
            {
                currentSong.Stop();
            }

            currentSong = songs[songName];
            currentSong.Start(Volume);
            currentSongName = songName;
        }

        public void LoadContent(ContentManager content)
        {
            ParseSongs(content);
        }

        private void ParseSongs(ContentManager content)
        {
            using (StreamReader file = File.OpenText(SONG_LIST_PATH))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject songList = (JObject)JToken.ReadFrom(reader);
                foreach (var song in songList)
                {
                    string songName = song.Key;
                    JObject songObject = song.Value as JObject;
                    songs[songName] = ParseSong(content, songObject);
                }
            }
        }

        private static Song ParseSong(ContentManager content, JObject songObject)
        {
            if (songObject.ContainsKey("intro"))
            {
                string introName = songObject["intro"].ToObject<string>();
                SoundEffect intro = content.Load<SoundEffect>(Path.Combine("Music", introName));

                string loopName = songObject["loop"].ToObject<string>();
                SoundEffect loop = content.Load<SoundEffect>(Path.Combine("Music", loopName));

                return new Song(intro, loop);
            }
            else
            {
                string loopName = songObject["loop"].ToObject<string>();
                SoundEffect loop = content.Load<SoundEffect>(Path.Combine("Music", loopName));

                return new Song(loop);
            }
        }
    }
}
