namespace PixelHunter1995.Inputs
{
    /// <summary>
    /// Ideally, anything could be bound,
    /// and all 'inputs' were 'actions' instead of 'keys'
    /// Although mouse-position might be tricky to do so for :/
    /// And for writing/text-input it obviosuly wont work well.
    /// 
    /// Rename stuff (please give inputs! pun intended)
    /// </summary>
    public enum Action
    {
        // Global
        Exit,
        ToggleFullscreen,
        QuickSave,
        QuickLoad,
        // Menu
        MENU_Exit,
        MENU_Accept,
        Back, // can be held?
        Resume, // is this equivalent to Action.Back when at the root?
        Decline,
        MenuNext, // can be held? // up/down?
        MenuPrior, // can be held? 
        MenuRight, // can be held // right/left? Like when controlling volume in options with arrowkeys
        MenuLeft, // can be held
        // Playing
        PLAYING_Pause,
        PLAYING_Move,
        MouseLeft, // nothing says it has to be a mouse-button
        MouseRight,
        InventoryScrollUp,
        InventoryScrollDown,
        MonkeyIslandGive,// dummy actions for debugging purposes
        MonkeyIslandPickUp,
        MonkeyIslandUse,
        MonkeyIslandOpen,
        MonkeyIslandLookAt,
        MonkeyIslandPush,
        MonkeyIslandClose,
        MonkeyIslandTalkTo,
        MonkeyIslandPull,
        MoveUp, // Should it be possible to control without a mouse? Would this move the pointer then, or the character?
        MoveDown,
        MoveLeft,
        MoveRight,
        // Cutscene (dialog, credits, actual cutscenes, etc.) Most of those are actually same key/action, but writing down everything so I know what to think about.
        Skip, // skip to next part player can do anything
        JumpAhead, // next dialog please!
        Complete, // skip animation (ie. writing-animation, while remaining on same dialog to read it)!
        FastForward, // Hold to remove (most) delays
        HistoryBack, // Missed something?
        HistoryForward // same as jump ahead? Should not go further than where history started, if held
    }
}
