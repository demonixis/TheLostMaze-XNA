using System;

namespace Maze3D.Data
{
#if !NETFX_CORE && !WINDOWS_PHONE
    [Serializable]
#endif
    public struct MessageBoxTextItem
    {
        public string Label { get; set; }
        public string Content { get; set; }
    }

#if !NETFX_CORE && !WINDOWS_PHONE
    [Serializable]
#endif
    public struct MenuText
    {
        public string New { get; set; }
        public string Continue { get; set; }
        public string Choose { get; set; }
        public string Options { get; set; }
        public string HowTo { get; set; }
        public string Credits { get; set; }
        public string Exit { get; set; }
    }

#if !NETFX_CORE && !WINDOWS_PHONE
    [Serializable]
#endif
    public struct MessageBoxText
    {
        public MessageBoxTextItem GameFinished { get; set; }
        public MessageBoxTextItem LevelFinished { get; set; }
        public MessageBoxTextItem ConfirmQuit { get; set; }
        public string ConfirmAction { get; set; }
        public string CancelAction { get; set; }
        public string MenuAction { get; set; }
        public string NewPlusAction { get; set; }
        public string NextAction { get; set; }
    }

#if !NETFX_CORE && !WINDOWS_PHONE
    [Serializable]
#endif
    public struct CreditText
    {
        public string Title { get; set; }
        public MessageBoxTextItem Description { get; set; }
        public MessageBoxTextItem Greeting { get; set; }
    }

#if !NETFX_CORE && !WINDOWS_PHONE
    [Serializable]
#endif
    public class GameText
    {
        public string Lang { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public MenuText Menus;
        public MessageBoxText Messages;
        public CreditText Credits;

        public GameText()
        {
            Lang = "en";
            Name = "The Lost Maze";
            Version = "2.0.0.0";

            Menus = new MenuText()
            {
                Choose = "Levels",
                Continue = "Continue",
                Credits = "Credits",
                Exit = "Exit",
                HowTo = "Help",
                New = "New game",
                Options = "Options"
            };

            Messages = new MessageBoxText()
            {
                CancelAction = "Cancel",
                ConfirmAction = "Ok",
                MenuAction = "Menu",
                NewPlusAction = "New +",
                NextAction = "Next",
                ConfirmQuit = new MessageBoxTextItem()
                {
                    Label = "Quit the current game",
                    Content = "Do you want to quit the current game ?"
                },
                GameFinished = new MessageBoxTextItem()
                {
                    Label = "Game done",
                    Content = "Congratulations you have completed the game, you have {0}\nWhy not start with a higher difficulty mode?"
                },
                LevelFinished = new MessageBoxTextItem()
                {
                    Label = "Level done !",
                    Content = "You have completed the level with {0} {1} \n\nGo to the next level or return to the menu?"
                }
            };

            string description = "The lost maze is an exploration game where the player is trapped in a maze and must get out. During his exploration, he must collect crystals to increase its score.";
            description += "\n\nProgramming and Game Design: Yannick Comte";
            description += "\n\nGraphics and music: Christophe Belleuvre";

            string greeting = "I also thank all those who have supported me and who have tested this game";
            greeting += "\n\nFinally thank you to you to play The Lost Maze, have fun :)";

            Credits = new CreditText()
            {
                Title = "About",
                Description = new MessageBoxTextItem()
                {
                    Label = "About this game",
                    Content = ""
                },
                Greeting = new MessageBoxTextItem()
                {
                    Label = "Greeting",
                    Content = ""
                }
            };
        }
    }
}
