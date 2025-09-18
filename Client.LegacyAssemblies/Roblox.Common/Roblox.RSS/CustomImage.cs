namespace Roblox.RSS;

public class CustomImage : IImage
{
	private string url;

	private int width;

	private int height;

	public string Url => url;

	public int Width => width;

	public int Height => height;

	public CustomImage(string url, int width, int height)
	{
		this.url = url;
		this.width = width;
		this.height = height;
	}
}
