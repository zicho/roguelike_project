using Godot;

namespace Helpers
{
    public static class GameHelper
    {
        public static RichTextLabel MessageLog { get; set; }
        public static void ShowMessage(string msg) { 
            MessageLog.Newline();
            MessageLog.AddText(msg);
        }
    }
}