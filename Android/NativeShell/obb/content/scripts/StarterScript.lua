local Settings = UserSettings()
local GameSettings = Settings.GameSettings
local YearSettingEnum = GameSettings.YearSetting
local scriptContext = game:GetService("ScriptContext")
local RobloxGui = game:GetService("CoreGui"):WaitForChild("RobloxGui")
local Backup = RobloxGui:Clone()
Backup.Name = "RobloxGuiBackup"
Backup.Parent = game:GetService("CoreGui")

--local success, YearSettingEnum = pcall(function()
--	print(game.isXboxClient)
--	return GameSettings.YearSetting
--end)
--if not UserSettings().GameSettings:InStudioMode() then
--	game:GetService("RunService"):Run()
--	game:Load("rbxasset://ScaledWorldv4.7.rbxl")
--end
--scriptContext:AddCoreScriptLocal("CoreScripts/xbox/ui/XStarterScript", RobloxGui)

if not success or type(YearSettingEnum) ~= "number" or YearSettingEnum < 0 or YearSettingEnum > 3 then
	YearSettingEnum = 0
end

local switch = {
	[0] = function()
		scriptContext:AddCoreScriptLocal("CoreScripts/2016/StarterScript", RobloxGui)
	end,
	[1] = function()
		scriptContext:AddCoreScriptLocal("CoreScripts/2015/StarterScript", RobloxGui)
	end,
	[2] = function()
		scriptContext:AddCoreScriptLocal("CoreScripts/2014/StarterScript", RobloxGui)
	end,
	[3] = function()
		scriptContext:AddCoreScriptLocal("CoreScripts/2013/StarterScript", RobloxGui)
	end,
	["default"] = function() 
		scriptContext:AddCoreScriptLocal("CoreScripts/2016/StarterScript", RobloxGui)
	end
}

if switch[YearSettingEnum] then
   switch[YearSettingEnum]()
else
   switch["default"]()
end
