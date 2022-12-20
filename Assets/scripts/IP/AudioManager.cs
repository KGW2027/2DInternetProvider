using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IP
{
    public class AudioManager
    {
        private static AudioManager _instance;
        public static AudioManager Instance
        {
            get
            {
                if (_instance == null) _instance = new AudioManager();
                return _instance;
            }
        }

        public enum Audios
        {
            Unknown,
            ConstructStart, ConstructEnd, LoanConfirm, BGM, GameOver
        }

        private readonly Dictionary<Audios, AudioClip> _audios;

        private AudioSource bgmSource;

        public AudioManager()
        {
            _audios = new Dictionary<Audios, AudioClip>();
            AudioClip[] clips = Resources.LoadAll<AudioClip>($"audio");
            foreach (AudioClip clip in clips)
            {
                Audios key = GetEnumByName(clip.name);
                if(key == Audios.Unknown) continue;
                _audios[key] = clip;
            }
        }

        public void PlayOneShot(Audios type)
        {
            if(_audios.ContainsKey(type)) AudioSource.PlayClipAtPoint(_audios[type], Vector3.zero);
        }

        public void RunBGM()
        {
            if (bgmSource == null)
            {
                bgmSource = Object.FindObjectOfType<AudioSource>();
            }
            
            bgmSource.Play();
        }

        public void StopBGM()
        {
            bgmSource.Stop();
        }

        private Audios GetEnumByName(string name)
        {
            switch (name.ToLower())
            {
                case "construct_confirm":
                    return Audios.ConstructStart;
                case "construct_end":
                    return Audios.ConstructEnd;
                case "loan_confirm":
                    return Audios.LoanConfirm;
                case "backgroundmusic":
                    return Audios.BGM;
                case "gameover":
                    return Audios.GameOver;
            }

            return Audios.Unknown;
        }
    }
}