using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Roblox.Grid.Rcc.ServerInterface;

[Serializable]
[GeneratedCode("wsdl", "4.8.3928.0")]
[XmlType(Namespace = "http://roblox.com/")]
public enum LuaType
{
	LUA_TNIL,
	LUA_TBOOLEAN,
	LUA_TNUMBER,
	LUA_TSTRING,
	LUA_TTABLE
}
