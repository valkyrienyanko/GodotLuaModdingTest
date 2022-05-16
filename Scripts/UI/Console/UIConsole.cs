using Godot;
using System;

namespace GodotModules
{
    public class UIConsole : Control
    {
        [Export] public readonly NodePath NodePathConsoleLogs;
        [Export] public readonly NodePath NodePathConsoleInput;
        private TextEdit ConsoleLogs;
        private LineEdit ConsoleInput;

        public override void _Ready()
        {
            ConsoleLogs = GetNode<TextEdit>(NodePathConsoleLogs);
            ConsoleInput = GetNode<LineEdit>(NodePathConsoleInput);
        }

        public void AddException(Exception e) => AddMessage($"{e.Message}\n{e.StackTrace}");

        public void AddMessage(string message)
        {
            ConsoleLogs.Text += $"{message}\n";
            ScrollToBottom();
        }

        public void ToggleVisibility()
        {
            Visible = !Visible;
            ConsoleInput.GrabFocus();
            ScrollToBottom();
        }

        private void ScrollToBottom() => ConsoleLogs.ScrollVertical = Mathf.Inf;

        private void _on_Console_Input_text_entered(string text)
        {
            var inputArr = text.Trim().ToLower().Split(' ');
            var cmd = inputArr[0];

            if (string.IsNullOrWhiteSpace(cmd))
                return;

            var command = Command.Instances.FirstOrDefault(x => x.IsMatch(cmd));

            if (command != null)
                command.Run(inputArr.Skip(1).ToArray());
            else
                Logger.Log($"The command '{cmd}' does not exist");

            ConsoleInput.Clear();
        }
    }
}