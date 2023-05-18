public class MainUIScreen : UIScreen
{
    public override void Open()
    {
        base.Open();
        
        gameCanvas.Open(UIScreenType.CHARACTER_CANVASES);
    }

    public override UIScreenType GetUIType()
    {
        return UIScreenType.MAIN;
    }
}