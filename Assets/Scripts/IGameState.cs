
public interface IGameState
{
    public IGameState Enter();
    public IGameState Update();
    
    public void Exit();

}
