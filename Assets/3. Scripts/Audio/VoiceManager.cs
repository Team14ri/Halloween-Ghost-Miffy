using System;
using System.Collections;
using System.Collections.Generic;
using DS.Core;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    public static VoiceManager Instance { get; private set; }
    
    private Dictionary<string, EventInstance> eventInstances = new();
    
    private Coroutine _eventRoutine;
    private float textSpeed;

    private void LoadEventReferences()
    {
        eventInstances = new Dictionary<string, EventInstance>();

        foreach (var data in VoiceReferences.instance.voiceDictionary)
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
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
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
            yield return new WaitForSeconds(textSpeed * 1.6f);
        }
    }

    public void VoicePlay(char inputText, string characterName)
    {
        char vowel = ParsingKorean(inputText);
        int conv = ConvertingVowel(vowel);

        // 발음 파라미터 설정
        eventInstances[characterName].setParameterByName("vowel type", conv);
        eventInstances[characterName].start();
    }

    private char ParsingKorean(char inputText)
    {
        const int startOfKoreanUnicode = 0xAC00;
        const int endOfKoreanUnicode = 0xD7A3;

        int charCode = (int)inputText;

        // 주어진 문자가 한글 문자인지 확인
        if (charCode >= startOfKoreanUnicode && charCode <= endOfKoreanUnicode)
        {
            // 초성은 19자, 중성은 21자, 종성은 28자
            // 따라서 중성은 ((charCode - startOfKoreanUnicode) / 종성개수) % 중성개수
            int vowelIndex = (charCode - startOfKoreanUnicode) / 28 % 21;

            // 한글 모음을 반환
            // 시작 유니코드(12623) + 모음 인덱스
            return (char)(12623 + vowelIndex);
        }
        else
        {
            Debug.Log("ParsingKorean(): 파싱하려는 글자가 한글이 아닙니다.");
            return '\0';
        }
    }

    private int ConvertingVowel(char vowel)
    {
        switch (vowel)
        {
            case 'ㅏ':
            case 'ㅑ':
                return 0;

            case 'ㅓ':
            case 'ㅕ':
                return 1;

            case 'ㅗ':
            case 'ㅛ':
                return 2;

            case 'ㅜ':
            case 'ㅠ':
            case 'ㅡ':
                return 3;

            case 'ㅣ':
            case 'ㅟ':
                return 4;

            case 'ㅐ':
            case 'ㅔ':
            case 'ㅚ':
                return 5;

            default:
                Debug.Log("ConvertingVowel(): 해당되는 모음이 없습니다.");
                return 9;
        }
    }
}