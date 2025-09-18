-- Creates all neccessary scripts for the gui on initial load, everything except build tools
-- Created by Ben T. 10/29/10
-- Please note that these are loaded in a specific order to diminish errors/perceived load time by user
local scriptContext = game:GetService("ScriptContext")
local touchEnabled = false
--pcall(function() touchEnabled = game:GetService("UserInputService").TouchEnabled end)
local RobloxGui = game:GetService("CoreGui"):WaitForChild("RobloxGui")

game:SetMessage("Loading...")

-- library registration
print("Loading Modules...")
scriptContext:AddCoreScriptLocal("CoreScripts/2013/ModuleRegistration", RobloxGui)

local function waitForChild(instance, name)
	while not instance:FindFirstChild(name) do
		print('(SS): Waiting for ...', instance, name)
		instance.ChildAdded:wait()
	end
end
local function waitForProperty(instance, property)
	while not instance[property] do
		instance.Changed:wait()
	end
end

-- Responsible for tracking logging items
local scriptContext = game:GetService("ScriptContext")
scriptContext:AddCoreScriptLocal("CoreScripts/2013/Sections", RobloxGui)


waitForChild(game:GetService("CoreGui"),"RobloxGui")
local screenGui = game:GetService("CoreGui"):FindFirstChild("RobloxGui")

if not touchEnabled then
	-- ToolTipper  (creates tool tips for gui)
	scriptContext:AddCoreScriptLocal("CoreScripts/2013/ToolTip", RobloxGui)
	-- SettingsScript 
	scriptContext:AddCoreScriptLocal("CoreScripts/2013/Settings", RobloxGui)
end

-- MainBotChatScript
scriptContext:AddCoreScriptLocal("CoreScripts/2013/MainBotChatScript", RobloxGui)

-- Popup Script
scriptContext:AddCoreScriptLocal("CoreScripts/2013/PopupScript", RobloxGui)
-- Friend Notification Script (probably can use this script to expand out to other notifications)
scriptContext:AddCoreScriptLocal("CoreScripts/2013/NotificationScript", RobloxGui)
-- Chat script
scriptContext:AddCoreScriptLocal("CoreScripts/2013/ChatScript", screenGui)	
-- Purchase Prompt Script
scriptContext:AddCoreScriptLocal("CoreScripts/2013/PurchasePromptScript", RobloxGui)

--[[ 
	This script controls the gui the player sees in regards to his or her health.
	Can be turned with Game.StarterGui:SetCoreGuiEnabled(Enum.CoreGuiType.Health,false)
	Copyright ROBLOX 2014. Written by Ben Tkacheff.
--]]

---------------------------------------------------------------------
-- Initialize/Variables
while not Game do
	wait(1/60)
end
while not Game.Players do
	wait(1/60)
end

local useCoreHealthBar = false
local success = pcall(function() useCoreHealthBar = Game.Players:GetUseCoreScriptHealthBar() end)
if not success or not useCoreHealthBar then
	return
end

local currentHumanoid = nil

local HealthGui = nil
local lastHealth = 100
local HealthPercentageForOverlay = 5
local maxBarTweenTime = 0.3

local guiEnabled = false
local healthChangedConnection = nil
local humanoidDiedConnection = nil
local characterAddedConnection = nil

local greenBarImage = "http://www.watrbx.wtf/asset/?id=35238053"
local redBarImage = "http://www.watrbx.wtf/asset/?id=35238036"
local hurtOverlayImage = "http://www.watrbx.wtf/asset/?id=34854607"

Game:GetService("ContentProvider"):Preload(greenBarImage)
Game:GetService("ContentProvider"):Preload(redBarImage)
Game:GetService("ContentProvider"):Preload(hurtOverlayImage)


while not Game.Players.LocalPlayer do
	wait(1/60)
end

---------------------------------------------------------------------
-- Functions

function CreateGui()
	if HealthGui then 
		HealthGui.Parent = Game.CoreGui.RobloxGui
		return 
	end
	
	HealthGui = Instance.new("Frame")
	HealthGui.Name = "HealthGui"
	HealthGui.BackgroundTransparency = 1
	HealthGui.Size = UDim2.new(1,0,1,0)
	
	local hurtOverlay = Instance.new("ImageLabel")
	hurtOverlay.Name = "HurtOverlay"
	hurtOverlay.BackgroundTransparency = 1
	hurtOverlay.Image = hurtOverlayImage
	hurtOverlay.Position = UDim2.new(-10,0,-10,0)
	hurtOverlay.Size = UDim2.new(20,0,20,0)
	hurtOverlay.Visible = false
	hurtOverlay.Parent = HealthGui
	
	local healthFrame = Instance.new("Frame")
	healthFrame.Name = "HealthFrame"
	healthFrame.BackgroundColor3 = Color3.new(0,0,0)
	healthFrame.BorderColor3 = Color3.new(0,0,0)
	healthFrame.Position = UDim2.new(0.5,-85,1,-22)
	healthFrame.Size = UDim2.new(0,170,0,18)
	healthFrame.Parent = HealthGui
	
	local healthBar = Instance.new("ImageLabel")
	healthBar.Name = "HealthBar"
	healthBar.BackgroundTransparency = 1
	healthBar.Image = greenBarImage
	healthBar.Size = UDim2.new(1,0,1,0)
	healthBar.Parent = healthFrame
	
	local healthLabel = Instance.new("TextLabel")
	healthLabel.Name = "HealthLabel"
	
	healthLabel.Text = "Health  " -- gives room at end of health bar
	healthLabel.Font = Enum.Font.SourceSansBold
	healthLabel.FontSize = Enum.FontSize.Size14
	healthLabel.TextColor3 = Color3.new(1,1,1)
	healthLabel.TextStrokeTransparency = 0
	healthLabel.TextXAlignment = Enum.TextXAlignment.Right
	
	healthLabel.BackgroundTransparency = 1
	healthLabel.Size = UDim2.new(1,0,1,0)
	healthLabel.Parent = healthFrame
	
	HealthGui.Parent = Game.CoreGui.RobloxGui
end

function UpdateGui(health)
	if not HealthGui then return end
	
	local healthFrame = HealthGui:FindFirstChild("HealthFrame")
	if not healthFrame then return end
	
	local healthBar = healthFrame:FindFirstChild("HealthBar")
	if not healthBar then return end
	
	-- If more than 1/4 health, bar = green.  Else, bar = red.
	if (health/currentHumanoid.MaxHealth) > 0.25  then		
		healthBar.Image = greenBarImage	
	else
		healthBar.Image = redBarImage
	end
		
	local width = (health / currentHumanoid.MaxHealth)
 	width = math.max(math.min(width,1),0) -- make sure width is between 0 and 1
		
	local healthDelta = lastHealth - health
	lastHealth = health
	
	local percentOfTotalHealth = math.abs(healthDelta/currentHumanoid.MaxHealth)
	percentOfTotalHealth = math.max(math.min(percentOfTotalHealth,1),0) -- make sure percentOfTotalHealth is between 0 and 1

	local newHealthSize = UDim2.new(width,0,1,0)
	
	healthBar:TweenSize(newHealthSize, Enum.EasingDirection.InOut, Enum.EasingStyle.Linear, percentOfTotalHealth * maxBarTweenTime, true)

	local thresholdForHurtOverlay = currentHumanoid.MaxHealth * (HealthPercentageForOverlay/100)
	
	if healthDelta >= thresholdForHurtOverlay then
		AnimateHurtOverlay()
	end
end

function AnimateHurtOverlay()
	if not HealthGui then return end
	
	local overlay = HealthGui:FindFirstChild("HurtOverlay")
	if not overlay then return end
	
	-- stop any tweens on overlay
	overlay:TweenSizeAndPosition(UDim2.new(20, 0, 20, 0), UDim2.new(-10, 0, -10, 0),Enum.EasingDirection.Out,Enum.EasingStyle.Linear,0,true,function()
		
		-- show the gui
		overlay.Size = UDim2.new(1,0,1,0)
		overlay.Position = UDim2.new(0,0,0,0)
		overlay.Visible = true
		
		-- now tween the hide
		overlay:TweenSizeAndPosition(UDim2.new(20, 0, 20, 0) ,UDim2.new(-10, 0, -10, 0),Enum.EasingDirection.Out,Enum.EasingStyle.Quad,10,false,function()
			overlay.Visible = false
		end)
	end)

end

function humanoidDied()
	 UpdateGui(0)
end

function disconnectPlayerConnections()
	humanoidDiedConnection:disconnect()
	healthChangedConnection:disconnect()
	characterAddedConnection:disconnect()
end

function newPlayerCharacter()
	disconnectPlayerConnections()
	startGui()
end

function startGui()
	local character = Game.Players.LocalPlayer.Character

	while (character == nil) do
		character = Game.Players.LocalPlayer.Character
		wait(1/30)
	end

	currentHumanoid = character:WaitForChild("Humanoid")
	if currentHumanoid then
		CreateGui()
		healthChangedConnection = currentHumanoid.HealthChanged:connect(UpdateGui)
		humanoidDiedConnection = currentHumanoid.Died:connect(humanoidDied)
	end

	UpdateGui(currentHumanoid.Health)

	characterAddedConnection = Game.Players.LocalPlayer.CharacterAdded:connect(newPlayerCharacter)
end



---------------------------------------------------------------------
-- Start Script

if Game.StarterGui:GetCoreGuiEnabled(Enum.CoreGuiType.Health) then
	guiEnabled = true
	startGui()
end

Game.StarterGui.CoreGuiChangedSignal:connect(function(coreGuiType,enabled)
	
	if guiEnabled and not enabled then
		if HealthGui then
			HealthGui.Parent = nil
		end
		disconnectPlayerConnections()
	elseif not guiEnabled and enabled then
		startGui()
	end
	
	guiEnabled = enabled
	
end)



if not touchEnabled then 
	-- New Player List
	scriptContext:AddCoreScriptLocal("CoreScripts/2013/PlayerListScript", RobloxGui)
elseif screenGui.AbsoluteSize.Y > 600 then 	
	-- New Player List
	scriptContext:AddCoreScriptLocal("CoreScripts/2013/PlayerListScript",RobloxGui)
else 
	delay(5, function()
		if RobloxGui.AbsoluteSize.Y >= 600 then 			
			-- New Player List
			scriptContext:AddCoreScriptLocal("CoreScripts/2013/PlayerListScript", RobloxGui)
		end 
	end) 
end 

-- Backpack Builder, creates most of the backpack gui
scriptContext:AddCoreScriptLocal("CoreScripts/2013/BackpackScripts/BackpackBuilder", RobloxGui)

waitForChild(RobloxGui, "CurrentLoadout")
waitForChild(RobloxGui, "Backpack")

local Backpack = RobloxGui.Backpack

game:GetService("ScriptContext"):AddCoreScriptLocal("CoreScripts/2013/BackpackScripts/BackpackManager", Backpack)
-- Backpack Gear (handles all backpack gear tab stuff)
game:GetService("ScriptContext"):AddCoreScriptLocal("CoreScripts/2013/BackpackScripts/BackpackGear", Backpack)

-- Loadout Script, used for gear hotkeys
scriptContext:AddCoreScriptLocal("CoreScripts/2013/BackpackScripts/LoadoutScript", RobloxGui.CurrentLoadout)


--scriptContext:AddCoreScriptLocal("CoreScripts/BackpackScripts/BackpackWardrobe", Backpack)

local IsPersonalServer = not not game.Workspace:FindFirstChild("PSVariable")
if IsPersonalServer then	
	
end
game.Workspace.ChildAdded:connect(function(nchild)
	if nchild.Name=='PSVariable' and nchild:IsA('BoolValue') then
		IsPersonalServer = true		
		
	end
end)

if touchEnabled then -- touch devices don't use same control frame
	waitForChild(screenGui, "ControlFrame")
	waitForChild(screenGui.ControlFrame, 'BottomLeftControl')
	screenGui.ControlFrame.BottomLeftControl.Visible = false

	waitForChild(screenGui.ControlFrame, 'TopLeftControl')
	screenGui.ControlFrame.TopLeftControl.Visible = false 
end 
