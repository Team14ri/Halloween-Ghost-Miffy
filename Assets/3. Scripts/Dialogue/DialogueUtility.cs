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

        public static List<Command> ParseCommands(string input)
        {
            List<Command> commands = new List<Command>();
            float currentSpeed = 1f;

            while (input.Length > 0)
            {
                if (input.StartsWith("<speed:"))
                {
                    var (command, len) = ParseSpeedTag(input, currentSpeed);
                    commands.Add(command);
                    input = input.Substring(len);
                }
                else if (input.StartsWith("<anim:"))
                {
                    var (command, len) = ParseAnimTag(input, currentSpeed);
                    commands.Add(command);
                    input = input.Substring(len);
                }
                else if (input.StartsWith("<pause:"))
                {
                    var (command, len) = ParsePauseTag(input);
                    commands.Add(command);
                    input = input.Substring(len);
                }
                else
                {
                    var (command, len) = ParseNormalText(input, currentSpeed);
                    commands.Add(command);
                    input = input.Substring(len);
                }
            }

            return commands;
        }

        private static (Command, int) ParseSpeedTag(string input, float currentSpeed)
        {
            var match = Regex.Match(input, @"<speed:(\d+)>");
            currentSpeed = float.Parse(match.Groups[1].Value);
            var command = new Command
            {
                commandType = CommandType.TextSpeedChange,
                textAnimationType = TextAnimationType.None,
                stringValue = "",
                floatValue = currentSpeed
            };
            return (command, match.Length);
        }

        private static (Command, int) ParseAnimTag(string input, float currentSpeed)
        {
            var match = Regex.Match(input, @"<anim:(\w+)>([^<]+)</anim>");
            var animation = match.Groups[1].Value;
            var command = new Command
            {
                commandType = CommandType.Animation,
                textAnimationType = Enum.Parse<TextAnimationType>(animation, true),
                stringValue = match.Groups[2].Value,
                floatValue = currentSpeed
            };
            return (command, match.Length);
        }

        private static (Command, int) ParsePauseTag(string input)
        {
            var match = Regex.Match(input, @"<pause:(\d+)>");
            var command = new Command
            {
                commandType = CommandType.Pause,
                textAnimationType = TextAnimationType.None,
                stringValue = "",
                floatValue = float.Parse(match.Groups[1].Value)
            };
            return (command, match.Length);
        }

        private static (Command, int) ParseNormalText(string input, float currentSpeed)
        {
            var match = Regex.Match(input, @"[^<]+");
            var command = new Command
            {
                commandType = CommandType.Normal,
                textAnimationType = TextAnimationType.None,
                stringValue = match.Value,
                floatValue = currentSpeed
            };
            return (command, match.Length);
        }
    }
}
