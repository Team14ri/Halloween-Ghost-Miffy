using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textBox;
        [SerializeField] private Button playDialogueButton;
        
        private void Start()
        {
            playDialogueButton.onClick.AddListener(PlayDialogue1);
        }
        
        private void PlayDialogue1() {
            PlayDialogue(_textBox, "<speed:0.03>으아악!\n<pause:0.3><speed:0.15>배.. 고... 파<pause:0.5><speed:0.05>\n너.. <pause:0.2>혹시.... <pause:0.24><anim:wave>먹을 것</anim> 좀 있어?");
        }
        
        public float waveSpeed = 7f;
        public float waveHeight = 4f;

        void Update()
        {
            AnimateWaveOnSubstring("애니메이션");
        }

        void AnimateWaveOnSubstring(string substring)
        {
            _textBox.ForceMeshUpdate();
            TMP_TextInfo textInfo = _textBox.textInfo;
            Vector3[] vertices;

            int first = _textBox.text.IndexOf(substring);
            if (first == -1) return; // Substring not found
            int last = first + substring.Length - 1;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (i < first || i > last) continue;  // Not part of the substring

                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible)
                    continue;

                vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

                for (int j = 0; j < 4; j++)
                {
                    Vector3 vertex = vertices[charInfo.vertexIndex + j];
                    vertex.y += Mathf.Sin(Time.time * waveSpeed + vertex.x * 0.1f) * waveHeight;
                    vertices[charInfo.vertexIndex + j] = vertex;
                }
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                _textBox.UpdateGeometry(meshInfo.mesh, i);
            }
        }

        private DialogueAnimator _dialogueAnimator;
        private void Awake()
        {
            _dialogueAnimator = new DialogueAnimator();
        }

        private Coroutine _typeRoutine;
        
        private void PlayDialogue(TMP_Text textBox, string value)
        {
            this.EnsureCoroutineStopped(ref _typeRoutine);
            List<DialogueUtility.Command> commands = DialogueUtility.ParseCommands(value);
            _dialogueAnimator.ChangeTextBox(textBox);
            _typeRoutine = StartCoroutine(_dialogueAnimator.AnimateTextIn(commands));
            
            // 출력 테스트
            foreach (var command in commands)
            {
                Debug.Log($"{command.commandType}, {command.textAnimationType}, \"{command.stringValue}\", {command.floatValue}, {command.startIndex}, {command.endIndex}");
            }
        }
    }
}