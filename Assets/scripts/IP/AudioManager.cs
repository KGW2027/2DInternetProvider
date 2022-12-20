using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

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
            ConstructStart, ConstructEnd, LoanConfirm, Unknown
        }

        private readonly Dictionary<Audios, AudioClip> _audios;

        public AudioManager()
        {
            _audios = new Dictionary<Audios, AudioClip>();
            AudioClip[] clips = Resources.LoadAll<AudioClip>($"audio");
            foreach (AudioClip clip in clips)
            {
                Audios key = GetEnumByName(clip.name);
                Debug.Log($"{clip.name} -> {key.ToString()}");
                if(key == Audios.Unknown) continue;
                _audios[key] = clip;
            }
        }

        public void PlayOneShot(Audios type)
        {
            if(_audios.ContainsKey(type)) AudioSource.PlayClipAtPoint(_audios[type], Vector3.zero);
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
            }

            return Audios.Unknown;
        }
    }
}