using System.CodeDom.Compiler;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace Roblox.Grid.Rcc.Server;

[GeneratedCode("wsdl", "4.8.3928.0")]
[WebService(Namespace = "http://roblox.com/")]
[WebServiceBinding(Name = "RCCServiceSoap", Namespace = "http://roblox.com/")]
public abstract class RCCServiceSoap : WebService
{
	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/HelloWorld", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public abstract string HelloWorld();

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/GetVersion", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public abstract string GetVersion();

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/GetStatus", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public abstract Status GetStatus();

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/OpenJob", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement("OpenJobResult")]
	public abstract LuaValue[] OpenJob(Job job, ScriptExecution script);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/OpenJobEx", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public abstract LuaValue[] OpenJobEx(Job job, ScriptExecution script);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/RenewLease", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public abstract double RenewLease(string jobID, double expirationInSeconds);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/Execute", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement("ExecuteResult", IsNullable = true)]
	public abstract LuaValue[] Execute(string jobID, ScriptExecution script);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/ExecuteEx", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public abstract LuaValue[] ExecuteEx(string jobID, ScriptExecution script);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/CloseJob", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public abstract void CloseJob(string jobID);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/BatchJob", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement("BatchJobResult", IsNullable = true)]
	public abstract LuaValue[] BatchJob(Job job, ScriptExecution script);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/BatchJobEx", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public abstract LuaValue[] BatchJobEx(Job job, ScriptExecution script);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/GetExpiration", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public abstract double GetExpiration(string jobID);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/GetAllJobs", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement("GetAllJobsResult", IsNullable = true)]
	public abstract Job[] GetAllJobs();

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/GetAllJobsEx", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public abstract Job[] GetAllJobsEx();

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/CloseExpiredJobs", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public abstract int CloseExpiredJobs();

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/CloseAllJobs", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public abstract int CloseAllJobs();

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/Diag", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement("DiagResult", IsNullable = true)]
	public abstract LuaValue[] Diag(int type, string jobID);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/DiagEx", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public abstract LuaValue[] DiagEx(int type, string jobID);
}
