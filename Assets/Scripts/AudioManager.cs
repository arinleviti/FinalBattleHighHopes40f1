using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public SoundEffect[] effects;
    private Dictionary<string, SoundEffect> _effectDictionary;
    private AudioListener _listener;

    protected override void Awake ()
    {
        _effectDictionary = new Dictionary<string, SoundEffect>();
        foreach (var effect in effects)
        {
            Debug.LogFormat("Registered effect {0}", effect.name);
            _effectDictionary[effect.name] = effect;
        }
    }
    public void PlayEffect(string effectName, float delay)
    {
        if (_listener != null)
        {
            _listener = FindObjectOfType<AudioListener>();
        }
        PlayEffect(effectName, _listener.transform.position, delay);
    }
    public void PlayEffect(string effectName, Vector3 worldPosition, float delay)
    {
        if (_effectDictionary.ContainsKey(effectName) == false)
        {
            Debug.LogWarningFormat("Effect {0} is not registered.", effectName);
            return;
        }
        AudioClip clip = _effectDictionary[effectName].GetRandomClip();
        if (clip == null)
        {
            Debug.LogWarningFormat("Effect {0} has no clips to play.", effectName);
            return;
        }      
        StartCoroutine(PlayWithDelay(clip, worldPosition, delay));
    }
    private IEnumerator PlayWithDelay( AudioClip clip, Vector3 worldPosition, float delay)
    {
        yield return new WaitForSeconds(delay);
		AudioSource.PlayClipAtPoint(clip, worldPosition);
	}
}
