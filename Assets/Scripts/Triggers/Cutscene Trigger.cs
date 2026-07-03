using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using YourNewFriend;

public class CutsceneTrigger : MonoBehaviour
{
    [Tooltip("If true, the dialogue can only be triggered once")]
    [SerializeField] private bool _triggerOnce = false;
    [SerializeField] private bool _save = false;

    private bool _hasTriggered = false;

    public PlayableDirector playableDirector;
    public TimelineAsset cutscene;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("CS_SAVE")) PlayerPrefs.SetInt("CS_SAVE", 0);
    }
    public void TriggerCutscene()
    {
        if (_triggerOnce && _hasTriggered) return;
        if (PlayerPrefs.GetInt("CS_SAVE") == 1) return;

        playableDirector.Play(cutscene);
        _hasTriggered = true;
        if (_save) PlayerPrefs.SetInt("CS_SAVE", 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            TriggerCutscene();
    }
}
