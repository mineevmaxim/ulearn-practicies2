namespace Generics.Robots;

public interface IRobotAI<out T>
{
	public T GetCommand();
}

public class ShooterAI : IRobotAI<IShooterMoveCommand>
{
	int counter = 1;

	public IShooterMoveCommand GetCommand()
		=> ShooterCommand.ForCounter(counter++);
}

public class BuilderAI : IRobotAI<IMoveCommand>
{
	int counter = 1;

	public IMoveCommand GetCommand()
		=> BuilderCommand.ForCounter(counter++);
}

public interface IDevice<in T>
{
	public string ExecuteCommand(T command);
}

public class Mover : IDevice<IMoveCommand>
{
	public string ExecuteCommand(IMoveCommand _command)
	{
		if (_command == null)
			throw new ArgumentException();
		return $"MOV {_command.Destination.X}, {_command.Destination.Y}";
	}
}

public class ShooterMover : IDevice<IShooterMoveCommand>
{
	public string ExecuteCommand(IShooterMoveCommand _command)
	{
		if (_command == null)
			throw new ArgumentException();
		var hide = _command.ShouldHide ? "YES" : "NO";
		return $"MOV {_command.Destination.X}, {_command.Destination.Y}, USE COVER {hide}";
	}
}

public static class Robot
{
	public static Robot<TCommand> Create<TCommand>(IRobotAI<TCommand> ai, IDevice<TCommand> executor)
		=> new(ai, executor);
}

public class Robot<T>
{
	private readonly IRobotAI<T> ai;
	private readonly IDevice<T> device;

	public Robot(IRobotAI<T> ai, IDevice<T> executor)
	{
		this.ai = ai;
		device = executor;
	}

	public IEnumerable<string> Start(int steps)
	{
		for (var i = 0; i < steps; i++)
		{
			var command = ai.GetCommand();
			if (command == null)
				yield break;
			yield return device.ExecuteCommand(command);
		}
	}
}
