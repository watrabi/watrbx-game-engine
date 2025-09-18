namespace Roblox.Common.Mail.Pop3;

public struct EmailUid
{
	public int EmailId;

	public string Uid;

	public EmailUid(int EmailId, string Uid)
	{
		this.EmailId = EmailId;
		this.Uid = Uid;
	}
}
