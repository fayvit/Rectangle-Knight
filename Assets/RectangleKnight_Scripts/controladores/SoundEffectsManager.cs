using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundEffectsManager
{
#pragma warning disable 0649
    [SerializeField] private AudioSource[] audios;
#pragma warning restore 0649
    private List<AudioSource> ativos = new List<AudioSource>();

    public float VolumeBase { get; set; } = 0.5f;

    public void DisparaAudio(string s)
    {
        AudioSource a = RetornaMelhorCandidato();

        a.clip = (AudioClip)Resources.Load(s);
        a.volume = VolumeBase;
        a.Play();
    }

    AudioSource RetornaMelhorCandidato()
    {
        VerificaAudioAtivo();
        for (int i = 0; i < audios.Length; i++)
        {
            if (!ativos.Contains(audios[i]))
            {
                ativos.Add(audios[i]);
                return audios[i];
            }
        }

        return ativos[0];
    }

    void VerificaAudioAtivo()
    {
        for (int i = 0; i < audios.Length; i++)
        {
            if (!audios[i].isPlaying)
            {
                ativos.Remove(audios[i]);
            }
        }
    }

}
