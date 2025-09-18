namespace Roblox.Common;

public class EntityCountJson : Json
{
	public static int ApiVersion = 1;

	public string count { get; private set; }

	public EntityCountJson(string count)
	{
		this.count = count;
	}
}
