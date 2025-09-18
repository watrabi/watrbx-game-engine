using System.Collections.Generic;

namespace Roblox.RSS;

public interface IChannel
{
	string Title { get; }

	string Description { get; }

	IImage Image { get; }

	bool Complete { get; }

	IEnumerable<IItem> GetItems();
}
