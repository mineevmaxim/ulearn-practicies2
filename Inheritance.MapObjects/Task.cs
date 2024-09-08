namespace Inheritance.MapObjects;

public interface IHaveArmy
{
	public Army Army { get; set; }
}

public interface IHaveOwner
{
	public int Owner { get; set; }
}

public interface IHaveTreasure
{
	public Treasure Treasure { get; set; }
}

public class Dwelling : IHaveOwner
{
	public int Owner { get; set; }
}

public class Mine : IHaveOwner, IHaveArmy, IHaveTreasure
{
	public int Owner { get; set; }
	public Army Army { get; set; }
	public Treasure Treasure { get; set; }
}

public class Creeps: IHaveArmy, IHaveTreasure
{
	public Army Army { get; set; }
	public Treasure Treasure { get; set; }
}

public class Wolves: IHaveArmy
{
	public Army Army { get; set; }
}

public class ResourcePile: IHaveTreasure
{
	public Treasure Treasure { get; set; }
}

public static class Interaction
{
	public static void Make(Player player, object mapObject)
	{
		if (mapObject is IHaveArmy armyObj && !player.CanBeat(armyObj.Army))
		{
			player.Die();
			return;
		}
		if (mapObject is IHaveOwner ownerObj) ownerObj.Owner = player.Id;
		if (mapObject is IHaveTreasure treasureObj) player.Consume(treasureObj.Treasure);
	}
}