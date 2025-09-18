-- Library Registration Script
-- This script is used to register RbxLua Modules on game servers, so game scripts have
-- access to all of the Modules (otherwise only local scripts do)

local function waitForChild(instance, name)
	while not instance:FindFirstChild(name) do
		instance.ChildAdded:wait()
		print("Waiting...")
	end
	return instance:FindFirstChild(name)
end
local function waitForProperty(instance, property)
	while not instance[property] do
		instance.Changed:wait()
		print("Waiting...")
	end
end

local sc = game:GetService("ScriptContext")
local RobloxGui = game:GetService("CoreGui"):WaitForChild("RobloxGui")
local tries = 0
 
while not sc and tries < 3 do
	tries = tries + 1
	sc = game:GetService("ScriptContext")
	wait(0.2)
end
 
if sc then
	print("Loading Modules...")
	sc:AddCoreScriptLocal("Modules/RbxGui", RobloxGui)
	print("RbxGui Loaded")
	sc:AddCoreScriptLocal("Modules/RbxGear", RobloxGui)
	print("RbxGear Loaded")
	sc:AddCoreScriptLocal("Modules/RbxUtility", RobloxGui)
	print("RbxUtility Loaded")
	sc:AddCoreScriptLocal("Modules/RbxStamper", RobloxGui)
	print("RbxStamper Loaded")
else
	print("failed to find script context, Modules did not load")
end
