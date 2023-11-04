using System;
using System.Collections;
using System.Collections.Generic;
using DS.Core;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Random = UnityEngine.Random;

public class VoiceManager : MonoBehaviour
{
    public static VoiceManager Instance { get; private set; }
    
    private Dictionary<string, EventInstance> eventInstances = new();
    
    private Coroutine _eventRoutine;
    private float textSpeed;

    private void LoadEventReferences()
    {
        eventInstances = new Dictionary<string, EventInstance>();

        foreach (var data in VoiceReferences.Instance.VoiceDictionary)
        {
            string eventName = data.Key;
            EventReference eventReference = data.Value;
            
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            eventInstances[eventName] = eventInstance;
        }
    }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("한 씬에 VoiceManager가 여러 개 있습니다.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    private void Start()
    {
        LoadEventReferences();
    }

    public void VoiceSentenceIn(List<DialogueUtility.Command> commands)
    {
        this.EnsureCoroutineStopped(ref _eventRoutine);
        _eventRoutine = StartCoroutine(VoiceSentenceExecute(commands));
    }

    private IEnumerator VoiceSentenceExecute(List<DialogueUtility.Command> commands)
    {
        textSpeed = DialogueUtility.TextAnimationSpeed["normal"];
        foreach (var command in commands)
        {
            yield return ExecuteCommand(command);
        }
    }
    
    private IEnumerator ExecuteCommand(DialogueUtility.Command command)
    {
        switch (command.commandType)
        {
            case DialogueUtility.CommandType.Pause:
                yield return new WaitForSeconds(command.floatValue);
                break;
            case DialogueUtility.CommandType.TextSpeedChange:
                textSpeed = command.floatValue;
                break;
            case DialogueUtility.CommandType.Size:
                break;
            case DialogueUtility.CommandType.State:
                break;
            default:
                yield return RunSentence(command.stringValue);
                break;
        }
    }
    
    private IEnumerator RunSentence(string sentence)
    {
        foreach (var ch in sentence)
        {
            VoicePlay(ch, "Miffy");
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void VoicePlay(char inputText, string characterName)
    {
        char vowel = ParsingKorean(inputText);
        int conv = ConvertingInitialConsonant(vowel);
        
        Debug.Log($"{inputText} => {vowel} => {conv}");

        // 발음 파라미터 설정
        eventInstances[characterName].setParameterByName("vowel type", conv);
        //eventInstances[characterName].setPitch(2.8f * Random.Range(0.96f, 1.15f));
        eventInstances[characterName].start();
    }

    private char ParsingKorean(char hangul)
    {
        char[] initialConsonants = 
        {
            'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ'
        };
        
        if (hangul < '가' || hangul > '힣')  // 한글 범위 외의 문자인 경우
        {
            Debug.LogWarning("Given character is not a valid Hangul syllable.");
            return '\0';
        }

        int initialIndex = (hangul - '가') / (21 * 28);
        return initialConsonants[initialIndex];
    }
    
    private int ConvertingInitialConsonant(char initialConsonant)
    {
        switch (initialConsonant)
        {
            case 'ㄱ':
                return 0;
            case 'ㄲ':
                return 1;
            case 'ㄴ':
                return 2;
            case 'ㄷ':
                return 3;
            case 'ㄸ':
                return 4;
            case 'ㄹ':
                return 5;
            case 'ㅁ':
                return 6;
            case 'ㅂ':
                return 7;
            case 'ㅃ':
                return 8;
            case 'ㅅ':
                return 9;
            case 'ㅆ':
                return 10;
            case 'ㅇ':
                return 11;
            case 'ㅈ':
                return 12;
            case 'ㅉ':
                return 13;
            case 'ㅊ':
                return 14;
            case 'ㅋ':
                return 15;
            case 'ㅌ':
                return 16;
            case 'ㅍ':
                return 17;
            case 'ㅎ':
                return 18;
            default:
                Debug.Log("ConvertingInitialConsonant(): 해당되는 초성이 없습니다.");
                return 19;
        }
    }
}