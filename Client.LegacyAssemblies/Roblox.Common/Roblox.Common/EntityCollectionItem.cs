namespace Roblox.Common;

public class EntityCollectionItem
{
	public static int ApiVersion = 1;

	public string id { get; private set; }

	public string __stamp { get; private set; }

	public EntityCollectionItem(string id, string stamp)
	{
		this.id = id;
		__stamp = stamp;
	}
}
