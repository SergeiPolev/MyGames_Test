using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseUIScreen : UIScreen
{
    [SerializeField] private Button _restartButton;
    
    public override void Init(GameDataContainer gameDataContainer, GameCanvas gameCanvas)
    {
        base.Init(gameDataContainer, gameCanvas);
        
        gameDataContainer.GetGameData().GameStateMachine.SubscribeToState(GameState.LOSE, Open);
        _restartButton.onClick.AddListener(Restart);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public override UIScreenType GetUIType()
    {
        return UIScreenType.LOSE;
    }
}
