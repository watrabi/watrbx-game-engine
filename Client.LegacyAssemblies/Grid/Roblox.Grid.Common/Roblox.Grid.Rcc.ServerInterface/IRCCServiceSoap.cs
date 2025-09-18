using System.CodeDom.Compiler;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace Roblox.Grid.Rcc.ServerInterface;

[GeneratedCode("wsdl", "4.8.3928.0")]
[WebServiceBinding(Name = "RCCServiceSoap", Namespace = "http://roblox.com/")]
public interface IRCCServiceSoap
{
	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/HelloWorld", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	string HelloWorld();

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/GetVersion", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	string GetVersion();

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/GetStatus", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	Status GetStatus();

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/OpenJob", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement("OpenJobResult")]
	LuaValue[] OpenJob(Job job, ScriptExecution script);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/OpenJobEx", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	LuaValue[] OpenJobEx(Job job, ScriptExecution script);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/RenewLease", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	double RenewLease(string jobID, double expirationInSeconds);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/Execute", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement("ExecuteResult", IsNullable = true)]
	LuaValue[] Execute(string jobID, ScriptExecution script);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/ExecuteEx", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	LuaValue[] ExecuteEx(string jobID, ScriptExecution script);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/CloseJob", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	void CloseJob(string jobID);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/BatchJob", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement("BatchJobResult", IsNullable = true)]
	LuaValue[] BatchJob(Job job, ScriptExecution script);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/BatchJobEx", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	LuaValue[] BatchJobEx(Job job, ScriptExecution script);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/GetExpiration", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	double GetExpiration(string jobID);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/GetAllJobs", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement("GetAllJobsResult", IsNullable = true)]
	Job[] GetAllJobs();

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/GetAllJobsEx", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	Job[] GetAllJobsEx();

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/CloseExpiredJobs", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	int CloseExpiredJobs();

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/CloseAllJobs", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	int CloseAllJobs();

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/Diag", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement("DiagResult", IsNullable = true)]
	LuaValue[] Diag(int type, string jobID);

	[WebMethod]
	[SoapDocumentMethod("http://roblox.com/DiagEx", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	LuaValue[] DiagEx(int type, string jobID);
}
