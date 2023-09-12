using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DS
{
    public class DialogueUtility : MonoBehaviour
    {
        public struct Command
        {
            // Todo: ColorValue 추가하기
            public CommandType commandType;
            public TextAnimationType textAnimationType;
            public string stringValue;
            public float floatValue;
            public int startIndex;
            public int endIndex;
        }

        public enum CommandType
        {
            Normal,
            Pause,
            TextSpeedChange,
            Animation
        }

        public enum TextAnimationType
        {
            None,
            Shake,
            Wave
        }

        public static readonly Dictionary<string, float> TextAnimationSpeed = new()
        {
            { "tiny", .1f },
            { "short", .25f },
            { "normal", 0.666f },
            { "long", 1f },
            { "read", 2f },
        };

        public static List<Command> ParseCommands(string input)
        {
            List<Command> commands = new List<Command>();
            float currentSpeed = 1f;
            int currentIndex = 0;

            while (input.Length > 0)
            {
                if (input.StartsWith("<speed:"))
                {
                    var (command, len, speed) = ParseSpeedTag(input, currentIndex);
                    commands.Add(command);
                    input = input.Substring(len);
                    currentSpeed = speed;
                }
                else if (input.StartsWith("<anim:"))
                {
                    var (command, len, idx) = ParseAnimTag(input, currentIndex, currentSpeed);
                    commands.Add(command);
                    input = input.Substring(len);
                    currentIndex = idx;
                }
                else if (input.StartsWith("<pause:"))
                {
                    var (command, len) = ParsePauseTag(input, currentIndex);
                    commands.Add(command);
                    input = input.Substring(len);
                }
                else
                {
                    var (command, len, idx) = ParseNormalText(input, currentIndex, currentSpeed);
                    commands.Add(command);
                    input = input.Substring(len);
                    currentIndex = idx;
                }
            }

            return commands;
        }

        private static (Command, int, float) ParseSpeedTag(string input, int currentIndex)
        {
            var match = Regex.Match(input, @"<speed:([\d]+(\.\d+)?)>");
            var newSpeed = float.Parse(match.Groups[1].Value);
            var command = new Command
            {
                commandType = CommandType.TextSpeedChange,
                textAnimationType = TextAnimationType.None,
                stringValue = "",
                floatValue = newSpeed,
                startIndex = currentIndex,
                endIndex = currentIndex
            };
            return (command, match.Length, newSpeed);
        }

        private static (Command, int, int) ParseAnimTag(string input, int currentIndex, float currentSpeed)
        {
            var match = Regex.Match(input, @"<anim:(\w+)>([^<]+)</anim>");
            var animation = match.Groups[1].Value;
            var newIndex = currentIndex + match.Groups[2].Value.Length - 1;
            var command = new Command
            {
                commandType = CommandType.Animation,
                textAnimationType = Enum.Parse<TextAnimationType>(animation, true),
                stringValue = match.Groups[2].Value,
                floatValue = currentSpeed,
                startIndex = currentIndex,
                endIndex = newIndex
            };
            return (command, match.Length, newIndex);
        }

        private static (Command, int) ParsePauseTag(string input, int currentIndex)
        {
            var match = Regex.Match(input, @"<pause:([\d]+(\.\d+)?)>");
            var command = new Command
            {
                commandType = CommandType.Pause,
                textAnimationType = TextAnimationType.None,
                stringValue = "",
                floatValue = float.Parse(match.Groups[1].Value),
                startIndex = currentIndex,
                endIndex = currentIndex
            };
            return (command, match.Length);
        }

        private static (Command, int, int) ParseNormalText(string input, int currentIndex, float currentSpeed)
        {
            var match = Regex.Match(input, @"[^<]+");
            var newIndex = currentIndex + match.Value.Length - 1;
            var command = new Command
            {
                commandType = CommandType.Normal,
                textAnimationType = TextAnimationType.None,
                stringValue = match.Value,
                floatValue = currentSpeed,
                startIndex = currentIndex,
                endIndex = newIndex
            };
            return (command, match.Length, newIndex);
        }
    }
}
