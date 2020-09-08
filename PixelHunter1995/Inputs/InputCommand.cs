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
    public enum InputCommand
    {
        ToggleFullscreen,
        
        MENU_Exit,
        MENU_Accept,
        
        EXPLORING_Pause,
        EXPLORING_Move,
        
        Dialog_Skip, // skip to next part player can do anything
        Dialog_HistoryBack, // Missed something?
        Dialog_HistoryForward // Back To The Future!
    }
}
