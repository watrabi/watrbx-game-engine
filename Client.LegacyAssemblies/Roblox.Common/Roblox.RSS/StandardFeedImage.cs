namespace Roblox.RSS;

public class StandardFeedImage : IImage
{
	private string url;

	public int Width => 118;

	public int Height => 31;

	public string Url => url;

	public StandardFeedImage(string url)
	{
		this.url = url;
	}
}
