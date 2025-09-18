using System;

namespace Roblox.RSS;

public interface IItem
{
	string Title { get; }

	DateTime PubDate { get; }

	string Description { get; }

	string Link { get; }

	IImage Image { get; }
}
