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

    public float VolumeBase { get; set; } = 0.95f;

    public void Instantiate3dSound(Vector3 pos, AudioClip som, float spartial = 1)
    {
        AudioSource a = (AudioSource)MonoBehaviour.Instantiate(audios[0], pos, Quaternion.identity);

        a.clip = som;
        a.volume = VolumeBase;
        a.Play();
        a.spatialBlend = spartial;

        MonoBehaviour.Destroy(a, 2 * a.clip.length);
    }

    public void Instantiate3dSound(Vector3 pos, SoundEffectID som, float spartial = 1)
    {
        Instantiate3dSound(pos, (AudioClip)Resources.Load(som.ToString()), spartial);
    }

    public void DisparaAudio(SoundEffectID s)
    {
        DisparaAudio(s.ToString());
    }

    public void DisparaAudio(string s)
    {
        DisparaAudio((AudioClip)Resources.Load(s));
    }

    public void DisparaAudio(AudioClip s)
    {
        AudioSource a = RetornaMelhorCandidato();

        a.clip = s;
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
