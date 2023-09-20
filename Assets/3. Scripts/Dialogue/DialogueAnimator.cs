using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DS
{
    public class DialogueAnimator : MonoBehaviour
    {
        public static DialogueAnimator Instance;
        
        private TMP_Text _textBox;
        private List<DialogueUtility.Command> _commands = new();
        private float _originTextFontSize;
        private string _renderText;

        [Header("Wave Animation Settings")]
        [SerializeField] private float WaveSpeed = 7f;
        [SerializeField] private float WaveHeight = 0.1f;
        [SerializeField] private float WaveDifference = 1f;
        
        [Header("Shake Animation Settings")]
        [SerializeField] private float ShakeMagnitude = 0.04f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        public void ChangeTextBox(TMP_Text textBox)
        {
            _textBox = textBox;
        }
        
        public void StopCurrentAnimation()
        {
            if (_textBox == null
                || _textBox.text.Length == 0)
                return;
            
            _textBox.text = "";
            _renderText = "";
            _originTextFontSize = _textBox.fontSize;
        }

        public IEnumerator AnimateTextIn(List<DialogueUtility.Command> commands)
        {
            _textBox.text = "";
            _renderText = "";
            _commands = commands;
            _originTextFontSize = _textBox.fontSize;

            float currentTextSpeed = DialogueUtility.TextAnimationSpeed["normal"];
            
            AppendToTextBox($"<size={_originTextFontSize}>");

            foreach (var command in _commands)
            {
                switch (command.commandType)
                {
                    case DialogueUtility.CommandType.Pause:
                        yield return new WaitForSeconds(command.floatValue);
                        break;
                    case DialogueUtility.CommandType.TextSpeedChange:
                        currentTextSpeed = command.floatValue;
                        break;
                    case DialogueUtility.CommandType.Size:
                        AppendToTextBox($"</size><size={_originTextFontSize * command.floatValue}>");
                        break;
                    default:
                        foreach (var t in command.stringValue)
                        {
                            AppendToTextBox($"{t}");
                            yield return new WaitForSeconds(currentTextSpeed);
                        }
                        break;
                }
            }
            
            yield return null;
        }

        private void AppendToTextBox(string text)
        {
            _renderText += text;
            _textBox.text = $"{_renderText}</size>";
        }
        
        private void Update()
        {
            if (_textBox == null
                || _textBox.text.Length == 0)
                return;
            
            _textBox.ForceMeshUpdate();
            
            TMP_TextInfo textInfo = _textBox.textInfo;
            
            foreach (var command in _commands)
            {
                if (command.commandType != DialogueUtility.CommandType.Animation
                    || command.textAnimationType == DialogueUtility.TextAnimationType.None)
                    continue;
                    
                switch (command.textAnimationType)
                {
                    case DialogueUtility.TextAnimationType.Wave:
                        AnimateWave(textInfo, command, WaveSpeed, WaveHeight);
                        break;
                    case DialogueUtility.TextAnimationType.Shake:
                        AnimateShake(textInfo, command, ShakeMagnitude);
                        break;
                }
            }
            
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                _textBox.UpdateGeometry(meshInfo.mesh, i);
            }
        }

        private void AnimateWave(TMP_TextInfo textInfo, DialogueUtility.Command command, float waveSpeed = 7f, float waveHeight = 4f)
        {
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (i < command.startIndex || i > command.endIndex) 
                    continue;

                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible)
                    continue;

                var vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

                for (int j = 0; j < 4; j++)
                {
                    Vector3 vertex = vertices[charInfo.vertexIndex + j];
                    vertex.y += Mathf.Sin(Time.time * waveSpeed + vertex.x * WaveDifference) * waveHeight;
                    vertices[charInfo.vertexIndex + j] = vertex;
                }
            }
        }
        
        private void AnimateShake(TMP_TextInfo textInfo, DialogueUtility.Command command, float shakeMagnitude = 3f)
        {
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (i < command.startIndex || i > command.endIndex) 
                    continue;

                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible)
                    continue;

                var vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

                for (int j = 0; j < 4; j++)
                {
                    Vector3 vertex = vertices[charInfo.vertexIndex + j];
                    var offsetX = Random.Range(-shakeMagnitude, shakeMagnitude);
                    var offsetY = Random.Range(-shakeMagnitude, shakeMagnitude);
                    vertex.x += offsetX;
                    vertex.y += offsetY;
                    vertices[charInfo.vertexIndex + j] = vertex;
                }
            }
        }
    }
}
