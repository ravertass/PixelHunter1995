using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PixelHunter1995
{
    class MusicManager
    {
        private static readonly string SONG_LIST_PATH = Path.Combine("Content", "Music", "songs.json");

        private Song currentSong = null;
        private string currentSongName;
        private IDictionary<string, Song> songs = new Dictionary<string, Song>();

        public void Update()
        {
            if (currentSong != null)
            {
                currentSong.Update();
            }
        }

        public void StartSong(string songName)
        {
            if (songName == currentSongName)
            {
                return;
            }

            if (currentSong != null)
            {
                currentSong.Stop();
            }

            currentSong = songs[songName];
            currentSong.Start();
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
