using System.Collections.Generic;

namespace Roblox.Common;

public class EntityCollectionJson : Json
{
	public static int ApiVersion = 1;

	public IEnumerable<EntityCollectionItem> data { get; private set; }

	public EntityCollectionJson(IEnumerable<EntityCollectionItem> data)
	{
		this.data = data;
	}
}
