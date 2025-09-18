using System;
using Roblox.Grid.Rcc;

namespace Roblox.Grid;

public class Lua
{
	public static ScriptExecution EmptyScript = NewScript("EmptyScript", "return");

	public static ScriptExecution NewScript(string name, string script)
	{
		return NewScript(name, script, NewArgs());
	}

	public static ScriptExecution NewScript(string name, string script, params object[] args)
	{
		return NewScript(name, script, NewArgs(args));
	}

	public static ScriptExecution NewScript(string name, string script, LuaValue[] args)
	{
		ScriptExecution scriptExecution = new ScriptExecution();
		scriptExecution.name = name;
		scriptExecution.script = script;
		scriptExecution.arguments = args ?? NewArgs();
		return scriptExecution;
	}

	public static string ToString(LuaValue[] result)
	{
		string str = null;
		foreach (LuaValue value in result)
		{
			str = ((!string.IsNullOrEmpty(str)) ? (str + ", " + value.value) : value.value);
		}
		return str;
	}

	public static void SetArg(LuaValue[] args, int index, object value)
	{
		LuaValue luaValue = new LuaValue();
		if (value is int || value is float || value is double || value is long || value is decimal || value is short || value is ushort || value is uint || value is ulong)
		{
			luaValue.type = LuaType.LUA_TNUMBER;
			luaValue.value = value.ToString();
		}
		else if (value is string)
		{
			luaValue.type = LuaType.LUA_TSTRING;
			luaValue.value = value.ToString();
		}
		else if (value is bool boolean)
		{
			luaValue.type = LuaType.LUA_TBOOLEAN;
			luaValue.value = (boolean ? "true" : "false");
		}
		else if (value == null)
		{
			luaValue.type = LuaType.LUA_TNIL;
			luaValue.value = string.Empty;
		}
		else
		{
			if (!(value is LuaValue[]))
			{
				throw new ArgumentException("Unsupported Lua argument type " + value.GetType());
			}
			luaValue.type = LuaType.LUA_TTABLE;
			luaValue.table = value as LuaValue[];
		}
		args[index] = luaValue;
	}

	private static object ConvertLua(LuaValue luaValue)
	{
		return luaValue.type switch
		{
			LuaType.LUA_TBOOLEAN => Convert.ToBoolean(luaValue.value), 
			LuaType.LUA_TNUMBER => Convert.ToDouble(luaValue.value), 
			LuaType.LUA_TSTRING => luaValue.value, 
			LuaType.LUA_TTABLE => GetValues(luaValue.table), 
			_ => null, 
		};
	}

	public static LuaValue[] NewArgs(params object[] args)
	{
		LuaValue[] newArgs = new LuaValue[args.Length];
		for (int i = 0; i < args.Length; i++)
		{
			SetArg(newArgs, i, args[i]);
		}
		return newArgs;
	}

	public static object[] GetValues(LuaValue[] args)
	{
		object[] values = new object[args.Length];
		for (int i = 0; i < args.Length; i++)
		{
			values[i] = ConvertLua(args[i]);
		}
		return values;
	}
}
