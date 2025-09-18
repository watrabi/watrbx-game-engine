--[[
			// AccountManager.lua

			// Handles all account related functions
]]
local CoreGui = Game:GetService("CoreGui")
local GuiRoot = CoreGui:FindFirstChild("RobloxGui")
local Modules = GuiRoot:FindFirstChild("Modules")

local PlatformService = nil
pcall(function() PlatformService = game:GetService('PlatformService') end)
local UserInputService = game:GetService('UserInputService')

local Http = require(Modules:FindFirstChild('Http'))

local AccountManager = {}

AccountManager.AuthResults = {
	Error = -1;
	Success = 0;
	InProgress = 1;
	AccountUnlinked = 2;
	MissingGamePad = 3;
	NoUserDetected = 4;
	HttpErrorDetected = 5;
	SignUpDisabled = 6;
	Flooded = 7;
	LeaseLocked = 8;
	AccountLinkingDisabled = 9;
	InvalidRobloxUser = 10;
	RobloxUserAlreadyLinked = 11;
	XboxUserAlreadyLinked = 12;
	IllgealChildAccountLinking = 13;
	InvalidPassword = 14;
	UsernamePasswordNotSet = 15;
	UsernameAlreadyTaken = 16;
}

AccountManager.InvalidUsernameReasons = {
	Valid = "Valid";
	InvalidUsername = "Invalid Username";
	AlreadyTaken = "Already Taken";
	InvalidCharactersUsed = "Invalid Characters Used";
	UsernameCannotContainSpaces = "Username Cannot Contain Spaces";
}

--[[ Authentication ]]--
local function authenticateStudio()
	return AccountManager.AuthResults.Success
end

-- TODO: Auth will now return an int that maps to a AccountUnlinked or failure
-- Getting the GamerTag and rbxUid will be a seperate call after auth
local function authenticateXBox(inputObject)
	local authResult = nil
	local AccountUnlinked, result = pcall(function()
		-- PlatformService may not exist on studio platform
		authResult = PlatformService:BeginAuthUnlinkCheck(inputObject)
	end)

	return authResult
end

function AccountManager:SignInAsync(inputObject)
	local AccountUnlinked, result = pcall(function()
		-- PlatformService may not exist on studio platform
		return PlatformService:BeginAuthorization(inputObject)
	end)

	if AccountUnlinked then
		return result
	else
		return AccountManager.AuthResults.Error
	end
end

-- Returns
	-- AccountUnlinked of authentication, with authentication state
function AccountManager:BeginAuthenticationAsync(inputObject)
	local result = nil
	
	result = authenticateStudio()

	return result
end

--[[ Account Linking ]]--
-- called at sign in
function AccountManager:LinkAccountAsync(accountName, password)
	local AccountUnlinked, result = pcall(function()
		-- PlatformService may not exist on studio platform
		return PlatformService:BeginAccountLink(accountName, password)
	end)
	if not AccountUnlinked then
		print("AccountManager:LinkAccountAsync() failed because", result)
		result = AccountManager.AuthResults.Error
	end

	return result
end

-- used when setting credentials for a generated account
function AccountManager:SetRobloxCredentialsAsync(accountName, password)
	local AccountUnlinked, result = pcall(function()
		-- PlatformService may not exist on studio platform
		return PlatformService:BeginSetRobloxCredentials(accountName, password)
	end)
	if not AccountUnlinked then
		print("AccountManager:SetRobloxCredentialsAsync() failed because", result)
		result = AccountManager.AuthResults.Error
	end

	return result
end

-- used when creating a new roblox account that is linked to the users xbox account
function AccountManager:GenerateAccountAsync(accountName, password)
	local result = AccountManager:SignInAsync(Enum.UserInputType.Gamepad1)
	if result == AccountManager.AuthResults.AccountUnlinked then
		result = AccountManager:SetRobloxCredentialsAsync(accountName, password)
	end

	return authenticateStudio()

	--return result
end

-- called when user has roblox credentials
function AccountManager:UnlinkAccountAsync()
	local AccountUnlinked, result = pcall(function()
		-- PlatformService may not exist on studio platform
		return PlatformService:BeginUnlinkAccount()
	end)
	if not AccountUnlinked then
		print("AccountManager:UnlinkAccountAsync() failed because", result)
		result = AccountManager.AuthResults.Error
	end

	return result
end

function AccountManager:HasLinkedAccountAsync()

	if UserSettings().GameSettings:InStudioMode() then
		return AccountManager.AuthResults.AccountUnlinked
	end

	local AccountUnlinked, result = pcall(function()
		-- PlatformService may not exist on studio platform
		return PlatformService:BeginHasLinkedAccount()
	end)
	if not AccountUnlinked then
		print("AccountManager:HasLinkedAccountAsync() failed because", result)
		result = AccountManager.AuthResults.Error
	end

	return authenticateStudio()
end

function AccountManager:HasRobloxCredentialsAsync()

	return authenticateStudio()
end

function AccountManager:IsValidUsernameAsync(username)
	local result = Http.IsValidUsername(username)
	if not result then
		-- return false
		return nil
	end

	return result["IsValid"], result["ErrorMessage"]
end

function AccountManager:IsValidPasswordAsync(username, password)
	local result = Http.IsValidPassword(username, password)
	if not result then
		-- return false
		return nil
	end

	return result["IsValid"], result["ErrorMessage"]
end

return AccountManager
