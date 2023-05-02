namespace Game.Scripts.Controls
{
	public class PlayerInputLocator
	{
		private static PlayerInput PlayerInput { get; } = new();

		public PlayerInputLocator()
		{
			PlayerInput.Player.Enable();
		}

		public static PlayerInput GetPlayerInput()
		{
			return PlayerInput;
		}
	}
}