--      __  __                __        ______                  __                   __         
--     / / / /___ _____  ____/ /____   / ____/_______  ___     / /   ___ _   _____  / /__  _____
--    / /_/ / __ `/ __ \/ __  / ___/  / /_  / ___/ _ \/ _ \   / /   / _ \ | / / _ \/ / _ \/ ___/
--   / __  / /_/ / / / / /_/ (__  )  / __/ / /  /  __/  __/  / /___/  __/ |/ /  __/ /  __/ /    
--  /_/ /_/\__,_/_/ /_/\__,_/____/  /_/   /_/   \___/\___/  /_____/\___/|___/\___/_/\___/_/     
--       
--
--
--                                                                                       
editMode = false
debugMode = false
--
-- Global Vars
--
MAPNAME = GetGame().map.shortName
MAPHFLNUMBER = "11"
if MAPNAME == "summonerRift" then
	MAPHFLNUMBER = "11"
else
	MAPHFLNUMBER = "12"
end
TEAMNUMBER = myHero.team
VERSION = "1.0"
CLIENTVERSION = ""
require "Collision"
VP = nil
--
-- Class Objects
--
Packets = nil
LOCALAWARENESS = nil
MINIONS = nil
MyHero = nil
TOWERS = nil
ORBWALKER = nil
Tasks = nil
Data = nil
Items = nil
Crosshair = nil
Skills = nil
Structures = nil
DamagePred = nil
Remote = nil
Helper = nil
StateManager = nil
AutoSkill = nil
AutoItem = nil
RandomPath = nil
--
-- Class Enums
--
PRED_LAST_HIT = 0
PRED_TWO_HITS = 1
PRED_SKILL = 2
PRED_UNKILLABLE = 3
BOT_LANE = "0"
MID_LANE = "1"
TOP_LANE = "2"
SELECTED_LANE = BOT_LANE
PUSH_STATE = 0
RECALL_STATE = 1
SURVIVAL_STATE = 2
HOLDLANE_STATE = 3
COMBAT_STATE = 4
SPAWN_STATE = 5
LANESELECT_STATE = 6
LOADING_STATE = 7
WAIT_STATE = 8
DEAD_STATE = 9
WAYPOINT = {x = 0,z = 0}
STAGE_SHOOT = 0
STAGE_MOVE = 1
STAGE_SHOOTING = 2
RegisteredOnAttacked = {}
local cba
--
-- Classes
--
class 'init'
	function init:__init()
		CLIENTVERSION = split(GetGameVersion()," ")[1]
		self.sprite = false
		self.sep = false
		self.sep2 = false
		if FileExist(LIB_PATH .. "/HfLib.lua") then
			self:load()
		end
		if (not _G.hflTasks or not _G.hflTasks[MAPNAME] or not _G.hflTasks[MAPNAME][TEAMNUMBER]) and not editMode then
			PrintSystemMessage("This map is not supported, please report the map name to law to make him update for this map")
		else
			if editMode then
				debugMode = false
			end
			if not _G.hflTasks then
				_G.hflTasks = {}
			end
			if not _G.hflTasks[MAPNAME] then
				_G.hflTasks[MAPNAME] = {}
			end
			if not _G.hflTasks[MAPNAME][TEAMNUMBER] then
				_G.hflTasks[MAPNAME][TEAMNUMBER]  = {}
			end

			if self:checkAccess() then
				if editMode then
					editor()
				else
					if debugMode then
						DEBUGGER = debugger()
					end
					require "VPrediction"
					
					VP = VPrediction()
					Helper = _Helper()
					Packets = _Packets()

					Skills = _Skills()
					Items = _Items()
					Data = _Data()
					DamagePred = _DamagePred()

					Jungle = _Jungle()
					Orbwalker = _Orbwalker()
					MyHero = _MyHero()
					Crosshair = _Crosshair(DAMAGE_PHYSICAL, MyHero.TrueRange, 0, false, false)
					Structures = _Structures()
					MINIONS = _Minions()
					LOCALAWARENESS = LocalAwareness()
					TOWERS = towers()
					Tasks = _Tasks()
					
					StateManager = _StateManager()
					RandomPath = _RandomPath()
					AutoSkill = _AutoSkill()
					AutoItem = _AutoItem()
					--if _G[myHero.charName] then
					--	CHAMPION = _G[myHero.charName]()
					--else
					--	CHAMPION = _G["Default"]()
					--end
				end

				AddDrawCallback(function()
					self:drawInfo()
				end)
			end
			self:loadSprite()

			AddDrawCallback(function()
				self:drawSprite()
			end)
		end
	end

	function init:load()
		local file = io.open(LIB_PATH .. "/HfLib.lua", "rb")
		local content = file:read("*all")
		file:close()
		_G.hflTasks = unpickle(content)
	end

	function init:checkAccess()
		return true
	end

	function init:drawInfo()
		DrawRectangle(0, 119, 320, 300, ARGB(150,0,0,0))
		DrawText("Hands Free Leveler " .. VERSION, 21, 50, 130, ARGB(255,255,255,0))
		DrawText("Champion: " .. myHero.charName, 18, 20, 170, ARGB(255,0,160,255))
		if(_G[myHero.charName]) then
			DrawText("Profile: " .. myHero.charName, 18, 60, 170, ARGB(255,0,255,0))
		else
			DrawText("Profile: Default", 18, 180, 170, ARGB(255,220,100,0))
		end
		DrawText("Client Version: " .. CLIENTVERSION, 18, 20, 200, ARGB(255,0,160,255))
		if (Packets:isUpdated()) then
			DrawText("Updated", 18, 200, 200, ARGB(255,0,255,0))
		else
			DrawText("Not Updated", 18, 200, 200, ARGB(255,255,0,0))
		end
		self.sep:Draw(0,230,500)
		self:drawAIinfo()
		self:drawRemoteInfo()
	end

	function init:drawSprite()
		if self.sprite ~= false then
			self.sprite:Draw(0,0,500)
		end
	end

	function init:drawAIinfo()
		if LOCALAWARENESS.posDanger then
			DrawText("Danger: " .. LOCALAWARENESS.posDanger, 15, 20, 245, ARGB(255,0,255,0))
		end
		DrawText("Power: " .. LOCALAWARENESS:HeroStrength(myHero), 15, 180, 245, ARGB(255,0,255,0))
		DrawText("Wave: ".. MINIONS:getWave(), 15, 180, 265, ARGB(255,0,255,0))	
		if Orbwalker.Stage == STAGE_MOVE then
			DrawText("Orbwalker State: Moving", 15, 180, 285, ARGB(255,0,255,0))
		end
		if Orbwalker.Stage == STAGE_SHOOTING then
			DrawText("Orbwalker State: Shooting", 15, 180, 285, ARGB(255,170,100,0))
		end
		if Orbwalker.Stage == STAGE_SHOOT then
			DrawText("Orbwalker State: Ready", 15, 180, 285, ARGB(255,0,255,0))
		end
		DrawText("AI State: " .. StateManager:getActiveState().name, 15, 20, 265, ARGB(255,0,255,0))
		if SELECTED_LANE == BOT_LANE then
			DrawText("Lane: Bottom", 15, 20, 285, ARGB(255,0,255,0))
		end
		if SELECTED_LANE == TOP_LANE then
			DrawText("Lane: Top", 15, 20, 285, ARGB(255,0,255,0))
		end
		if SELECTED_LANE == MID_LANE then
			DrawText("Lane: Middle", 15, 20, 285, ARGB(255,0,255,0))
		end
	end

	function init:drawRemoteInfo()
		self.sep:Draw(0,310,500)
		DrawText("User: ".. Remote.user, 15, 20, 325, ARGB(255,0,255,0))
		DrawText("Server Version: ".. Remote.version, 15, 20, 345, ARGB(255,0,255,0))
		if Remote.loggedIn then
			DrawText("Auth: Authenticated", 15, 180, 325, ARGB(255,0,255,0))
		else
			DrawText("Auth: Failed", 15, 180, 325, ARGB(255,255,0,0))
		end
	end

	function init:loadSprite()
		if FileExist(SPRITE_PATH .. "/hfl.png") then
			self.sprite = createSprite(SPRITE_PATH .. "/hfl.png")
			self.sprite:SetScale(0.5,0.5)

			self.sep = createSprite(SPRITE_PATH .. "/hflsep.png")
			self.sep:SetScale(0.5,0.5)

			self.sep2 = createSprite(SPRITE_PATH .. "/hflsep.png")
			self.sep2:SetScale(0.5,0.5)
		end
	end
class 'debugger'
	function debugger:__init()
		AddDrawCallback(function()
			self:nodeManagerDraw()
		end)

		AddMsgCallback(function(e,t)
			if e == 257 and t == 77 then --m key up add minion ranged debug
				self:addRangedEnemyMinion(mousePos)
			end
			if e == 257 and t == 78 then --n key up add minion melee debug
				self:addMeleeEnemyMinion(mousePos)
			end
			if e == 257 and t == 67 then --c key up add minion clear all debug
				self.debugMinions = {}
				self.towers = {}
				self.enemyHeroes = {}
			end
			if e == 257 and t == 84 then --t key up add enemy tower debug
				self:addTower(mousePos)
			end
			if e == 257 and t == 72 then --t key up add enemy tower debug
				self:addEnemyHero(mousePos)
			end
		end)

		self.debugMinions = {}
		self.towers = {}
		self.enemyHeroes = {}
	end

	function debugger:addTower(pos)
		table.insert(self.towers, {x=pos.x,z=pos.z,y=pos.y,charName="Enemy Tower",range=1000})
	end

	function debugger:addEnemyHero(pos)
		table.insert(self.enemyHeroes, {x=pos.x,z=pos.z,y=pos.y,charName="Enemy Hero",range=math.random(150,600)})
	end

	function debugger:addMeleeEnemyMinion(pos)
		table.insert(self.debugMinions, {x=pos.x,z=pos.z,y=pos.y,charName="Debug Minion Melee",range=300})
	end

	function debugger:addRangedEnemyMinion(pos)
		table.insert(self.debugMinions, {x=pos.x,z=pos.z,y=pos.y,charName="Debug Minion Ranged",range=600})
	end

	function debugger:nodeManagerDraw()
		if not _G.hflTasks[MAPNAME][TEAMNUMBER] then
			return
		end
		for i,minion in pairs(self.debugMinions) do
			DrawCircle(minion.x, minion.y, minion.z, 50, ARGB(255, 255, 0, 0))
			DrawCircle(minion.x, minion.y, minion.z, minion.range, ARGB(255, 255, 0, 0))
			local po = WorldToScreen(D3DXVECTOR3(minion.x,minion.y,minion.z))
			DrawText(minion.charName, 20, po.x, po.y, ARGB(255, 255, 255, 0))
		end
		for i,tower in pairs(self.towers) do
			DrawCircle(tower.x, tower.y, tower.z, 50, ARGB(255, 255, 0, 0))
			DrawCircle(tower.x, tower.y, tower.z, tower.range, ARGB(255, 255, 0, 0))
			local po = WorldToScreen(D3DXVECTOR3(tower.x,tower.y,tower.z))
			DrawText(tower.charName, 20, po.x, po.y, ARGB(255, 255, 255, 0))
		end
		for i,hero in pairs(self.enemyHeroes) do
			DrawCircle(hero.x, hero.y, hero.z, 50, ARGB(255, 255, 0, 0))
			DrawCircle(hero.x, hero.y, hero.z, hero.range, ARGB(255, 255, 0, 0))
			local po = WorldToScreen(D3DXVECTOR3(hero.x,hero.y,hero.z))
			DrawText(hero.charName, 20, po.x, po.y, ARGB(255, 255, 255, 0))
		end
		for i,task in pairs(_G.hflTasks[MAPNAME][TEAMNUMBER]) do
			if task.type == "Object" then
				DrawCircle(task.point.x, task.point.y, task.point.z, 500, ARGB(255, 255, 255, 0))
				local po = WorldToScreen(D3DXVECTOR3(task.point.x,task.point.y,task.point.z))
				DrawText("" .. i, 35, po.x, po.y, ARGB(255, 255, 255, 0))
			end
			if task.type == "Node" then
				DrawCircle(task.point.x, task.point.y, task.point.z, 150, ARGB(255, 255, 255, 0))
				local po = WorldToScreen(D3DXVECTOR3(task.point.x,task.point.y,task.point.z))
				DrawText("" .. i, 25, po.x, po.y, ARGB(255, 255, 255, 0))
			end
			if task.type == "Base" then
				DrawCircle(task.point.x, task.point.y, task.point.z, 150, ARGB(255, 255, 255, 0))
				local po = WorldToScreen(D3DXVECTOR3(task.point.x,task.point.y,task.point.z))
				DrawText("Spawn", 25, po.x-30, po.y, ARGB(255, 0, 0, 255))
			end
			if task.type ~= "Base" then
				if task.next ~= nil then
					if not _G.hflTasks[MAPNAME][TEAMNUMBER][task.next] then
						task.next = nil
					else
						local ne,curr
						ne = WorldToScreen(D3DXVECTOR3(_G.hflTasks[MAPNAME][TEAMNUMBER][task.next].point.x,_G.hflTasks[MAPNAME][TEAMNUMBER][task.next].point.y,_G.hflTasks[MAPNAME][TEAMNUMBER][task.next].point.z))
						curr = WorldToScreen(D3DXVECTOR3(task.point.x,task.point.y,task.point.z))
					    DrawLine(curr.x, curr.y, ne.x, ne.y, 3, ARGB(0xFF,0xFF,0xFF,0xFF))
					end
				end
			else
				for i,lane in pairs(task.lanes) do
					local ne,curr
					ne = WorldToScreen(D3DXVECTOR3(_G.hflTasks[MAPNAME][TEAMNUMBER][lane].point.x,_G.hflTasks[MAPNAME][TEAMNUMBER][lane].point.y,_G.hflTasks[MAPNAME][TEAMNUMBER][lane].point.z))
					curr = WorldToScreen(D3DXVECTOR3(task.point.x,task.point.y,task.point.z))
				    DrawLine(curr.x, curr.y, ne.x, ne.y, 3, ARGB(0xFF,0xFF,0xFF,0xFF))
				end
			end
			if self.leftNodeSelected ~= nil then
				local po = WorldToScreen(D3DXVECTOR3(self.leftNodeSelected.point.x,self.leftNodeSelected.point.y,self.leftNodeSelected.point.z))
				local ms = WorldToScreen(D3DXVECTOR3(mousePos))
			    DrawLine(po.x, po.y, ms.x, ms.y, 3, ARGB(0xFF,0xFF,0xFF,0xFF))
			end
		end

		for i = 1, objManager.maxObjects do
	        local object = objManager:getObject(i)
	        if object ~= nil and object.name ~= nil and #object.name > 1 and GetDistance2D(mousePos,object.pos) < 150 and not string.find(object.name,".troy") then
	          	local po = WorldToScreen(D3DXVECTOR3(object.pos.x,object.pos.y,object.pos.z))
				DrawText(object.name, 15, po.x, po.y, ARGB(255, 0, 255, 0))
				DrawText(object.type, 15, po.x, po.y+25, ARGB(255, 255, 255, 0))
				if(string.find(object.name,"Minion")) then
					DrawText(object.charName, 15, po.x, po.y+50, ARGB(255, 255, 100, 0))
				end
	        end
      	end
	end
class 'editor'
	function editor:__init()
		AddMsgCallback(function(e,t)
			if e == 257 and t == 17 then
				self:deleteHover()
			end
			if e == 257 and t == 16 then
				self:connectHover()
			end
			if e == 514 and t == 0 then
				self:mouseUp()
			end
			if e == 513 and t == 1 then
				self:mouseDown()
			end
			if self.selectedTask ~= nil then
				if self.selectedTask.type == "Node" or self.selectedTask.type == "Base" then
					self.selectedTask.point = {x=mousePos.x,y=mousePos.y,z=mousePos.z}
				end
			end
		end)

		AddDrawCallback(function()
			self:drawManager()
		end)

		--Editor Locals
		self.selectedTask = nil
		self.deletePressed = false
		self.leftNodeSelected = nil
		if _G.hflTasks[MAPNAME][TEAMNUMBER][1] and _G.hflTasks[MAPNAME][TEAMNUMBER][1].type == "Base" then
			self.spawnAdded = true
		else
			self.spawnAdded = false
		end
		
		self.towers = {}
		self.hqs = {}
		self.baracks = {}
		self:collectTowers()
		self:collectHqs()
		self:collectBaracks()
	end

	function editor:collectTowers()
		for i = 1, objManager.maxObjects do
	        local tow = objManager:getObject(i)
	        if tow and tow.type == "obj_AI_Turret" then
	        	table.insert(self.towers, tow)
	        end
	    end
	end
	function editor:collectHqs()
		for i = 1, objManager.maxObjects do
	        local tow = objManager:getObject(i)
	        if tow and tow.type == "obj_HQ" then
	        	table.insert(self.towers, tow)
	        end
	    end
	end
	function editor:collectBaracks()
		for i = 1, objManager.maxObjects do
	        local tow = objManager:getObject(i)
	        if tow and tow.type == "obj_BarracksDampener" then
	        	table.insert(self.towers, tow)
	        end
	    end
	end

	function editor:connectHover(  )
		if self.leftNodeSelected == nil then
			for i,task in pairs(_G.hflTasks[MAPNAME][TEAMNUMBER]) do
				if task.type == "Object" then
					if GetDistance(mousePos, task.point) < 500 then
						self.leftNodeSelected = task
					end
				end
				if task.type == "Node" or task.type == "Base" then
					if GetDistance(mousePos, task.point)  < 150  then
						self.leftNodeSelected = task
					end
				end
			end
		else
			for i,task in pairs(_G.hflTasks[MAPNAME][TEAMNUMBER]) do
				if task.type == "Object" then
					if GetDistance(mousePos, task.point) < 300 then
						if self.leftNodeSelected.type == "Base" then
							table.insert(self.leftNodeSelected.lanes,i)
						else
							self.leftNodeSelected.next = i
						end
					end
				end
				if task.type == "Node" then
					if GetDistance(mousePos, task.point)  < 300  then
						if self.leftNodeSelected.type == "Base" then
							table.insert(self.leftNodeSelected.lanes,i)
						else
							self.leftNodeSelected.next = i
						end
					end
				end
			end
			if self.leftNodeSelected.type ~= "Base" then
				if self.leftNodeSelected.next == nil then
					local towerDetected = nil
					local buildingDetected = nil
					for c,tow in pairs(self.towers) do
						if GetDistance(tow,mousePos) < 300 then
							towerDetected = tow
						end
					end

					if towerDetected ~= nil then
						table.insert(_G.hflTasks[MAPNAME][TEAMNUMBER],{point={x=towerDetected.x,y=towerDetected.y,z=towerDetected.z},type="Object",next=nil})
					else
						if buildingDetected == nil then
							table.insert(_G.hflTasks[MAPNAME][TEAMNUMBER],{point={x=mousePos.x,y=mousePos.y,z=mousePos.z},type="Node",next=nil})
						end
					end
					self.leftNodeSelected.next = #_G.hflTasks[MAPNAME][TEAMNUMBER]
					self.leftNodeSelected = _G.hflTasks[MAPNAME][TEAMNUMBER][#_G.hflTasks[MAPNAME][TEAMNUMBER]]
				else
					self.leftNodeSelected = _G.hflTasks[MAPNAME][TEAMNUMBER][self.leftNodeSelected.next]
				end
				self:save()
			end
		end
	end

	function editor:deleteHover(  )
		for i,task in pairs(_G.hflTasks[MAPNAME][TEAMNUMBER]) do
			if task.type == "Object" then
				if GetDistance(mousePos, task.point) < 500 then
					table.remove(_G.hflTasks[MAPNAME][TEAMNUMBER], i)
				end
			end
			if task.type == "Node" then
				if GetDistance(mousePos, task.point)  < 150  then
					table.remove(_G.hflTasks[MAPNAME][TEAMNUMBER], i)
				end
			end
			if task.type == "Base" then
				if GetDistance(mousePos, task.point)  < 150  then
					_G.hflTasks[MAPNAME][TEAMNUMBER] = {}
					self.spawnAdded = false
				end
			end
		end
		self:save()
	end

	function editor:mouseDown()
		if self.leftNodeSelected == nil then
			for i,task in pairs(_G.hflTasks[MAPNAME][TEAMNUMBER]) do
				if task.type == "Object" then
					if GetDistance(mousePos, task.point) < 500 then
						self.selectedTask = task
					end
				end
				if task.type == "Node" then
					if GetDistance(mousePos, task.point)  < 150  then
						self.selectedTask = task
					end
				end
				if task.type == "Base" then
					if GetDistance(mousePos, task.point)  < 150  then
						self.selectedTask = task
					end
				end
			end
		end
	end

	function editor:mouseUp()
		if self.leftNodeSelected == nil then
			if self.selectedTask == nil then
				if self.spawnAdded then
					local towerDetected = nil
					for c,tow in pairs(self.towers) do
						if GetDistance(tow,mousePos) < 300 then
							towerDetected = tow
						end
					end
					if towerDetected ~= nil then
						table.insert(_G.hflTasks[MAPNAME][TEAMNUMBER],{point={x=towerDetected.x,y=towerDetected.y,z=towerDetected.z},type="Object",next=nil})
					else
						local baracksDetected = nil
						for c,barack in pairs(self.baracks) do
							if GetDistance(barack,mousePos) < 300 then
								baracksDetected = barack
							end
						end
						if baracksDetected ~= nil then
							table.insert(_G.hflTasks[MAPNAME][TEAMNUMBER],{point={x=towerDetected.x,y=towerDetected.y,z=towerDetected.z},type="Object",next=nil})
						else
							local hqDetected = nil
							for c,hq in pairs(self.hqs) do
								if GetDistance(hq,mousePos) < 300 then
									hqDetected = hq
								end
							end
							if hqDetected ~= nil then
								table.insert(_G.hflTasks[MAPNAME][TEAMNUMBER],{point={x=towerDetected.x,y=towerDetected.y,z=towerDetected.z},type="Object",next=nil})
							else
								table.insert(_G.hflTasks[MAPNAME][TEAMNUMBER],{point={x=mousePos.x,y=mousePos.y,z=mousePos.z},type="Node",next=nil})
							end
						end
						
					end
				else
					table.insert(_G.hflTasks[MAPNAME][TEAMNUMBER],{point={x=mousePos.x,y=mousePos.y,z=mousePos.z},type="Base",lanes={}})
					self.spawnAdded = true
				end
			end
		else
			for i,task in pairs(_G.hflTasks[MAPNAME][TEAMNUMBER]) do
				if task.type == "Object" then
					if GetDistance(mousePos, task.point) < 200 then
						if self.leftNodeSelected.type == "Base" then
							if GetDistance(mousePos, task.point)  < 200  then
								table.insert(self.leftNodeSelected.lanes,i)
							end
						else
							self.leftNodeSelected.next = i
						end
					end
				end
				if task.type == "Node" then
					if GetDistance(mousePos, task.point)  < 200  then
						if self.leftNodeSelected.type == "Base" then
							if GetDistance(mousePos, task.point)  < 200  then
								table.insert(self.leftNodeSelected.lanes,i)
							end
						else
							self.leftNodeSelected.next = i
						end
					end
				end
			end
			self.leftNodeSelected = nil
		end
		self.selectedTask = nil
		self:save()
	end

	function editor:drawManager()
		for i,task in pairs(_G.hflTasks[MAPNAME][TEAMNUMBER]) do

			if task.type == "Object" then
				DrawCircle(task.point.x, task.point.y, task.point.z, 500, ARGB(255, 255, 255, 0))
				local po = WorldToScreen(D3DXVECTOR3(task.point.x,task.point.y,task.point.z))
				DrawText("" .. i, 35, po.x, po.y, ARGB(255, 255, 255, 0))
			end
			if task.type == "Node" then
				DrawCircle(task.point.x, task.point.y, task.point.z, 150, ARGB(255, 255, 255, 0))
				local po = WorldToScreen(D3DXVECTOR3(task.point.x,task.point.y,task.point.z))
				DrawText("" .. i, 25, po.x, po.y, ARGB(255, 255, 255, 0))
			end
			if task.type == "Base" then
				DrawCircle(task.point.x, task.point.y, task.point.z, 150, ARGB(255, 255, 255, 0))
				local po = WorldToScreen(D3DXVECTOR3(task.point.x,task.point.y,task.point.z))
				DrawText("Spawn", 25, po.x-30, po.y, ARGB(255, 0, 0, 255))
			end
			if task.type ~= "Base" then
				if task.next ~= nil then
					if not _G.hflTasks[MAPNAME][TEAMNUMBER][task.next] then
						task.next = nil
					else
						local ne,curr
						ne = WorldToScreen(D3DXVECTOR3(_G.hflTasks[MAPNAME][TEAMNUMBER][task.next].point.x,_G.hflTasks[MAPNAME][TEAMNUMBER][task.next].point.y,_G.hflTasks[MAPNAME][TEAMNUMBER][task.next].point.z))
						curr = WorldToScreen(D3DXVECTOR3(task.point.x,task.point.y,task.point.z))
					    DrawLine(curr.x, curr.y, ne.x, ne.y, 3, ARGB(0xFF,0xFF,0xFF,0xFF))
					end
				end
			else
				for i,lane in pairs(task.lanes) do
					local ne,curr
					ne = WorldToScreen(D3DXVECTOR3(_G.hflTasks[MAPNAME][TEAMNUMBER][lane].point.x,_G.hflTasks[MAPNAME][TEAMNUMBER][lane].point.y,_G.hflTasks[MAPNAME][TEAMNUMBER][lane].point.z))
					curr = WorldToScreen(D3DXVECTOR3(task.point.x,task.point.y,task.point.z))
				    DrawLine(curr.x, curr.y, ne.x, ne.y, 3, ARGB(0xFF,0xFF,0xFF,0xFF))
				end
			end
			if self.leftNodeSelected ~= nil then
				local po = WorldToScreen(D3DXVECTOR3(self.leftNodeSelected.point.x,self.leftNodeSelected.point.y,self.leftNodeSelected.point.z))
				local ms = WorldToScreen(D3DXVECTOR3(mousePos))
			    DrawLine(po.x, po.y, ms.x, ms.y, 3, ARGB(0xFF,0xFF,0xFF,0xFF))
			end
		end

		for i = 1, objManager.maxObjects do
	        local object = objManager:getObject(i)
	        if object ~= nil and object.name ~= nil and #object.name > 1 and GetDistance2D(mousePos,object.pos) < 150 and not string.find(object.name,".troy") then
	          	local po = WorldToScreen(D3DXVECTOR3(object.pos.x,object.pos.y,object.pos.z))
				DrawText(object.name, 15, po.x, po.y, ARGB(255, 0, 255, 0))
				DrawText(object.type, 15, po.x, po.y+25, ARGB(255, 255, 255, 0))
	        end
	  	end
	end

	function editor:save()
		local pickledString = pickle(_G.hflTasks)
		local file = io.open(LIB_PATH .. "/HfLib.lua", "w")
		file:write(pickledString)
		file:close()
	end

	function editor:load()
		local file = io.open(LIB_PATH .. "/HfLib.lua", "rb")
		local content = file:read("*all")
		file:close()
		_G.hflTasks = unpickle(content)
	end
class '_Tasks'
	function _Tasks:__init()	
		self.taskLane = nil
		self.towers = {}
		self.hqs = {}
		self.baracks = {}
		self:collectTowers()
		self:collectHqs()
		self:collectBaracks()
		self:buildTaskObjects()

		self:getCurrentTask()

		AddTickCallback(function ( )
			self:pickLane()
		end)

		return self
	end

	function _Tasks:updateLane()

	end

	function _Tasks:collectTowers()
		for i = 1, objManager.maxObjects do
	        local tow = objManager:getObject(i)
	        if tow and tow.type == "obj_AI_Turret" then
	        	table.insert(self.towers, tow)
	        end
	    end
	end
	function _Tasks:collectHqs()
		for i = 1, objManager.maxObjects do
	        local tow = objManager:getObject(i)
	        if tow and tow.type == "obj_HQ" then
	        	table.insert(self.towers, tow)
	        end
	    end
	end
	function _Tasks:collectBaracks()
		for i = 1, objManager.maxObjects do
	        local tow = objManager:getObject(i)
	        if tow and tow.type == "obj_BarracksDampener" then
	        	table.insert(self.towers, tow)
	        end
	    end
	end

	function _Tasks:buildTaskObjects()
		for i,task in pairs(_G.hflTasks[MAPNAME][TEAMNUMBER]) do
			if task.type == "Object" then
				local towerDetected = nil
				for c,tow in pairs(self.towers) do
					if GetDistance(tow,mousePos) < 300 then
						towerDetected = tow
					end
				end
				if towerDetected ~= nil then
					task.object = towerDetected
				else
					local baracksDetected = nil
					for c,barack in pairs(self.baracks) do
						if GetDistance(barack,mousePos) < 300 then
							baracksDetected = barack
						end
					end
					if baracksDetected ~= nil then
						task.object = baracksDetected
					else
						local hqDetected = nil
						for c,hq in pairs(self.hqs) do
							if GetDistance(hq,mousePos) < 300 then
								hqDetected = hq
							end
						end
						if hqDetected ~= nil then
							task.object = hqDetected
						else
							task.type = "Node"
						end
					end
					
				end
			end
		end
	end

	function _Tasks:pickLane()
		if #_G.hflTasks[MAPNAME][TEAMNUMBER][1].lanes > 1 then
			self.taskLane = _G.hflTasks[MAPNAME][TEAMNUMBER][1].lanes[tonumber(SELECTED_LANE) + 1]
		end
	end

	function _Tasks:getCurrentTask()
		if self.taskLane ~= nil then
			local nearestTask = nil
			local looper = _G.hflTasks[MAPNAME][TEAMNUMBER][self.taskLane]
			while looper.next ~= nil do
				local task = looper
				if nearestTask == nil then
					nearestTask = task
					if GetDistance2D(task.point,_G.hflTasks[MAPNAME][TEAMNUMBER][1].point) > GetDistance2D(myHero,_G.hflTasks[MAPNAME][TEAMNUMBER][1].point) then
						if GetDistance2D(task.point,myHero) < 200 then
							nearestTask =  _G.hflTasks[MAPNAME][TEAMNUMBER][task.next]
						end
						break
					end
				else
					if GetDistance2D(task.point,_G.hflTasks[MAPNAME][TEAMNUMBER][1].point) > GetDistance2D(myHero,_G.hflTasks[MAPNAME][TEAMNUMBER][1].point) then
						nearestTask = task
						if GetDistance2D(task.point,myHero) < 200 then
							nearestTask =  _G.hflTasks[MAPNAME][TEAMNUMBER][task.next]
						end
						break
					end
				end
				looper = _G.hflTasks[MAPNAME][TEAMNUMBER][looper.next]
			end
			return nearestTask
		else
			return nil
		end
		return _G.hflTasks[MAPNAME][TEAMNUMBER][1]
	end

	function _Tasks:getPreviousTask()
		local currentTask = self:getCurrentTask()
		if currentTask == _G.hflTasks[MAPNAME][TEAMNUMBER][self.taskLane] then
			return _G.hflTasks[MAPNAME][TEAMNUMBER][1]
		else
			local current = _G.hflTasks[MAPNAME][TEAMNUMBER][self.taskLane]
			while _G.hflTasks[MAPNAME][TEAMNUMBER][current.next] ~= nil do
				if _G.hflTasks[MAPNAME][TEAMNUMBER][current.next] == currentTask then
					return current
				else
					current = _G.hflTasks[MAPNAME][TEAMNUMBER][current.next]
				end
			end
			return current
		end
	end
class '_Packets'
	function _Packets:__init()
		self.disabledPacket = false
		self.version = split(GetGameVersion()," ")[1]
		self.idBytes = {}
		self.spellLevel = {}
		self.buyItem = {}
		self.sellItem = {}

		self:initBytes()
		self:initFunctions()
		if not self.idBytes[self.version] then
			self.disabledPacket = true
		end
	end

	function _Packets:isUpdated()
		if not self.idBytes[self.version] then
			return false
		else
			return true
		end
	end

	function _Packets:buyItemId(id)
		self.buyItem[self.version](id)
	end

	function _Packets:spellUp(id)
		self.spellLevel[self.version](id)
	end

	function _Packets:initBytes()
		self.idBytes["5.22.0.289"] = {
		    [0x01] = 0x39,[0x02] = 0x38,[0x03] = 0x36,[0x04] = 0xB7,[0x05] = 0xB9,[0x06] = 0xB8,[0x07] = 0xB6,[0x08] = 0xF7,
		    [0x09] = 0xF9,[0x0A] = 0xF8,[0x0B] = 0xF6,[0x0C] = 0x77,[0x0D] = 0x79,[0x0E] = 0x78,[0x0F] = 0x76,[0x10] = 0x57,
		    [0x11] = 0x59,[0x12] = 0x58,[0x13] = 0x56,[0x14] = 0xD7,[0x15] = 0xD9,[0x16] = 0xD8,[0x17] = 0xD6,[0x18] = 0x17,
		    [0x19] = 0x19,[0x1A] = 0x18,[0x1B] = 0x16,[0x1C] = 0x97,[0x1D] = 0x99,[0x1E] = 0x98,[0x1F] = 0x96,[0x20] = 0x67,
		    [0x21] = 0x69,[0x22] = 0x68,[0x23] = 0x66,[0x24] = 0xE7,[0x25] = 0xE9,[0x26] = 0xE8,[0x27] = 0xE6,[0x28] = 0x27,
		    [0x29] = 0x29,[0x2A] = 0x28,[0x2B] = 0x26,[0x2C] = 0xA7,[0x2D] = 0xA9,[0x2E] = 0xA8,[0x2F] = 0xA6,[0x30] = 0x4B,
		    [0x31] = 0x4D,[0x32] = 0x4C,[0x33] = 0x4A,[0x34] = 0xCB,[0x35] = 0xCD,[0x36] = 0xCC,[0x37] = 0xCA,[0x38] = 0x0B,
		    [0x39] = 0x0D,[0x3A] = 0x0C,[0x3B] = 0x0A,[0x3C] = 0x8B,[0x3D] = 0x8D,[0x3E] = 0x8C,[0x3F] = 0x8A,[0x40] = 0x30,
		    [0x41] = 0x2E,[0x42] = 0x31,[0x43] = 0x2F,[0x44] = 0xB0,[0x45] = 0xAE,[0x46] = 0xB1,[0x47] = 0xAF,[0x48] = 0xF0,
		    [0x49] = 0xEE,[0x4A] = 0xF1,[0x4B] = 0xEF,[0x4C] = 0x70,[0x4D] = 0x6E,[0x4E] = 0x71,[0x4F] = 0x6F,[0x50] = 0x50,
		    [0x51] = 0x4E,[0x52] = 0x51,[0x53] = 0x4F,[0x54] = 0xD0,[0x55] = 0xCE,[0x56] = 0xD1,[0x57] = 0xCF,[0x58] = 0x10,
		    [0x59] = 0x0E,[0x5A] = 0x11,[0x5B] = 0x0F,[0x5C] = 0x90,[0x5D] = 0x8E,[0x5E] = 0x91,[0x5F] = 0x8F,[0x60] = 0x60,
		    [0x61] = 0x5E,[0x62] = 0x61,[0x63] = 0x5F,[0x64] = 0xE0,[0x65] = 0xDE,[0x66] = 0xE1,[0x67] = 0xDF,[0x68] = 0x20,
		    [0x69] = 0x1E,[0x6A] = 0x21,[0x6B] = 0x1F,[0x6C] = 0xA0,[0x6D] = 0x9E,[0x6E] = 0xA1,[0x6F] = 0x9F,[0x70] = 0x44,
		    [0x71] = 0x42,[0x72] = 0x45,[0x73] = 0x43,[0x74] = 0xC4,[0x75] = 0xC2,[0x76] = 0xC5,[0x77] = 0xC3,[0x78] = 0x04,
		    [0x79] = 0x02,[0x7A] = 0x05,[0x7B] = 0x03,[0x7C] = 0x84,[0x7D] = 0x82,[0x7E] = 0x85,[0x7F] = 0x83,[0x80] = 0x3B,
		    [0x81] = 0x3D,[0x82] = 0x3C,[0x83] = 0x3A,[0x84] = 0xBB,[0x85] = 0xBD,[0x86] = 0xBC,[0x87] = 0xBA,[0x88] = 0xFB,
		    [0x89] = 0xFD,[0x8A] = 0xFC,[0x8B] = 0xFA,[0x8C] = 0x7B,[0x8D] = 0x7D,[0x8E] = 0x7C,[0x8F] = 0x7A,[0x90] = 0x5B,
		    [0x91] = 0x5D,[0x92] = 0x5C,[0x93] = 0x5A,[0x94] = 0xDB,[0x95] = 0xDD,[0x96] = 0xDC,[0x97] = 0xDA,[0x98] = 0x1B,
		    [0x99] = 0x1D,[0x9A] = 0x1C,[0x9B] = 0x1A,[0x9C] = 0x9B,[0x9D] = 0x9D,[0x9E] = 0x9C,[0x9F] = 0x9A,[0xA0] = 0x6B,
		    [0xA1] = 0x6D,[0xA2] = 0x6C,[0xA3] = 0x6A,[0xA4] = 0xEB,[0xA5] = 0xED,[0xA6] = 0xEC,[0xA7] = 0xEA,[0xA8] = 0x2B,
		    [0xA9] = 0x2D,[0xAA] = 0x2C,[0xAB] = 0x2A,[0xAC] = 0xAB,[0xAD] = 0xAD,[0xAE] = 0xAC,[0xAF] = 0xAA,[0xB0] = 0x40,
		    [0xB1] = 0x3E,[0xB2] = 0x41,[0xB3] = 0x3F,[0xB4] = 0xC0,[0xB5] = 0xBE,[0xB6] = 0xC1,[0xB7] = 0xBF,[0xB8] = 0x00,
		    [0xB9] = 0xFE,[0xBA] = 0x01,[0xBB] = 0xFF,[0xBC] = 0x80,[0xBD] = 0x7E,[0xBE] = 0x81,[0xBF] = 0x7F,[0xC0] = 0x34,
		    [0xC1] = 0x32,[0xC2] = 0x35,[0xC3] = 0x33,[0xC4] = 0xB4,[0xC5] = 0xB2,[0xC6] = 0xB5,[0xC7] = 0xB3,[0xC8] = 0xF4,
		    [0xC9] = 0xF2,[0xCA] = 0xF5,[0xCB] = 0xF3,[0xCC] = 0x74,[0xCD] = 0x72,[0xCE] = 0x75,[0xCF] = 0x73,[0xD0] = 0x54,
		    [0xD1] = 0x52,[0xD2] = 0x55,[0xD3] = 0x53,[0xD4] = 0xD4,[0xD5] = 0xD2,[0xD6] = 0xD5,[0xD7] = 0xD3,[0xD8] = 0x14,
		    [0xD9] = 0x12,[0xDA] = 0x15,[0xDB] = 0x13,[0xDC] = 0x94,[0xDD] = 0x92,[0xDE] = 0x95,[0xDF] = 0x93,[0xE0] = 0x64,
		    [0xE1] = 0x62,[0xE2] = 0x65,[0xE3] = 0x63,[0xE4] = 0xE4,[0xE5] = 0xE2,[0xE6] = 0xE5,[0xE7] = 0xE3,[0xE8] = 0x24,
		    [0xE9] = 0x22,[0xEA] = 0x25,[0xEB] = 0x23,[0xEC] = 0xA4,[0xED] = 0xA2,[0xEE] = 0xA5,[0xEF] = 0xA3,[0xF0] = 0x48,
		    [0xF1] = 0x46,[0xF2] = 0x49,[0xF3] = 0x47,[0xF4] = 0xC8,[0xF5] = 0xC6,[0xF6] = 0xC9,[0xF7] = 0xC7,[0xF8] = 0x08,
   			[0xF9] = 0x06,[0xFA] = 0x09,[0xFB] = 0x07,[0xFC] = 0x88,[0xFD] = 0x86,[0xFE] = 0x89,[0xFF] = 0x87,[0x00] = 0x37,
		}

		self.idBytes["6.3.0.240"] = {}
	end

	function _Packets:initFunctions()
		--
		--SPELL LEVELUPS
		--
		self.spellLevel["4.24.0.249"] = (function(id)
			local offsets = { 
			    [_Q] = 0x1E,
			    [_W] = 0xD3,
			    [_E] = 0x3A,
			    [_R] = 0xA8,
		  	}
		  	local p = CLoLPacket(0x00B6)
		  	p.vTable = 0xFE3124
		  	p:EncodeF(myHero.networkID)
		  	p:Encode1(0xC1)
		  	p:Encode1(offsets[id])
		  	for i = 1, 4 do p:Encode1(0x63) end
		  	for i = 1, 4 do p:Encode1(0xC5) end
		  	for i = 1, 4 do p:Encode1(0x6A) end
	  		for i = 1, 4 do p:Encode1(0x00) end
	  		SendPacket(p)
		end)
		self.spellLevel["5.22.0.289"] = (function (id)
		  	local offsets = {
		  	  	[_Q] = 0xB8,
		  	  	[_W] = 0xBA,
		  	  	[_E] = 0x79,
		  	  	[_R] = 0x7B,
		  	}
		  	local p = CLoLPacket(0x0050)
		  	p.vTable = 0xF38DAC
		  	p:EncodeF(myHero.networkID)
		  	p:Encode1(offsets[id])
		  	p:Encode1(0x3C)
		  	for i = 1, 4 do p:Encode1(0xF6) end
		  	for i = 1, 4 do p:Encode1(0x5E) end
		  	for i = 1, 4 do p:Encode1(0xE0) end
		  	p:Encode1(0x24)
		  	p:Encode1(0xF1)
		  	p:Encode1(0x27)
		  	p:Encode1(0x00)
		  	SendPacket(p)
		end)
		self.spellLevel["5.21"] = (function (id)
		  	local offsets = {
		   [_Q] = 0x85,
		   [_W] = 0x45,
		   [_E] = 0x15,
		   [_R] = 0xC5,
		   }
		   local p
		   p = CLoLPacket(0x130)
		   p.vTable = 0xEDB360
		   p:EncodeF(myHero.networkID)
		   for i = 1, 4 do p:Encode1(0x55) end
		   for i = 1, 4 do p:Encode1(0x74) end
		   p:Encode1(offsets[id])
		   p:Encode1(0xB3)
		   for i = 1, 4 do p:Encode1(0x4F) end
		   p:Encode1(0x01)
		   for i = 1, 3 do p:Encode1(0x00) end
		   SendPacket(p)
		end)
		self.spellLevel["6.3.0.240"] = function (spell)
			LevelSpell(spell)
		end
	    --
		--BUY ITEMS
		--
		self.buyItem["5.22.0.289"] = (function (id)
   			local rB = {}
			for i=0, 255 do rB[self:getTableByte(i)] = i end
			local p = CLoLPacket(0x121)
			p.vTable = 0xEE7E24
			p:EncodeF(myHero.networkID)
			local b1 = bit32.lshift(bit32.band(rB[bit32.band(bit32.rshift(bit32.band(id,0xFFFF),24),0xFF)],0xFF),24)
			local b2 = bit32.lshift(bit32.band(rB[bit32.band(bit32.rshift(bit32.band(id,0xFFFFFF),16),0xFF)],0xFF),16)
			local b3 = bit32.lshift(bit32.band(rB[bit32.band(bit32.rshift(bit32.band(id,0xFFFFFFFF),8),0xFF)],0xFF),8)
			local b4 = bit32.band(rB[bit32.band(id ,0xFF)],0xFF)
			p:Encode4(bit32.bxor(b1,b2,b3,b4))
			p:Encode4(0xED2C0000)
			SendPacket(p)
		end)
		self.buyItem["6.3.0.240"] = (function(id) 
			BuyItem(id)
		end)
		--
		--Sell Items
		--
		self.sellItem["6.3.0.240"] = (function (id)
			SellItem(myHero:getInventorySlot(id))
		end)
	end

	function _Packets:getTableByte(i)
		return self.idBytes[self.version][i]
	end
class '_DamagePred'
	function _DamagePred:__init()
	    self.Preds = {}
	    DamagePred = self
	end

	function _DamagePred:Reset()
	    self.Preds = {}
	end

	function _DamagePred:GetPred(Minion, Type, Skill)
	    local result = Minion.health
	    local predhealth = Minion.health
	    if Type == PRED_LAST_HIT then
	            local time = Orbwalker:GetWindUp() + GetDistance(Minion.visionPos, myHero.visionPos) / MyHero.ProjectileSpeed - 0.07
	            predhealth, _, count = VP:GetPredictedHealth(Minion, time)

	            result = predhealth
	    elseif Type == PRED_TWO_HITS then
	            local time = 0
	            time = Orbwalker:GetAnimationTime() + GetDistance(Minion.visionPos, myHero.visionPos) / MyHero.ProjectileSpeed - 0.07
                time = time * 2

	            predhealth, _, count = VP:GetPredictedHealth2(Minion, time)

	            result = predhealth
	    elseif Type == PRED_SKILL then
	            local object = self.Preds[Minion.networkID]
	            if object and object.Skill and object.Skill[Skill.Key] then
	                    return object.Skill[Skill.Key]
	            else
	                    local time = (Skill.Delay / 1000) + GetDistance(Minion.visionPos, myHero.visionPos) / (Skill.Speed * 1000) - 0.07
	                    predhealth, _, count = VP:GetPredictedHealth(Minion, time)
	                    result = predhealth
	            end
	    elseif Type == PRED_UNKILLABLE then
	            -- local attackTime = Helper:GetTime() + GetLatency() / 2 - Orbwalker.LastAttack >= (1000 / (myHero.attackSpeed * Orbwalker.BaseAttackSpeed)) and 0 or Helper:GetTime() + GetLatency() / 2 - Orbwalker.LastAttack
	            -- attackTime = Orbwalker:GetAnimationTime() - attackTime
	            -- time = attackTime +  Orbwalker:GetWindUp() * 2 + GetDistance(Minion.visionPos, myHero.visionPos) / MyHero.ProjectileSpeed - 0.07
	            -- predhealth, _, count = VP:GetPredictedHealth(Minion, time)
	            -- result = predhealth
	            local time = Orbwalker:GetWindUp() + GetDistance(Minion.visionPos, myHero.visionPos) / MyHero.ProjectileSpeed - 0.07
	            time = time * 1.5
	            predhealth, _, count = VP:GetPredictedHealth(Minion, time)

	            result = predhealth
	    end

	    -- if count > 4 then
	    --      local dmg = Minion.health - predhealth
	    --      predhealth = predhealth - (dmg * (0.1 * count))
	    --      result = predhealth
	    -- end
	    return result
	end
class '_Items' 
	function _Items:__init()
	    self.ItemList = {}
	    Items = self

	    AddTickCallback(function() self:_OnTick() end)
	end

	function _Items:_OnTick()
	    for _, Item in pairs(self.ItemList) do
	            if StateManager:getActiveState().id ~= HOLDLANE_STATE then
                    Item.Active = true
	            else
                    Item.Active = false
	            end
	    end
	end

	function _Items:UseAll(Target)
	    if Target and Target.type == myHero.type then
	            for _, Item in pairs(self.ItemList) do
	                    Item:Use(Target)
	            end
	    end
	end

	function _Items:UseItem(ID, Target)
	    for _, Item in pairs(self.ItemList) do
	            if Item.ID == ID then
	                    Item:Use(Target)
	            end
	    end
	end

	function _Items:GetItem(ID)
	    for _, Item in pairs(self.ItemList) do
	            if Item.ID == ID then
	                    return Item
	            end
	    end
	end

	function _Items:GetBotrkBonusLastHitDamage(StartingDamage, Target)
	    local _BonusDamage = 0
	    if GetInventoryHaveItem(3153) then
	            if ValidTarget(Target) then
	                    _BonusDamage = Target.health / 20
	                    if _BonusDamage >= 60 then
	                            _BonusDamage = 60
	                    end
	            end
	    end
	    return _BonusDamage
	end
class '_Item'
	--TODO: Add Muramana
	function _Item:__init(_Name, _ID, _RequiresTarget, _Range, _Override)
	    self.Name = _Name
	    self.RawName = self.Name:gsub("[^A-Za-z0-9]", "")
	    self.ID = _ID
	    self.RequiresTarget = _RequiresTarget
	    self.Range = _Range
	    self.Slot = nil
	    self.Override = _Override
	    self.Active = true
	    self.Enabled = true

	    table.insert(Items.ItemList, self)
	end

	function _Item:Use(Target)
	    if self.Override then
	            return self.Override()
	    end
	    if self.RequiresTarget and not Target then
	            return
	    end
	    if not self.Active or not self.Enabled then
	            return
	    end

	    self.Slot = GetInventorySlotItem(self.ID)

	    if self.Slot then      
	            if self.ID == 3153 then -- BRK
	                    local _Menu = MenuManager:GetActiveMenu()
	                    if _Menu and _Menu.botrkSave then
	                            if  myHero.health <= myHero.maxHealth * 0.65 then
	                                    CastSpell(self.Slot, Target)
	                            end
	                    elseif _Menu and _Menu.Active then
	                            CastSpell(self.Slot, Target)
	                    end
	            elseif self.ID == 3042 then -- Muramana
	                    if not MuramanaIsActive() then
	                            MuramanaOn()
	                    end
	            elseif self.ID == 3069 then -- Talisman of Ascension
	                    if Helper:CountAlliesInRange(600) > 0 then
	                            CastSpell(self.Slot)
	                    end
	            elseif not self.RequiresTarget and Orbwalker:CanOrbwalkTarget(Target) then
	                    CastSpell(self.Slot)
	            elseif self.RequiresTarget and ValidTarget(Target) and Helper:GetDistance(Target) <= self.Range then
	                    CastSpell(self.Slot, Target)
	            end
	    end
	end
class '_Skills'
	function _Skills:__init()
	    self.SkillsList = {}
	    Skills = self
	    AddTickCallback(function() self:_OnTick() end)
	end

	function _Skills:_OnTick()
	    for _, Skill in pairs(self.SkillsList) do
            if StateManager:getActiveState().id ~= HOLDLANE_STATE then
                Skill.Active = true
            else
                Skill.Active = false
            end
	    end
	end

	function _Skills:CastAll(Target)
	    for _, Skill in ipairs(self.SkillsList) do
	            if Skill.Enabled then
	                    Skill:Cast(Target)
	            end
	    end
	end

	function _Skills:GetSkill(Key)
	    for _, Skill in pairs(self.SkillsList) do
	            if Skill.Key == Key then
	                    return Skill
	            end
	    end
	end

	function _Skills:HasSkillReady()
	    for _, Skill in pairs(self.SkillsList) do
	            if Skill.Ready then
	                    return true
	            end
	    end
	end

	function _Skills:NewSkill(enabled, key, range, displayName, type, minMana, afterAttack, reqAttackTarget, speed, delay, width, collision, isReset)
	    return _Skill(enabled, key, range, displayName, type, minMana, afterAttack, reqAttackTarget, speed, delay, width, collision, isReset, true)
	end

	function _Skills:DisableAll()
	    for _, Skill in pairs(self.SkillsList) do
	            Skill.Enabled = false
	    end
	end

	function _Skills:GetLastHitSkills()
	    local Skills = {}
	    for _, Skill in pairs(self.SkillsList) do
	            if Skill.Type == SPELL_TARGETED or Skill.IsReset or Skill.Type == SPELL_LINEAR_COL or Skill.Type == SPELL_LINEAR then
	                    table.insert(Skills, Skill)
	            end
	    end
	    return Skills
	end
class '_Skill'
    SPELL_TARGETED = 1
    SPELL_LINEAR = 2
    SPELL_CIRCLE = 3
    SPELL_CONE = 4
    SPELL_LINEAR_COL = 5
    SPELL_SELF = 6
    SPELL_SELF_AT_MOUSE = 7

    -- --[[
    --              Initialise _Skill class

    --              enabled                         Boolean - set true for auto carry to automatically cast it, false for manual control in plugin
    --              key                             Spell key, e.g _Q
    --              range                           Spell range
    --              displayName             The name to display in menus
    --              type                            SPELL_TARGETED, SPELL_LINEAR, SPELL_CIRCLE, SPELL_CONE, SPELL_LINEAR_COL, SPELL_SELF, SPELL_SELF_AT_MOUSE
    --              minMana                         Minimum percentage mana before cast is allowed
    --              afterAttack             Boolean - set true to only cast right after an auto attack
    --              reqAttackTarget         Boolean - set true to only cast if a target is in attack range
    --              speed                           Speed of the projectile for skillshots
    --              delay                           Delay of the spell for skillshots
    --              width                           Width of the projectile for skillshots
    --              collision                       Boolean - set true to check minion collision before casting

    -- ]]
    function _Skill:__init(enabled, key, range, displayName, type, minMana, afterAttack, reqAttackTarget, speed, delay, width, collision, isReset, custom)
            self.Key = key
            self.Range = range
            self.DisplayName = displayName
            self.RawName = self.DisplayName:gsub("[^A-Za-z0-9]", "")
            self.Type = type
            self.MinMana = minMana or 0
            self.AfterAttack = afterAttack or false
            self.ReqAttackTarget = reqAttackTarget or false
            self.Speed = speed or 0
            self.Delay = delay or 0
            self.Width = width or 0
            self.Collision = collision
            self.IsReset = isReset or false
            self.IsCustom = custom
            self.Active = true
            self.Enabled = enabled or false
            self.Ready = false

            AddTickCallback(function() self:_OnTick() end)

            table.insert(Skills.SkillsList, self)
    end

    function _Skill:_OnTick()
            self.Ready = myHero:CanUseSpell(self.Key) == READY
    end

    function _Skill:Cast(Target, ForceCast)
            if not ForceCast then
                    if (not self.Active and self.Enabled) or (not self.Enabled and not self.IsCustom) then
                            return
                    elseif self.AfterAttack and not Orbwalker:IsAfterAttack() then
                            return
                    elseif (self.ReqAttackTarget and not Orbwalker:CanOrbwalkTarget(Target)) then
                            return
                    end
            end
            if not self:IsReady() then
                    return
            end

            if self.Type == SPELL_SELF then
                    CastSpell(self.Key)
            elseif self.Type == SPELL_SELF_AT_MOUSE then
                    CastSpell(self.Key, mousePos.x, mousePos.z)
            elseif self.Type == SPELL_TARGETED then
                    if ValidTarget(Target, self.Range) then
                            CastSpell(self.Key, Target)
                    end
            elseif self.Type == SPELL_LINEAR or self.Type == SPELL_LINEAR_COL or self.Type == SPELL_CONE then
                    if ValidTarget(Target) then    
                            local predPos = self:GetPrediction(Target, true, ForceCast)
                            if predPos and GetDistance(predPos) <= self.Range then
                                    CastSpell(self.Key, predPos.x, predPos.z)
                            end
                    end
            elseif self.Type == SPELL_CIRCLE then
                    if ValidTarget(Target) then    
                            local predPos = self:GetPrediction(Target, false, ForceCast)
                            if predPos and GetDistance(predPos) <= self.Range then
                                    CastSpell(self.Key, predPos.x, predPos.z)
                            end
                    end
            end
    end

    function _Skill:ForceCast(Target)
            self:Cast(Target, true)
    end

    function _Skill:GetPrediction(Target, isLine, forceCast)
            local isCol = false
            if self.Collision or self.Type == SPELL_LINEAR_COL then
                    isCol = true
            end

            if forceCast then
                    isCol = false
            end

            if VIP_USER then
                    if isLine then
                            CastPosition,  HitChance,  Position = VP:GetLineCastPosition(Target, self.Delay / 1000, self.Width, self.Range, self.Speed * 1000, myHero, isCol)
                    else
                            CastPosition,  HitChance,  Position = VP:GetCircularCastPosition(Target, self.Delay / 1000, self.Width, self.Range, self.Speed * 1000, myHero, isCol)
                    end

                    if HitChance >= 2 then
                            return CastPosition
                    end
            elseif not VIP_USER then
                    pred = TargetPrediction(self.Range, self.Speed, self.Delay, self.Width)
                    pred = pred:GetPrediction(Target)

                    if isCol then
                            local collision = self:GetCollision(pred)
                            if not collision then
                                    return pred
                            end
                    else
                            return pred
                    end
            end
    end

    function _Skill:GetLinePrediction(Target)
            return self:GetPrediction(Target, true)
    end

    function _Skill:GetCirclePrediction(Target)
            return self:GetPrediction(Target, false)
    end

    function _Skill:GetCollision(pos)
            if VIP_USER and self.Collision then
                    local col = Collision(self.Range, self.Speed*1000, self.Delay/1000, self.Width)
                    return col:GetMinionCollision(myHero, pos)
            elseif self.Collision then
                    for _, Minion in pairs(Minions.EnemyMinions.objects) do
                            if ValidTarget(Minion) and myHero.x ~= Minion.x then
                                    local myX = myHero.x
                                    local myZ = myHero.z
                                    local tarX = pos.x
                                    local tarZ = pos.z
                                    local deltaX = myX - tarX
                                    local deltaZ = myZ - tarZ
                                    local m = deltaZ/deltaX
                                    local c = myX - m*myX
                                    local minionX = Minion.x
                                    local minionZ = Minion.z
                                    local distanc = (math.abs(minionZ - m*minionX - c))/(math.sqrt(m*m+1))
                                    if distanc < self.Width and ((tarX - myX)*(tarX - myX) + (tarZ - myZ)*(tarZ - myZ)) > ((tarX - minionX)*(tarX - minionX) + (tarZ - minionZ)*(tarZ - minionZ)) then
                                            return true
                                    end
                            end
               end
               return false
            end
    end

    function _Skill:GetHitChance(pred)
            if VIP_USER then
                    return pred:GetHitChance(target) > ConfigMenu.HitChance/100
            end
    end

    function _Skill:GetRange()
            return self.reqAttackTarget and MyHero.TrueRange or self.Range
    end

    function _Skill:IsReady()
            return myHero:CanUseSpell(self.Key) == READY
    end
class '_Crosshair' 
	--[[
	            Initialise _Crosshair class

	            damageType      DAMAGE_PHYSICAL or DAMAGE_MAGIC
	            attackRange     Integer
	            skillRange              Integer
	            targetFocused   Boolean. Whether targets selected with left click should be focused.
	            isCaster                Boolean. Whether spells should be prioritised over auto attacks.
	]]

	function _Crosshair:__init(damageType, attackRange, skillRange, targetFocused, isCaster)
	    self.DamageType = damageType and damageType or DAMAGE_PHYSICAL
	    self.AttackRange = attackRange
	    self.SkillRange = skillRange
	    self.TargetFocused = targetFocused
	    self.TargetLock = nil
	    self.IsCaster = isCaster
	    self.Target = nil
	    self.TargetMinion = nil
	    self.Attack_Crosshair = TargetSelector(TARGET_LOW_HP_PRIORITY, 2000, DAMAGE_PHYSICAL, self.TargetFocused)
	    self.Skills_Crosshair = TargetSelector(TARGET_LOW_HP_PRIORITY, skillRange, self.DamageType, self.TargetFocused)
	    self.Attack_Crosshair:SetConditional(function(Hero) return self:Conditional(Hero) end)
	    self.Attack_Crosshair:SetDamages(0, myHero.totalDamage, 0)
	    self:ArrangePriorities()
	    self.RangeScaling = true
	    Crosshair = self

	    self:UpdateCrosshairRange()
	    self:LoadTargetSelector()

	    AddTickCallback(function() self:_OnTick() end)
	    AddUnloadCallback(function() self:_OnUnload() end)
	    AddExitCallback(function() self:_OnExit() end)
	end

	function _Crosshair:_OnTick()
	    self.Attack_Crosshair:update()

	    if (self.TargetLock and self.TargetLock.dead) then
	            self.TargetLock = nil
	    elseif not self.TargetLock then
	            self:SetTargetLock(self:GetTarget())
	    end

	    if self.Attack_Crosshair.target then
	            self.Target = self.Attack_Crosshair.target
	    else
	            self.Skills_Crosshair:update()
	            self.Target = self.Skills_Crosshair.target
	    end
	    self.TargetMinion = Minions.Target
	end

	function _Crosshair:_OnUnload()
	    self:SaveTargetSelector()
	end

	function _Crosshair:_OnExit()
	    self:SaveTargetSelector()
	end

	function _Crosshair:GetTarget()
	    if self.TargetLock then
	            return self.TargetLock
	    elseif ValidTarget(self.Attack_Crosshair.target) and not self.IsCaster then
	            return self.Attack_Crosshair.target
	    elseif ValidTarget(self.Skills_Crosshair.target) then
	            return self.Skills_Crosshair.target
	    end
	end

	function _Crosshair:HasOrbwalkTarget()
	    return self and self.Target and self.Attack_Crosshair.Target and self.Target == self.Attack_Crosshair.target
	end

	function _Crosshair:ArrangePriorities()
	    if #GetEnemyHeroes() < 5 then return end
	    for _, Champion in pairs(Data.ChampionData) do
	            TS_SetHeroPriority(Champion.Priority, Champion.Name)
	    end
	end

	function _Crosshair:SetSkillCrosshairRange(Range)
	    self.RangeScaling = false
	    self.Skills_Crosshair.range = Range
	end

	function _Crosshair:UpdateCrosshairRange()
	    for _, Skill in pairs(Skills.SkillsList) do
            if Skill:GetRange() > self.Skills_Crosshair.range then
                self.Skills_Crosshair.range = Skill:GetRange()
            end
	    end
	end

	function _Crosshair:SaveTargetSelector()
	    local save = GetSave("SidasAutoCarry")
	    save.TargetSelectorMode = Crosshair.Attack_Crosshair.mode
	    save:Save()
	end

	function _Crosshair:LoadTargetSelector()
	    local save = GetSave("SidasAutoCarry")
	    if save.TargetSelectorMode then
	            Crosshair.Attack_Crosshair.mode = save.TargetSelectorMode
	            Crosshair.Skills_Crosshair.mode = save.TargetSelectorMode
	    end
	end

	function _Crosshair:Conditional(Hero)
	    return Hero.team ~= myHero.team and Orbwalker:CanOrbwalkTarget(Hero) and not Data:EnemyIsImmune(Hero)
	end

	function _Crosshair:SetTargetLock(Target)
	    self.TargetLock = Target
	end
class '_Jungle' 
	function _Jungle:__init()
	    self.JungleMonsters = {}
	    Jungle = self
	    for i = 0, objManager.maxObjects do
	    local object = objManager:getObject(i)
            if Data:IsJungleMinion(object) then
                table.insert(self.JungleMonsters, object)
            end
	    end

	    AddCreateObjCallback(function(Object) self:_OnCreateObj(Object) end)
	    AddDeleteObjCallback(function(Object) self:_OnDeleteObj(Object) end)
	end

	function _Jungle:_OnCreateObj(Object)
	    if Data:IsJungleMinion(Object) then
	            table.insert(self.JungleMonsters, Object)
	    end
	end

	function _Jungle:_OnDeleteObj(Object)
	    if Data:IsJungleMinion(Object) then
	            for i, Obj in pairs(self.JungleMonsters) do
	                    if obj == Object then
	                            table.remove(self.JungleMonsters, i)
	                    end
	            end
	    end
	end

	function _Jungle:GetJungleMonsters()
	    return self.JungleMonsters
	end

	function _Jungle:GetAttackableMonster()
	    local HighestPriorityMonster =  nil
	    local Priority = 0
	    for _, Monster in pairs(self.JungleMonsters) do
	            if Orbwalker:CanOrbwalkTarget(Monster) then
	                    local CurrentPriority = Data:GetJunglePriority(Monster.name)
	                    if Monster.health < MyHero:GetTotalAttackDamageAgainstTarget(Monster) then
	                            return Monster
	                    elseif not HighestPriorityMonster then
	                            HighestPriorityMonster = Monster
	                            Priority = CurrentPriority
	                    else
	                            if CurrentPriority < Priority then
	                                    HighestPriorityMonster = Monster
	                                    Priority = CurrentPriority
	                            end
	                    end
	            end
	    end
	    return HighestPriorityMonster
	end

	function _Jungle:GetFocusedMonster()
	    if GetTarget() and Data:IsJungleMinion(GetTarget()) then
	            return GetTarget()
	    end
	end
class '_Structures' 
	function _Structures:__init()
	    Structures = self
	    self.TowerCollisionRange = 88.4
	    self.InhibCollisionRange = 205
	    self.NexusCollisionRange = 300
	    self.TowerRange = 950
	    self.EnemyTowers = {}
	    self.AllyTowers = {}

	    for i = 1, objManager.maxObjects do
	            local Object = objManager:getObject(i)
	            if Object and Object.type == "obj_AI_Turret" then
	                    if Object.team == myHero.team then
	                            table.insert(self.AllyTowers, Object)
	                    else
	                            table.insert(self.EnemyTowers, Object)
	                    end
	            end
	    end

	    AddDeleteObjCallback(function(obj) self:_OnDeleteObj(obj) end)
	end

	function _Structures:_OnDeleteObj(Object)
	    for i, Tower in pairs(self.AllyTowers) do
	            if Object == Tower then
	                    table.remove(self.AllyTowers, i)
	                    return
	            end
	    end
	    for i, Tower in pairs(self.EnemyTowers) do
	            if Object == Tower then
	                    table.remove(self.EnemyTowers, i)
	                    return
	            end
	    end
	end

	function _Structures:TowerTargetted()
	    return GetTarget() and GetTarget().type == "obj_AI_Turret" and GetTarget().team ~= myHero.team
	end

	function _Structures:InhibTargetted()
	    return GetTarget() and GetTarget().type == "obj_BarracksDampener" and GetTarget().team ~= myHero.team
	end

	function _Structures:NexusTargetted()
	    return GetTarget() and GetTarget().type == "obj_HQ" and GetTarget().team ~= myHero.team
	end

	function _Structures:CanOrbwalkStructure()
	    return self:CanOrbwalkTower() or self:CanOrbwalkInhib() or self:CanOrbwalkNexus()
	end

	function _Structures:GetTargetStructure()
	    return GetTarget()
	end

	function _Structures:CanOrbwalkTower()
	    return self:TowerTargetted() and Helper:GetDistance(GetTarget()) - self.TowerCollisionRange < MyHero.TrueRange
	end

	function _Structures:CanOrbwalkInhib()
	    return self:InhibTargetted() and Helper:GetDistance(GetTarget()) - self.InhibCollisionRange < MyHero.TrueRange
	end

	function _Structures:CanOrbwalkNexus()
	    return self:NexusTargetted() and Helper:GetDistance(GetTarget()) - self.NexusCollisionRange < MyHero.TrueRange
	end

	function _Structures:PositionInEnemyTowerRange(Pos)
	    for _, Tower in pairs(self.EnemyTowers) do
	            if Helper:GetDistance(Tower, Pos) <= self.TowerRange then
	                    return true
	            end
	    end
	    return false
	end

	function _Structures:PositionInAllyTowerRange(Pos)
	    for _, Tower in pairs(self.AllyTowers) do
	            if Helper:GetDistance(Tower, Pos) <= self.TowerRange then
	                    return true
	            end
	    end
	    return false
	end

	function _Structures:GetClosestEnemyTower(Pos)
	    local ClosestTower, Distance = nil, 0
	    for i, Tower in pairs(self.EnemyTowers) do
	            if not Tower or not Pos then return end
	            if not ClosestTower then
	                    ClosestTower, Distance = Tower, Helper:GetDistance(Pos, Tower)
	            elseif Helper:GetDistance(Pos, Tower) < Distance then
	                    ClosestTower, Distance = Tower, Helper:GetDistance(Pos, Tower)
	            end
	    end
	    return ClosestTower
	end

	function _Structures:GetClosestAllyTower(Pos)
	    local ClosestTower, Distance = nil, 0
	    for _, Tower in pairs(self.AllyTowers) do
	            if not ClosestTower then
	                    ClosestTower, Distance = Tower, Helper:GetDistance(Pos, Tower)
	            elseif Helper:GetDistance(Pos, Tower) < Distance then
	                    ClosestTower, Distance = Tower, Helper:GetDistance(Pos, Tower)
	            end
	    end
	    return ClosestTower
	end
class '_Minions'
	function _Minions:__init()
        self.KillableMinion = nil
        self.AlmostKillable = nil
        self.AttackRangeBuffer = myHero.range + 50
        self.LastWait = 0
        self.LastMove = 0
        self.TowerHitTime = 0
        self.LowerLimit = -20
		self.laneTable = {}
        self.Cannons = {}
        self.EnemyMinions = minionManager(MINION_ENEMY, 2000, myHero, MINION_SORT_HEALTH_ASC)
        self.OtherMinions = minionManager(MINION_OTHER, 2000, myHero, MINION_SORT_HEALTH_ASC)
        self.allies = minionManager(MINION_ALLY, 1500, myHero, MINION_SORT_HEALTH_ASC)

        AddTickCallback(function() self:_OnTick() end)
        AddProcessAttackCallback(function(u,s) self:OnProcessSpell(u,s) end)

        Minions = self

        AddCreateObjCallback(function (obj)
			if(string.find(obj.name,"Minion") and not string.find(obj.name,".troy")) then
				if obj.team == myHero.team then
					self:newMinion(obj)
				end
			end
		end)

		AddDrawCallback(function ()
			if debugMode then
				self:drawManager()
			end
		end)


		for i = 1, objManager.maxObjects, 1 do
	        local obj = objManager:getObject(i)
	        if (obj and obj.name and string.find(obj.name,"Minion") and not string.find(obj.name,".troy")) then
	            if obj.team == myHero.team then
					self:newMinion(obj)
				end
	        end
	    end
    end

    function _Minions:newMinion(minion)
		local tier,lane,wave,id = self:minionData(minion)
		if(not self.laneTable[lane]) then
			self.laneTable[lane] = {}
		end
		if not self.laneTable[lane].waves then
			self.laneTable[lane].waves = {}
		end

		if (not self.laneTable[lane].minions) then
			self.laneTable[lane].minions = {}
		end
		if not self.laneTable[lane].waves[wave] then
			self.laneTable[lane].waves[wave] = {
				pos = {
					x = 0,
					y = 0,
					z = 0,
					count = 0
				},
				minions = {}
			}
		end
		table.insert(self.laneTable[lane].waves[wave].minions,minion)
		self.laneTable[lane].waves[wave].tier = tier
		self.laneTable[lane].waves[wave].number = wave
		self:minionLaneUpdate()
	end

	function _Minions:minionData(obj)
		--!!!Tier
		--??:WAVE
		--*:Lane
		--&&:ID
		--Minion_T!!!L*S??N&&
		--Example:Minion_T100L1S02N0004
		return string.match(obj.name,"Minion_T(.*)L(.*)S(.*)N(.*)")
	end

	function _Minions:minionLaneUpdate() --burdayiz
		for k, lane in pairs(self.laneTable) do
			for i, wave in pairs(lane.waves) do
				wave.pos = {
					x = 0,
					y = 0,
					z = 0,
					count = 0
				}
				for c, minion in pairs(wave.minions) do
					if(not minion or minion.dead) then
						table.remove(wave.minions, c)
					else
						wave.pos.x = wave.pos.x + minion.x
						wave.pos.y = wave.pos.y + minion.y
						wave.pos.z = wave.pos.z + minion.z
						wave.pos.count = wave.pos.count + 1
					end
				end

				if(wave.pos.count == 0) then
					table.remove(lane.waves, i)
				end

				wave.pos.x = wave.pos.x / wave.pos.count
				wave.pos.y = wave.pos.y / wave.pos.count
				wave.pos.z = wave.pos.z / wave.pos.count
			end
		end
	end

	function _Minions:drawManager()
		for k, lane in pairs(self.laneTable) do
			for i, wave in pairs(lane.waves) do
				if wave.pos.count > 0 then
					DrawCircle(wave.pos.x, wave.pos.y, wave.pos.z, 150, ARGB(255, 0, 0, 150))
				end
			end
		end
	end

	function _Minions:getWave()
		for k, lane in pairs(self.laneTable) do
			for i, wave in pairs(lane.waves) do
				return i
			end
		end
		return "?"
	end

	function _Minions:getCenterMyWave()
		local lane = tostring(SELECTED_LANE)
		if self.laneTable[lane] then
			local farWave = nil
			for i, wave in pairs(self.laneTable[lane].waves) do
				if wave.pos.count > 0 then
					if not farWave then
						farWave = wave
					else
						if GetDistance2D(_G.hflTasks[MAPNAME][TEAMNUMBER][1].point,farWave.pos) < GetDistance2D(_G.hflTasks[MAPNAME][TEAMNUMBER][1].point,wave.pos) then
							farWave = wave
						end
					end
				end
			end
			return farWave.pos
		end
		return nil
	end

    function _Minions:_OnTick()
		self.allies:update()
		self.EnemyMinions:update()
		self:minionLaneUpdate()
        self.AttackRangeBuffer = myHero.range + 50
    end

    function _Minions:MyDamage(Minion)
            return VP:CalcDamageOfAttack(myHero, Minion, {name = "Basic"}, 0) + self:BonusDamage(Minion)
    end

    function _Minions:OnProcessSpell(Unit, Spell)
            if Unit and Unit.valid and Spell.target and Unit.type ~= myHero.type and Spell.target.type == 'obj_AI_Minion' and Unit.team == myHero.team and Spell and Unit.type == "obj_AI_Turret" and GetDistance(Spell.target) <= 2000 then
                    self.TowerTarget = Spell.target
                    local time = VP:GetTime() + Spell.windUpTime + GetDistance(Spell.target, Unit) / VP:GetProjectileSpeed(Unit) - GetLatency()/2000 + 1000
                    --DelayAction(function() self.TowerTarget = nil end, time/1000)
            end
    end

    function _Minions:GetLaneClearTarget()
            for i, minion in ipairs(self.EnemyMinions.objects) do

                    -- local pdamage = minion.health - DamagePred:GetPred(minion, PRED_TWO_HITS)
                    -- local health = DamagePred:GetPred(minion, PRED_TWO_HITS)
                    -- local mydmg = self:MyDamage(minion)

                    -- -- if Orbwalker:CanOrbwalkTarget(minion) and pdamage > 2* mydmg or pdamage2 == 0 then
                    -- --   return minion
                    -- -- end

                    -- if Orbwalker:CanOrbwalkTarget(minion) and health > pdamage * 2 + mydmg then
                    --      return minion
                    -- end

                    local time = Orbwalker:GetAnimationTime() + GetDistance(minion.visionPos, myHero.visionPos) / MyHero.ProjectileSpeed - 0.07
                    local pdamage2 = minion.health - VP:GetPredictedHealth(minion, time)
                    local pdamage = VP:GetPredictedHealth2(minion, time * 2)
                    if Orbwalker:CanOrbwalkTarget(minion) and (pdamage > 2 * VP:CalcDamageOfAttack(myHero, minion, {name = "Basic"}, 0) + self:BonusDamage(minion) or pdamage2 == 0)  then
                            return minion
                    end
            end

            self.OtherMinions:update()
            for i, minion in ipairs(self.OtherMinions.objects) do
                    if Orbwalker:CanOrbwalkTarget(minion) then
                            return minion
                    end
            end


            return Jungle:GetAttackableMonster()
    end

    function _Minions:ContainsTowerAttack(target)
            for _, attack in pairs(VP.ActiveAttacks) do
                    if attack.Target == target and attack.Attacker.type == "obj_AI_Turret" then
                            self.TowerHitTime = attack.hittime
                            return true
                    end
            end
            return false
    end

    local TOWER_TYPE_AA = 0
    local TOWER_TYPE_SKILL = 1

    function _Minions:GetTowerMinion()
            if Orbwalker:CanOrbwalkTarget(self.TowerTarget) and self:ContainsTowerAttack(self.TowerTarget) then            
                    local myDamage = VP:CalcDamageOfAttack(myHero, self.TowerTarget, {name = "Basic"}, 0)

                    local time = (Orbwalker:GetWindUp() * 2) + Orbwalker:GetAnimationTime() + GetDistance(self.TowerTarget.visionPos, myHero.visionPos) / MyHero.ProjectileSpeed - 0.07
                    local remainingHealth = VP:GetPredictedHealth2(self.TowerTarget, time)

                    -- 1 tower 1 me
                    if remainingHealth > 0 and remainingHealth < myDamage then
                            return nil
                    end
                    time = (Orbwalker:GetWindUp() * 2) + Orbwalker:GetAnimationTime() + GetDistance(self.TowerTarget.visionPos, myHero.visionPos) / MyHero.ProjectileSpeed - 0.07
                    remainingHealth = VP:GetPredictedHealth2(self.TowerTarget, time)

                    -- 1 tower 2 me
                    remainingHealth = remainingHealth - myDamage
                    if remainingHealth > 0 and remainingHealth < myDamage then
                            return self.TowerTarget, TOWER_TYPE_AA
                    end


                    -- time = self.TowerHitTime - Helper:GetTime() --Orbwalker:GetNextAttackTime() - Helper:GetTime() + Orbwalker:GetWindUp() + GetDistance(self.TowerTarget.visionPos, myHero.visionPos) / MyHero.ProjectileSpeed - 0.07
                    -- remainingHealth = VP:GetPredictedHealth(self.TowerTarget, time)

                    -- if remainingHealth < 0 then
                    --      return self.TowerTarget, TOWER_TYPE_SKILL
                    -- end

                    -- -- need skill
                    -- if remainingHealth < 0 then
                    --      return self.TowerTarget, TOWER_TYPE_SKILL
                    -- end

                    -- remainingHealth = VP:GetPredictedHealth2(self.TowerTarget, time)

                    -- -- need skill
                    -- remainingHealth = remainingHealth - myDamage
                    -- if remainingHealth < 0 then
                    --      return self.TowerTarget, TOWER_TYPE_SKILL
                    -- end

            end
    end

    function _Minions:WaitForCannon()
            local cans = {}
            for _, min in pairs(self.EnemyMinions.objects) do
                    if Helper:GetDistance(min) <= 2000 and Data:IsCannonMinion(min) then
                            table.insert(cans, min)
                    end
            end

            for i, Cannon in pairs(cans) do
                    if Orbwalker:CanOrbwalkTargetCustomRange(Cannon, self.AttackRangeBuffer) then
                            if DamagePred:GetPred(Cannon, PRED_TWO_HITS) < self:MyDamage(Cannon) then
                                    return Cannon
                            end
                    end
            end
    end

    function _Minions:FindUnkillable()
            local cannon = self:WaitForCannon()

            if cannon then
                    if DamagePred:GetPred(cannon, PRED_UNKILLABLE) < self:MyDamage(cannon) then
                            if minion ~= self.LastHitMinion then
                                    local minionhealth = DamagePred:GetPred(cannon, PRED_UNKILLABLE)
                                    if minionhealth < 0 then
                                            return cannon
                                    end
                            end
                    end
            end

            for i, minion in ipairs(self.EnemyMinions.objects) do
                    if minion ~= self.LastHitMinion then
                            local minionhealth = DamagePred:GetPred(minion, PRED_UNKILLABLE)
                            if minionhealth < 0 then
                                    return minion
                            end
                    end
            end
    end

    function _Minions:FindKillable()
            local cannon = self:WaitForCannon()

            if cannon then
                    if Orbwalker:CanOrbwalkTarget(cannon, self.AttackRangeBuffer) and DamagePred:GetPred(cannon, PRED_LAST_HIT) < self:MyDamage(cannon) then
                            local mydmg = self:MyDamage(cannon)
                            local minionhealth = DamagePred:GetPred(cannon, PRED_LAST_HIT)
                            if minionhealth < mydmg and minionhealth > self.LowerLimit then
                                    return cannon
                            end
                    end
                    return
            end

            for i, minion in ipairs(self.EnemyMinions.objects) do
                    local minionhealth = DamagePred:GetPred(minion, PRED_LAST_HIT)
                    local mydmg = self:MyDamage(minion)

                    if Orbwalker:CanOrbwalkTarget(minion) and minionhealth < mydmg and minionhealth > self.LowerLimit then
                            self.LastHitMinion = minion
                            return minion
                    end
            end
    end

    function _Minions:ShouldWait()
            for i, minion in ipairs(self.EnemyMinions.objects) do
                    local mydmg = self:MyDamage(minion)
                    local minionhealth = DamagePred:GetPred(minion, PRED_TWO_HITS)
                    if Orbwalker:CanOrbwalkTarget(minion, self.AttackRangeBuffer) and minionhealth < mydmg then
                            self.LastWait = Helper:GetTime()
                            return minion
                    end
            end
    end

    function _Minions:TowerFarm()
            self.EnemyMinions:update()
            --DamagePred:Reset()

            local target = self:FindKillable()

            if target then
                    self.KillableMinion = target
            else
                    self.KillableMinion = nil
            end

            if target then
                    Orbwalker:Orbwalk(target)
                    return
            end

            if myHero.mana / myHero.maxMana * 100 >= 30 then
                    target = self:FindUnkillable()
                    if target then
                            target, skill = self:GetKillableSkillMinion(false, target)
                            if target then
                                    self:CastOnMinion(target, skill)
                            end
                    end
            end

            target, _type = self:GetTowerMinion()
            if target and _type == TOWER_TYPE_AA then
                    Orbwalker:Orbwalk(target)
                    return
            end

            if target and _type == TOWER_TYPE_SKILL then
                    self:LastHitWithSkill(target)
                    return
            end
    end

    function _Minions:LaneClear()
            if ValidTarget(self.TowerTarget) then
                    self:TowerFarm()
                    return
            end


            self.EnemyMinions:update()
            --DamagePred:Reset()

            if myHero.mana / myHero.maxMana * 100 >= 30 then
                    local target, skill = self:GetKillableSkillMinion(true)
                    if target then
                            self:CastOnMinion(target, skill)
                    end
            end

            local target = self:FindKillable()

            if not target and Structures:CanOrbwalkStructure() then
                Orbwalker:OrbwalkIgnoreChecks(Structures:GetTargetStructure())
                return
            end

            if Orbwalker:CanOrbwalkTarget(Crosshair.Attack_Crosshair.target) and not target then
                Orbwalker:Orbwalk(Crosshair.Attack_Crosshair.target)
                return
            end


            if target then
                self.KillableMinion = target
            else
                self.KillableMinion = nil
            end

            if target and Orbwalker:CanShoot() then
                Orbwalker:Orbwalk(target)
                return
            end

            local waitMinion = self:ShouldWait()
            if not waitMinion and Helper:GetTime() > self.LastWait + 500 then
                Orbwalker:Orbwalk(self:GetLaneClearTarget())
            elseif Helper:GetTime() > self.LastMove then
                self.LastMove = Helper:GetTime() + 100
                MyHero:Move()
            end

            if waitMinion and not self.KillableMinion then
                self.AlmostKillable = waitMinion
            elseif Helper:GetTime() > self.LastWait + 500 or not ValidTarget(waitMinion) then
                self.AlmostKillable = nil
            end
    end

    function _Minions:LastHit()
            if ValidTarget(self.TowerTarget) then
                    self:TowerFarm()
                    return
            end

            self.EnemyMinions:update()
            --DamagePred:Reset()

            target = self:FindKillable()


            if target then
                    self.KillableMinion = target
            else
                    self.KillableMinion = nil
            end

            if target then
                    Orbwalker:Orbwalk(target)
                    return
            end

            -- if myHero.mana / myHero.maxMana * 100 >= LastHitSkillsMenu.MinMana then
            --      target = self:FindKillable()
            --      if target then
            --              Orbwalker:Orbwalk(target)
            --              return
            --      end
            -- end

            MyHero:Move()
    end

    function _Minions:MarkerOnly()
            self.EnemyMinions:update()

            local target = self:FindKillable()

            if target then
                    self.KillableMinion = target
            else
                    self.KillableMinion = nil
            end

            if target then
                    return
            end

            local waitMinion = self:ShouldWait()

            if waitMinion and not self.KillableMinion then
                    self.AlmostKillable = waitMinion
            elseif Helper:GetTime() > self.LastWait + 500 or not ValidTarget(waitMinion) then
                    self.AlmostKillable = nil
            end
    end

    function _Minions:CastOnMinion(Minion, Skill)
            local dmgString = "";
            if Skill.Key == _Q then
                    dmgString = "Q"
            elseif Skill.Key == _W then
                    dmgString = "W"
            elseif Skill.Key == _E then
                    dmgString = "E"
            elseif Skill.Key == _R then
                    dmgString = "R"
            end

            if ValidTarget(Minion, Skill.Range) then
                    if Skill.Type == SPELL_TARGETED then
                            CastSpell(Skill.Key, Minion)
                    elseif Skill.Type == SPELL_SELF_AT_MOUSE then
                            CastSpell(Skill.Key, Minion.x, Minion.z)
                    elseif Skill.Type == SPELL_LINEAR then
                            Skill:Cast(Minion)
                    elseif Skill.Type == SPELL_LINEAR_COL then
                            local pred = Skill:GetLinePrediction(Minion)
                            if pred then
                                    CastSpell(Skill.Key, pred.x, pred.z)
                            end
                    else
                            CastSpell(Skill.Key)
                    end
            end
    end

    function _Minions:GetKillableSkillMinion(isLaneClear, fixedMinion)
        for i, Miniona in ipairs(self.EnemyMinions.objects) do
            Minion = fixedMinion or Miniona
            if Minion ~= self.LastHitMinion then
                for _, Skill in pairs(Skills:GetLastHitSkills()) do
                    local dmgString = "";
                    if Skill.Key == _Q then
                            dmgString = "Q"
                    elseif Skill.Key == _W then
                            dmgString = "W"
                    elseif Skill.Key == _E then
                            dmgString = "E"
                    elseif Skill.Key == _R then
                            dmgString = "R"
                    end

                    if myHero:CanUseSpell(Skill.Key) == READY and ValidTarget(Minion, Skill.Range) and StateManager:getActiveState().id ~= PUSH_STATE then
                        local _Damage = getDmg(dmgString, Minion, myHero)
                        if Skill.Type == SPELL_LINEAR or Skill.Type == SPELL_LINEAR_COL then
                            local minionhealth = DamagePred:GetPred(Minion, PRED_SKILL, Skill)
                            if _Damage > minionhealth and minionhealth > self.LowerLimit then
                                local pred = Skill:GetLinePrediction(Minion)
                                if pred then
                                        return Minion, Skill
                                end
                            end
                        elseif _Damage > Minion.health then
                            return Minion, Skill
                        end
                    end
                end
            end
            if fixedMinion then
                    return
            end
        end
    end

    function _Minions:PushWithSkills()
            for i, minion in ipairs(self.EnemyMinions.objects) do
                    if minion ~= self.LastHitMinion then
                            self:LastHitWithSkill(minion, true)
                    end
            end
    end

    function _Minions:BonusDamage(minion)
            local AD = myHero:CalcDamage(minion, myHero.totalDamage)
            local BONUS = 0
            if myHero.charName == 'Vayne' then
                    if myHero:GetSpellData(_Q).level > 0 and myHero:CanUseSpell(_Q) == SUPRESSED then
                            BONUS = BONUS + myHero:CalcDamage(minion, ((0.05 * myHero:GetSpellData(_Q).level) + 0.25 ) * myHero.totalDamage)
                    end
                    if not VayneCBAdded then
                            VayneCBAdded = true
                            function VayneParticle(obj)
                                    if GetDistance(obj) < 1000 and obj.name:lower():find("vayne_w_ring2.troy") then
                                            VayneWParticle = obj
                                    end
                            end
                            AddCreateObjCallback(VayneParticle)
                    end
                    if VayneWParticle and VayneWParticle.valid and GetDistance(VayneWParticle, minion) < 10 then
                            BONUS = BONUS + 10 + 10 * myHero:GetSpellData(_W).level + (0.03 + (0.01 * myHero:GetSpellData(_W).level)) * minion.maxHealth
                    end
            elseif myHero.charName == 'Teemo' and myHero:GetSpellData(_E).level > 0 then
                    BONUS = BONUS + myHero:CalcMagicDamage(minion, (myHero:GetSpellData(_E).level * 10) + (myHero.ap * 0.3) )
            elseif myHero.charName == 'Corki' then
                    BONUS = BONUS + myHero.totalDamage/10
            elseif myHero.charName == 'MissFortune' and myHero:GetSpellData(_W).level > 0 then
                    BONUS = BONUS + myHero:CalcMagicDamage(minion, (4 + 2 * myHero:GetSpellData(_W).level) + (myHero.ap/20))
            elseif myHero.charName == 'Varus' and myHero:GetSpellData(_W).level > 0 then
                    BONUS = BONUS + (6 + (myHero:GetSpellData(_W).level * 4) + (myHero.ap * 0.25))
            elseif myHero.charName == 'Caitlyn' then
                            if not CallbackCaitlynAdded then
                                    function CaitlynParticle(obj)
                                            if GetDistance(obj) < 100 and obj.name:lower():find("caitlyn_headshot_rdy") then
                                                            HeadShotParticle = obj
                                            end
                                    end
                                    AddCreateObjCallback(CaitlynParticle)
                                    CallbackCaitlynAdded = true
                            end
                            if HeadShotParticle and HeadShotParticle.valid then
                                    BONUS = BONUS + AD * 1.5
                            end
            elseif myHero.charName == 'Orianna' then
                    BONUS = BONUS + myHero:CalcMagicDamage(minion, 10 + 8 * ((myHero.level - 1) % 3))
            elseif myHero.charName == 'TwistedFate' then
                            if not TFCallbackAdded then
                                    function TFParticle(obj)
                                                    if GetDistance(obj) < 100 and obj.name:lower():find("cardmaster_stackready.troy") then
                                                                    TFEParticle = obj
                                                    elseif GetDistance(obj) < 100 and obj.name:lower():find("card_blue.troy") then
                                                                    TFWParticle = obj
                                                    end
                                    end
                                    AddCreateObjCallback(TFParticle)
                                    TFCallbackAdded = true
                            end
                            if TFEParticle and TFEParticle.valid then
                                    BONUS = BONUS + myHero:CalcMagicDamage(minion, myHero:GetSpellData(_E).level * 15 + 40 + 0.5 * myHero.ap)  
                            end
                            if TFWParticle and TFWParticle.valid then
                                    BONUS = BONUS + math.max(myHero:CalcMagicDamage(minion, myHero:GetSpellData(_W).level * 20 + 20 + 0.5 * myHero.ap) - 40, 0)
                            end
            elseif myHero.charName == 'Nasus' and VIP_USER then
                    if myHero:GetSpellData(_Q).level > 0 and myHero:CanUseSpell(_Q) == SUPRESSED then
                            local Qdamage = {30, 50, 70, 90, 110}
                            NasusQStacks = NasusQStacks or 0
                            BONUS = BONUS + myHero:CalcDamage(minion, 10 + 20 * (myHero:GetSpellData(_Q).level) + NasusQStacks)
                            if not RecvPacketNasusAdded then
                                    function NasusOnRecvPacket(p)
                                            if p.header == 0xFE and p.size == 0xC then
                                                    p.pos = 1
                                                    pNetworkID = p:DecodeF()
                                                    unk01 = p:Decode2()
                                                    unk02 = p:Decode1()
                                                    stack = p:Decode4()
                                                    if pNetworkID == myHero.networkID then
                                                            NasusQStacks = stack
                                                    end
                                            end
                                    end
                                    RecvPacketNasusAdded = true
                                    AddRecvPacketCallback(NasusOnRecvPacket)
                            end
                    end
            end

            return BONUS
    end

    function _Minions:GetLowestHealthMinion()
            for i =1, #self.EnemyMinions.objects, 1 do
                    local Minion = self.EnemyMinions.objects[i]
                    if Orbwalker:CanOrbwalkTarget(Minion) then
                            return Minion
                    end
            end
    end

    function _Minions:GetSecondLowestHealthMinion()
            local found = nil
            for i =1, #self.EnemyMinions.objects, 1 do
                    local Minion = self.EnemyMinions.objects[i]
                    if Orbwalker:CanOrbwalkTarget(Minion) and found then
                            return Minion
                    elseif Orbwalker:CanOrbwalkTarget(Minion) then
                            found = Minion
                    end
            end
            return found
    end
class 'towers'
	function towers:__init( )
		self.towers = {}
		self.baracks = {}
		self.hqs = {}

	    for i = 1, objManager.maxObjects do
	        local tower = objManager:getObject(i)
			if tower ~= nil and tower.valid and tower.type == "obj_AI_Turret" and tower.visible  then
				table.insert(self.towers, tower)
			end
		end

		for i = 1, objManager.maxObjects do
	        local tower = objManager:getObject(i)
			if tower ~= nil and tower.valid and tower.type == "obj_BarracksDampener" and tower.visible  then
				table.insert(self.baracks, tower)
			end
		end

		for i = 1, objManager.maxObjects do
	        local tower = objManager:getObject(i)
			if tower ~= nil and tower.valid and tower.type == "obj_HQ" and tower.visible  then
				table.insert(self.hqs, tower)
			end
		end
	end

	function towers:GetCloseBaracks(hero, team)
		local foundTowers = {}
		local t = 0

		for a,tower in pairs(self.baracks) do
			if team then
				if tower.valid and not tower.dead and tower.team == myHero.team then
					table.insert(foundTowers,tower)
				end
			else
				if tower.valid and not tower.dead and tower.team ~= myHero.team then
					table.insert(foundTowers,tower)
				end
			end
		end 
			
		if(#foundTowers > 0) then
			local candidate = foundTowers[1]
			for i = 2, #foundTowers, 1 do
				if (foundTowers[i].health / foundTowers[i].maxHealth > 0.1) and  GetDistance2D(hero,candidate) > GetDistance2D(hero,foundTowers[i]) then 
					candidate = foundTowers[i] 
				end
			end
			return candidate
		else
			return false
		end
	end

	function towers:GetCloseHqs(hero, team)
		local foundTowers = {}
		local t = 0

		for a,tower in pairs(self.hqs) do
			if team then
				if tower.valid and not tower.dead and tower.team == myHero.team then
					table.insert(foundTowers,tower)
				end
			else
				if tower.valid and not tower.dead and tower.team ~= myHero.team then
					table.insert(foundTowers,tower)
				end
			end
		end 
			
		if(#foundTowers > 0) then
			local candidate = foundTowers[1]
			for i = 2, #foundTowers, 1 do
				if (foundTowers[i].health / foundTowers[i].maxHealth > 0.1) and  GetDistance2D(hero,candidate) > GetDistance2D(hero,foundTowers[i]) then 
					candidate = foundTowers[i] 
				end
			end
			return candidate
		else
			return false
		end
	end

	function towers:GetCloseTower(hero, team)
		local foundTowers = {}
		local t = 0

		for a,tower in pairs(self.towers) do
			if team then
				if tower.valid and not tower.dead and tower.team == myHero.team then
					table.insert(foundTowers,tower)
				end
			else
				if tower.valid and not tower.dead and tower.team ~= myHero.team then
					table.insert(foundTowers,tower)
				end
			end
		end 
			
		if(#foundTowers > 0) then
			local candidate = foundTowers[1]
			for i = 2, #foundTowers, 1 do
				if (foundTowers[i].health / foundTowers[i].maxHealth > 0.1) and  GetDistance2D(hero,candidate) > GetDistance2D(hero,foundTowers[i]) then 
					candidate = foundTowers[i] 
				end
			end
			return candidate
		else
			return false
		end
	end
class 'LocalAwareness'
	function LocalAwareness:__init()
		self.heroTable = {}
		self.posDanger = 0
		self.lastUpdate = GetTickCount()
		for i=1,heroManager.iCount do 
			table.insert(self.heroTable,heroManager:GetHero(i))
		end

		AddTickCallback(function ()
			self.posDanger = self:LocalDomination(myHero.pos)
		end)
	end

	function LocalAwareness:LocalDomination(pos)
		if (self.lastUpdate + 20 > GetTickCount()) then return end
		local danger = 0
		for i,hero in pairs(self.heroTable) do
			if(hero.visible and not hero.dead and GetDistance2D(pos,hero) < 900) then
				danger = danger + (-0.0042857142857143 * (GetDistance2D(hero,pos) + 100) + 4.4285714285714) * self:HeroStrength(hero) * (hero.team ~= myHero.team and 1 or -1);
			end
		end
		for i,minion in pairs(MINIONS.allies.objects) do
			if(minion.health > 0 and GetDistance2D(minion,pos) < 550 + getHitBoxRadius(myHero)) then
				if(string.find(minion.charName,"H28-G")) then
					danger = danger - 10000
				elseif (string.find(minion.charName,"Ranged")) then
					danger = danger - 800
				elseif (GetDistance2D(minion,myHero) < 130 + getHitBoxRadius(myHero)) then
					danger = danger - 800
				end
			end
		end

		for i,minion in pairs(MINIONS.EnemyMinions.objects) do
			if(minion.health > 0 and GetDistance2D(minion,pos) < 550 + getHitBoxRadius(myHero)) then
				if(string.find(minion.charName,"H28-G")) then
					danger = danger + 10000
				elseif (string.find(minion.charName,"Ranged")) then
					danger = danger + 1600
				elseif (GetDistance2D(minion,myHero) < 130 + getHitBoxRadius(myHero)) then
					danger = danger + 1600
				end
			end
		end

		local allyTower = TOWERS:GetCloseTower(myHero,true)
		if (allyTower) then
			if(GetDistance2D(allyTower,myHero) < 400) then
				danger = danger - 35000;
			end
		end

		local enemyTower = TOWERS:GetCloseTower(myHero,false)
		if (enemyTower) then
			if(GetDistance2D(enemyTower,myHero) < 1000 + getHitBoxRadius(myHero)) then
				danger = danger + 35000;
			end
		end
		return danger
	end

	function LocalAwareness:HeroStrength(hero)
		return ((hero.health*100)/hero.maxHealth) * (100 + hero.level * 10 + hero:GetInt('CHAMPIONS_KILLED') * 5);
	end
class '_StateManager'
	function _StateManager:__init()
		self.State = LOADING_STATE
		self.debugging = false
		self.stateList = {}
		self.appendedState = false
		

		AddTickCallback(function ()
			if self.appendedState then
				self:runState()
			end
			if self.appendState and not self.appendedState then
				self.appendedState = true
				self:appendState()
			end
		end)
	end

	function _StateManager:addState(object)
		table.insert(self.stateList, object)
	end

	function _StateManager:getActiveState()
		if self.appendedState then
			for i, state in pairs(self.stateList) do
				if state.valid then
					return state
				end
			end
		else
			return {name = "Authenticating", id = LOADING_STATE}
		end
	end

	function _StateManager:runState()
		if not self.debugging then self:getActiveState():run() end
	end
class '_Dead'
	function _Dead:__init()
		self.id = DEAD_STATE
		self.name = "Dead"
		self.valid = false
	end

	function _Dead:check()
		self.valid = myHero.dead
	end
class '_HoldLane'
	function _HoldLane:__init()
		self.id = HOLDLANE_STATE
		self.name = "Safe Farm"
		self.valid = true
	end

	function _HoldLane:run()
		local centerPos = MINIONS:getCenterMyWave()
		WAYPOINT = centerPos
		MINIONS:LastHit()
	end
class '_PushLane'
	function _PushLane:__init()
		self.id = PUSH_STATE
		self.name = "Push Lane"
		self.valid = false

		AddTickCallback(function (  )
			self:check()
		end)
	end

	function _PushLane:check( )
		local seemsValid = true
		for i, enemy in pairs(Helper.EnemyTable) do
			if enemy and not enemy.dead and GetDistance2D(myHero,enemy) < 2000  then
				if LocalAwareness:HeroStrength(myHero) < LocalAwareness:HeroStrength(enemy) * 1.5 then
					if seemsValid then
						seemsValid = false
					end
				end
			end
		end
		self.valid = seemsValid
	end

	function _PushLane:run()
		local centerPos = MINIONS:getCenterMyWave()
		WAYPOINT = centerPos
		local allyTower = TOWERS:GetCloseTower(myHero,true)
		if (allyTower) then
			if(GetDistance2D(allyTower,WAYPOINT) < 700) then
				self:underMyTower(allyTower)
				return true
			end
		end

		local enemyTower = TOWERS:GetCloseTower(WAYPOINT,false)
		if (enemyTower) then
			if(GetDistance2D(enemyTower,myHero) < 700 + getHitBoxRadius(myHero)) then
				self:underEnemyTower(enemyTower)
				return true
			end
		end

		local enemyTower = TOWERS:GetCloseBaracks(WAYPOINT,false)
		if (enemyTower) then
			if(GetDistance2D(enemyTower,myHero) < 700 + getHitBoxRadius(myHero)) then
				self:underEnemyTower(enemyTower)
				return true
			end
		end

		local enemyTower = TOWERS:GetCloseHqs(WAYPOINT,false)
		if (enemyTower) then
			if(GetDistance2D(enemyTower,myHero) < 700 + getHitBoxRadius(myHero)) then
				self:underEnemyTower(enemyTower)
				return true
			end
		end
		self:between()
	end

	function _PushLane:between()
		MINIONS:LaneClear()
	end

	function _PushLane:underMyTower(tower)
		MINIONS:LaneClear()
	end

	function _PushLane:underEnemyTower(tower)
		if(GetTarget()) then
			MINIONS:LaneClear()
		else
			Orbwalker:Orbwalk(tower)
		end
	end
class '_Recall'
	function _Recall:__init()
		self.id = RECALL_STATE
		self.name = "Recall"
		self.valid = false
	end
class '_Survive'
	function _Survive:__init()
		self.id = SURVIVAL_STATE
		self.name = "Survive"
		self.valid = false

		AddTickCallback(function ()
			self:check()
		end)
	end

	function _Survive:check()
		local valida = false
		if not LOCALAWARENESS or LOCALAWARENESS.posDanger == nil then return true end
		if LOCALAWARENESS.posDanger > -2000 then
			valida = true
		end
		self.valid = valida

		for k, enemy in pairs(Helper.EnemyTable) do
			if GetDistance2D(enemy,myHero) < 2000 and not enemy.dead then
				local Enemyposition = VP:GetPredictedPos(enemy, 250)
				if GetDistance2D(Enemyposition,myHero) < enemy.range then
					valida = true
				end
			end
		end
	end

	function _Survive:run()
		local allyTower = TOWERS:GetCloseTower(myHero,true)
		if (allyTower) then
			MyHero:MoveTo(allyTower.pos.x,allyTower.pos.z)
		end
	end
class '_Combat'
	function _Combat:__init()
		self.id = COMBAT_STATE
		self.name = "Combat"
		self.target = nil
		self.valid = false

		AddTickCallback(function ()
			self:check()
		end)
	end

	function _Combat:check()
		local valida = false
		if not LOCALAWARENESS or LOCALAWARENESS.posDanger == nil then return true end
		if LOCALAWARENESS.posDanger < -15000 then
			if Crosshair.Attack_Crosshair.target and Orbwalker:CanOrbwalkTarget(Crosshair.Attack_Crosshair.target) then
				valida = true
				self.target = Crosshair.Attack_Crosshair.target
			end
			
			for k, enemy in pairs(Helper.EnemyTable) do
				if GetDistance2D(enemy,myHero) < 2000 and not enemy.dead then
					local nearTower = TOWERS:GetCloseTower(myHero,false)
					if not nearTower or GetDistance2D(nearTower,enemy) > 1100 then
						if LOCALAWARENESS:HeroStrength(myHero) / LOCALAWARENESS:HeroStrength(enemy) > 1.5 then
							valida = true
							self.target = enemy
						end
					end
				end
			end
		end
		self.valid = valida
		if not valida then
			self.target = nil
		end
	end

	function _Combat:run()
		if self.target then
			Orbwalker:OrbwalkToPosition(self.target,self.target.pos)
		end
	end
class '_Spawn'
	function _Spawn:__init()
		self.id = SPAWN_STATE
		self.name = "Spawn"
		self.valid = false

		AddTickCallback(function ()
			self:check()
		end)
	end

	function _Spawn:check()
		if GetDistance2D(_G.hflTasks[MAPNAME][TEAMNUMBER][1].point,myHero) < 450 then
			if AutoItem:itemPhaseDone() and self:lifeFull() then
				self.valid = false
			else
				self.valid = true
			end 
		else
			self.valid = false
		end
	end

	function _Spawn:lifeFull()
		return ((myHero.health*100)/myHero.maxHealth) > 80
	end

	function _Spawn:run()
		AutoItem:run()
	end
class '_LaneSelect'
	function _LaneSelect:__init()
		self.id = LANESELECT_STATE
		self.name = "Lane Select"
		self.valid = false

		AddTickCallback(function ()
			self:check()
		end)
	end

	function _LaneSelect:check()
		--Also check latest messages here
		if GetDistance2D(_G.hflTasks[MAPNAME][TEAMNUMBER][1].point,myHero) < 650 then
			self:selectLane()
		end
	end

	function _LaneSelect:selectLane()
		local bottom, mid, top = self:getLaneChamps()
		local lowestLane = nil
		if not lowestLane or top < lowestLane then
			SELECTED_LANE = TOP_LANE
		end
		if not lowestLane or mid < lowestLane then
			SELECTED_LANE = MID_LANE
		end
		if not lowestLane or bottom < lowestLane then
			SELECTED_LANE = BOT_LANE
		end
	end

	function _LaneSelect:getLaneChamps()
		local maxDistFront = 3000
		local maxDistBack = 2000
		local bottom,mid,top = 0,0,0
		for i, ally in pairs(GetAllyHeroes()) do
			for k, tower in pairs(TOWERS) do
				if tower.team == myHero.team then
					local match = false
					if string.find(tower.name, "L_03_A") then
						if GetDistance2D(ally,tower) < maxDistFront and not match then
							match = true
							top = top + 1
						end
					end
					if string.find(tower.name, "L_02_A") then
						if GetDistance2D(ally,tower) < maxDistBack and not match then
							match = true
							top = top + 1
						end
					end
					if string.find(tower.name, "C_05_A") then
						if GetDistance2D(ally,tower) < maxDistFront and not match then
							match = true
							mid = mid + 1
						end
					end
					if string.find(tower.name, "C_04_A") then
						if GetDistance2D(ally,tower) < maxDistBack and not match then
							match = true
							mid = mid + 1
						end
					end
					if string.find(tower.name, "R_03_A") then
						if GetDistance2D(ally,tower) < maxDistFront and not match then
							match = true
							bottom = bottom + 1
						end
					end
					if string.find(tower.name, "R_02_A") then
						if GetDistance2D(ally,tower) < maxDistBack and not match then
							match = true
							bottom = bottom + 1
						end
					end
				end
			end
		end
		return bottom,mid,top
	end
class '_Loading'
	function _Loading:__init()
		self.id = LOADING_STATE
		self.name = "Loading"
		self.valid = true
	end
class '_WaitWave'
	function _WaitWave:__init()
		self.valid = true
		self.name = "Waiting"
		self.id = WAIT_STATE
		self.waypoint = {
			x = 0,
			z = 0
		}

		AddTickCallback(function( )
			if (GetInGameTimer() < 100) then
				self.valid = true
			else
				self.valid = false
			end
		end)
	end

	function _WaitWave:run( )
		--print(GetInGameTimer())
		self:createWayPoint()
	end

	function _WaitWave:createWayPoint()
		if SELECTED_LANE == TOP_LANE then
			for k, tower in pairs(TOWERS.towers) do
				if string.find(tower.name, "L_03_A") then
					if myHero.team == 100 then
						MyHero:MoveTo(1028,10092)
					else
						MyHero:MoveTo(4660,13722)
					end
				end
			end
		end
		if SELECTED_LANE == MID_LANE then
			for k, tower in pairs(TOWERS.towers) do
				if string.find(tower.name, "C_05_A") then
					if myHero.team == 100 then
						MyHero:MoveTo(6022,5859)
					else
						MyHero:MoveTo(8886,8933)
					end
				end
			end
		end
		if SELECTED_LANE == BOT_LANE then
			for k, tower in pairs(TOWERS.towers) do
				if string.find(tower.name, "R_03_A") then
					if myHero.team == 100 then
						MyHero:MoveTo(10182,1195)
					else
						MyHero:MoveTo(13573,4745)
					end
				end
			end
		end
	end
class '_RandomPath'
	function _RandomPath:__init( )
		self.debugMode = false
		self.relativePoint = {x=0,z=0,y=0}
		self.movePoint = {x=0,z=0,y=0}
		self.pos1 = {x=0,z=0,y=0}
		self.pos2 = {x=0,z=0,y=0}
		self.absolutePath = {x=0,z=0}

		AddTickCallback(function (  )
			self:tick()
		end)

		AddDrawCallback(function (  )
			if debugMode then
				self:draw()
			end
		end)
	end

	function _RandomPath:tick()
		if self.debugMode then WAYPOINT = mousePos end
		if GetDistance2D(self.relativePoint, WAYPOINT) > 100 then
			self.relativePoint.x = WAYPOINT.x
			self.relativePoint.z = WAYPOINT.z
			self:random({x = self.relativePoint.x, z = self.relativePoint.z})
			self:setAbs(true)
		else
			self:setAbs(false)
		end
	end

	function _RandomPath:setAbs(relativeChange)
		local activeState = StateManager:getActiveState().id
		if activeState == PUSH_STATE or activeState == HOLDLANE_STATE then
			if not relativeChange then
				if GetDistance2D(self.pos1,myHero) < 50 then
					self.absolutePath = {x=self.pos2.x,z=self.pos2.z}
				end
				if GetDistance2D(self.pos2,myHero) < 50 then
					self.absolutePath = {x=self.pos1.x,z=self.pos1.z}
				end
			else
				if GetDistance2D(self.pos1,myHero) > GetDistance2D(self.pos2,myHero) then
					self.absolutePath = {x=self.pos2.x,z=self.pos2.z}
				else
					self.absolutePath = {x=self.pos1.x,z=self.pos1.z}
				end
			end
		else
			self.absolutePath = {x=self.movePoint.x,z=self.movePoint.z}
		end
	end

	function _RandomPath:draw()
		if self.debugMode then WAYPOINT = mousePos end
		DrawCircle(WAYPOINT.x, myHero.y, WAYPOINT.z, 50, ARGB(255, 0, 255, 0))
		DrawCircle(self.relativePoint.x, myHero.y, self.relativePoint.z, 40, ARGB(255, 180, 150,150))
		DrawCircle(self.movePoint.x, myHero.y, self.movePoint.z, 50, ARGB(255, 0, 0,255))

		DrawCircle(self.pos1.x, myHero.y, self.pos1.z, 50, ARGB(255, 150, 150,0))
		DrawCircle(self.pos2.x, myHero.y, self.pos2.z, 50, ARGB(255, 150, 150,0))
		DrawCircle(self.absolutePath.x, myHero.y, self.absolutePath.z, 40, ARGB(255, 255, 255,255))

		local mouseVec = WorldToScreen(D3DXVECTOR3(self.movePoint.x,myHero.y,self.movePoint.z))
		local enemyVec = WorldToScreen(D3DXVECTOR3(self.relativePoint.x,myHero.y,self.relativePoint.z))
		DrawLine(enemyVec.x, enemyVec.y, mouseVec.x, mouseVec.y, 3, ARGB(255,50,0,255))
	end

	function _RandomPath:random(pos)
		local activeState = StateManager:getActiveState().id
		self.movePoint.x = pos.x
		self.movePoint.z = pos.z
		if (activeState == PUSH_STATE or activeState == HOLDLANE_STATE) and Tasks:getPreviousTask().point then
			local moveToPos = self.relativePoint - (Vector(WAYPOINT) - Tasks:getPreviousTask().point):normalized()*MyHero.TrueRange/3.5
			self.pos1 = moveToPos + (Vector(moveToPos) - self.relativePoint):normalized():perpendicular() * MyHero.TrueRange/3
			self.pos2 = moveToPos + (Vector(moveToPos) - self.relativePoint):normalized():perpendicular2() * MyHero.TrueRange/3
			self.movePoint.x = moveToPos.x
			self.movePoint.z = moveToPos.z
			if IsWall(D3DXVECTOR3(self.pos1.x,self.pos1.y,self.pos1.z)) then
				self.pos1 = moveToPos 
			end
			if IsWall(D3DXVECTOR3(self.pos2.x,self.pos2.y,self.pos2.z)) then
				self.pos2 = moveToPos 
			end
		end
	end
class '_Orbwalker'
	function _Orbwalker:__init()
	    self.LastAttack = 0
	    self.LastWindUp = 0
	    self.LastAttackCooldown = 0
	    self.AttackCompletesAt = 0
	    self.AfterAttackTime = 0
	    self.Stage = STAGE_MOVE
	    self.AttackBufferMax = 400
	    self.BaseWindUp = 0.5
	    self.BaseAttackSpeed = 0.5
	    self.OrbwalkLocationOverride = nil
	    self.LastAttackedPosition = {x = myHero.x, z = myHero.z}

	    self.LowestAttackSpeed = myHero.attackSpeed

	    AddTickCallback(function() self:_OnTick() end)
	    AddProcessAttackCallback(function(Unit, Spell) self:_OnProcessSpell(Unit, Spell) end)
	    AddAnimationCallback(function(Unit, Animation) self:_OnAnimation(Unit, Animation) end)
	end

	--[[ Callbacks ]]
	function _Orbwalker:_OnTick()
	    if not self:CanShoot() and self:CanMove() and not self.DoneOnAttacked then
            self.AfterAttackTime = self.LastAttack + self.LastWindUp + self.AttackBufferMax
            self:_OnAttacked()
	    end
	    if Helper.Tick + Helper.Latency / 2 > self.LastAttack + self.LastAttackCooldown then
            self.Stage = STAGE_SHOOT
        elseif Helper.Tick + Helper.Latency / 2 > self.LastAttack + self.LastWindUp + self.AttackBufferMax + (Data.ChampionData[myHero.charName] and Data.ChampionData[myHero.charName].BugDelay or 0) then
            self.Stage = STAGE_MOVE
        end
	end

	function _Orbwalker:_OnProcessSpell(Unit, Spell)
	    if Unit.isMe then
            if Data:IsAttack(Spell) then
            		self.Stage = STAGE_SHOOTING
                    self.LastAttack = Helper:GetTime() - GetLatency() / 2
                    self.LastWindUp = Spell.windUpTime * 1000
                    self.LastAttackCooldown = Spell.animationTime * 1000
                    self.AttackCompletesAt = self.LastAttack + self.LastWindUp
                    self.LastAttackedPosition = {x = myHero.x, z = myHero.z}
                    self.DoneOnAttacked = false
                    MyHero.DonePreAttack = false
                    if self.BaseAttackSpeed == 0.5 then
                        self.BaseWindUp = 1 / (Spell.windUpTime * myHero.attackSpeed)
                        self.BaseAttackSpeed = 1 / (Spell.animationTime * myHero.attackSpeed)
                    end
            elseif Data:IsResetSpell(Spell) then
                    self:ResetAttackTimer()
            end
	    end
	end

	function _Orbwalker:_OnAnimation(Unit, Animation)
	    if Helper:GetTime() < self.AttackCompletesAt and Unit.isMe and (Animation == "Run" or Animation == "Idle") then
            self:ResetAttackTimer()
	    end
	end

	function _Orbwalker:GetAnimationTime()
	    if self then
	            return (1 / (myHero.attackSpeed * self.BaseAttackSpeed))
	    end
	    return 0.5
	end

	function _Orbwalker:GetWindUp()
	    if self then
	            return (1 / (myHero.attackSpeed * self.BaseWindUp))
	    end
	    return 0.5
	end

	function _Orbwalker:ResetAttackTimer()
	    self.LastAttack = Helper:GetTime() - GetLatency() / 2 - self.LastAttackCooldown
	end

	function _Orbwalker:Orbwalk(Target)
	    if MyHero.CanOrbwalk then
	        if self:CanOrbwalkTarget(Target) then
                if self:CanShoot() then
                    MyHero:Attack(Target)
                elseif self:CanMove()  then
                    MyHero:Move()
                end
	        else
                MyHero:Move()
	        end
	    end
	end

	function _Orbwalker:OrbwalkToPosition(Target, Position)
	    if self:CanOrbwalkTarget(Target) then
            if self:CanShoot() then
                MyHero:Attack(Target)
            elseif self:CanMove()  then
                MyHero:Move(Position)
            end
	    else
            MyHero:Move(Position)
	    end
	end

	function _Orbwalker:OrbwalkIgnoreChecks(target)
	    if target and self:CanShoot() then
	            MyHero:Attack(target, true)
	    elseif not self:CanShoot() then
	            MyHero:Move()
	    end
	end

	function _Orbwalker:CanMove(Time)
	    --return (Helper:GetTime() + (GetLatency() / 2) - ModesMenu.aaDelay > self.LastAttack + self.LastWindUp + 20)
	    Time = Time or 0
	    return Helper:GetTime() + Time - 20 + GetLatency() / 2 - self.LastAttack >= (1000 / (myHero.attackSpeed * self.BaseWindUp))
	end

	function _Orbwalker:CanShoot(Time)
	    --return (Helper:GetTime() + (GetLatency() / 2) > self.LastAttack + (self.LastAttackCooldown * self:GetAttackSlowModifier()))
	    Time = Time or 0
	    return Helper:GetTime() + Time + GetLatency() / 2 - self.LastAttack >= (1000 / (myHero.attackSpeed * self.BaseAttackSpeed))
	end

	function _Orbwalker:GetHitboxRadius(Unit)
	    if Unit ~= nil then
	            return Helper:GetDistance(Unit.minBBox, Unit.maxBBox) / 2
	    end
	end

	function _Orbwalker:CanOrbwalkTarget(Target)
	    if ValidTarget(Target) then
	            if Target.type == myHero.type then
	                    if Helper:GetDistance(Target) - Data:GetGameplayCollisionRadius(Target.charName) - self:GetScalingRange(Target) < MyHero.TrueRange then
	                            return true
	                    end
	            else
	                    if Helper:GetDistance(Target) - Data:GetGameplayCollisionRadius(Target.charName) + 20 < MyHero.TrueRange then
	                            return true
	                    end
	            end
	    end
	    return false
	end

	function _Orbwalker:CanOrbwalkTargetCustomRange(Target, Range)
	    if ValidTarget(Target) then
	            if Target.type == myHero.type then
	                    if Helper:GetDistance(Target) - Data:GetGameplayCollisionRadius(Target.charName) - self:GetScalingRange(Target) < Range + MyHero.GameplayCollisionRadius + MyHero:GetScalingRange() then
	                            return true
	                    end
	            else
	                    if Helper:GetDistance(Target) - Data:GetGameplayCollisionRadius(Target.charName) + 20 < Range + MyHero.GameplayCollisionRadius + MyHero:GetScalingRange() then
	                            return true
	                    end
	            end
	    end
	    return false
	end

	function _Orbwalker:CanOrbwalkTargetFromPosition(Target, Position)
	    if ValidTarget(Target) then
            if Target.type == myHero.type then
                if Helper:GetDistance(Target, Position) - Data:GetGameplayCollisionRadius(Target.charName) - self:GetScalingRange(Target) < MyHero.TrueRange then
                    return true
                end
            else
                if Helper:GetDistance(Target, Position) - Data:GetGameplayCollisionRadius(Target.charName) < MyHero.TrueRange then
                    return true
                end
            end
	    end
	    return false
	end

	function _Orbwalker:IsAfterAttack()
	    return Helper:GetTime() + (GetLatency() / 2) < self.AfterAttackTime
	end

	function _Orbwalker:GetScalingRange(Target)
	    if Target.type == myHero.type and Target.team ~= myHero.team then
	            local scale = Data:GetOriginalHitBox(Target)
	            return (scale and (Helper:GetDistance(Target.minBBox, Target.maxBBox) - Data:GetOriginalHitBox(Target)) / 2 or 0)
	    end
	    return 0
	end

	function _Orbwalker:GetNextAttackTime()
	    return self.LastAttack + (1000 / (myHero.attackSpeed * self.BaseAttackSpeed))
	end

	function _Orbwalker:IsShooting()
	    return not self:CanMove(-GetLatency() / 2) and not self:CanShoot()
	end

	function _Orbwalker:AttackOnCooldown()
	    return Helper:GetTime() < self:GetNextAttackTime()
	end

	function _Orbwalker:AttackReady()
	    return self:CanShoot()
	end

	function RegisterOnAttacked(func)
	    table.insert(RegisteredOnAttacked, func)
	end

	function _Orbwalker:_OnAttacked()
	    for _, func in pairs(RegisteredOnAttacked) do
	            func()
	    end
	    self.DoneOnAttacked = true
	end

	function _Orbwalker:OverrideOrbwalkLocation(Position)
	    self.OrbwalkLocationOverride = Position
	end
class '_MyHero' 
	function _MyHero:__init()
        self.Range = myHero.range
        self.HitBox = Helper:GetDistance(myHero.minBBox)
        self.GameplayCollisionRadius = Data:GetGameplayCollisionRadius(myHero.charName)
        self.TrueRange = self.Range + self.GameplayCollisionRadius
        self.IsMelee = myHero.range < 300
        self.MoveDistance = 480
        self.LastHitDamageBuffer = -15 --TODO
        self.StartAttackSpeed = 0.665
        self.ChampionAdditionalLastHitDamage = 0
        self.ItemAdditionalLastHitDamage = 0
        self.MasteryAdditionalLastHitDamage = 0
        self.Team = myHero.team == 100 and "Blue" or "Red"
        self.ProjectileSpeed = myHero.range > 300 and VP:GetProjectileSpeed(myHero) or math.huge
        self.LastMoved = 0
        self.MoveDelay = 50
        self.CanMove = true
        self.CanAttack = true
        self.CanOrbwalk = true
        self.InStandZone = false
        self.HasStopped = false
        self.HasSpoils = false
        self.SpoilStacks = 0
        self.LastSpoil = 0
        self.IsAttacking = false
        self.Spoils = {}
        MyHero = self

        for i = 0, objManager.maxObjects do
                local Object = objManager:getObject(i)
                if Object and Helper:GetDistance(Object) < 80 and  Object.name:find("GLOBAL_Item_FoM_Charge") then
                        if Object.name:find("GLOBAL_Item_FoM_Charge01") then
                                self.LastSpoilCreated = 1
                                self.SpoilStacks = 1
                        elseif Object.name:find("GLOBAL_Item_FoM_Charge02") then
                                self.LastSpoilCreated = 2
                                self.SpoilStacks = 2
                        elseif Object.name:find("GLOBAL_Item_FoM_Charge03") then
                                self.LastSpoilCreated = 3
                                self.SpoilStacks = 3
                        elseif Object.name:find("GLOBAL_Item_FoM_Charge04") then
                                self.LastSpoilCreated = 4
                                self.SpoilStacks = 4
                        end
                        table.insert(self.Spoils, Object)
                end
        end

        AddTickCallback(function() self:_OnTick() end)
        AddCreateObjCallback(function(Object) self:_OnCreateObj(Object) end)
        AddDeleteObjCallback(function(Object) self:_OnDeleteObj(Object) end)
	end

	function _MyHero:_OnTick()
	        self.TrueRange = myHero.range + self.GameplayCollisionRadius + self:GetScalingRange()
	        if myHero.range ~= self.Range then
	                if myHero.range and myHero.range > 0 and myHero.range < 1500 then
	                        self.Range = myHero.range
	                        self.IsMelee = myHero.range < 300
	                end
	        end

	        self.HasSpoils = self.SpoilStacks > 0
	        self:CheckStopMovement()
	        --self.ChampionAdditionalLastHitDamage = ChampionBuffs:GetBonusDamage()
	end

	function _MyHero:_OnCreateObj(Object)
	        if Helper:GetDistance(Object) < 80 and Object.name:find("GLOBAL_Item_FoM_Charge") then
	                if Object.name:find("GLOBAL_Item_FoM_Charge01") then
	                        self.LastSpoilCreated = 1
	                        self.SpoilStacks = 1
	                elseif Object.name:find("GLOBAL_Item_FoM_Charge02") then
	                        self.LastSpoilCreated = 2
	                elseif Object.name:find("GLOBAL_Item_FoM_Charge03") then
	                        self.LastSpoilCreated = 3
	                elseif Object.name:find("GLOBAL_Item_FoM_Charge04") then
	                        self.LastSpoilCreated = 4
	                        self.SpoilStacks = 4
	                end
	                table.insert(self.Spoils, Object)
	        end
	end

	function _MyHero:_OnDeleteObj(Object)
	        for i, Spoil in pairs(self.Spoils) do
	                if Object and Object == Spoil then
	                        if Object.name:find("GLOBAL_Item_FoM_Charge01") then
	                                if self.LastSpoilCreated == 1 then
	                                        self.SpoilStacks = 0
	                                else
	                                        self.SpoilStacks = 2
	                                end
	                        elseif Object.name:find("GLOBAL_Item_FoM_Charge02") then
	                                if self.LastSpoilCreated == 1 then
	                                        self.SpoilStacks = 1
	                                else
	                                        self.SpoilStacks = 3
	                                end
	                        elseif Object.name:find("GLOBAL_Item_FoM_Charge03") then
	                                if self.LastSpoilCreated == 4 then
	                                        self.SpoilStacks = 4
	                                else
	                                        self.SpoilStacks = 2
	                                end
	                        elseif Object.name:find("GLOBAL_Item_FoM_Charge04") then
	                                self.SpoilStacks = 3
	                        end
	                        table.remove(self.Spoils, i)
	                end
	               
	        end
	end

	function _MyHero:GetScalingRange()
	        local scale = Data:GetOriginalHitBox(myHero)
	        return (scale and (Helper:GetDistance(myHero.minBBox, myHero.maxBBox) - Data:GetOriginalHitBox(myHero)) / 2 or 0)
	end

	function _MyHero:SetProjectileSpeed(Speed)
	        self.ProjectileSpeed = Speed
	end

	function _MyHero:GetTimeToHitTarget(Target)
	        if self.IsMelee then
	                return Helper:GetTime() + Orbwalker.GetWindUp() + GetLatency() / 2
	        else
	                --return Orbwalker.LastWindUp + (math.max(GetDistance(Target.visionPos), GetDistance(Target)) - MyHero.GameplayCollisionRadius) / self.ProjectileSpeed - GetLatency() / 2000 - 0.07
	                return (GetLatency() / 2 + (GetDistance(Target.visionPos, myHero.visionPos)) / MyHero.ProjectileSpeed + 1000 / (myHero.attackSpeed * Orbwalker.BaseWindUp))
	        end
	end

	function _MyHero:GetTotalAttackDamageAgainstTarget(Target, LastHit)
	        local MyDamage = myHero.totalDamage --:CalcDamage(Target, myHero.totalDamage)
	        if LastHit then
	                MyDamage = self:GetMasteryAdditionalLastHitDamage(MyDamage, Target)
	                --MyDamage = MyDamage + self.ChampionAdditionalLastHitDamage
	                --MyDamage = MyDamage + self.ItemAdditionalLastHitDamage
	        end
	        return MyDamage
	end

	function _MyHero:GetMasteryAdditionalLastHitDamage(Damage, Target)
	        if not ConfigMenu then return Damage end

	        local armorPen = 0
	        local armorPenPercent = 0
	        local magicPen = 0
	        local magicPenPercent = 0
	        local magicDamage = 0
	        local physDamage = _Damage
	        local dmgReductionPercent = 0

	        local totalDamage = physDamage

	        if ConfigMenu.ArcaneBlade then
	                magicDamage = myHero.ap * .05
	        end

	        if ConfigMenu.DevastatingStrike then
	                armorPenPercent = .06
	        end

	        if ConfigMenu.DoubleEdgedSword then
	                physDamage = myHero.range < 400 and physDamage*1.02 or (physDamage*1.015)
	                magicDamage = myHero.range < 400 and magicDamage*1.02 or (magicDamage*1.015)
	        end

	        if ConfigMenu.Butcher then
	                physDamage = physDamage + 2
	        end

	        return ((physDamage * (100/(100 + target.armor * (1-armorPenPercent)))  
	         + magicDamage * (100/(100 + target.magicArmor * (1-magicPenPercent))) ) * (1-dmgReductionPercent))
	end

	function _MyHero:Move(Position)
        if self:HeroCanMove() and not Helper:IsEvading() and not Orbwalker:IsShooting() and Orbwalker:CanMove() and (not Orbwalker:CanShoot(60) or Orbwalker:CanShoot()) then
        	if not Position then
				if GetDistance2D(myHero,{x=RandomPath.absolutePath.x,z=RandomPath.absolutePath.z}) > 50 then  
		            myHero:MoveTo(RandomPath.absolutePath.x, RandomPath.absolutePath.z)
		            self.LastMoved = Helper.Tick
		            self.HasStopped = false
		        end
		    else
		    	myHero:MoveTo(Position.x, Position.z)
		    end
        end
	end

	function _MyHero:MoveTo(x,z)
        if self:HeroCanMove() and not Helper:IsEvading() and not Orbwalker:IsShooting() and Orbwalker:CanMove() and (not Orbwalker:CanShoot(60) or Orbwalker:CanShoot()) then
            if GetDistance2D(myHero,{x=x,z=z}) > 50 then
            	myHero:MoveTo(x, z)
            	self.LastMoved = Helper.Tick
            	self.HasStopped = false
           	end
		end
	end

	function _MyHero:Attack(target, packetOverride)
	        if not self:HeroCanAttack() then
	                MyHero:Move()
	                return
	        end
	        if self.CanAttack and not Helper:IsEvading() and Orbwalker:CanShoot() then
	                if not self.DonePreAttack then
                        self.DonePreAttack = true
	                end
                    myHero:Attack(target)
	                Orbwalker.LastEnemyAttacked = target
	        end
	end

	function _MyHero:MovementEnabled(canMove)
	        self.CanMove = canMove
	end

	function _MyHero:AttacksEnabled(canAttack)
	        self.CanAttack = canAttack
	end

	function _MyHero:OrbwalkingEnabled(canOrbwalk)
	        self.CanOrbwalk = canOrbwalk
	end

	function _MyHero:HeroCanAttack()
	        return true
	end

	function _MyHero:HeroCanMove()
	        if self.InStandZone or not self.CanMove then
	                return false
	        else
	        	return true
	        end
	end

	function _MyHero:CheckStopMovement()
	        if not MyHero:HeroCanMove() and not self.HasStopped then
	                myHero:HoldPosition()
	                self.HasStopped = true
	        end
	end
class '_Attack'
	function _Attack:__init(Source, Spell)
	    self.Source = Source
	    self.Target = Spell.target
	    self.Damage = Source:CalcDamage(self.Target)
	    self.Started = GetTickCount()
	    self.Delay = Spell.windUpTime * 1000
	    self.Speed = MINIONS.Data[Source.charName] and MINIONS.Data[Source.charName].ProjectileSpeed or MINIONS.Data[Source.type].ProjectileSpeed
	    self.LandsAt = ((self.Speed == 0 and self.Delay or self.Delay + GetDistance(Source, self.Target) / self.Speed) + GetTickCount())
	    self.FiresAt = GetTickCount() + self.Delay
	    self.Origin = {x = Source.x, z = Source.z}
	    self.IsTowerShot = false
	end
class '_Data'
 	function _Data:__init()
	    self.ResetSpells = {}
	    self.SpellAttacks = {}
	    self.NoneAttacks = {}
	    self.ChampionData = {}
	    self.MinionData = {}
	    self.JungleData = {}
        self.ItemData = {}
        self.Skills = {}
	    self.EnemyHitBoxes = {}
	    self.ImmuneEnemies = {}
	    self.WardData = {}
	    Data = self

	    self:__GenerateNoneAttacks()
	    self:__GenerateSpellAttacks()
	    self:__GenerateResetSpells()
	    self:_GenerateMinionData()
	    self:_GenerateJungleData()
	    self:_GenerateItemData()
	    self:__GenerateChampionData()
	    self:__GenerateSkillData()
	    Data:_GenerateWardData()

	    AdvancedCallback:bind("OnGainBuff", function(Unit, Buff) self:OnGainBuff(Unit, Buff) end)
	    AdvancedCallback:bind("OnLoseBuff", function(Unit, Buff) self:OnLoseBuff(Unit, Buff) end)

	    if GetGameTimer() < self:GetHitBoxLastSavedTime() then
	            self:GenerateHitBoxData()
	    else
	            self:LoadHitBoxData()
	    end
	end

	function _Data:OnGainBuff(Unit, Buff)
	    if Unit.team ~= myHero.team and (Buff.name == "UndyingRage" or Buff.name == "JudicatorIntervention") then
	            self.ImmuneEnemies[Unit.charName] = true
	    end
	end

	function _Data:OnLoseBuff(Unit, Buff)
	    if Unit.team ~= myHero.team and (Buff.name == "UndyingRage" or Buff.name == "JudicatorIntervention") then
	            self.ImmuneEnemies[Unit.charName] = nil
	    end
	end

	function _Data:__GenerateResetSpells()
	    self:AddResetSpell("Powerfist")
	    self:AddResetSpell("DariusNoxianTacticsONH")
	    self:AddResetSpell("Takedown")
	    self:AddResetSpell("Ricochet")
	    self:AddResetSpell("BlindingDart")
	    self:AddResetSpell("VayneTumble")
	    self:AddResetSpell("JaxEmpowerTwo")
	    self:AddResetSpell("MordekaiserMaceOfSpades")
	    self:AddResetSpell("SiphoningStrikeNew")
	    self:AddResetSpell("RengarQ")
	    self:AddResetSpell("MonkeyKingDoubleAttack")
	    self:AddResetSpell("YorickSpectral")
	    self:AddResetSpell("ViE")
	    self:AddResetSpell("GarenSlash3")
	    self:AddResetSpell("HecarimRamp")
	    self:AddResetSpell("XenZhaoComboTarget")
	    self:AddResetSpell("LeonaShieldOfDaybreak")
	    self:AddResetSpell("ShyvanaDoubleAttack")
	    self:AddResetSpell("shyvanadoubleattackdragon")
	    self:AddResetSpell("TalonNoxianDiplomacy")
	    self:AddResetSpell("TrundleTrollSmash")
	    self:AddResetSpell("VolibearQ")
	    self:AddResetSpell("PoppyDevastatingBlow")
	    self:AddResetSpell("SivirW")
	    self:AddResetSpell("Ricochet")
	end

	function _Data:__GenerateSpellAttacks()
	    self:AddSpellAttack("frostarrow")
	    self:AddSpellAttack("CaitlynHeadshotMissile")
	    self:AddSpellAttack("QuinnWEnhanced")
	    self:AddSpellAttack("TrundleQ")
	    self:AddSpellAttack("XenZhaoThrust")
	    self:AddSpellAttack("XenZhaoThrust2")
	    self:AddSpellAttack("XenZhaoThrust3")
	    self:AddSpellAttack("GarenSlash2")
	    self:AddSpellAttack("RenektonExecute")
	    self:AddSpellAttack("RenektonSuperExecute")
	    self:AddSpellAttack("KennenMegaProc")
	    self:AddSpellAttack("redcardpreattack")
	    self:AddSpellAttack("bluecardpreattack")
	    self:AddSpellAttack("goldcardpreattack")
	    self:AddSpellAttack("MasterYiDoubleStrike")
	end

	function _Data:__GenerateNoneAttacks()
	    self:AddNoneAttack("shyvanadoubleattackdragon")
	    self:AddNoneAttack("ShyvanaDoubleAttack")
	    self:AddNoneAttack("MonkeyKingDoubleAttack")
	end

	function _Data:_GenerateMinionData()
	    self:AddMinionData((myHero.team == 100 and "Blue" or "Red").."_Minion_Basic", 400, 0)
	    self:AddMinionData((myHero.team == 100 and "Blue" or "Red").."_Minion_Caster", 484, 0.65)
	    self:AddMinionData((myHero.team == 100 and "Blue" or "Red").."_Minion_Wizard", 484, 0.65)
	    self:AddMinionData((myHero.team == 100 and "Blue" or "Red").."_Minion_MechCannon", 365, 1.2)
	    self:AddMinionData("obj_AI_Turret", 150, 1.2)
	end

	function _Data:_GenerateJungleData()
	    self:AddJungleMonster("Worm12.1.1",             1)              -- Baron
	    self:AddJungleMonster("Dragon6.1.1",            1)              -- Dragon
	    self:AddJungleMonster("AncientGolem1.1.1",      1)              -- Blue Buff
	    self:AddJungleMonster("AncientGolem7.1.1",      1)              -- Blue Buff
	    self:AddJungleMonster("YoungLizard1.1.2",       2)              -- Blue Buff Add
	    self:AddJungleMonster("YoungLizard7.1.3",       2)              -- Blue Buff Add
	    self:AddJungleMonster("YoungLizard1.1.3",       2)              -- Blue Buff Add
	    self:AddJungleMonster("YoungLizard7.1.2",       2)              -- Blue Buff Add
	    self:AddJungleMonster("LizardElder4.1.1",       1)              -- Red Buff
	    self:AddJungleMonster("LizardElder10.1.1",      1)              -- Red Buff
	    self:AddJungleMonster("YoungLizard4.1.2",       2)              -- Red Buff Add
	    self:AddJungleMonster("YoungLizard4.1.3",       2)              -- Red Buff Add
	    self:AddJungleMonster("YoungLizard10.1.2",      2)              -- Red Buff Add
	    self:AddJungleMonster("YoungLizard10.1.3",      2)              -- Red Buff Add
	    self:AddJungleMonster("GiantWolf2.1.3",         1)              -- Big Wolf
	    self:AddJungleMonster("GiantWolf2.1.1",         1)              -- Big Wolf
	    self:AddJungleMonster("GiantWolf8.1.3",         1)              -- Big Wolf
	    self:AddJungleMonster("GiantWolf8.1.1",         1)              -- Big Wolf
	    self:AddJungleMonster("wolf2.1.1",                      2)              -- Small Wolf
	    self:AddJungleMonster("wolf2.1.2",                      2)              -- Small Wolf
	    self:AddJungleMonster("wolf8.1.1",                      2)              -- Small Wolf
	    self:AddJungleMonster("wolf8.1.2",                      2)              -- Small Wolf
	    self:AddJungleMonster("Wolf8.1.3",                      2)              -- Small Wolf
	    self:AddJungleMonster("Wolf8.1.2",                      2)              -- Small Wolf
	    self:AddJungleMonster("Wolf2.1.3",                      2)              -- Small Wolf
	    self:AddJungleMonster("Wolf2.1.2",                      2)              -- Small Wolf
	    self:AddJungleMonster("Wraith3.1.3",            1)              -- Big Wraith
	    self:AddJungleMonster("Wraith3.1.1",            1)              -- Big Wraith
	    self:AddJungleMonster("Wraith9.1.3",            1)              -- Big Wraith
	    self:AddJungleMonster("Wraith9.1.1",            1)              -- Big Wraith
	    self:AddJungleMonster("LesserWraith3.1.1",      2)              -- Small Wraith
	    self:AddJungleMonster("LesserWraith3.1.3",      2)              -- Small Wraith
	    self:AddJungleMonster("LesserWraith3.1.2",      2)              -- Small Wraith
	    self:AddJungleMonster("LesserWraith3.1.4",      2)              -- Small Wraith
	    self:AddJungleMonster("LesserWraith9.1.1",      2)              -- Small Wraith
	    self:AddJungleMonster("LesserWraith9.1.2",      2)              -- Small Wraith
	    self:AddJungleMonster("LesserWraith9.1.4",      2)              -- Small Wraith
	    self:AddJungleMonster("LesserWraith9.1.3",      2)              -- Small Wraith
	    self:AddJungleMonster("Golem5.1.2",             1)              -- Big Golem
	    self:AddJungleMonster("Golem11.1.2",            1)              -- Big Golem
	    self:AddJungleMonster("SmallGolem5.1.1",        2)              -- Small Golem
	    self:AddJungleMonster("SmallGolem11.1.1",       2)              -- Small Golem
	    self:AddJungleMonster("GreatWraith13.1.1",      2)              -- Great Wraith
	    self:AddJungleMonster("GreatWraith14.1.1",      2)              -- Great Wraith
	end

	function _Data:_GenerateItemData()
	    self:AddItemData("Blade of the Ruined King",    3153, true, 500)
	    self:AddItemData("Bilgewater Cutlass",                  3144, true, 500)
	    self:AddItemData("Deathfire Grasp",                     3128, true, 750)
	    self:AddItemData("Hextech Gunblade",                    3146, true, 400)
	    self:AddItemData("Blackfire Torch",                     3188, true, 750)
	    self:AddItemData("Frost Queens Claim",                  3098, true, 750)
	    self:AddItemData("Talisman of Ascension",               3098, false)
	    self:AddItemData("Ravenous Hydra",                              3074, false)
	    self:AddItemData("Sword of the Divine",                 3131, false)
	    self:AddItemData("Tiamat",                                              3077, false)
	    self:AddItemData("Entropy",                                     3184, false)
	    self:AddItemData("Youmuu's Ghostblade",                 3142, false)
	    self:AddItemData("Muramana",                                    3042, false)
	    self:AddItemData("Randuins Omen",                               3143, false)
	end
	OBJECT_TYPE_WARD = 0
	OBJECT_TYPE_BOX = 1
	OBJECT_TYPE_TRAP = 2
	function _Data:_GenerateWardData()
	                                      -- charName            -- Name                        --spellName             -- Type                 --Range   --Duration
	    self:AddWardData("VisionWard",          "VisionWard",           "visionward",           OBJECT_TYPE_WARD,        1450,          180000)
	    self:AddWardData("SightWard",           "SightWard",            "sightward",            OBJECT_TYPE_WARD,        1450,          180000)
	    self:AddWardData("YellowTrinket",       "SightWard",            "sightward",            OBJECT_TYPE_WARD,        1450,          180000)
	    self:AddWardData("SightWard",           "VisionWard",           "itemghostward",        OBJECT_TYPE_WARD,        1450,          180000)
	    self:AddWardData("SightWard",           "VisionWard",           "itemminiward",         OBJECT_TYPE_WARD,        1450,          60000)
	    self:AddWardData("SightWard",           "SightWard",            "wrigglelantern",       OBJECT_TYPE_WARD,        1450,          180000)
	    self:AddWardData("ShacoBox",            "Jack In The Box",      "jackinthebox",         OBJECT_TYPE_BOX,         300,           60000)
	end

	ROLE_AD_CARRY = 1
	ROLE_AP = 2
	ROLE_SUPPORT = 3
	ROLE_BRUISER = 4
	ROLE_TANK = 5

	function _Data:__GenerateChampionData()
	                                            -- Champion, Projectile Speed,  GameplayCollisionRadius         Anti-bug delay                  Role
	    self:AddChampionData("Aatrox",                  0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Ahri",            1.6,                            65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Akali",           0,                              65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Alistar",         0,                              80,                                             0,                      ROLE_SUPPORT)
	    self:AddChampionData("Amumu",           0,                              55,                                             0,                      ROLE_TANK)
	    self:AddChampionData("Anivia",          1.4,                            65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Annie",           1,                              55,                                             0,                      ROLE_AP)
	    self:AddChampionData("Ashe",            2,                              65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Blitzcrank",      0,                              80,                                             0,                      ROLE_SUPPORT)
	    self:AddChampionData("Brand",           1.975,                          65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Caitlyn",         2.5,                            65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Cassiopeia",      1.22,                           65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Chogath",         0,                              80,                                             0,                      ROLE_TANK)
	    self:AddChampionData("Corki",           2,                              65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Darius",                  0,                                      80,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Diana",           0,                              65,                                             0,                      ROLE_AP)
	    self:AddChampionData("DrMundo",         0,                              80,                                             0,                      ROLE_TANK)
	    self:AddChampionData("Draven",          1.4,                            65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Elise",                   0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Evelynn",         0,                              65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Ezreal",          2,                              65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("FiddleSticks",    1.75,                           65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Fiora",                   0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Fizz",            0,                              65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Galio",           0,                              80,                                             0,                      ROLE_TANK)
	    self:AddChampionData("Gangplank",               0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Garen",                   0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Gragas",          0,                              80,                                             0,                      ROLE_AP)
	    self:AddChampionData("Graves",          3,                              65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Hecarim",         0,                              80,                                             0,                      ROLE_TANK)
	    self:AddChampionData("Heimerdinger",    1.4,                            55,                                             0,                      ROLE_AP)
	    self:AddChampionData("Irelia",                  0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Janna",           1.2,                            65,                                             0,                      ROLE_SUPPORT)
	    self:AddChampionData("JarvanIV",                0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Jax",                             0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Jayce",           2.2,                            65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Jinx",            2,                              65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Karma",           1.2,                            65,                                             0,                      ROLE_SUPPORT)
	    self:AddChampionData("Karthus",         1.25,                           65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Kassadin",        0,                              65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Katarina",        0,                              65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Kayle",           1.8,                            65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Kennen",          1.35,                           55,                                             0,                      ROLE_AP)
	    self:AddChampionData("Khazix",                  0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("KogMaw",          1.8,                            65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Leblanc",         1.7,                            65,                                             0,                      ROLE_AP)
	    self:AddChampionData("LeeSin",                  0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Leona",           0,                              65,                                             0,                      ROLE_SUPPORT)
	    self:AddChampionData("Lissandra",       0,                              65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Lucian",          2,                              65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Lulu",            2.5,                            65,                                             0,                      ROLE_SUPPORT)
	    self:AddChampionData("Lux",             1.55,                           65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Malphite",        0,                              80,                                             0,                      ROLE_TANK)
	    self:AddChampionData("Malzahar",        1.5,                            65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Maokai",          0,                              80,                                             0,                      ROLE_TANK)
	    self:AddChampionData("MasterYi",        0,                              65,                                             0,                      ROLE_AP)
	    self:AddChampionData("MissFortune",     2,                              65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("MonkeyKing",              0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Mordekaiser",     0,                              80,                                             0,                      ROLE_AP)
	    self:AddChampionData("Morgana",         1.6,                            65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Nami",            0,                              65,                                             0,                      ROLE_SUPPORT)
	    self:AddChampionData("Nasus",           0,                              80,                                             0,                      ROLE_TANK)
	    self:AddChampionData("Nautilus",                0,                                      80,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Nidalee",         1.7,                            65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Nocturne",                0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Nunu",            0,                              65,                                             0,                      ROLE_SUPPORT)
	    self:AddChampionData("Olaf",                    0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Orianna",         1.4,                            65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Pantheon",        0,                              65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Poppy",                   0,                                      55,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Quinn",           1.85,                           65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Rammus",          0,                              65,                                             0,                      ROLE_TANK)
	    self:AddChampionData("Renekton",                0,                                      80,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Rengar",                  0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Riven",                   0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Rumble",          0,                              80,                                             0,                      ROLE_AP)
	    self:AddChampionData("Ryze",            2.4,                            65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Sejuani",         0,                              80,                                             0,                      ROLE_TANK)
	    self:AddChampionData("Shaco",           0,                              65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Shen",            0,                              65,                                             0,                      ROLE_TANK)
	    self:AddChampionData("Shyvana",                 0,                                      50,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Singed",          0,                              65,                                             0,                      ROLE_TANK)
	    self:AddChampionData("Sion",            0,                              80,                                             0,                      ROLE_AP)
	    self:AddChampionData("Sivir",           1.4,                            65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Skarner",         0,                              80,                                             0,                      ROLE_TANK)
	    self:AddChampionData("Sona",            1.6,                            65,                                             0,                      ROLE_SUPPORT)
	    self:AddChampionData("Soraka",          1,                              65,                                             0,                      ROLE_SUPPORT)
	    self:AddChampionData("Swain",           1.6,                            65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Syndra",          1.2,                            65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Talon",           0,                              65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Taric",           0,                              65,                                             0,                      ROLE_SUPPORT)
	    self:AddChampionData("Teemo",           1.3,                            55,                                             0,                      ROLE_AP)
	    self:AddChampionData("Thresh",          0,                              55,                                             0,                      ROLE_SUPPORT)
	    self:AddChampionData("Tristana",        2.25,                           55,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Trundle",                 0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Tryndamere",              0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("TwistedFate",     1.5,                            65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Twitch",          2.5,                            65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Udyr",                    0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Urgot",           1.3,                            80,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Varus",           2,                              65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Vayne",           2,                              65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Veigar",          1.05,                           55,                                             0,                      ROLE_AP)
	    self:AddChampionData("Velkoz",                  1.8,                            55,                                             0,                              ROLE_AP)
	    self:AddChampionData("Vi",                              0,                                      50,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Viktor",          2.25,                           65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Vladimir",        1.4,                            65,                                             0,                      ROLE_AP)
	    self:AddChampionData("Volibear",        0,                              80,                                             0,                      ROLE_TANK)
	    self:AddChampionData("Warwick",         0,                              65,                                             0,                      ROLE_TANK)
	    self:AddChampionData("Xerath",          1.2,                            65,                                             0,                      ROLE_AP)
	    self:AddChampionData("XinZhao",                 0,                                      65,                                             0,                              ROLE_BRUISER)
	    self:AddChampionData("Yasuo",           0,                              65,                                             0,                      ROLE_BRUISER)
	    self:AddChampionData("Yorick",          0,                              80,                                             0,                      ROLE_TANK)
	    self:AddChampionData("Zac",             0,                              65,                                             0,                      ROLE_TANK)
	    self:AddChampionData("Zed",             0,                              65,                                             0,                      ROLE_AD_CARRY)
	    self:AddChampionData("Ziggs",           1.5,                            55,                                             0,                      ROLE_AP)
	    self:AddChampionData("Zilean",          1.25,                           65,                                             0,                      ROLE_SUPPORT)
	    self:AddChampionData("Zyra",            1.7,                            65,                                             0,                      ROLE_AP)
	end                                                    
	                                                   
	function _Data:__GenerateSkillData()
	                              --Name                          Enabled    Key         Range          Display Name                            Type                    MinMana  AfterAA Require Attack Target     Speed                Delay   Width   Collision        ResetAA
	    self:AddSkillData("Aatrox",                     true,    _E,     1000,   "E (Blades of Torment)",       SPELL_LINEAR,                   0,       false,         false,                                  1.2,            500,    150,    false,           false)
	    self:AddSkillData("Ahri",                       true,    _Q,     880,    "Q (Orb of Deception)",        SPELL_LINEAR,                   0,   false,             false,                                  1.1,            500,    100,    false,           false)
	    self:AddSkillData("Ahri",                       true,    _E,     880,    "E (Orb of Deception)",        SPELL_LINEAR_COL,               0,   false,             false,                                  1.2,            0.5,    60,             false,           false)
	    self:AddSkillData("Ezreal",                     true,    _Q,     1100,   "Q (Mystic Shot)",                     SPELL_LINEAR_COL,               0,       false,         false,                                  2,                      250,    70,             true,            false)
	    self:AddSkillData("Ezreal",                     true,    _W,     1050,   "W (Essence Flux)",            SPELL_LINEAR,                   0,       false,         false,                                  1.6,            250,    90,             false,           false)
	    self:AddSkillData("KogMaw",                     true,    _Q,     625,    "Q (Caustic Spittle)",         SPELL_TARGETED,                 0,       true,          true,                                   1.3,            260,    200,    false,           false)
	    self:AddSkillData("KogMaw",                     true,    _W,     625,    "W (Bio-Arcane Barrage)",      SPELL_SELF,                             0,       false,         false,                                  1.3,            260,    200,    false,           false)
	    self:AddSkillData("KogMaw",                     true,    _E,     850,    "E (Void Ooze)",                       SPELL_LINEAR,                   0,       false,         false,                                  1.3,            260,    200,    false,           false)
	    self:AddSkillData("KogMaw",                     true,    _R,     1700,   "R (Living Artillery)",        SPELL_LINEAR,                   0,       false,         false,                                  math.huge,      1000,   200,    false,           false)
	    self:AddSkillData("Sivir",                      true,    _Q,     1000,   "Q (Boomerang Blade)",         SPELL_LINEAR,                   0,       false,         false,                                  1.33,           250,    120,    false,           false)
	    self:AddSkillData("Sivir",                      true,    _W,     900,    "W (Ricochet)",                        SPELL_SELF,                             0,       true,          true,                                   1,                      0,              200,    false,           true)
	    self:AddSkillData("Graves",                     true,    _Q,     750,    "Q (Buck Shot)",                       SPELL_CONE,                             0,       false,         false,                                  2,                      250,    200,    false,           false)
	    self:AddSkillData("Graves",                     true,    _W,     700,    "W (Smoke Screen)",            SPELL_CIRCLE,                   0,       false,         false,                                  1400,           300,    500,    false,           false)
	    self:AddSkillData("Graves",                     true,    _E,     580,    "E (Quick Draw)",                      SPELL_SELF_AT_MOUSE,    0,       true,          true,                                   1450,           250,    200,    false,           false)
	    self:AddSkillData("Caitlyn",            true,    _Q,     1300,   "Q (Piltover Peacemaker)",     SPELL_LINEAR,                   0,       false,         false,                                  2.1,            625,    100,    true,            false)
	    self:AddSkillData("Corki",                      true,    _Q,     600,    "Q (Phosphorus Bomb)",         SPELL_CIRCLE,                   0,       false,         false,                                  2,                      200,    500,    false,           false)
	    self:AddSkillData("Corki",                      true,    _R,     1225,   "R (Missile Barrage)",         SPELL_LINEAR_COL,               0,       false,         false,                                  2,                      200,    50,             true,            false)
	    self:AddSkillData("Teemo",                      true,    _Q,     580,    "Q (Blinding Dart)",           SPELL_TARGETED,                 0,       false,         false,                                  2,                      0,              200,    false,           true)
	    self:AddSkillData("TwistedFate",        true,    _Q,     1200,   "Q (Wild Cards)",                      SPELL_LINEAR,                   0,       false,         false,                                  1.45,           250,    200,    false,           false)
	    self:AddSkillData("Vayne",                      true,    _Q,     750,    "Q (Tumble)",                          SPELL_SELF_AT_MOUSE,    0,       true,          true,                                   1.45,           250,    200,    false,           true)
	    self:AddSkillData("Vayne",                      true,    _R,     580,    "R (Final Hour)",                      SPELL_SELF,                             0,       false,         true,                                   1.45,           250,    200,    false,           false)
	    self:AddSkillData("MissFortune",        true,    _Q,     650,    "Q (Double Up)",                       SPELL_TARGETED,                 0,       true,          true,                                   1.45,           250,    200,    false,           false)
	    self:AddSkillData("MissFortune",        true,    _W,     580,    "W (Impure Shots)",            SPELL_SELF,                             0,       false,         true,                                   1.45,           250,    200,    false,           false)
	    self:AddSkillData("MissFortune",        true,    _E,     800,    "E (Make It Rain)",            SPELL_CIRCLE,                   0,       false,         false,                                  math.huge,      500,    500,    false,           false)
	    self:AddSkillData("Tristana",           true,    _Q,     580,    "Q (Rapid Fire)",                      SPELL_SELF,                             0,       false,         true,                                   1.45,           250,    200,    false,           false)
	    self:AddSkillData("Tristana",           true,    _E,     550,    "E (Explosive Shot)",          SPELL_TARGETED,                 0,       true,          false,                                  1.45,           250,    200,    false,           false)
	    self:AddSkillData("Draven",                     true,    _E,     950,    "E (Stand Aside)",                     SPELL_LINEAR,                   0,       false,         false,                                  1.37,           300,    130,    false,           false)
	    self:AddSkillData("Kennen",                     true,    _Q,     1050,   "Q (Thundering Shuriken)",     SPELL_LINEAR_COL,               0,       false,         false,                                  1.65,           180,    80,             true,            false)
	    self:AddSkillData("Ashe",                       true,    _W,     1200,   "W (Volley)",                          SPELL_LINEAR_COL,               0,       false,         false,                                  2,                      120,    85,             true,            false)
	    self:AddSkillData("Syndra",                     true,    _Q,     800,    "Q (Dark Sphere)",                     SPELL_CIRCLE,                   0,       false,         false,                                  math.huge,      400,    100,    false,           false)
	    self:AddSkillData("Jayce",                      true,    _Q,     1600,   "Q (Shock Blast)",                     SPELL_LINEAR_COL,               0,       false,         false,                                  2,                      350,    90,             true,            false)
	    self:AddSkillData("Nidalee",            true,    _Q,     1500,   "Q (Javelin Toss)",            SPELL_LINEAR_COL,               0,       false,         false,                                  1.3,            125,    80,             true,            false)
	    self:AddSkillData("Varus",                      true,    _E,     925,    "E (Hail of Arrows)",          SPELL_CIRCLE,                   0,       false,         false,                                  1.75,           240,    235,    false,           false)
	    self:AddSkillData("Quinn",                      true,    _Q,     1050,   "Q (Blinding Assault)",        SPELL_LINEAR_COL,               0,       false,         false,                                  1.55,           220,    90,             true,            false)
	    self:AddSkillData("LeeSin",                     true,    _Q,     975,    "Q (Sonic Wave)",                      SPELL_LINEAR_COL,               0,       false,         false,                                  1.5,            250,    70,             true,            false)
	    self:AddSkillData("Twitch",                     true,    _W,     950,    "W (Venom Cask)",                      SPELL_CIRCLE,                   0,       false,         false,                                  1.4,            250,    275,    false,           false)
	    self:AddSkillData("Darius",                     true,    _W,     300,    "W (Crippling Strike)",        SPELL_SELF,                             0,       true,          true,                                   2,                      0,              200,    true,            true)
	    self:AddSkillData("Hecarim",            true,    _Q,     300,    "Q (Rampage)",                         SPELL_SELF,                             0,       true,          true,                                   2,                      0,              200,    true,            false)
	    self:AddSkillData("Warwick",            true,    _Q,     300,    "Q (Hungering Strike)",        SPELL_TARGETED,                 0,       true,          true,                                   2,                      0,              200,    true,            false)
	    self:AddSkillData("MonkeyKing",         true,    _Q,     300,    "Q (Crushing Blow)",           SPELL_SELF,                             0,       true,          true,                                   2,                      0,              200,    true,            true)
	    self:AddSkillData("Poppy",                      true,    _Q,     300,    "Q (Devastating Blow)",        SPELL_SELF,                             0,       true,          true,                                   2,                      0,              200,    true,            true)
	    self:AddSkillData("Talon",                      true,    _Q,     300,    "Q (Noxian Diplomacy)",        SPELL_SELF,                             0,       true,          true,                                   2,                      0,              200,    true,            false)
	    self:AddSkillData("Nautilus",           true,    _W,     300,    "W (Titans Wrath)",            SPELL_SELF,                             0,       true,          true,                                   2,                      0,              200,    true,            false)
	    self:AddSkillData("Vi",                         true,    _E,     300,    "E (Excessive Force)",         SPELL_SELF,                             0,       true,          true,                                   2,                      0,              200,    true,            true)
	    self:AddSkillData("Rengar",                     true,    _Q,     300,    "Q (Savagery)",                        SPELL_SELF,                             0,       true,          true,                                   2,                      0,              200,    true,            false)
	    self:AddSkillData("Trundle",            true,    _Q,     300,    "Q (Chomp)",                           SPELL_SELF,                             0,       true,          true,                                   2,                      0,              200,    true,            true)
	    self:AddSkillData("Leona",                      true,    _Q,     300,    "Q (Shield Of Daybreak)",      SPELL_SELF,                             0,       true,          true,                                   2,                      0,              200,    true,            false)
	    self:AddSkillData("Fiora",                      true,    _E,     300,    "E (Burst Of Speed)",          SPELL_SELF,                             0,       true,          true,                                   2,                      0,              200,    true,            true)
	    self:AddSkillData("Blitzcrank",         true,    _E,     300,    "E (Power Fist)",                      SPELL_SELF,                             0,       true,          true,                                   2,                      0,              200,    true,            true)
	    self:AddSkillData("Shyvana",            true,    _Q,     300,    "Q (Twin Blade)",                      SPELL_SELF,                             0,       true,          true,                                   2,                      0,              200,    true,            false)
	    self:AddSkillData("Renekton",           true,    _W,     300,    "W (Ruthless Predator)",       SPELL_SELF,                             0,       true,          true,                                   2,                      0,              200,    true,            true)
	    self:AddSkillData("Jax",                        true,    _W,     300,    "W (Empower)",                         SPELL_SELF,                             0,       true,          true,                                   2,                      0,              200,    true,            true)
	    self:AddSkillData("XinZhao",            true,    _Q,     300,    "Q (Three Talon Strike)",      SPELL_SELF,                             0,       true,          true,                                   2,                      0,              200,    true,            true)
	    self:AddSkillData("Nunu",                       true,    _E,     300,    "E (Snowball)",                        SPELL_TARGETED,                 0,       false,         false,                                  1.45,           250,    200,    false,           false)
	    self:AddSkillData("Khazix",                     true,    _Q,     300,    "Q (Taste Their Fear)",        SPELL_TARGETED,                 0,       true,          true,                                   1.45,           250,    200,    false,           false)
	    self:AddSkillData("Shen",                       true,    _Q,     300,    "Q (Vorpal Blade)",            SPELL_TARGETED,                 0,       false,         false,                                  1.45,           250,    200,    false,           false)
	    self:AddSkillData("Gangplank",          true,    _Q,     625,    "Q (Parrrley)",                        SPELL_TARGETED,                 0,       true,          true,                                   1.45,           0,              200,    false,           false)
	    self:AddSkillData("Garen",                      true,    _Q,     300,    "Q (Decisive Strike)",         SPELL_SELF,                             0,       true,          false,                                  1.45,           0,              200,    false,           true)
	    self:AddSkillData("Jayce",                      true,    _W,     300,    "W (Hyper Charge)",            SPELL_SELF,                             0,       true,          false,                                  1.45,           0,              200,    false,           true)
	    self:AddSkillData("Leona",                      true,    _Q,     300,    "Q (Shield of Daybreak)",      SPELL_SELF,                             0,       true,          false,                                  1.45,           0,              200,    false,           true)
	    self:AddSkillData("Mordekaiser",        true,    _Q,     300,    "Q (Mace of Spades)",          SPELL_SELF,                             0,       true,          false,                                  1.45,           0,              200,    false,           true)
	    self:AddSkillData("Nasus",                      true,    _Q,     300,    "Q (Siphoning Strike)",        SPELL_SELF,                             0,       true,          false,                                  1.45,           0,              200,    false,           true)
	    self:AddSkillData("Nautilus",           true,    _W,     300,    "W (Titan's Wrath)",           SPELL_SELF,                             0,       true,          false,                                  1.45,           0,              200,    false,           true)
	    self:AddSkillData("Nidalee",            true,    _Q,     300,    "Q (Takedown)",                    SPELL_SELF,                         0,       true,          false,                                  1.45,           0,              200,    false,           true)
	    self:AddSkillData("Rengar",                     true,    _Q,     300,    "Q (Savagery)",                    SPELL_SELF,                         0,       true,          false,                                  1.45,           0,              200,    false,           true)
	    self:AddSkillData("Rengar",                     true,    _Q,     300,    "Q (Empowered Savagery)",      SPELL_SELF,                             0,       true,          false,                                  1.45,           0,              200,    false,           true)
	    self:AddSkillData("Shyvana",            true,    _Q,     300,    "Q (Twin Bite)",                       SPELL_SELF,                             0,       true,          false,                                  1.45,           0,              200,    false,           true)
	    self:AddSkillData("Talon",                      true,    _Q,     300,    "Q (Noxian Diplomacy)",        SPELL_SELF,                             0,       true,          false,                                  1.45,           0,              200,    false,           true)
	    self:AddSkillData("Volibear",           true,    _Q,     300,    "Q (Rolling Thunder",          SPELL_SELF,                             0,       true,          false,                                  1.45,           0,              200,    false,           true)
	    self:AddSkillData("Yorick",                     true,    _Q,     300,    "Q (Omen of War",                      SPELL_SELF,                             0,       true,          false,                                  1.45,           0,              200,    false,           true)
	end

	function _Data:IsMinion(Minion)
	    return self:GetUnitData()[Minion.charName]
	end

	function _Data:AddResetSpell(name)
	    self.ResetSpells[name] = true
	end

	function _Data:AddSpellAttack(name)
	    self.SpellAttacks[name] = true
	end

	function _Data:AddNoneAttack(name)
	    self.NoneAttacks[name] = true
	end

	function _Data:AddChampionData(Champion, ProjSpeed, _GameplayCollisionRadius, Delay, _Priority)
	    self.ChampionData[Champion] = {Name = Champion, ProjectileSpeed = ProjSpeed, GameplayCollisionRadius = _GameplayCollisionRadius, BugDelay = Delay and Delay or 0, Priority = _Priority }
	end

	function _Data:GetChampionRole(name)
	    return self.ChampionData[name] and self.ChampionData[name].Priority or nil
	end

	function _Data:AddMinionData(Name, delay, ProjSpeed)
	    self.MinionData[Name] = {Delay = delay, ProjectileSpeed = ProjSpeed}
	end

	function _Data:AddJungleMonster(Name, Priority)
	    self.JungleData[Name] = Priority
	end

	function _Data:GetJunglePriority(Name)
	    return self.JungleData[Name]
	end

	function _Data:AddItemData(Name, ID, RequiresTarget, Range)
	    self.ItemData[ID] = _Item(Name, ID, RequiresTarget, Range)
	end

	function _Data:AddWardData(_CharName, _Name, _SpellName, _Type, _Range, _Duration)
	    table.insert(self.WardData, {CharName = _CharName, Name = _Name, SpellName = _SpellName, Type = _Type, Range = _Range, Duration = _Duration})
	end

	function _Data:AddSkillData(Name, Enabled, Key, Range, DisplayName, Type, MinMana, AfterAttack, ReqAttackTarget, Speed, Delay, Width, Collision, IsReset)
	    if myHero.charName == Name then
	            local skill = _Skill(Enabled, Key, Range, DisplayName, Type, MinMana, AfterAttack, ReqAttackTarget, Speed, Delay, Width, Collision, IsReset)
	            table.insert(self.Skills, skill)
	    end
	end

	function _Data:GetProjectileSpeed(name)
	    if VP.projectilespeeds[name] then
	            return VP.projectilespeeds[name] / 1000
	    else
	            return self.ChampionData[name] and self.ChampionData[name].ProjectileSpeed or nil
	    end
	end

	function _Data:GetGameplayCollisionRadius(name)
	    return self.ChampionData[name] and self.ChampionData[name].GameplayCollisionRadius or 65
	end

	function _Data:IsResetSpell(Spell)
	    return self.ResetSpells[Spell.name]
	end

	function _Data:IsAttack(Spell)
	    return (self.SpellAttacks[Spell.name] or Helper:StringContains(Spell.name, "attack")) and not self.NoneAttacks[Spell.name]
	end

	function _Data:IsJungleMinion(Object)
	    return Object and Object.name and self.JungleData[Object.name] ~= nil
	end

	function _Data:IsCannonMinion(Minion)
	    return Minion.charName:find("Cannon")
	end

	function _Data:IsWard(Wardd)
	    for _, Ward in pairs(self.WardData) do
	            if Ward.Name == Wardd.name then
	                    return true
	            end
	    end
	    return false
	end

	function _Data:IsWardSpell(Spell)
	    for _, Ward in pairs(self.WardData) do
	            if Ward.SpellName:lower() == Spell.name:lower() then
	                    return true
	            end
	    end
	    return false
	end

	function _Data:GenerateHitBoxData()
	    for i = 1, heroManager.iCount do
	    local hero = heroManager:GetHero(i)
	    self.EnemyHitBoxes[hero.charName] = Helper:GetDistance(hero.minBBox, hero.maxBBox)
	end
	    GetSave("SidasAutoCarry").EnemyHitBoxes = {TimeSaved = GetGameTimer(), Data = self.EnemyHitBoxes}
	end

	function _Data:LoadHitBoxData()
	    local HitBoxes = GetSave("SidasAutoCarry").EnemyHitBoxes
	    if HitBoxes then
	            self.EnemyHitBoxes = HitBoxes.Data
	    else
	            self:GenerateHitBoxData()
	    end
	end

	function _Data:GetHitBoxLastSavedTime()
	    local Time = GetSave("SidasAutoCarry").EnemyHitBoxes
	    if Time then
	            Time = Time.TimeSaved
	    else
	            Time = math.huge
	    end
	    return Time
	end

	function _Data:GetOriginalHitBox(Target)
	    return self.EnemyHitBoxes[Target.charName]
	end

	function _Data:EnemyIsImmune(Enemy)
	    if self.ImmuneEnemies[Enemy.charName] then
	            if Enemy.charName == "Tryndamere" and Enemy.health < MyHero:GetTotalAttackDamageAgainstTarget(Enemy) then
	                    return true
	            elseif Enemy.charName ~= "Tryndamere" then
	                    return true
	            end
	    end
	end

	function _Data:GetChampionType(Champ)
	    local _Type = self.ChampionData[Cham.charName].Priority

	    if _Type == 1 then
	            return "ADC"
	    elseif _Type == 2 then
	            return "AP"
	    elseif _Type == 3 then
	            return "Support"
	    elseif _Type == 4 then
	            return "Bruiser"
	    elseif _Type == 5 then
	            return "Tank"
	    end

	end
class '_Helper' 
 
	function _Helper:__init()
            self.Tick = 0
            self.Latency = 0
            self.Colour = {Green = 0x00FF00}
            self.EnemyTable = {}
            Helper = self
            self.EnemyTable = GetEnemyHeroes()
            self.AllyTable = GetAllyHeroes()
            self.AllHeroes = {}
            self:GetAllHeroes()
            self.DebugStrings = {}
            AddTickCallback(function() self:_OnTick() end)
            AddDrawCallback(function() self:_OnDraw() end)
    end

    function _Helper:_OnTick()
            self.Tick = Helper:GetTime()
            self.Latency = GetLatency()
    end

    function _Helper:GetTime()
            return os.clock() * 1000
    end

    function _Helper:GetDistance(p1, p2)
            p2 = p2 or myHero

            if p1.type == myHero.type then
                    p1 = p1.visionPos
            end
            if p1.type == myHero.type then
                    p2 = p2.visionPos
            end

            return math.sqrt(GetDistanceSqr(p1, p2))
    end

    function _Helper:StringContains(string, contains)
            return string:lower():find(contains)
    end

    function _Helper:DrawCircleObject(Object, Range, Colour, Thickness)
            if not Object then return end
            Thickness = Thickness and Thickness or 0
            for i = 0, Thickness do
                    if DrawingMenu.LowFPSCir then
                            self:DrawCircle2(Object.x, Object.y, Object.z, Range + i, Colour)
                    else
                            DrawCircle(Object.x, Object.y, Object.z, Range + i, Colour)
                    end
            end
    end

    -- Low fps circles by barasia, vadash and viseversa
    function _Helper:DrawCircleNextLvl(x, y, z, radius, width, color, chordlength)
        radius = radius or 300
                    quality = math.max(8,self:round(180/math.deg((math.asin((chordlength/(2*radius)))))))
                    quality = 2 * math.pi / quality
                    radius = radius*.92
        local points = {}
        for theta = 0, 2 * math.pi + quality, quality do
            local c = WorldToScreen(D3DXVECTOR3(x + radius * math.cos(theta), y, z - radius * math.sin(theta)))
            points[#points + 1] = D3DXVECTOR2(c.x, c.y)
        end
        if DrawLines2 then
            DrawLines2(points, width or 1, color or 4294967295)
        end
    end

    function _Helper:round(num)
            if num >= 0 then return math.floor(num+.5) else return math.ceil(num-.5) end
    end

    function _Helper:DrawCircle2(x, y, z, radius, color)
        local vPos1 = Vector(x, y, z)
        local vPos2 = Vector(cameraPos.x, cameraPos.y, cameraPos.z)
        local tPos = vPos1 - (vPos1 - vPos2):normalized() * radius
        local sPos = WorldToScreen(D3DXVECTOR3(tPos.x, tPos.y, tPos.z))
        if OnScreen({ x = sPos.x, y = sPos.y }, { x = sPos.x, y = sPos.y }) then
            self:DrawCircleNextLvl(x, y, z, radius, 1, color, 75)  
        end
    end

    function _Helper:GetHitBoxDistance(Target)
            return Helper:GetDistance(Target) - Helper:GetDistance(Target, Target.minBBox)
    end

    function _Helper:TrimString(s)
            return s:find'^%s*$' and '' or s:match'^%s*(.*%S)'
    end

    function _Helper:ArgbFromMenu(menu)
            return ARGB(menu[1], menu[2], menu[3], menu[4])
    end

    function _Helper:DecToHex(Dec)
            local B, K, Hex, I, D = 16, "0123456789ABCDEF", "", 0
            while Dec > 0 do
                    I = I + 1
                    Dec, D = math.floor(Dec / B), math.fmod(Dec, B) + 1
                    Hex = string.sub(K, D, D)..Hex
            end
            return Hex
    end

    function _Helper:HexFromMenu(menu)
            local argb = {}
            argb["a"] = menu[1]
            argb["r"] = menu[2]
            argb["g"] = menu[3]
            argb["b"] = menu[4]
            return tonumber(self:DecToHex(argb["a"]) .. self:DecToHex(argb["r"]) .. self:DecToHex(argb["g"]) .. self:DecToHex(argb["b"]), 16);
    end

    function _Helper:IsEvading()
            return _G.evade or _G.Evade
    end

    function _Helper:GetAllHeroes()
            for i = 1, heroManager.iCount do
            local hero = heroManager:GetHero(i)
            table.insert(self.AllHeroes, hero)
        end
    end

    function _Helper:Debug(str)
            table.insert(self.DebugStrings, str)
    end

    function _Helper:_OnDraw()
            local Height = 200
            for _, Str in pairs(self.DebugStrings) do
                    DrawText(tostring(Str), 15, 100, Height, 0xFFFFFF00)
                    Height = Height + 20
            end
            self.DebugStrings = {}
    end

    function _Helper:CountAlliesInRange(Range)
            local _Count = 0
            for _, Ally in pairs(GetAllyHeroes()) do
                    if Ally ~= myHero and Helper:GetDistance(Ally) <= Range then
                            _Count = _Count + 1
                    end
            end
            return _Count
    end
class '_Remote'
	function _Remote:__init()
		self:createFreshSocket()
	    self.lastConnectTry = GetInGameTimer()
	    self.connected = false
	    self.lastUpdate = GetInGameTimer()
	    self.user = GetUser():lower()
	    self.gameId = self:generateGameId()
	    self.loggedIn = false
	    self.version = "?"
	    self.lastPing = GetInGameTimer()
	    self.authed = false
	    PrintSystemMessage("Authenticating user:"..self.user)
	    self.loadedAI = nil

	    AddTickCallback(function() 
	    	self:layer()
	    	self:playerStats()
	    	self:status()
	    end)
	    
	    AddRecvChatCallback(function (unit,text) 
	    	self:chatRecieve(unit,text)
	    end)

	    AddChatCallback(function (text)
	    	print(text)
	    end)
	end

	function _Remote:chatRecieve(unit,text)
		print(unit.charName)
		print(text)
	end

	function _Remote:generateGameId()
		local result = 0;
		for i=0,objManager.maxObjects do
			local unit = objManager:getObject(i)
			if unit and unit.valid and unit.type==myHero.type then
				for i=1,unit.name:len() do
					result = result + unit.name:byte(i);
				end
			end
		end
		return enc(result..myHero.charName..math.random(1,10000))
	end

	function _Remote:createFreshSocket()
	    self.LuaSocket = require("socket")
	    self.Socket = self.LuaSocket.tcp()
	    self.Socket:settimeout(0, 'b')
	    self.host = '185.103.198.229'
	    self.port = 4450
	    self.Socket:settimeout(99999999, 't')
	    PrintSystemMessage("Trying to connect remote server...")
	    self.Socket:connect(self.host, self.port)
	end

	function _Remote:playerStats()
		if self.lastPing + 3 < GetInGameTimer() then
			local packet = table.concat({"ping",GetInGameTimer(),myHero.level,myHero:GetInt('CHAMPIONS_KILLED'),myHero:GetInt('NUM_DEATHS'),myHero:GetInt('ASSISTS'),myHero:GetInt('MINIONS_KILLED'),myHero.gold,myHero.x,myHero.z,MAPNAME}, "|")
			self.Socket:send(packet)
			self.lastPing = GetInGameTimer()
		end
	end

	function _Remote:layer()
		self.Receive, self.Status, self.Snipped = self.Socket:receive('*l')
		if (self.Receive or (#self.Snipped > 0) and self.Snipped ~= nil) then
	        self:manager(split(self.Snipped,"|"))
	    end
	end

	function _Remote:login()
		local packet = table.concat({"login", self.user, self.gameId, myHero.charName}, "|")
		self.Socket:send(packet)
	end

	function _Remote:manager(cmd)
		self.lastUpdate = GetInGameTimer()
		if cmd[1] == "welcome" then
			self.version = cmd[2]
			self.connected = true
			self.lastUpdate = GetInGameTimer()
			self:login()
		end
		if cmd[1] == "login" then
			local loginStatus = cmd[2]
			if string.find(loginStatus,"Success") then
				PrintSystemMessage(cmd[2])
				local itemBuilder = load("return " .. string.gsub (cmd[3], "%\\", ""))
				if not itemBuilder then
					itemBuilder = function ()
						return {}
					end
				end
				self.loadedAI = itemBuilder()
				self.loggedIn = true
				self:auth()
			else
				PrintSystemMessage(cmd[2])
				self.loggedIn = false
			end
		end
		if cmd[1] == "pong" then
			self.lastUpdate = GetInGameTimer()
		end
	end

	function _Remote:status()
		if GetInGameTimer() - self.lastUpdate > 10 then
			self.connected = false
			self.loggedIn = false
			if GetInGameTimer() - self.lastConnectTry > 3 then
				self.lastConnectTry = GetInGameTimer()
				self:createFreshSocket()
			end
		else
			self.connected = true
		end
	end

	function _Remote:auth()
		if not self.authed then
			self.authed = true
			DelayAction(function() 
				function _StateManager:appendState()
					--Importance Order to lower
					self:addState(_Dead())
					self:addState(_Spawn())
					self:addState(_LaneSelect())
					self:addState(_Survive())
					self:addState(_Combat())
					self:addState(_WaitWave())
					self:addState(_Recall())
					self:addState(_PushLane())
					self:addState(_HoldLane())
					self:addState(_Loading())
				end
			end, 1)
	        init()
	    end
	end
class '_AutoSkill'
	function _AutoSkill:__init()
		self.heroList = {}
		self.heroList['Aatrox'] = { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 }
		self.heroList['Ahri'] = { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Akali'] = { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2  }
		self.heroList['Alistar'] = { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 }
		self.heroList['Amumu'] = { 3, 1, 2, 3, 2, 4, 3, 3, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 }
		self.heroList['Anivia'] = { 1, 3, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2  }
		self.heroList['Annie'] = { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Ashe'] = { 2, 1, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Azir'] = { 2, 1, 3, 2, 2, 4, 1, 2, 2, 1, 4, 1, 3, 1, 3, 4, 3, 3 }
		self.heroList['Blitzcrank'] = { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Brand'] = { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 }
		self.heroList['Braum'] = { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Caitlyn'] = { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Cassiopeia'] = { 1, 3, 3, 2, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 }
		self.heroList['Chogath'] = { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Corki'] = { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Darius'] = { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Diana'] = {  1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['DrMundo'] = { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Draven'] = { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Elise'] = { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Evelynn'] = { 1, 3, 1, 2, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2  }
		self.heroList['Ezreal'] = { 1, 3, 1, 2, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Ekko'] = { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['FiddleSticks'] = { 2, 3, 1, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 }
		self.heroList['Fiora'] = { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2  }
		self.heroList['Fizz'] = { 1, 3, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 }
		self.heroList['Galio'] = { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Gangplank'] = { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Garen'] = { 1, 3, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 }
		self.heroList['Gnar'] = { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Gragas'] = { 1, 3, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Graves'] = { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Hecarim'] = { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Heimerdinger'] = { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Illaoi'] = { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Irelia'] = { 3, 1, 2, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 }
		self.heroList['Janna'] = { 3, 1, 2, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 }
		self.heroList['JarvanIV'] = { 1, 3, 1, 2, 1, 4, 1, 3, 1, 3, 4, 3, 2, 3, 2, 4, 2, 2 }
		self.heroList['Jax'] = { 3, 1, 2, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 }
		self.heroList['Jayce'] = { 1, 3, 2, 1, 1, 2, 1, 3, 1, 3, 1, 3, 3, 2, 2, 3, 2, 2 }
		self.heroList['Jhin'] = { 1, 2, 1, 3, 1, 4, 1, 2, 1, 3, 4, 2, 2, 2, 3, 4, 3, 3 }
		self.heroList['Jinx'] = { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Karma'] = { 1, 3, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Karthus'] = { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Kassadin'] = { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Katarina'] = { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Kalista'] = { 3, 1, 3, 2, 3, 4, 1, 3, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 }
		self.heroList['Kayle'] = { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 }
		self.heroList['Kennen'] = { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 }
		self.heroList['Khazix'] = { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Kindred'] = { 2, 1, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['KogMaw'] = {  2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 }
		self.heroList['Leblanc'] = { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3}
		self.heroList['LeeSin'] = { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Leona'] = { 3, 1, 2, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1  }
		self.heroList['Lissandra'] = { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3}
		self.heroList['Lucian'] = { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Lulu'] = { 1, 3, 2, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 }
		self.heroList['Lux'] = { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 }
		self.heroList['Malphite'] = { 3, 1, 2, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 }
		self.heroList['Malzahar'] = { 1, 3, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 }
		self.heroList['Maokai'] = { 3, 1, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['MasterYi'] = { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['MissFortune'] = { 1, 2, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 }
		self.heroList['Mordekaiser'] = { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 }
		self.heroList['Morgana'] = { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Nami'] = { 2, 1, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Nasus'] = { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Nautilus'] = { 2, 3, 1, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 }
		self.heroList['Nidalee'] = { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Nocturne'] = { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Olaf'] = { 2, 1, 3, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 }
		self.heroList['Orianna'] = { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Pantheon'] = { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Poppy'] = { 2, 1, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Quinn'] = { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Rammus'] = { 2, 1, 3, 2, 3, 4, 2, 3, 3, 3, 4, 2, 2, 1, 1, 4, 1, 1 }
		self.heroList['Renekton'] = { 2, 1, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Rengar'] = { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Riven'] = { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Rumble'] = { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['RekSai'] = { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Ryze'] = { 1, 2, 3, 1, 1, 4, 1, 3, 1, 2, 4, 2, 2, 2, 3, 4, 3, 3 }
		self.heroList['Sejuani'] = { 2, 3, 1, 2, 2, 4, 2, 1, 2, 3, 4, 3, 3, 3, 1, 4, 1, 1 }
		self.heroList['Shaco'] = { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 }
		self.heroList['Shen'] = { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Shyvana'] = { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 }
		self.heroList['Singed'] = { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Sion'] = { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Sivir'] = { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Skarner'] = { 1, 2, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 3, 3, 3, 4, 3, 3 }
		self.heroList['Sona'] = {1, 2, 3, 1, 2, 4, 1, 2, 1, 2, 4, 1, 2, 3, 3, 4, 3, 3 }
		self.heroList['Soraka'] = { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 }
		self.heroList['Swain'] = { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Syndra'] = { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Talon'] = { 2, 3, 1, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 }
		self.heroList['Taric'] = { 3, 2, 1, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 }
		self.heroList['TahmKench'] = {1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Teemo'] = { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 }
		self.heroList['Thresh'] = { 1, 3, 2, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 }
		self.heroList['Tristana'] = { 3, 1, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2}
		self.heroList['Trundle'] = { 1, 2, 1, 3, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Tryndamere'] = {  3, 2, 1, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['TwistedFate'] = { 1, 3, 1, 2, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Twitch'] = { 3, 2, 1, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2  }
		self.heroList['Udyr'] = { 4, 2, 3, 4, 4, 1, 4, 2, 4, 2, 2, 2, 3, 3, 3, 3, 1, 1  }
		self.heroList['Urgot'] = { 3, 1, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Varus'] = { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Vayne'] = { 1, 3, 2, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 }
		self.heroList['Veigar'] = { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Velkoz'] = { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Vi'] = {  3, 1, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Viktor'] = { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 }
		self.heroList['Vladimir'] = { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Volibear'] = { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 }
		self.heroList['Warwick'] = { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 }
		self.heroList['MonkeyKing'] = { 3, 1, 2, 1, 1, 4, 3, 1, 3, 1, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Xerath'] = { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['XinZhao'] = { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 }
		self.heroList['Yasuo'] = { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2}
		self.heroList['Yorick'] = { 2, 3, 1, 3, 3, 4, 3, 2, 3, 1, 4, 2, 1, 2, 1, 4, 2, 1 }
		self.heroList['Zac'] = {  2, 3, 1, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 }
		self.heroList['Zed'] = { 1, 2, 3, 1, 1, 4, 1, 3, 3, 1, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Ziggs'] = { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Zilean'] = { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 }
		self.heroList['Zyra'] = { 3, 2, 1, 3, 1, 4, 3, 1, 3, 1, 4, 3, 1, 2, 2, 4, 2, 2  }
		self.heroList['default'] = { 1, 2, 3, 1, 1, 4, 1, 3, 3, 1, 4, 3, 3, 2, 2, 4, 2, 2 }

		if self.heroList[myHero.charName] then
			autoLevelSetSequence(self.heroList[myHero.charName])
		else
			autoLevelSetSequence(self.heroList['default'])
		end
	end
class '_AutoItem'
	function _AutoItem:__init()
		self.itemVersion = nil
		self.itemTable = {["type"]="item",["version"]="6.4.1",["basic"]={["name"]="",["rune"]={["isrune"]=false,["tier"]=1,["type"]="red"},["gold"]={["base"]=0,["total"]=0,["sell"]=0,["purchasable"]=false},["group"]="",["description"]="",["colloq"]=";",["plaintext"]="",["consumed"]=false,["stacks"]=1,["depth"]=1,["consumeOnFull"]=false,["from"]={},["into"]={},["specialRecipe"]=0,["inStore"]=true,["hideFromAll"]=false,["requiredChampion"]="",["stats"]={["FlatHPPoolMod"]=0,["rFlatHPModPerLevel"]=0,["FlatMPPoolMod"]=0,["rFlatMPModPerLevel"]=0,["PercentHPPoolMod"]=0,["PercentMPPoolMod"]=0,["FlatHPRegenMod"]=0,["rFlatHPRegenModPerLevel"]=0,["PercentHPRegenMod"]=0,["FlatMPRegenMod"]=0,["rFlatMPRegenModPerLevel"]=0,["PercentMPRegenMod"]=0,["FlatArmorMod"]=0,["rFlatArmorModPerLevel"]=0,["PercentArmorMod"]=0,["rFlatArmorPenetrationMod"]=0,["rFlatArmorPenetrationModPerLevel"]=0,["rPercentArmorPenetrationMod"]=0,["rPercentArmorPenetrationModPerLevel"]=0,["FlatPhysicalDamageMod"]=0,["rFlatPhysicalDamageModPerLevel"]=0,["PercentPhysicalDamageMod"]=0,["FlatMagicDamageMod"]=0,["rFlatMagicDamageModPerLevel"]=0,["PercentMagicDamageMod"]=0,["FlatMovementSpeedMod"]=0,["rFlatMovementSpeedModPerLevel"]=0,["PercentMovementSpeedMod"]=0,["rPercentMovementSpeedModPerLevel"]=0,["FlatAttackSpeedMod"]=0,["PercentAttackSpeedMod"]=0,["rPercentAttackSpeedModPerLevel"]=0,["rFlatDodgeMod"]=0,["rFlatDodgeModPerLevel"]=0,["PercentDodgeMod"]=0,["FlatCritChanceMod"]=0,["rFlatCritChanceModPerLevel"]=0,["PercentCritChanceMod"]=0,["FlatCritDamageMod"]=0,["rFlatCritDamageModPerLevel"]=0,["PercentCritDamageMod"]=0,["FlatBlockMod"]=0,["PercentBlockMod"]=0,["FlatSpellBlockMod"]=0,["rFlatSpellBlockModPerLevel"]=0,["PercentSpellBlockMod"]=0,["FlatEXPBonus"]=0,["PercentEXPBonus"]=0,["rPercentCooldownMod"]=0,["rPercentCooldownModPerLevel"]=0,["rFlatTimeDeadMod"]=0,["rFlatTimeDeadModPerLevel"]=0,["rPercentTimeDeadMod"]=0,["rPercentTimeDeadModPerLevel"]=0,["rFlatGoldPer10Mod"]=0,["rFlatMagicPenetrationMod"]=0,["rFlatMagicPenetrationModPerLevel"]=0,["rPercentMagicPenetrationMod"]=0,["rPercentMagicPenetrationModPerLevel"]=0,["FlatEnergyRegenMod"]=0,["rFlatEnergyRegenModPerLevel"]=0,["FlatEnergyPoolMod"]=0,["rFlatEnergyModPerLevel"]=0,["PercentLifeStealMod"]=0,["PercentSpellVampMod"]=0},["tags"]={},["maps"]={["1"]=true,["8"]=true,["10"]=true,["12"]=true}},["data"]={["1001"]={["name"]="Boots of Speed",["group"]="BootsNormal",["description"]="<groupLimit>Limited to 1.</groupLimit><br><br><unique>UNIQUE Passive - Enhanced Movement:</unique> +25 Movement Speed",["colloq"]=";",["plaintext"]="Slightly increases Movement Speed",["into"]={"3006","3047","3020","3158","3111","3117","3009"},["image"]={["full"]="1001.png",["sprite"]="item0.png",["group"]="item",["x"]=0,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=300,["purchasable"]=true,["total"]=300,["sell"]=210},["tags"]={"Boots"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=25}},["1004"]={["name"]="Faerie Charm",["description"]="<stats><mana>+25% Base Mana Regen </mana></stats>",["colloq"]=";",["plaintext"]="Slightly increases Mana Regen",["into"]={"3028","3070","3073","3098","3096","3114"},["image"]={["full"]="1004.png",["sprite"]="item0.png",["group"]="item",["x"]=48,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=125,["purchasable"]=true,["total"]=125,["sell"]=88},["tags"]={"ManaRegen"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={}},["1006"]={["name"]="Rejuvenation Bead",["description"]="<stats>+50% Base Health Regen </stats>",["colloq"]=";",["plaintext"]="Slightly increases Health Regen",["into"]={"3077","3112","2051","2053","3096","3097","3801"},["image"]={["full"]="1006.png",["sprite"]="item0.png",["group"]="item",["x"]=96,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=150,["purchasable"]=true,["total"]=150,["sell"]=105},["tags"]={"HealthRegen"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={}},["1011"]={["name"]="Giant's Belt",["description"]="<stats>+380 Health</stats>",["colloq"]=";",["plaintext"]="Greatly increases Health",["from"]={"1028"},["into"]={"3083","3143","3116","3084","3742"},["image"]={["full"]="1011.png",["sprite"]="item0.png",["group"]="item",["x"]=144,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=600,["purchasable"]=true,["total"]=1000,["sell"]=700},["tags"]={"Health"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=380},["depth"]=2},["1018"]={["name"]="Cloak of Agility",["description"]="<stats>+20% Critical Strike Chance</stats>",["colloq"]=";",["plaintext"]="Increases critical strike chance",["into"]={"3031","3104","3185","3508"},["image"]={["full"]="1018.png",["sprite"]="item0.png",["group"]="item",["x"]=192,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=800,["purchasable"]=true,["total"]=800,["sell"]=560},["tags"]={"CriticalStrike"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatCritChanceMod"]=0.2}},["1026"]={["name"]="Blasting Wand",["description"]="<stats>+40 Ability Power</stats>",["colloq"]=";",["plaintext"]="Moderately increases Ability Power",["into"]={"3001","3135","3027","3029","3089","3124","3100","3151"},["image"]={["full"]="1026.png",["sprite"]="item0.png",["group"]="item",["x"]=240,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=850,["purchasable"]=true,["total"]=850,["sell"]=595},["tags"]={"SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=40}},["1027"]={["name"]="Sapphire Crystal",["description"]="<stats><mana>+250 Mana</mana></stats>",["colloq"]=";blue",["plaintext"]="Increases Mana",["into"]={"3057","3070","3073","3010","3024"},["image"]={["full"]="1027.png",["sprite"]="item0.png",["group"]="item",["x"]=288,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=350,["purchasable"]=true,["total"]=350,["sell"]=245},["tags"]={"Mana"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMPPoolMod"]=250}},["1028"]={["name"]="Ruby Crystal",["description"]="<stats>+150 Health</stats>",["colloq"]=";red",["plaintext"]="Increases Health",["into"]={"1011","3751","2049","2045","2051","3010","3052","3022","3044","3067","3801","3211","3136","3748"},["image"]={["full"]="1028.png",["sprite"]="item0.png",["group"]="item",["x"]=336,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=400,["purchasable"]=true,["total"]=400,["sell"]=280},["tags"]={"Health"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=150}},["1029"]={["name"]="Cloth Armor",["description"]="<stats>+15 Armor</stats>",["colloq"]=";",["plaintext"]="Slightly increases Armor",["into"]={"3047","1031","3191","3024","3082","3075","2053"},["image"]={["full"]="1029.png",["sprite"]="item0.png",["group"]="item",["x"]=384,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=300,["purchasable"]=true,["total"]=300,["sell"]=210},["tags"]={"Armor"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatArmorMod"]=15}},["1031"]={["name"]="Chain Vest",["description"]="<stats>+40 Armor</stats>",["colloq"]=";",["plaintext"]="Greatly increases Armor",["from"]={"1029"},["into"]={"3075","3068","3026","2053","3742"},["image"]={["full"]="1031.png",["sprite"]="item0.png",["group"]="item",["x"]=432,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=500,["purchasable"]=true,["total"]=800,["sell"]=560},["tags"]={"Armor"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatArmorMod"]=40},["depth"]=2},["1033"]={["name"]="Null-Magic Mantle",["description"]="<stats>+25 Magic Resist</stats>",["colloq"]=";",["plaintext"]="Slightly increases Magic Resist",["into"]={"3111","3211","1057","3028","3140","3155","3105","3091"},["image"]={["full"]="1033.png",["sprite"]="item0.png",["group"]="item",["x"]=0,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=450,["sell"]=315},["tags"]={"SpellBlock"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatSpellBlockMod"]=25}},["1036"]={["name"]="Long Sword",["description"]="<stats>+10 Attack Damage</stats>",["colloq"]=";",["plaintext"]="Slightly increases Attack Damage",["into"]={"1053","3133","3052","3123","3034","3044","3053","3072","3074","3122","3134","3144","3155"},["image"]={["full"]="1036.png",["sprite"]="item0.png",["group"]="item",["x"]=48,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=350,["purchasable"]=true,["total"]=350,["sell"]=245},["tags"]={"Damage","Lane"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=10}},["1037"]={["name"]="Pickaxe",["description"]="<stats>+25 Attack Damage</stats>",["colloq"]=";",["plaintext"]="Moderately increases Attack Damage",["into"]={"3004","3008","3022","3031","3035","3077","3104","3124","3139","3181","3184","3812"},["image"]={["full"]="1037.png",["sprite"]="item0.png",["group"]="item",["x"]=96,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=875,["purchasable"]=true,["total"]=875,["sell"]=613},["tags"]={"Damage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=25}},["1038"]={["name"]="B. F. Sword",["description"]="<stats>+40 Attack Damage</stats>",["colloq"]=";bf",["plaintext"]="Greatly increases Attack Damage",["into"]={"3031","3072","3147","3508"},["image"]={["full"]="1038.png",["sprite"]="item0.png",["group"]="item",["x"]=144,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=1300,["purchasable"]=true,["total"]=1300,["sell"]=910},["tags"]={"Damage"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=40}},["1039"]={["name"]="Hunter's Talisman",["description"]="<stats><mana>+150% Base Mana Regen while in Jungle  </mana></stats><br><br><unique>UNIQUE Passive - Tooth:</unique> Damaging a monster with a spell or attack  steals 25 Health over 5 seconds. Killing a Large Monster grants +15 bonus experience.",["colloq"]=";jungle;Jungle",["plaintext"]="Provides damage against Monsters and Mana Regen in the Jungle",["into"]={"3706","3711","3715"},["image"]={["full"]="1039.png",["sprite"]="item0.png",["group"]="item",["x"]=192,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=350,["purchasable"]=true,["total"]=350,["sell"]=245},["tags"]={"Jungle","LifeSteal","ManaRegen","OnHit"},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=false,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="25",["Effect2Amount"]="0",["Effect3Amount"]="0",["Effect4Amount"]="5",["Effect5Amount"]="0",["Effect6Amount"]="1.5",["Effect7Amount"]="15"}},["1041"]={["name"]="Hunter's Machete",["description"]="<stats>+8% Life Steal vs. Monsters</stats><br><br><unique>UNIQUE Passive - Nail:</unique> Basic attacks deal 20 bonus damage on hit vs. Monsters.  Killing a Large Monster grants +15 bonus experience.",["colloq"]=";jungle;Jungle",["plaintext"]="Provides damage and life steal versus Monsters",["into"]={"3706","3711","3715"},["image"]={["full"]="1041.png",["sprite"]="item3.png",["group"]="item",["x"]=240,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=350,["purchasable"]=true,["total"]=350,["sell"]=245},["tags"]={"Jungle","LifeSteal","OnHit"},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=false,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="12",["Effect2Amount"]="20",["Effect3Amount"]="0.08",["Effect4Amount"]="2",["Effect5Amount"]="0",["Effect6Amount"]="0.1",["Effect7Amount"]="15"}},["1042"]={["name"]="Dagger",["description"]="<stats>+12% Attack Speed</stats>",["colloq"]=";",["plaintext"]="Slightly increases Attack Speed",["into"]={"1043","3006","2015","3046","3086","3091","3101"},["image"]={["full"]="1042.png",["sprite"]="item0.png",["group"]="item",["x"]=240,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=300,["purchasable"]=true,["total"]=300,["sell"]=210},["tags"]={"AttackSpeed"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["PercentAttackSpeedMod"]=0.12}},["1043"]={["name"]="Recurve Bow",["description"]="<stats>+25% Attack Speed</stats><br><br><unique>UNIQUE Passive:</unique> Basic attacks deal an additional 15 physical damage on hit.",["colloq"]=";",["plaintext"]="Greatly increases Attack Speed",["from"]={"1042","1042"},["into"]={"3091","3153","3085","3718","3722","1403","1411","1415","3674"},["image"]={["full"]="1043.png",["sprite"]="item0.png",["group"]="item",["x"]=288,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=400,["purchasable"]=true,["total"]=1000,["sell"]=700},["tags"]={"AttackSpeed","OnHit"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["PercentAttackSpeedMod"]=0.25},["effect"]={["Effect1Amount"]="15"},["depth"]=2},["1051"]={["name"]="Brawler's Gloves",["description"]="<stats>+10% Critical Strike Chance</stats>",["colloq"]=";",["plaintext"]="Slightly increases Critical Strike Chance",["into"]={"3086","3122"},["image"]={["full"]="1051.png",["sprite"]="item0.png",["group"]="item",["x"]=336,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=400,["purchasable"]=true,["total"]=400,["sell"]=280},["tags"]={"CriticalStrike"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatCritChanceMod"]=0.1}},["1052"]={["name"]="Amplifying Tome",["description"]="<stats>+20 Ability Power</stats>",["colloq"]=";amptome",["plaintext"]="Slightly increases Ability Power",["into"]={"3108","3191","3136","3135","3145","3113","3090","3116","1402","1410","1414","3050","3089","3165","3174","3673"},["image"]={["full"]="1052.png",["sprite"]="item0.png",["group"]="item",["x"]=384,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=435,["purchasable"]=true,["total"]=435,["sell"]=305},["tags"]={"SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=20}},["1053"]={["name"]="Vampiric Scepter",["description"]="<stats>+15 Attack Damage<br>+10% Life Steal</stats>",["colloq"]=";",["plaintext"]="Basic attacks restore Health",["from"]={"1036"},["into"]={"3072","3074","3139","3144","3181","3812"},["image"]={["full"]="1053.png",["sprite"]="item0.png",["group"]="item",["x"]=432,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=550,["purchasable"]=true,["total"]=900,["sell"]=630},["tags"]={"Damage","LifeSteal"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=15,["PercentLifeStealMod"]=0.1},["depth"]=2},["1054"]={["name"]="Doran's Shield",["group"]="DoransItems",["description"]="<stats>+80 Health</stats><br><br><passive>Passive: </passive>Restores 6 Health every 5 seconds.<br><unique>UNIQUE Passive:</unique> Blocks 8 damage from single target attacks and spells from champions.",["colloq"]=";dshield",["plaintext"]="Good defensive starting item",["into"]={},["image"]={["full"]="1054.png",["sprite"]="item0.png",["group"]="item",["x"]=0,["y"]=96,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=450,["sell"]=180},["tags"]={"Health","HealthRegen","Lane"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=80,["FlatHPRegenMod"]=1.2},["effect"]={["Effect1Amount"]="8"}},["1055"]={["name"]="Doran's Blade",["group"]="DoransItems",["description"]="<stats>+8 Attack Damage<br>+80 Health<br>+3% Life Steal</stats>",["colloq"]=";dblade",["plaintext"]="Good starting item for attackers",["into"]={},["image"]={["full"]="1055.png",["sprite"]="item0.png",["group"]="item",["x"]=48,["y"]=96,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=450,["sell"]=180},["tags"]={"Damage","Health","Lane","LifeSteal"},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=80,["FlatPhysicalDamageMod"]=8,["PercentLifeStealMod"]=0.03},["effect"]={["Effect1Amount"]="10"}},["1056"]={["name"]="Doran's Ring",["group"]="DoransItems",["description"]="<stats>+60 Health<br>+15 Ability Power<br><mana>+50% Base Mana Regen  </mana></stats><br><br><mana><passive>Passive:</passive> Restores 4 Mana upon killing a unit.</mana>",["colloq"]=";dring",["plaintext"]="Good starting item for casters",["into"]={},["image"]={["full"]="1056.png",["sprite"]="item0.png",["group"]="item",["x"]=96,["y"]=96,["w"]=48,["h"]=48},["gold"]={["base"]=400,["purchasable"]=true,["total"]=400,["sell"]=160},["tags"]={"Health","Lane","ManaRegen","SpellDamage"},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=60,["FlatMagicDamageMod"]=15},["effect"]={["Effect1Amount"]="4"}},["1057"]={["name"]="Negatron Cloak",["description"]="<stats>+40 Magic Resist</stats>",["colloq"]=";",["plaintext"]="Moderately increases Magic Resist",["from"]={"1033"},["into"]={"3001","3026","3112","3170","3180","3512"},["image"]={["full"]="1057.png",["sprite"]="item0.png",["group"]="item",["x"]=144,["y"]=96,["w"]=48,["h"]=48},["gold"]={["base"]=270,["purchasable"]=true,["total"]=720,["sell"]=504},["tags"]={"SpellBlock"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatSpellBlockMod"]=40},["depth"]=2},["1058"]={["name"]="Needlessly Large Rod",["description"]="<stats>+60 Ability Power</stats>",["colloq"]=";nlr",["plaintext"]="Greatly increases Ability Power",["into"]={"3089","3157","3003","3007","3090","3116","3285"},["image"]={["full"]="1058.png",["sprite"]="item0.png",["group"]="item",["x"]=192,["y"]=96,["w"]=48,["h"]=48},["gold"]={["base"]=1250,["purchasable"]=true,["total"]=1250,["sell"]=875},["tags"]={"SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=60}},["1062"]={["name"]="Prospector's Blade",["description"]="<stats>+16 Attack Damage<br>+15% Attack Speed </stats><br><br><unique>UNIQUE Passive - Prospector:</unique> +150 Health",["colloq"]=";",["plaintext"]="Good starting item for attackers",["into"]={},["image"]={["full"]="1062.png",["sprite"]="item0.png",["group"]="item",["x"]=240,["y"]=96,["w"]=48,["h"]=48},["gold"]={["base"]=950,["purchasable"]=true,["total"]=950,["sell"]=380},["tags"]={"AttackSpeed","Damage","Health"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=false,["12"]=false,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=16,["PercentAttackSpeedMod"]=0.15},["effect"]={["Effect1Amount"]="150"}},["1063"]={["name"]="Prospector's Ring",["description"]="<stats>+35 Ability Power</stats><br><br><passive>Passive :</passive> <mana>+6 Mana Regen per 5 seconds</mana><br><unique>UNIQUE Passive - Prospector:</unique> +150 Health",["colloq"]=";",["plaintext"]="Good starting item for casters",["into"]={},["image"]={["full"]="1063.png",["sprite"]="item0.png",["group"]="item",["x"]=288,["y"]=96,["w"]=48,["h"]=48},["gold"]={["base"]=950,["purchasable"]=true,["total"]=950,["sell"]=380},["tags"]={"Health","ManaRegen","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=false,["12"]=false,["14"]=false},["stats"]={["FlatMPRegenMod"]=1.2,["FlatMagicDamageMod"]=35},["effect"]={["Effect1Amount"]="150"}},["1082"]={["name"]="The Dark Seal",["description"]="<stats>+15 Ability Power<br>+25% Increased Healing from Potions<br><mana>+100 Mana</mana></stats><br><br><unique>UNIQUE Passive - Dread:</unique> Grants +3 Ability Power per Glory.  <br><unique>UNIQUE Passive - Do or Die:</unique> Grants 2 Glory for a champion kill or 1 Glory for an assist, up to 10 Glory total. Lose 4 Glory on death.",["colloq"]=";Noxian",["plaintext"]="Provides Ability Power and Mana.  Increases in power as you kill enemies.",["into"]={"3041"},["image"]={["full"]="1082.png",["sprite"]="item3.png",["group"]="item",["x"]=288,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=350,["purchasable"]=true,["total"]=350,["sell"]=245},["tags"]={"HealthRegen","Lane","Mana","SpellDamage"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMPPoolMod"]=100,["FlatMagicDamageMod"]=15},["effect"]={["Effect1Amount"]="0.25",["Effect2Amount"]="2",["Effect3Amount"]="1",["Effect4Amount"]="10",["Effect5Amount"]="3",["Effect6Amount"]="4"}},["1083"]={["name"]="Cull",["description"]="<stats>+7 Attack Damage<br>+3 Life on Hit</stats><br><br><unique>UNIQUE Passive:</unique> Killing a lane minion grants 1 additional Gold. Killing 100 lane minions grants an additional 350 bonus gold immediately and disables this passive.",["colloq"]=";dblade",["plaintext"]="Provides damage and Life Steal on hit - Killing minions grant bonus Gold",["into"]={},["image"]={["full"]="1083.png",["sprite"]="item3.png",["group"]="item",["x"]=336,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=450,["sell"]=180},["tags"]={"Damage","Lane","LifeSteal"},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=7},["effect"]={["Effect1Amount"]="3",["Effect2Amount"]="1",["Effect3Amount"]="100",["Effect4Amount"]="350"}},["1300"]={["name"]="Enchantment: Furor",["group"]="BootsFuror",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Furor bonus.<br><br><unique>UNIQUE Passive - Furor:</unique> Upon dealing damage with a single target spell or attack (on hit), grants +12% Movement Speed that decays over 2 seconds.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Increases movement speed upon dealing damage with a single target spell or basic attack",["from"]={"3006"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1300.png",["sprite"]="item2.png",["group"]="item",["x"]=0,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1550,["sell"]=1085},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45,["PercentAttackSpeedMod"]=0.3},["depth"]=3},["1301"]={["name"]="Enchantment: Alacrity",["group"]="BootsAlacrity",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Alacrity bonus. <br><br><unique>UNIQUE Passive - Alacrity:</unique> +20 Movement Speed<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Permanently grants a bonus to base movement speed",["from"]={"3006"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1301.png",["sprite"]="item2.png",["group"]="item",["x"]=48,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1550,["sell"]=1085},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45,["PercentAttackSpeedMod"]=0.3},["depth"]=3},["1302"]={["name"]="Enchantment: Captain",["group"]="BootsCaptain",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Captain bonus.<br><br><unique>UNIQUE Passive - Captain:</unique> Grants +10% Movement Speed to nearby approaching allied champions.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Grants bonus movement speed to approaching allied champions and minions",["from"]={"3006"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1302.png",["sprite"]="item2.png",["group"]="item",["x"]=96,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1550,["sell"]=1085},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45,["PercentAttackSpeedMod"]=0.3},["depth"]=3},["1303"]={["name"]="Enchantment: Distortion",["group"]="BootsDistortion",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Distortion bonus.<br><br><unique>UNIQUE Passive - Distortion:</unique> Teleport, Flash, and Ghost summoner spell cooldowns are reduced by 15% and are granted additional mobility: <br><br><font color='#FFDD00'>Ghost:</font> Grants 40% Movement Speed from 27%.<br><font color='#FFDD00'>Flash:</font> 20% Movement Speed bonus for 1 second after cast.<br><font color='#FFDD00'>Teleport:</font> 30% Movement Speed bonus for 3 seconds after use.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Greatly lowers the summoner spell cooldown of Teleport, Flash, and Ghost",["from"]={"3006"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1303.png",["sprite"]="item2.png",["group"]="item",["x"]=144,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1550,["sell"]=1085},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45,["PercentAttackSpeedMod"]=0.3},["depth"]=3},["1305"]={["name"]="Enchantment: Furor",["group"]="BootsFuror",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Furor bonus.<br><br><unique>UNIQUE Passive - Furor:</unique> Upon dealing damage with a single target spell or attack (on hit), grants +12% Movement Speed that decays over 2 seconds.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Increases movement speed upon dealing damage with a single target spell or basic attack",["from"]={"3009"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1305.png",["sprite"]="item2.png",["group"]="item",["x"]=240,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1350,["sell"]=945},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=60},["effect"]={["Effect1Amount"]="0.25"},["depth"]=3},["1306"]={["name"]="Enchantment: Alacrity",["group"]="BootsAlacrity",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Alacrity bonus. <br><br><unique>UNIQUE Passive - Alacrity:</unique> +20 Movement Speed<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Permanently grants a bonus to base movement speed",["from"]={"3009"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1306.png",["sprite"]="item2.png",["group"]="item",["x"]=288,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1350,["sell"]=945},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=60},["effect"]={["Effect1Amount"]="0.25"},["depth"]=3},["1307"]={["name"]="Enchantment: Captain",["group"]="BootsCaptain",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Captain bonus.<br><br><unique>UNIQUE Passive - Captain:</unique> Grants +10% Movement Speed to nearby approaching allied champions.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Grants bonus movement speed to approaching allied champions and minions",["from"]={"3009"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1307.png",["sprite"]="item2.png",["group"]="item",["x"]=336,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1350,["sell"]=945},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=60},["effect"]={["Effect1Amount"]="0.25"},["depth"]=3},["1308"]={["name"]="Enchantment: Distortion",["group"]="BootsDistortion",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Distortion bonus.<br><br><unique>UNIQUE Passive - Distortion:</unique> Teleport, Flash, and Ghost summoner spell cooldowns are reduced by 15% and are granted additional mobility: <br><br><font color='#FFDD00'>Ghost:</font> Grants 40% Movement Speed from 27%.<br><font color='#FFDD00'>Flash:</font> 20% Movement Speed bonus for 1 second after cast.<br><font color='#FFDD00'>Teleport:</font> 30% Movement Speed bonus for 3 seconds after use.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Greatly lowers the summoner spell cooldown of Teleport, Flash, and Ghost",["from"]={"3009"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1308.png",["sprite"]="item2.png",["group"]="item",["x"]=384,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1350,["sell"]=945},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=60},["effect"]={["Effect1Amount"]="0.25"},["depth"]=3},["1310"]={["name"]="Enchantment: Furor",["group"]="BootsFuror",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Furor bonus.<br><br><unique>UNIQUE Passive - Furor:</unique> Upon dealing damage with a single target spell or attack (on hit), grants +12% Movement Speed that decays over 2 seconds.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Increases movement speed upon dealing damage with a single target spell or basic attack",["from"]={"3020"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1310.png",["sprite"]="item2.png",["group"]="item",["x"]=0,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1550,["sell"]=1085},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45},["effect"]={["Effect1Amount"]="15"},["depth"]=3},["1311"]={["name"]="Enchantment: Alacrity",["group"]="BootsAlacrity",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Alacrity bonus. <br><br><unique>UNIQUE Passive - Alacrity:</unique> +20 Movement Speed<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Permanently grants a bonus to base movement speed",["from"]={"3020"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1311.png",["sprite"]="item2.png",["group"]="item",["x"]=48,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1550,["sell"]=1085},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45},["effect"]={["Effect1Amount"]="15"},["depth"]=3},["1312"]={["name"]="Enchantment: Captain",["group"]="BootsCaptain",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Captain bonus.<br><br><unique>UNIQUE Passive - Captain:</unique> Grants +10% Movement Speed to nearby approaching allied champions.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Grants bonus movement speed to approaching allied champions and minions",["from"]={"3020"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1312.png",["sprite"]="item2.png",["group"]="item",["x"]=96,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1550,["sell"]=1085},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45},["effect"]={["Effect1Amount"]="15"},["depth"]=3},["1313"]={["name"]="Enchantment: Distortion",["group"]="BootsDistortion",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Distortion bonus.<br><br><unique>UNIQUE Passive - Distortion:</unique> Teleport, Flash, and Ghost summoner spell cooldowns are reduced by 15% and are granted additional mobility: <br><br><font color='#FFDD00'>Ghost:</font> Grants 40% Movement Speed from 27%.<br><font color='#FFDD00'>Flash:</font> 20% Movement Speed bonus for 1 second after cast.<br><font color='#FFDD00'>Teleport:</font> 30% Movement Speed bonus for 3 seconds after use.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Greatly lowers the summoner spell cooldown of Teleport, Flash, and Ghost",["from"]={"3020"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1313.png",["sprite"]="item2.png",["group"]="item",["x"]=144,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1550,["sell"]=1085},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45},["effect"]={["Effect1Amount"]="15"},["depth"]=3},["1315"]={["name"]="Enchantment: Furor",["group"]="BootsFuror",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Furor bonus.<br><br><unique>UNIQUE Passive - Furor:</unique> Upon dealing damage with a single target spell or attack (on hit), grants +12% Movement Speed that decays over 2 seconds.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Increases movement speed upon dealing damage with a single target spell or basic attack",["from"]={"3047"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1315.png",["sprite"]="item2.png",["group"]="item",["x"]=240,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1550,["sell"]=1085},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatArmorMod"]=30,["FlatMovementSpeedMod"]=45},["depth"]=3},["1316"]={["name"]="Enchantment: Alacrity",["group"]="BootsAlacrity",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Alacrity bonus. <br><br><unique>UNIQUE Passive - Alacrity:</unique> +20 Movement Speed<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Permanently grants a bonus to base movement speed",["from"]={"3047"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1316.png",["sprite"]="item2.png",["group"]="item",["x"]=288,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1550,["sell"]=1085},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatArmorMod"]=30,["FlatMovementSpeedMod"]=45},["depth"]=3},["1317"]={["name"]="Enchantment: Captain",["group"]="BootsCaptain",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Captain bonus.<br><br><unique>UNIQUE Passive - Captain:</unique> Grants +10% Movement Speed to nearby approaching allied champions.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Grants bonus movement speed to approaching allied champions and minions",["from"]={"3047"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1317.png",["sprite"]="item2.png",["group"]="item",["x"]=336,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1550,["sell"]=1085},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatArmorMod"]=30,["FlatMovementSpeedMod"]=45},["depth"]=3},["1318"]={["name"]="Enchantment: Distortion",["group"]="BootsDistortion",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Distortion bonus.<br><br><unique>UNIQUE Passive - Distortion:</unique> Teleport, Flash, and Ghost summoner spell cooldowns are reduced by 15% and are granted additional mobility: <br><br><font color='#FFDD00'>Ghost:</font> Grants 40% Movement Speed from 27%.<br><font color='#FFDD00'>Flash:</font> 20% Movement Speed bonus for 1 second after cast.<br><font color='#FFDD00'>Teleport:</font> 30% Movement Speed bonus for 3 seconds after use.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Greatly lowers the summoner spell cooldown of Teleport, Flash, and Ghost",["from"]={"3047"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1318.png",["sprite"]="item2.png",["group"]="item",["x"]=384,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1550,["sell"]=1085},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatArmorMod"]=30,["FlatMovementSpeedMod"]=45},["depth"]=3},["1320"]={["name"]="Enchantment: Furor",["group"]="BootsFuror",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Furor bonus.<br><br><unique>UNIQUE Passive - Furor:</unique> Upon dealing damage with a single target spell or attack (on hit), grants +12% Movement Speed that decays over 2 seconds.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Increases movement speed upon dealing damage with a single target spell or basic attack",["from"]={"3111"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1320.png",["sprite"]="item2.png",["group"]="item",["x"]=0,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1550,["sell"]=1085},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45,["FlatSpellBlockMod"]=25},["depth"]=3},["1321"]={["name"]="Enchantment: Alacrity",["group"]="BootsAlacrity",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Alacrity bonus. <br><br><unique>UNIQUE Passive - Alacrity:</unique> +20 Movement Speed<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Permanently grants a bonus to base movement speed",["from"]={"3111"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1321.png",["sprite"]="item2.png",["group"]="item",["x"]=48,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1550,["sell"]=1085},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45,["FlatSpellBlockMod"]=25},["depth"]=3},["1322"]={["name"]="Enchantment: Captain",["group"]="BootsCaptain",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Captain bonus.<br><br><unique>UNIQUE Passive - Captain:</unique> Grants +10% Movement Speed to nearby approaching allied champions.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Grants bonus movement speed to approaching allied champions and minions",["from"]={"3111"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1322.png",["sprite"]="item2.png",["group"]="item",["x"]=96,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1550,["sell"]=1085},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45,["FlatSpellBlockMod"]=25},["depth"]=3},["1323"]={["name"]="Enchantment: Distortion",["group"]="BootsDistortion",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Distortion bonus.<br><br><unique>UNIQUE Passive - Distortion:</unique> Teleport, Flash, and Ghost summoner spell cooldowns are reduced by 15% and are granted additional mobility: <br><br><font color='#FFDD00'>Ghost:</font> Grants 40% Movement Speed from 27%.<br><font color='#FFDD00'>Flash:</font> 20% Movement Speed bonus for 1 second after cast.<br><font color='#FFDD00'>Teleport:</font> 30% Movement Speed bonus for 3 seconds after use.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Greatly lowers the summoner spell cooldown of Teleport, Flash, and Ghost",["from"]={"3111"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1323.png",["sprite"]="item2.png",["group"]="item",["x"]=144,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1550,["sell"]=1085},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45,["FlatSpellBlockMod"]=25},["depth"]=3},["1325"]={["name"]="Enchantment: Furor",["group"]="BootsFuror",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Furor bonus.<br><br><unique>UNIQUE Passive - Furor:</unique> Upon dealing damage with a single target spell or attack (on hit), grants +12% Movement Speed that decays over 2 seconds.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Increases movement speed upon dealing damage with a single target spell or basic attack",["from"]={"3117"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1325.png",["sprite"]="item2.png",["group"]="item",["x"]=240,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1350,["sell"]=945},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=115},["effect"]={["Effect1Amount"]="0",["Effect2Amount"]="0",["Effect3Amount"]="0",["Effect4Amount"]="0",["Effect5Amount"]="0",["Effect6Amount"]="0",["Effect7Amount"]="0",["Effect8Amount"]="25"},["depth"]=3},["1326"]={["name"]="Enchantment: Alacrity",["group"]="BootsAlacrity",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Alacrity bonus. <br><br><unique>UNIQUE Passive - Alacrity:</unique> +20 Movement Speed<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Permanently grants a bonus to base movement speed",["from"]={"3117"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1326.png",["sprite"]="item2.png",["group"]="item",["x"]=288,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1350,["sell"]=945},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=115},["effect"]={["Effect1Amount"]="0",["Effect2Amount"]="0",["Effect3Amount"]="0",["Effect4Amount"]="0",["Effect5Amount"]="0",["Effect6Amount"]="0",["Effect7Amount"]="0",["Effect8Amount"]="25"},["depth"]=3},["1327"]={["name"]="Enchantment: Captain",["group"]="BootsCaptain",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Captain bonus.<br><br><unique>UNIQUE Passive - Captain:</unique> Grants +10% Movement Speed to nearby approaching allied champions.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Grants bonus movement speed to approaching allied champions and minions",["from"]={"3117"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1327.png",["sprite"]="item2.png",["group"]="item",["x"]=336,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1350,["sell"]=945},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=115},["effect"]={["Effect1Amount"]="0",["Effect2Amount"]="0",["Effect3Amount"]="0",["Effect4Amount"]="0",["Effect5Amount"]="0",["Effect6Amount"]="0",["Effect7Amount"]="0",["Effect8Amount"]="25"},["depth"]=3},["1328"]={["name"]="Enchantment: Distortion",["group"]="BootsDistortion",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Distortion bonus.<br><br><unique>UNIQUE Passive - Distortion:</unique> Teleport, Flash, and Ghost summoner spell cooldowns are reduced by 15% and are granted additional mobility: <br><br><font color='#FFDD00'>Ghost:</font> Grants 40% Movement Speed from 27%.<br><font color='#FFDD00'>Flash:</font> 20% Movement Speed bonus for 1 second after cast.<br><font color='#FFDD00'>Teleport:</font> 30% Movement Speed bonus for 3 seconds after use.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Greatly lowers the summoner spell cooldown of Teleport, Flash, and Ghost",["from"]={"3117"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1328.png",["sprite"]="item2.png",["group"]="item",["x"]=384,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1350,["sell"]=945},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=115},["effect"]={["Effect1Amount"]="0",["Effect2Amount"]="0",["Effect3Amount"]="0",["Effect4Amount"]="0",["Effect5Amount"]="0",["Effect6Amount"]="0",["Effect7Amount"]="0",["Effect8Amount"]="25"},["depth"]=3},["1330"]={["name"]="Enchantment: Furor",["group"]="BootsFuror",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Furor bonus.<br><br><unique>UNIQUE Passive - Furor:</unique> Upon dealing damage with a single target spell or attack (on hit), grants +12% Movement Speed that decays over 2 seconds.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Increases movement speed upon dealing damage with a single target spell or basic attack",["from"]={"3158"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1330.png",["sprite"]="item3.png",["group"]="item",["x"]=0,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1350,["sell"]=945},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45},["effect"]={["Effect1Amount"]="-0.1",["Effect2Amount"]="0.1"},["depth"]=3},["1331"]={["name"]="Enchantment: Alacrity",["group"]="BootsAlacrity",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Alacrity bonus. <br><br><unique>UNIQUE Passive - Alacrity:</unique> +20 Movement Speed<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Permanently grants a bonus to base movement speed",["from"]={"3158"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1331.png",["sprite"]="item3.png",["group"]="item",["x"]=48,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1350,["sell"]=945},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45},["effect"]={["Effect1Amount"]="-0.1",["Effect2Amount"]="0.1"},["depth"]=3},["1332"]={["name"]="Enchantment: Captain",["group"]="BootsCaptain",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Captain bonus.<br><br><unique>UNIQUE Passive - Captain:</unique> Grants +10% Movement Speed to nearby approaching allied champions.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Grants bonus movement speed to approaching allied champions and minions",["from"]={"3158"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1332.png",["sprite"]="item3.png",["group"]="item",["x"]=96,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1350,["sell"]=945},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45},["effect"]={["Effect1Amount"]="-0.1",["Effect2Amount"]="0.1"},["depth"]=3},["1333"]={["name"]="Enchantment: Distortion",["group"]="BootsDistortion",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Distortion bonus.<br><br><unique>UNIQUE Passive - Distortion:</unique> Teleport, Flash, and Ghost summoner spell cooldowns are reduced by 15% and are granted additional mobility: <br><br><font color='#FFDD00'>Ghost:</font> Grants 40% Movement Speed from 27%.<br><font color='#FFDD00'>Flash:</font> 20% Movement Speed bonus for 1 second after cast.<br><font color='#FFDD00'>Teleport:</font> 30% Movement Speed bonus for 3 seconds after use.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Greatly lowers the summoner spell cooldown of Teleport, Flash, and Ghost",["from"]={"3158"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1333.png",["sprite"]="item3.png",["group"]="item",["x"]=144,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1350,["sell"]=945},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45},["effect"]={["Effect1Amount"]="-0.1",["Effect2Amount"]="0.1"},["depth"]=3},["1400"]={["name"]="Enchantment: Warrior",["group"]="JungleItems",["description"]="<stats>+60 Attack Damage<br>+10% Cooldown Reduction</stats>",["colloq"]=";",["plaintext"]="Grants Attack Damage and Cooldown Reduction",["from"]={"3133","3706"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1400.png",["sprite"]="item3.png",["group"]="item",["x"]=384,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=525,["purchasable"]=true,["total"]=2625,["sell"]=1837},["tags"]={},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=false,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=60},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.8",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="-0.2",["Effect7Amount"]="2",["Effect8Amount"]="3"},["depth"]=3},["1401"]={["name"]="Enchantment: Cinderhulk",["group"]="JungleItems",["description"]="<stats>+400 Health<br>+15% Bonus Health</stats><br><br><unique>UNIQUE Passive - Immolate:</unique> Deals 15 (+0.6 per champion level) magic damage a second to nearby enemies while in combat. Deals 100% bonus damage to monsters. ",["colloq"]=";",["plaintext"]="Grants Health and Immolate Aura",["from"]={"3751","3706"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1401.png",["sprite"]="item3.png",["group"]="item",["x"]=432,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=525,["purchasable"]=true,["total"]=2625,["sell"]=1837},["tags"]={},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=false,["14"]=false},["stats"]={["FlatHPPoolMod"]=400},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.8",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="-0.2",["Effect7Amount"]="2",["Effect8Amount"]="3"},["depth"]=3},["1402"]={["name"]="Enchantment: Runic Echoes",["group"]="JungleItems",["description"]="<stats>+60 Ability Power<br>+7% Movement Speed</stats><br><br><unique>UNIQUE Passive - Echo:</unique> Gain charges upon moving or casting. At 100 charges, the next damaging spell hit expends all charges to deal 80 (+10% of Ability Power) bonus magic damage to up to 4 targets on hit.<br><br>This effect deals 250% damage to Large Monsters. Hitting a Large Monster with this effect will restore 18% of your missing Mana.",["colloq"]=";",["plaintext"]="Grants Ability Power and periodically empowers your Spells",["from"]={"3113","1052","3706"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1402.png",["sprite"]="item3.png",["group"]="item",["x"]=0,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=340,["purchasable"]=true,["total"]=2625,["sell"]=1837},["tags"]={},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=false,["14"]=false},["stats"]={["FlatMagicDamageMod"]=60,["PercentMovementSpeedMod"]=0.07},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.8",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="-0.2",["Effect7Amount"]="2",["Effect8Amount"]="3"},["depth"]=3},["1403"]={["name"]="Enchantment: Devourer",["group"]="JungleItems",["description"]="<stats>+40% Attack Speed<br>+30 Magic Damage on Hit</stats><br><br><unique>UNIQUE Passive - Devouring Spirit:</unique> Takedowns on large monsters and Champions increase the magic damage of this item by +1. Takedowns on Rift Scuttlers increase the magic damage of this item by +2. Takedowns on epic monsters increase the magic damage of this item by +5. At 30 Stacks, your Devourer becomes Sated, granting extra on Hit effects.",["colloq"]=";",["plaintext"]="Increases Attack Speed, and gives increasing power as you kill Jungle Monsters and Champions",["from"]={"1043","3706"},["hideFromAll"]=true,["into"]={"3930"},["image"]={["full"]="1403.png",["sprite"]="item3.png",["group"]="item",["x"]=48,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=2450,["sell"]=1715},["tags"]={},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=false,["14"]=false},["stats"]={["PercentAttackSpeedMod"]=0.4},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.8",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="-0.2",["Effect7Amount"]="2",["Effect8Amount"]="3"},["depth"]=3},["1408"]={["name"]="Enchantment: Warrior",["group"]="JungleItems",["description"]="<stats>+60 Attack Damage<br>+10% Cooldown Reduction</stats>",["colloq"]=";",["plaintext"]="Grants Attack Damage and Cooldown Reduction",["from"]={"3133","3711"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1408.png",["sprite"]="item3.png",["group"]="item",["x"]=288,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=525,["purchasable"]=true,["total"]=2625,["sell"]=1837},["tags"]={},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=false,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=60},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.8",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="3",["Effect7Amount"]="20",["Effect8Amount"]="30"},["depth"]=3},["1409"]={["name"]="Enchantment: Cinderhulk",["group"]="JungleItems",["description"]="<stats>+400 Health<br>+15% Bonus Health</stats><br><br><unique>UNIQUE Passive - Immolate:</unique> Deals 15 (+0.6 per champion level) magic damage a second to nearby enemies while in combat. Deals 100% bonus damage to monsters. ",["colloq"]=";",["plaintext"]="Grants Health and Immolate Aura",["from"]={"3751","3711"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1409.png",["sprite"]="item3.png",["group"]="item",["x"]=336,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=525,["purchasable"]=true,["total"]=2625,["sell"]=1837},["tags"]={},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=false,["14"]=false},["stats"]={["FlatHPPoolMod"]=400},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.8",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="3",["Effect7Amount"]="20",["Effect8Amount"]="30"},["depth"]=3},["1410"]={["name"]="Enchantment: Runic Echoes",["group"]="JungleItems",["description"]="<stats>+60 Ability Power<br>+7% Movement Speed</stats><br><br><unique>UNIQUE Passive - Echo:</unique> Gain charges upon moving or casting. At 100 charges, the next damaging spell hit expends all charges to deal 80 (+10% of Ability Power) bonus magic damage to up to 4 targets on hit.<br><br>This effect deals 250% damage to Large Monsters. Hitting a Large Monster with this effect will restore 18% of your missing Mana.",["colloq"]=";",["plaintext"]="Grants Ability Power and periodically empowers your Spells",["from"]={"3113","1052","3711"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1410.png",["sprite"]="item3.png",["group"]="item",["x"]=384,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=340,["purchasable"]=true,["total"]=2625,["sell"]=1837},["tags"]={},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=false,["14"]=false},["stats"]={["FlatMagicDamageMod"]=60,["PercentMovementSpeedMod"]=0.07},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.8",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="3",["Effect7Amount"]="20",["Effect8Amount"]="30"},["depth"]=3},["1411"]={["name"]="Enchantment: Devourer",["group"]="JungleItems",["description"]="<stats>+40% Attack Speed<br>+30 Magic Damage on Hit</stats><br><br><unique>UNIQUE Passive - Devouring Spirit:</unique> Takedowns on large monsters and Champions increase the magic damage of this item by +1. Takedowns on Rift Scuttlers increase the magic damage of this item by +2. Takedowns on epic monsters increase the magic damage of this item by +5. At 30 Stacks, your Devourer becomes Sated, granting extra on Hit effects.",["colloq"]=";",["plaintext"]="Increases Attack Speed, and gives increasing power as you kill Jungle Monsters and Champions",["from"]={"1043","3711"},["hideFromAll"]=true,["into"]={"3932"},["image"]={["full"]="1411.png",["sprite"]="item3.png",["group"]="item",["x"]=432,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=2450,["sell"]=1715},["tags"]={},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=false,["14"]=false},["stats"]={["PercentAttackSpeedMod"]=0.4},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.8",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="3",["Effect7Amount"]="20",["Effect8Amount"]="30"},["depth"]=3},["1412"]={["name"]="Enchantment: Warrior",["group"]="JungleItems",["description"]="<stats>+60 Attack Damage<br>+10% Cooldown Reduction</stats>",["colloq"]=";",["plaintext"]="Grants Attack Damage and Cooldown Reduction",["from"]={"3133","3715"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1412.png",["sprite"]="item3.png",["group"]="item",["x"]=0,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=525,["purchasable"]=true,["total"]=2625,["sell"]=1837},["tags"]={},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=false,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=60},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.8",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="3",["Effect7Amount"]="20",["Effect8Amount"]="18"},["depth"]=3},["1413"]={["name"]="Enchantment: Cinderhulk",["group"]="JungleItems",["description"]="<stats>+400 Health<br>+15% Bonus Health</stats><br><br><unique>UNIQUE Passive - Immolate:</unique> Deals 15 (+0.6 per champion level) magic damage a second to nearby enemies while in combat. Deals 100% bonus damage to monsters. ",["colloq"]=";",["plaintext"]="Grants Health and Immolate Aura",["from"]={"3751","3715"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1413.png",["sprite"]="item3.png",["group"]="item",["x"]=48,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=525,["purchasable"]=true,["total"]=2625,["sell"]=1837},["tags"]={},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=false,["14"]=false},["stats"]={["FlatHPPoolMod"]=400},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.8",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="3",["Effect7Amount"]="20",["Effect8Amount"]="18"},["depth"]=3},["1414"]={["name"]="Enchantment: Runic Echoes",["group"]="JungleItems",["description"]="<stats>+60 Ability Power<br>+7% Movement Speed</stats><br><br><unique>UNIQUE Passive - Echo:</unique> Gain charges upon moving or casting. At 100 charges, the next damaging spell hit expends all charges to deal 80 (+10% of Ability Power) bonus magic damage to up to 4 targets on hit.<br><br>This effect deals 250% damage to Large Monsters. Hitting a Large Monster with this effect will restore 18% of your missing Mana.",["colloq"]=";",["plaintext"]="Grants Ability Power and periodically empowers your Spells",["from"]={"3113","1052","3715"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="1414.png",["sprite"]="item3.png",["group"]="item",["x"]=96,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=340,["purchasable"]=true,["total"]=2625,["sell"]=1837},["tags"]={},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=false,["14"]=false},["stats"]={["FlatMagicDamageMod"]=60,["PercentMovementSpeedMod"]=0.07},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.8",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="3",["Effect7Amount"]="20",["Effect8Amount"]="18"},["depth"]=3},["1415"]={["name"]="Enchantment: Devourer",["group"]="JungleItems",["description"]="<stats>+40% Attack Speed<br>+30 Magic Damage on Hit</stats><br><br><unique>UNIQUE Passive - Devouring Spirit:</unique> Takedowns on large monsters and Champions increase the magic damage of this item by +1. Takedowns on Rift Scuttlers increase the magic damage of this item by +2. Takedowns on epic monsters increase the magic damage of this item by +5. At 30 Stacks, your Devourer becomes Sated, granting extra on Hit effects.",["colloq"]=";",["plaintext"]="Increases Attack Speed, and gives increasing power as you kill Jungle Monsters and Champions",["from"]={"1043","3715"},["hideFromAll"]=true,["into"]={"3931"},["image"]={["full"]="1415.png",["sprite"]="item3.png",["group"]="item",["x"]=144,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=2450,["sell"]=1715},["tags"]={},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=false,["14"]=false},["stats"]={["PercentAttackSpeedMod"]=0.4},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.8",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="3",["Effect7Amount"]="20",["Effect8Amount"]="18"},["depth"]=3},["2003"]={["name"]="Health Potion",["group"]="HealthPotion",["description"]="<groupLimit>Limited to 5 at one time. Limited to 1 type of Healing Potion.</groupLimit><br><br><consumable>Click to Consume:</consumable> Restores 150 Health over 15 seconds.",["colloq"]=";",["plaintext"]="Consume to restore Health over time",["consumed"]=true,["stacks"]=5,["into"]={},["image"]={["full"]="2003.png",["sprite"]="item0.png",["group"]="item",["x"]=0,["y"]=144,["w"]=48,["h"]=48},["gold"]={["base"]=50,["purchasable"]=true,["total"]=50,["sell"]=20},["tags"]={"Consumable","Jungle","Lane"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="150",["Effect2Amount"]="15"}},["2009"]={["name"]="Total Biscuit of Rejuvenation",["description"]="<consumable>Click to Consume:</consumable> Restores 80 Health and 50 Mana over 10 seconds.",["colloq"]=";",["plaintext"]="",["consumed"]=true,["inStore"]=false,["into"]={},["image"]={["full"]="2009.png",["sprite"]="item0.png",["group"]="item",["x"]=96,["y"]=144,["w"]=48,["h"]=48},["gold"]={["base"]=0,["purchasable"]=false,["total"]=0,["sell"]=0},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={}},["2010"]={["name"]="Total Biscuit of Rejuvenation",["group"]="HealthPotion",["description"]="<consumable>Click to Consume:</consumable> Restores 20 health and 10 mana immediately and then 150 Health over 15 seconds.",["colloq"]=";",["plaintext"]="",["consumed"]=true,["stacks"]=5,["inStore"]=false,["into"]={},["image"]={["full"]="2010.png",["sprite"]="item0.png",["group"]="item",["x"]=144,["y"]=144,["w"]=48,["h"]=48},["gold"]={["base"]=50,["purchasable"]=false,["total"]=50,["sell"]=20},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="20",["Effect2Amount"]="10",["Effect3Amount"]="150",["Effect4Amount"]="15"}},["2015"]={["name"]="Kircheis Shard",["description"]="<stats>+15% Attack Speed</stats><br><br><passive>Passive:</passive> Moving and attacking will make an attack <a href='Energized'>Energized</a>.<br><br><unique>UNIQUE Passive - Energized Strike:</unique> Your Energized attacks deal 40 bonus magic damage on hit.",["colloq"]=";",["plaintext"]="Attack speed and a chargable magic hit",["from"]={"1042"},["into"]={"3087","3094"},["image"]={["full"]="2015.png",["sprite"]="item3.png",["group"]="item",["x"]=192,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=750,["sell"]=525},["tags"]={"AttackSpeed","OnHit"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["PercentAttackSpeedMod"]=0.15},["effect"]={["Effect1Amount"]="40"},["depth"]=2},["2031"]={["name"]="Refillable Potion",["group"]="FlaskGroup",["description"]="<groupLimit>Limited to 1 type of Healing Potion.</groupLimit><br><br><active>UNIQUE Active:</active> Consumes a charge to restore 125 Health over 12 seconds. Holds up to 2 charges and refills upon visiting the shop.",["colloq"]=";",["plaintext"]="Restores Health over time. Refills at shop.",["into"]={"2032","2033"},["image"]={["full"]="2031.png",["sprite"]="item3.png",["group"]="item",["x"]=240,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=150,["purchasable"]=true,["total"]=150,["sell"]=60},["tags"]={"Active","Consumable","HealthRegen","Jungle","Lane"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="125",["Effect2Amount"]="0",["Effect3Amount"]="12",["Effect4Amount"]="2"}},["2032"]={["name"]="Hunter's Potion",["group"]="FlaskGroup",["description"]="<groupLimit>Limited to 1 type of Healing Potion.</groupLimit><br><br><active>UNIQUE Active:</active> Consumes a charge to restore 60 Health and 35 Mana over 8 seconds. Holds up to 5 charges and refills upon visiting the shop.<br><br>Killing a Large Monster grants 1 charge.<br><br><rules>(Killing a Large Monster at full charges will automatically consume the newest charge.)</rules>",["colloq"]=";",["plaintext"]="Restores Health and Mana over time - Refills at shop and has increased capacity",["from"]={"2031"},["into"]={},["image"]={["full"]="2032.png",["sprite"]="item3.png",["group"]="item",["x"]=288,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=250,["purchasable"]=true,["total"]=400,["sell"]=160},["tags"]={"Active","Consumable","HealthRegen","Jungle","ManaRegen"},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="60",["Effect2Amount"]="35",["Effect3Amount"]="8",["Effect4Amount"]="5"},["depth"]=2},["2033"]={["name"]="Corrupting Potion",["group"]="FlaskGroup",["description"]="<groupLimit>Limited to 1 type of Healing Potion.</groupLimit><br><br><active>UNIQUE Active:</active> Consumes a charge to restore 150 Health and 50 Mana over 12 seconds and grants <font color='#FF8811'><u>Touch of Corruption</u></font> during that time. Holds up to 3 charges that refills upon visiting the shop.<br><br><font color='#FF8811'><u>Touch of Corruption:</u></font> Damaging spells and attacks burn enemy champions for <levelScale>15 - 30</levelScale> magic damage over 3 seconds. (Half Damage for Area of Effect or Damage over Time spells. Damage increases with champion level.)<br><br><rules>(Corrupting Potion can be used even at full Health and Mana.)</rules>",["colloq"]=";",["plaintext"]="Restores Health and Mana over time and boosts combat power - Refills at Shop",["from"]={"2031"},["into"]={},["image"]={["full"]="2033.png",["sprite"]="item3.png",["group"]="item",["x"]=336,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=350,["purchasable"]=true,["total"]=500,["sell"]=200},["tags"]={"Active","Consumable","HealthRegen","Lane","ManaRegen"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="150",["Effect2Amount"]="50",["Effect3Amount"]="12",["Effect4Amount"]="3",["Effect5Amount"]="10",["Effect6Amount"]="0.1",["Effect7Amount"]="15",["Effect8Amount"]="3"},["depth"]=2},["2043"]={["name"]="Vision Ward",["group"]="PinkWards",["description"]="<groupLimit>Can only carry 2 Vision Wards in inventory.</groupLimit><br><br><consumable>Click to Consume:</consumable> Places a visible ward that reveals the surrounding area and invisible units in the area until killed. Limit 1 <font color='#BBFFFF'>Vision Ward</font> on the map per player.<br><br><rules>(Revealing a ward in this manner grants a portion of the gold reward when that unit is killed.)</rules>",["colloq"]="pink;",["plaintext"]="Use to provide vision and stealth detection in an area until destroyed",["consumed"]=true,["stacks"]=2,["into"]={},["image"]={["full"]="2043.png",["sprite"]="item0.png",["group"]="item",["x"]=240,["y"]=144,["w"]=48,["h"]=48},["gold"]={["base"]=75,["purchasable"]=true,["total"]=75,["sell"]=30},["tags"]={"Consumable","Lane","Stealth","Vision"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=false,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="1",["Effect2Amount"]="2"}},["2045"]={["name"]="Ruby Sightstone",["description"]="<stats>+500 Health</stats><br><br><unique>UNIQUE Passive:</unique> Item Active cooldowns are reduced by 10%.<br><active>UNIQUE Active - Warding:</active> Consumes a charge to place a <font color='#BBFFFF'>Stealth Ward</font> that reveals the surrounding area for 150 seconds. Holds up to 4 charges and refills when visiting the shop.<br><br><rules>(A player may only have 3 <font color='#BBFFFF'>Stealth Wards</font> on the map at one time. Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Greatly increases Health and provides Stealth Wards over time",["from"]={"2049","1028"},["into"]={},["image"]={["full"]="2045.png",["sprite"]="item0.png",["group"]="item",["x"]=336,["y"]=144,["w"]=48,["h"]=48},["gold"]={["base"]=600,["purchasable"]=true,["total"]=1800,["sell"]=720},["tags"]={"Active","Health","Vision"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=500},["effect"]={["Effect1Amount"]="-0.1",["Effect2Amount"]="4",["Effect3Amount"]="150"},["depth"]=3},["2047"]={["name"]="Oracle's Extract",["description"]="<consumable>Click to Consume:</consumable> Grants detection of nearby invisible units for up to 5 minutes or until death.",["colloq"]=";",["plaintext"]="Allows champion to see invisible units",["consumed"]=true,["consumeOnFull"]=true,["into"]={},["image"]={["full"]="2047.png",["sprite"]="item0.png",["group"]="item",["x"]=384,["y"]=144,["w"]=48,["h"]=48},["gold"]={["base"]=250,["purchasable"]=true,["total"]=250,["sell"]=100},["tags"]={"Consumable","Stealth","Vision"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=false,["12"]=true,["14"]=false},["stats"]={}},["2049"]={["name"]="Sightstone",["description"]="<stats>+150 Health</stats><br><br><active>UNIQUE Active - Warding:</active> Consumes a charge to place a <font color='#BBFFFF'>Stealth Ward</font> that reveals the surrounding area for 150 seconds.  Holds up to 3 charges which refill upon visiting the shop. <br><br><rules>(A player may only have 3 <font color='#BBFFFF'>Stealth Wards</font> on the map at one time. Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Increases Health and provides Stealth Wards over time",["from"]={"1028"},["into"]={"2045","2301","2302","2303"},["image"]={["full"]="2049.png",["sprite"]="item0.png",["group"]="item",["x"]=432,["y"]=144,["w"]=48,["h"]=48},["gold"]={["base"]=400,["purchasable"]=true,["total"]=800,["sell"]=320},["tags"]={"Active","Health","Vision"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=150},["effect"]={["Effect1Amount"]="3",["Effect2Amount"]="150"},["depth"]=2},["2050"]={["name"]="Explorer's Ward",["description"]="<consumable>Click to Consume:</consumable> Places an invisible ward that reveals the surrounding area for 60 seconds.",["colloq"]=";",["plaintext"]="",["inStore"]=false,["into"]={},["image"]={["full"]="2050.png",["sprite"]="item0.png",["group"]="item",["x"]=0,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=0,["purchasable"]=false,["total"]=0,["sell"]=0},["tags"]={"Consumable"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={}},["2051"]={["name"]="Guardian's Horn",["description"]="<stats>+200 Health<br>+125% Base Health Regeneration </stats><br><br><unique>UNIQUE Passive:</unique> Enemy spellcasts reduce the cooldown of Battle Cry by 1 second.<br><active>UNIQUE Active - Battle Cry:</active> Gain 30% Movement Speed, 20 Armor, and 20 Magic Resist for 3 seconds. 25 second cooldown.",["colloq"]="Golden Arm of Kobe;Golden Bicep of Kobe;Horn; Horn of the ManWolf; ManWolf",["plaintext"]="Activate for Movement Speed and a defensive boost",["from"]={"1006","1028"},["into"]={},["image"]={["full"]="2051.png",["sprite"]="item0.png",["group"]="item",["x"]=48,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1000,["sell"]=700},["tags"]={"Active","Armor","Health","HealthRegen","NonbootsMovement","SpellBlock"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=false,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=200},["depth"]=2},["2052"]={["name"]="Poro-Snax",["group"]="RelicBase",["description"]="This savory blend of free-range, grass-fed Avarosan game hens and organic, non-ZMO Freljordian herbs contains the essential nutrients necessary to keep your Poro purring with pleasure.<br><br><i>All proceeds will be donated towards fighting Noxian animal cruelty.</i>",["colloq"]=";",["plaintext"]="",["consumed"]=true,["inStore"]=false,["into"]={},["image"]={["full"]="2052.png",["sprite"]="item0.png",["group"]="item",["x"]=96,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=0,["purchasable"]=false,["total"]=0,["sell"]=0},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={}},["2053"]={["name"]="Raptor Cloak",["description"]="<stats>+40 Armor<br>+125% Base Health Regen </stats><br><br><unique>UNIQUE Passive - Point Runner:</unique> Builds up to +20% Movement Speed over 2 seconds while near turrets and fallen turrets.",["colloq"]=";",["plaintext"]="Enhances Movement Speed near turrets",["from"]={"1006","1031"},["into"]={"3056","3512"},["image"]={["full"]="2053.png",["sprite"]="item0.png",["group"]="item",["x"]=144,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=250,["purchasable"]=true,["total"]=1200,["sell"]=840},["tags"]={"Armor","HealthRegen","NonbootsMovement"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatArmorMod"]=40},["effect"]={["Effect1Amount"]="20",["Effect2Amount"]="2"},["depth"]=3},["2054"]={["name"]="Diet Poro-Snax",["group"]="RelicBase",["description"]="All the flavor of regular Poro-Snax, without the calories! Keeps your Poro happy AND healthy.<br><br><consumable>Click to Consume:</consumable> Gives your Poros a delicious healthy treat.",["colloq"]=";",["plaintext"]="",["consumed"]=true,["inStore"]=false,["into"]={},["image"]={["full"]="2054.png",["sprite"]="item0.png",["group"]="item",["x"]=192,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=0,["purchasable"]=false,["total"]=0,["sell"]=0},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={}},["2138"]={["name"]="Elixir of Iron",["group"]="Flasks",["description"]="<stats><levelLimit>Level 9 required to purchase.</levelLimit></stats><br><br><consumable>Click to Consume:</consumable> Grants +300 Health, 25% Tenacity, increased champion size, and <font color='#FF8811'><u>Path of Iron</u></font> for 3 minutes.<br><br><font color='#FF8811'><u>Path of Iron:</u></font> Moving leaves a path behind that boosts allied champion's Movement Speed by 15%.<br><br><rules>(Only one Elixir effect may be active at a time.)</rules>",["colloq"]=";white",["plaintext"]="Temporarily increases defenses. Leaves a trail for allies to follow.",["consumed"]=true,["consumeOnFull"]=true,["into"]={},["image"]={["full"]="2138.png",["sprite"]="item0.png",["group"]="item",["x"]=288,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=500,["purchasable"]=true,["total"]=500,["sell"]=200},["tags"]={"Consumable","Health","NonbootsMovement","Tenacity"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="300",["Effect2Amount"]="0.25",["Effect3Amount"]="3",["Effect4Amount"]="0.15",["Effect5Amount"]="0.15",["Effect6Amount"]="0",["Effect7Amount"]="0",["Effect8Amount"]="9"}},["2139"]={["name"]="Elixir of Sorcery",["group"]="Flasks",["description"]="<stats><levelLimit>Level 9 required to purchase.</levelLimit></stats><br><br><consumable>Click to Consume:</consumable> Grants +50 Ability Power, 15 bonus Mana Regen per 5 seconds and <font color='#FF8811'><u>Sorcery</u></font> for 3 minutes. <br><br><font color='#FF8811'><u>Sorcery:</u></font> Damaging a champion or turret deals 25 bonus True Damage. This effect has a 5 second cooldown versus champions but no cooldown versus turrets.<br><br><rules>(Only one Elixir effect may be active at a time.)</rules><br>",["colloq"]=";blue",["plaintext"]="Temporarily grants Ability Power and Bonus Damage to champions and turrets.",["consumed"]=true,["consumeOnFull"]=true,["into"]={},["image"]={["full"]="2139.png",["sprite"]="item0.png",["group"]="item",["x"]=336,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=500,["purchasable"]=true,["total"]=500,["sell"]=200},["tags"]={"Consumable","ManaRegen","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="50",["Effect2Amount"]="50",["Effect3Amount"]="25",["Effect4Amount"]="3",["Effect5Amount"]="5",["Effect6Amount"]="3",["Effect7Amount"]="0",["Effect8Amount"]="9"}},["2140"]={["name"]="Elixir of Wrath",["group"]="Flasks",["description"]="<stats><levelLimit>Level 9 required to purchase.</levelLimit></stats><br><br><consumable>Click to Consume:</consumable> Grants +30 Attack Damage and <font color='#FF8811'><u>Bloodlust</u></font> for 3 minutes.<br><br><font color='#FF8811'><u>Bloodlust:</u></font> Dealing physical damage to champions heals for 15% of the damage dealt.<br><br><rules>(Only one Elixir effect may be active at a time.)</rules>",["colloq"]=";red",["plaintext"]="Temporarily grants Attack Damage and heals you when dealing physical damage to champions.",["consumed"]=true,["consumeOnFull"]=true,["into"]={},["image"]={["full"]="2140.png",["sprite"]="item0.png",["group"]="item",["x"]=384,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=500,["purchasable"]=true,["total"]=500,["sell"]=200},["tags"]={"Consumable","Damage","LifeSteal","SpellVamp"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="30",["Effect3Amount"]="0.15",["Effect4Amount"]="3",["Effect5Amount"]="0",["Effect6Amount"]="0",["Effect7Amount"]="0",["Effect8Amount"]="9"}},["2301"]={["name"]="Eye of the Watchers",["group"]="GoldBase",["description"]="<stats>+200 Health<br><mana>+100% Base Mana Regen </mana><br>+25 Ability Power<br>+10% Cooldown Reduction<br>+2 Gold per 10 seconds</stats><br><br><unique>UNIQUE Passive - Tribute:</unique> Spells and basic attacks against champions or buildings deal 15 additional damage and grant 15 Gold. This can occur up to 3 times every 30 seconds.<br><active>UNIQUE Active - Warding:</active> Consumes a charge to place a <font color='#BBFFFF'>Stealth Ward</font> that reveals the surrounding area for 150 seconds. Holds up to 4 charges which refill upon visiting the shop. <br><br><groupLimit>Limited to 1 Gold Income Item.</groupLimit><br><br><rules>(A player may only have 3 <font color='#BBFFFF'>Stealth Wards</font> on the map at one time. Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Provides Ability Power and Stealth Wards over time",["from"]={"2049","3098"},["into"]={},["image"]={["full"]="2301.png",["sprite"]="item3.png",["group"]="item",["x"]=384,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=550,["purchasable"]=true,["total"]=2200,["sell"]=880},["tags"]={"Active","CooldownReduction","GoldPer","Health","ManaRegen","SpellDamage","Vision"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=200,["FlatMagicDamageMod"]=25},["effect"]={["Effect1Amount"]="2",["Effect2Amount"]="15",["Effect3Amount"]="15",["Effect4Amount"]="4",["Effect5Amount"]="30",["Effect6Amount"]="12",["Effect7Amount"]="150",["Effect8Amount"]="3"},["depth"]=3},["2302"]={["name"]="Eye of the Oasis",["group"]="GoldBase",["description"]="<stats>+200 Health<br>+150% Base Health Regen <br><mana>+100% Base Mana Regen </mana><br>+10% Cooldown Reduction</stats><br><br><unique>UNIQUE Passive - Favor:</unique> Being near a minion's death without dealing the killing blow grants 6 Gold and 10 Health.<br><active>UNIQUE Active - Warding:</active> Consumes a charge to place a <font color='#BBFFFF'>Stealth Ward</font> that reveals the surrounding area for 150 seconds. Holds up to 4 charges which refill upon visiting the shop.<br><br><groupLimit>Limited to 1 Gold Income Item.</groupLimit><br><br><rules>(A player may only have 3 <font color='#BBFFFF'>Stealth Wards</font> on the map at one time. Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Provides Regeneration and Stealth Wards over time",["from"]={"2049","3096"},["into"]={},["image"]={["full"]="2302.png",["sprite"]="item3.png",["group"]="item",["x"]=432,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=550,["purchasable"]=true,["total"]=2200,["sell"]=880},["tags"]={"Active","CooldownReduction","GoldPer","Health","HealthRegen","ManaRegen","Vision"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=200},["effect"]={["Effect1Amount"]="0",["Effect2Amount"]="6",["Effect3Amount"]="10",["Effect4Amount"]="4",["Effect5Amount"]="0",["Effect6Amount"]="0",["Effect7Amount"]="150"},["depth"]=3},["2303"]={["name"]="Eye of the Equinox",["group"]="GoldBase",["description"]="<stats>+500 Health<br>+100% Base Health Regen <br>+2 Gold per 10 seconds</stats><br><br><unique>UNIQUE Passive - Spoils of War:</unique> Melee basic attacks execute minions below 400 Health. Killing a minion heals the owner and the nearest allied champion for 50 Health and grants them kill Gold. These effects require a nearby allied champion. Recharges every 30 seconds. Max 4 charges.<br><active>UNIQUE Active - Warding:</active> Consumes a charge to place a <font color='#BBFFFF'>Stealth Ward</font> that reveals the surrounding area for 150 seconds. Holds up to 4 charges which refill upon visiting the shop.<br><br><groupLimit>Limited to 1 Gold Income Item.</groupLimit><br><br><rules>(A player may only have 3 <font color='#BBFFFF'>Stealth Wards</font> on the map at one time. Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="Provides Health and Stealth Wards over time",["from"]={"2049","3097"},["into"]={},["image"]={["full"]="2303.png",["sprite"]="item3.png",["group"]="item",["x"]=0,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=550,["purchasable"]=true,["total"]=2200,["sell"]=880},["tags"]={"Active","GoldPer","Health","HealthRegen","Vision"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=500},["effect"]={["Effect1Amount"]="400",["Effect2Amount"]="50",["Effect3Amount"]="30",["Effect4Amount"]="4",["Effect5Amount"]="0",["Effect6Amount"]="0",["Effect7Amount"]="150",["Effect8Amount"]="0"},["depth"]=3},["3001"]={["name"]="Abyssal Scepter",["description"]="<stats>+70 Ability Power<br>+50 Magic Resist</stats><br><br><aura>UNIQUE Aura:</aura> Reduces the Magic Resist of nearby enemies by 20.",["colloq"]=";",["plaintext"]="Reduces Magic Resist of nearby enemies",["from"]={"1026","1057"},["into"]={},["image"]={["full"]="3001.png",["sprite"]="item0.png",["group"]="item",["x"]=432,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=780,["purchasable"]=true,["total"]=2350,["sell"]=1645},["tags"]={"Aura","MagicPenetration","SpellBlock","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=70,["FlatSpellBlockMod"]=50},["effect"]={["Effect1Amount"]="-20"},["depth"]=3},["3003"]={["name"]="Archangel's Staff",["description"]="<stats>+80 Ability Power<br><mana>+250 Mana<br>+50% Base Mana Regen </mana></stats><br><br><mana><unique>UNIQUE Passive - Insight:</unique> Grants Ability Power equal to 3% of maximum Mana.<br><unique>UNIQUE Passive - Mana Charge:</unique> Grants +8 maximum Mana (max +750 Mana) for each spell cast and Mana expenditure (occurs up to 2 times every 8 seconds). Transforms into Seraph's Embrace at +750 Mana.</mana>",["colloq"]=";aa",["plaintext"]="Increases Ability Power based on maximum Mana",["from"]={"3070","1058"},["into"]={"3040"},["image"]={["full"]="3003.png",["sprite"]="item0.png",["group"]="item",["x"]=0,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=1100,["purchasable"]=true,["total"]=3100,["sell"]=2170},["tags"]={"Mana","ManaRegen","SpellDamage"},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMPPoolMod"]=250,["FlatMagicDamageMod"]=80},["effect"]={["Effect1Amount"]="0.03",["Effect2Amount"]="8",["Effect3Amount"]="750",["Effect4Amount"]="2",["Effect5Amount"]="8"},["depth"]=3},["3004"]={["name"]="Manamune",["description"]="<stats>+25 Attack Damage<br><mana>+250 Mana<br>+25% Base Mana Regen </mana></stats><br><br><mana><unique>UNIQUE Passive - Awe:</unique> Grants bonus Attack Damage equal to 2% of maximum Mana.<br><unique>UNIQUE Passive - Mana Charge:</unique> Grants +4 maximum Mana (max +750 Mana) for each basic attack, spell cast, and Mana expenditure (occurs up to 2 times every 8 seconds). Grants 1 maximum Mana every 8 seconds.<br><br>Transforms into Muramana at +750 Mana.</mana>",["colloq"]=";",["plaintext"]="Increases Attack Damage based on maximum Mana",["from"]={"3070","1037"},["into"]={"3042"},["image"]={["full"]="3004.png",["sprite"]="item0.png",["group"]="item",["x"]=48,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=775,["purchasable"]=true,["total"]=2400,["sell"]=1680},["tags"]={"Damage","Mana","ManaRegen","OnHit"},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMPPoolMod"]=250,["FlatPhysicalDamageMod"]=25},["effect"]={["Effect1Amount"]="0.02",["Effect2Amount"]="4",["Effect3Amount"]="750",["Effect4Amount"]="2",["Effect5Amount"]="8",["Effect6Amount"]="1"},["depth"]=3},["3006"]={["name"]="Berserker's Greaves",["description"]="<stats> +30% Attack Speed</stats><br><br><unique>UNIQUE Passive - Enhanced Movement:</unique> +45 Movement Speed",["colloq"]=";",["plaintext"]="Enhances Movement Speed and Attack Speed",["from"]={"1001","1042"},["into"]={"1301","1303","1300","1302"},["image"]={["full"]="3006.png",["sprite"]="item0.png",["group"]="item",["x"]=96,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=500,["purchasable"]=true,["total"]=1100,["sell"]=770},["tags"]={"AttackSpeed","Boots"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45,["PercentAttackSpeedMod"]=0.3},["depth"]=2},["3007"]={["name"]="Archangel's Staff (Crystal Scar)",["description"]="<stats>+80 Ability Power<br><mana>+250 Mana<br>+50% Base Mana Regen </mana></stats><br><br><mana><unique>UNIQUE Passive - Insight:</unique> Grants Ability Power equal to 3% of maximum Mana.<br><unique>UNIQUE Passive - Mana Charge:</unique> Grants +10 maximum Mana (max +750 Mana) for each spell cast and Mana expenditure (occurs up to 2 times every 6 seconds). Transforms into Seraph's Embrace at +750 Mana.<br></mana>",["colloq"]=";aa",["plaintext"]="Increases Ability Power based on maximum Mana",["from"]={"3073","1058"},["into"]={"3048"},["image"]={["full"]="3007.png",["sprite"]="item0.png",["group"]="item",["x"]=144,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=1100,["purchasable"]=true,["total"]=3100,["sell"]=2170},["tags"]={"Mana","ManaRegen","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=false,["12"]=false,["14"]=false},["stats"]={["FlatMPPoolMod"]=250,["FlatMagicDamageMod"]=80},["effect"]={["Effect1Amount"]="0.03",["Effect2Amount"]="10",["Effect3Amount"]="750",["Effect4Amount"]="2",["Effect5Amount"]="6"},["depth"]=3},["3008"]={["name"]="Manamune (Crystal Scar)",["description"]="<stats>+25 Attack Damage<br><mana>+250 Mana<br>+25% Base Mana Regen </mana></stats><br><br><mana><unique>UNIQUE Passive - Awe:</unique> Grants bonus Attack Damage equal to 2% of maximum Mana.<br><unique>UNIQUE Passive - Mana Charge:</unique> Grants +8 maximum Mana (max +750 Mana) for each basic attack, spell cast, and Mana expenditure (occurs up to 2 times every 6 seconds).<br><br>Transforms into Muramana at +750 Mana.<br></mana>",["colloq"]=";",["plaintext"]="Increases Attack Damage based on maximum Mana",["from"]={"3073","1037"},["into"]={"3043"},["image"]={["full"]="3008.png",["sprite"]="item0.png",["group"]="item",["x"]=192,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=775,["purchasable"]=true,["total"]=2400,["sell"]=1680},["tags"]={"Damage","Mana","ManaRegen","OnHit"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=false,["12"]=false,["14"]=false},["stats"]={["FlatMPPoolMod"]=250,["FlatPhysicalDamageMod"]=25},["effect"]={["Effect1Amount"]="0.02",["Effect2Amount"]="8",["Effect3Amount"]="750",["Effect4Amount"]="2",["Effect5Amount"]="6"},["depth"]=3},["3009"]={["name"]="Boots of Swiftness",["description"]="<unique>UNIQUE Passive - Enhanced Movement:</unique> +60 Movement Speed<br><unique>UNIQUE Passive - Slow Resist:</unique> Movement slowing effects are reduced by 25%.",["colloq"]=";",["plaintext"]="Enhances Movement Speed and reduces the effect of slows",["from"]={"1001"},["into"]={"1306","1308","1305","1307"},["image"]={["full"]="3009.png",["sprite"]="item0.png",["group"]="item",["x"]=240,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=600,["purchasable"]=true,["total"]=900,["sell"]=630},["tags"]={"Boots"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=60},["effect"]={["Effect1Amount"]="0.25"},["depth"]=2},["3010"]={["name"]="Catalyst the Protector",["description"]="<stats>+225 Health<br><mana>+300 Mana</mana></stats><br><br><unique>UNIQUE Passive - Valor's Reward:</unique> Upon leveling up, restores 150 Health and 200 Mana over 8 seconds.",["colloq"]=";",["plaintext"]="Restores Health and Mana upon leveling up",["from"]={"1028","1027"},["into"]={"3027","3029","3180","3800"},["image"]={["full"]="3010.png",["sprite"]="item0.png",["group"]="item",["x"]=288,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1200,["sell"]=840},["tags"]={"Health","HealthRegen","Mana","ManaRegen"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=225,["FlatMPPoolMod"]=300},["effect"]={["Effect1Amount"]="150",["Effect2Amount"]="200",["Effect3Amount"]="8"},["depth"]=2},["3020"]={["name"]="Sorcerer's Shoes",["description"]="<stats>+15 <a href='FlatMagicPen'>Magic Penetration</a></stats><br><br><unique>UNIQUE Passive - Enhanced Movement:</unique> +45 Movement Speed<br><br><rules>(Magic Penetration: Magic damage is increased by ignoring an amount of the target's Magic Resist equal to Magic Penetration.)</rules>",["colloq"]=";",["plaintext"]="Enhances Movement Speed and magic damage",["from"]={"1001"},["into"]={"1311","1313","1310","1312"},["image"]={["full"]="3020.png",["sprite"]="item0.png",["group"]="item",["x"]=336,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=800,["purchasable"]=true,["total"]=1100,["sell"]=770},["tags"]={"Boots","MagicPenetration"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45},["effect"]={["Effect1Amount"]="15"},["depth"]=2},["3022"]={["name"]="Frozen Mallet",["description"]="<stats>+650 Health<br>+40 Attack Damage</stats><br><br><unique>UNIQUE Passive - Icy:</unique> Basic attacks slow the target's Movement Speed for 1.5 seconds on hit (40% slow for melee attacks, 30% slow for ranged attacks).",["colloq"]=";fm",["plaintext"]="Basic attacks slow enemies",["from"]={"3052","1037","1028"},["into"]={},["image"]={["full"]="3022.png",["sprite"]="item0.png",["group"]="item",["x"]=384,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=625,["purchasable"]=true,["total"]=3100,["sell"]=2170},["tags"]={"Damage","Health","OnHit","Slow"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=650,["FlatPhysicalDamageMod"]=40},["effect"]={["Effect1Amount"]="1.5",["Effect2Amount"]="0.4",["Effect3Amount"]="0.3"},["depth"]=3},["3024"]={["name"]="Glacial Shroud",["description"]="<stats>+25 Armor<br><mana>+250 Mana</mana></stats><br><br><unique>UNIQUE Passive:</unique> +10% Cooldown Reduction",["colloq"]=";",["plaintext"]="Increases Armor and Cooldown Reduction",["from"]={"1027","1029"},["into"]={"3110","3025","3050","3187"},["image"]={["full"]="3024.png",["sprite"]="item0.png",["group"]="item",["x"]=0,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=350,["purchasable"]=true,["total"]=1000,["sell"]=700},["tags"]={"Armor","CooldownReduction","Mana"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMPPoolMod"]=250,["FlatArmorMod"]=25},["effect"]={["Effect1Amount"]="-0.1"},["depth"]=2},["3025"]={["name"]="Iceborn Gauntlet",["description"]="<stats>+65 Armor<br>+20% Cooldown Reduction<br><mana>+500 Mana</mana></stats><br><br><unique>UNIQUE Passive - Spellblade:</unique> After using an ability, the next basic attack deals bonus physical damage equal to 125% of base Attack Damage in an area and creates an icy zone for 2 seconds that slows Movement Speed by 30% (1.5 second cooldown).<br><br>Size of zone increases with bonus armor.",["colloq"]=";frozen fist",["plaintext"]="Basic attacks create a slow field after spell cast",["from"]={"3057","3024"},["into"]={},["image"]={["full"]="3025.png",["sprite"]="item0.png",["group"]="item",["x"]=48,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=650,["purchasable"]=true,["total"]=2700,["sell"]=1890},["tags"]={"Armor","CooldownReduction","Mana","Slow"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMPPoolMod"]=500,["FlatArmorMod"]=65},["effect"]={["Effect1Amount"]="-0.2",["Effect2Amount"]="1.25",["Effect3Amount"]="2",["Effect4Amount"]="-0.3",["Effect5Amount"]="1.5"},["depth"]=3},["3026"]={["name"]="Guardian Angel",["description"]="<stats>+60 Armor<br>+60 Magic Resist</stats><br><br><unique>UNIQUE Passive:</unique> Upon taking lethal damage, restores 30% of maximum Health and Mana after 4 seconds of stasis (300 second cooldown).",["colloq"]=";ga",["plaintext"]="Periodically revives champion upon death",["from"]={"1057","1031"},["into"]={},["image"]={["full"]="3026.png",["sprite"]="item0.png",["group"]="item",["x"]=96,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=1380,["purchasable"]=true,["total"]=2900,["sell"]=1160},["tags"]={"Armor","SpellBlock"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatArmorMod"]=60,["FlatSpellBlockMod"]=60},["effect"]={["Effect1Amount"]="0.3",["Effect2Amount"]="4",["Effect3Amount"]="300"},["depth"]=3},["3027"]={["name"]="Rod of Ages",["description"]="<stats>+300 Health<br><mana>+400 Mana</mana><br>+80 Ability Power</stats><br><br><passive>Passive:</passive> Grants +20 Health, +40 Mana, and +4 Ability Power per stack (max +200 Health, +400 Mana, and +40 Ability Power). Grants 1 stack per minute (max 10 stacks).<br><unique>UNIQUE Passive - Valor's Reward:</unique> Upon leveling up, restores 150 Health and 200 Mana over 8 seconds.",["colloq"]=";roa",["plaintext"]="Greatly increases Health, Mana, and Ability Power",["from"]={"3010","1026"},["into"]={},["image"]={["full"]="3027.png",["sprite"]="item0.png",["group"]="item",["x"]=144,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=950,["purchasable"]=true,["total"]=3000,["sell"]=2100},["tags"]={"Health","HealthRegen","Mana","ManaRegen","SpellDamage"},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=300,["FlatMPPoolMod"]=400,["FlatMagicDamageMod"]=80},["effect"]={["Effect1Amount"]="20",["Effect2Amount"]="40",["Effect3Amount"]="4",["Effect4Amount"]="1",["Effect5Amount"]="10",["Effect6Amount"]="150",["Effect7Amount"]="200",["Effect8Amount"]="8"},["depth"]=3},["3028"]={["name"]="Chalice of Harmony",["description"]="<stats>+25 Magic Resist<br><mana>+50% Base Mana Regen </mana></stats><br><br><unique>UNIQUE Passive - Mana Font:</unique> Restores 2% of missing Mana every 5 seconds.",["colloq"]=";",["plaintext"]="Greatly increases Mana Regen",["from"]={"1004","1033","1004"},["into"]={"3174","3222"},["image"]={["full"]="3028.png",["sprite"]="item0.png",["group"]="item",["x"]=192,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=300,["purchasable"]=true,["total"]=1000,["sell"]=700},["tags"]={"ManaRegen","SpellBlock"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatSpellBlockMod"]=25},["effect"]={["Effect1Amount"]="2",["Effect2Amount"]="5"},["depth"]=2},["3029"]={["name"]="Rod of Ages (Crystal Scar)",["description"]="<stats>+300 Health<br><mana>+400 Mana</mana><br>+80 Ability Power</stats><br><br><passive>Passive:</passive> Grants +20 Health, +40 Mana, and +4 Ability Power per stack (max +200 Health, +400 Mana, and +40 Ability Power). Grants 1 stack per 40 seconds (max 10 stacks).<br><unique>UNIQUE Passive - Valor's Reward:</unique> Upon leveling up, restores 150 Health and 200 Mana over 8 seconds.",["colloq"]=";roa",["plaintext"]="Greatly increases Health, Mana, and Ability Power",["from"]={"3010","1026"},["into"]={},["image"]={["full"]="3029.png",["sprite"]="item0.png",["group"]="item",["x"]=240,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=950,["purchasable"]=true,["total"]=3000,["sell"]=2100},["tags"]={"Health","HealthRegen","Mana","ManaRegen","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=false,["12"]=false,["14"]=false},["stats"]={["FlatHPPoolMod"]=300,["FlatMPPoolMod"]=400,["FlatMagicDamageMod"]=80},["effect"]={["Effect1Amount"]="20",["Effect2Amount"]="40",["Effect3Amount"]="4",["Effect4Amount"]="1",["Effect5Amount"]="40",["Effect6Amount"]="10"},["depth"]=3},["3031"]={["name"]="Infinity Edge",["description"]="<stats>+65 Attack Damage<br>+20% Critical Strike Chance</stats><br><br><unique>UNIQUE Passive:</unique> Critical strike bonus damage is increased by 50%.",["colloq"]=";ie",["plaintext"]="Massively enhances critical strikes",["from"]={"1038","1037","1018"},["into"]={},["image"]={["full"]="3031.png",["sprite"]="item0.png",["group"]="item",["x"]=288,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=625,["purchasable"]=true,["total"]=3600,["sell"]=2520},["tags"]={"CriticalStrike","Damage"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=65,["FlatCritChanceMod"]=0.2},["effect"]={["Effect1Amount"]="0.5"},["depth"]=2},["3033"]={["name"]="Mortal Reminder",["description"]="<stats>+40 Attack Damage</stats><br><br><unique>UNIQUE Passive - Executioner:</unique> Physical damage inflicts <a href='GrievousWounds'>Grievous Wounds</a> on enemy champions for 5 seconds.<br><unique>UNIQUE Passive - Last Whisper:</unique> +45% <a href='BonusArmorPen'>Bonus Armor Penetration</a>.",["colloq"]=";lw;grievous",["plaintext"]="Overcomes enemies with high Health recovery and Armor",["from"]={"3035","3123"},["into"]={},["image"]={["full"]="3033.png",["sprite"]="item3.png",["group"]="item",["x"]=48,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=600,["purchasable"]=true,["total"]=2700,["sell"]=1890},["tags"]={"ArmorPenetration","Damage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=40},["effect"]={["Effect1Amount"]="5",["Effect2Amount"]="0.45"},["depth"]=3},["3034"]={["name"]="Giant Slayer",["description"]="<stats>+10 Attack Damage</stats><br><br><unique>UNIQUE Passive - Giant Slayer:</unique> Grants up to +10% physical damage against enemy champions with greater maximum Health than you (+1% damage per 50 Health difference, maxing at 500 Health difference).<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";gs",["plaintext"]="Overcomes enemies with high Health",["from"]={"1036"},["into"]={"3036"},["image"]={["full"]="3034.png",["sprite"]="item3.png",["group"]="item",["x"]=96,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=650,["purchasable"]=true,["total"]=1000,["sell"]=700},["tags"]={"Damage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=10},["effect"]={["Effect1Amount"]="0.1",["Effect2Amount"]="50",["Effect3Amount"]="0.01",["Effect4Amount"]="500"},["depth"]=2},["3035"]={["name"]="Last Whisper",["description"]="<stats>+25 Attack Damage</stats><br><br><unique>UNIQUE Passive - Last Whisper:</unique> +30% <a href='BonusArmorPen'>Bonus Armor Penetration</a>",["colloq"]=";lw",["plaintext"]="Overcomes enemies with high Armor",["from"]={"1037"},["into"]={"3033","3036"},["image"]={["full"]="3035.png",["sprite"]="item0.png",["group"]="item",["x"]=336,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=425,["purchasable"]=true,["total"]=1300,["sell"]=910},["tags"]={"ArmorPenetration","Damage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=25},["effect"]={["Effect1Amount"]="0.3",["Effect2Amount"]="0.45"},["depth"]=2},["3036"]={["name"]="Lord Dominik's Regards",["description"]="<stats>+40 Attack Damage</stats><br><br><unique>UNIQUE Passive - Giant Slayer:</unique> Grants up to +15% physical damage against enemy champions with greater maximum Health than you (+1.5% damage per 50 Health difference, maxing at 500 Health difference).<br><unique>UNIQUE Passive - Last Whisper:</unique> +45% <a href='BonusArmorPen'>Bonus Armor Penetration</a>",["colloq"]=";lw",["plaintext"]="Overcomes enemies with high health and armor",["from"]={"3035","3034"},["into"]={},["image"]={["full"]="3036.png",["sprite"]="item3.png",["group"]="item",["x"]=144,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=400,["purchasable"]=true,["total"]=2700,["sell"]=1890},["tags"]={"ArmorPenetration","Damage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=40},["effect"]={["Effect1Amount"]="0.45",["Effect2Amount"]="0.15",["Effect3Amount"]="0",["Effect4Amount"]="50",["Effect5Amount"]="0.015",["Effect6Amount"]="500"},["depth"]=3},["3040"]={["name"]="Seraph's Embrace",["description"]="<stats>+80 Ability Power<br><mana>+1000 Mana<br>+50% Base Mana Regen </mana></stats><br><br><mana><unique>UNIQUE Passive - Insight:</unique> Grants Ability Power equal to 3% of maximum Mana.</mana><br><active>UNIQUE Active - Mana Shield:</active> Consumes 20% of current Mana to grant a shield for 3 seconds that absorbs damage equal to 150 plus the amount of Mana consumed (120 second cooldown).",["colloq"]=";",["plaintext"]="",["from"]={"3003"},["specialRecipe"]=3003,["inStore"]=false,["into"]={},["image"]={["full"]="3040.png",["sprite"]="item0.png",["group"]="item",["x"]=384,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=3000,["purchasable"]=false,["total"]=3000,["sell"]=4270},["tags"]={"Active"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMPPoolMod"]=1000,["FlatMagicDamageMod"]=80},["effect"]={["Effect1Amount"]="0.03",["Effect2Amount"]="0.2",["Effect3Amount"]="3",["Effect4Amount"]="150",["Effect5Amount"]="120"},["depth"]=4},["3041"]={["name"]="Mejai's Soulstealer",["description"]="<stats>+20 Ability Power<br><mana>+200 Mana</mana></stats><br><br><unique>UNIQUE Passive - Dread:</unique> Grants +5 Ability Power per Glory. Grants 10% Movement Speed if you have at least 15 Glory.<br><unique>UNIQUE Passive - Do or Die:</unique> Grants 4 Glory for a champion kill or 2 Glory for an assist, up to 25 Glory total. Lose 10 stacks of Glory upon dying.",["colloq"]=";",["plaintext"]="Grants Ability Power for kills and assists",["from"]={"1082"},["into"]={},["image"]={["full"]="3041.png",["sprite"]="item0.png",["group"]="item",["x"]=432,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=1050,["purchasable"]=true,["total"]=1400,["sell"]=980},["tags"]={"Mana","SpellDamage"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMPPoolMod"]=200,["FlatMagicDamageMod"]=20},["effect"]={["Effect1Amount"]="5",["Effect2Amount"]="4",["Effect3Amount"]="2",["Effect4Amount"]="25",["Effect5Amount"]="0.5",["Effect6Amount"]="0.1",["Effect7Amount"]="10",["Effect8Amount"]="15"},["depth"]=2},["3042"]={["name"]="Muramana",["description"]="<stats>+25 Attack Damage<br><mana>+1000 Mana<br>+25% Base Mana Regen </mana></stats><br><br><mana><unique>UNIQUE Passive - Awe:</unique> Grants bonus Attack Damage equal to 2% of maximum Mana.<br><unique>UNIQUE Passive - Shock:</unique> Single target spells and attacks (on hit) on <font color='#FFFFFF'>Champions</font> consume 3% of current Mana to deal bonus physical damage equal to twice the amount of Mana consumed.<br>This effect only activates while you have greater than 20% maximum Mana.</mana>",["colloq"]=";",["plaintext"]="",["from"]={"3004"},["specialRecipe"]=3004,["inStore"]=false,["into"]={},["image"]={["full"]="3042.png",["sprite"]="item0.png",["group"]="item",["x"]=0,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=2200,["purchasable"]=false,["total"]=2200,["sell"]=3220},["tags"]={"OnHit"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMPPoolMod"]=1000,["FlatPhysicalDamageMod"]=25},["depth"]=4},["3043"]={["name"]="Muramana",["description"]="<stats>+25 Attack Damage<br><mana>+1000 Mana<br>+25% Base Mana Regen </mana></stats><br><br><mana><unique>UNIQUE Passive - Awe:</unique> Grants bonus Attack Damage equal to 2% of maximum Mana.<br><unique>UNIQUE Toggle:</unique> Single target spells and attacks (on hit) consume 3% of current Mana to deal bonus physical damage equal to twice the amount of Mana consumed.</mana>",["colloq"]=";",["plaintext"]="",["from"]={"3008"},["specialRecipe"]=3008,["inStore"]=false,["into"]={},["image"]={["full"]="3043.png",["sprite"]="item0.png",["group"]="item",["x"]=48,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=2200,["purchasable"]=false,["total"]=2200,["sell"]=3220},["tags"]={"OnHit"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=false,["12"]=false,["14"]=false},["stats"]={["FlatMPPoolMod"]=1000,["FlatPhysicalDamageMod"]=25},["depth"]=4},["3044"]={["name"]="Phage",["description"]="<stats>+200 Health<br>+15 Attack Damage</stats><br><br><unique>UNIQUE Passive - Rage:</unique> Basic attacks grant 20 Movement Speed for 2 seconds. Kills grant 60 Movement Speed instead. This Movement Speed bonus is halved for ranged champions.",["colloq"]=";",["plaintext"]="Attacks and kills give a small burst of speed",["from"]={"1028","1036"},["into"]={"3078","3071","3184"},["image"]={["full"]="3044.png",["sprite"]="item0.png",["group"]="item",["x"]=96,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=500,["purchasable"]=true,["total"]=1250,["sell"]=875},["tags"]={"Damage","Health","NonbootsMovement","OnHit"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=200,["FlatPhysicalDamageMod"]=15},["effect"]={["Effect1Amount"]="20",["Effect2Amount"]="2",["Effect3Amount"]="60"},["depth"]=2},["3046"]={["name"]="Phantom Dancer",["description"]="<stats>+45% Attack Speed<br>+30% Critical Strike Chance<br>+5% Movement Speed</stats><br><br><unique>UNIQUE Passive - Spectral Waltz:</unique> While within 550 units of an enemy champion you can see, +7% Movement Speed and you can move through units.<br><unique>UNIQUE Passive - Lament:</unique> The last champion hit deals 12% less damage to you (ends after 10 seconds of not hitting).",["colloq"]=";pd",["plaintext"]="Move faster near enemies and reduce incoming damage",["from"]={"1042","3086","1042"},["into"]={},["image"]={["full"]="3046.png",["sprite"]="item0.png",["group"]="item",["x"]=144,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=650,["purchasable"]=true,["total"]=2550,["sell"]=1785},["tags"]={"AttackSpeed","CriticalStrike","NonbootsMovement"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["PercentMovementSpeedMod"]=0.05,["PercentAttackSpeedMod"]=0.45,["FlatCritChanceMod"]=0.3},["effect"]={["Effect1Amount"]="0.12",["Effect2Amount"]="10",["Effect3Amount"]="550",["Effect4Amount"]="0.07"},["depth"]=3},["3047"]={["name"]="Ninja Tabi",["description"]="<stats>+30 Armor</stats><br><br><unique>UNIQUE Passive:</unique> Blocks 10% of the damage from basic attacks.<br><unique>UNIQUE Passive - Enhanced Movement:</unique> +45 Movement Speed",["colloq"]=";",["plaintext"]="Enhances Movement Speed and reduces incoming basic attack damage",["from"]={"1001","1029"},["into"]={"1316","1318","1315","1317"},["image"]={["full"]="3047.png",["sprite"]="item0.png",["group"]="item",["x"]=192,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=500,["purchasable"]=true,["total"]=1100,["sell"]=770},["tags"]={"Armor","Boots"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatArmorMod"]=30,["FlatMovementSpeedMod"]=45},["depth"]=2},["3048"]={["name"]="Seraph's Embrace",["description"]="<stats>+80 Ability Power<br><mana>+1000 Mana<br>+50% Base Mana Regen </mana></stats><br><br><mana><unique>UNIQUE Passive - Insight:</unique> Grants Ability Power equal to 3% of maximum Mana.</mana><br><active>UNIQUE Active - Mana Shield:</active> Consumes 20% of current Mana to grant a shield for 3 seconds that absorbs damage equal to 150 plus the amount of Mana consumed (120 second cooldown).",["colloq"]=";",["plaintext"]="",["from"]={"3007"},["specialRecipe"]=3007,["inStore"]=false,["into"]={},["image"]={["full"]="3048.png",["sprite"]="item0.png",["group"]="item",["x"]=240,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=2700,["purchasable"]=false,["total"]=2700,["sell"]=4060},["tags"]={"Active"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=false,["12"]=false,["14"]=false},["stats"]={["FlatMPPoolMod"]=1000,["FlatMagicDamageMod"]=80},["effect"]={["Effect1Amount"]="0.03",["Effect2Amount"]="0.2",["Effect3Amount"]="3",["Effect4Amount"]="150",["Effect5Amount"]="120"},["depth"]=4},["3050"]={["name"]="Zeke's Harbinger",["description"]="<stats><mana>+250 Mana</mana><br>+30 Armor<br>+50 Ability Power<br>+10% Cooldown Reduction</stats><br><br><active>UNIQUE Active - Conduit:</active> Bind to target ally (60 second cooldown).<br><unique>UNIQUE Passive:</unique> When within 1000 units of each other, you and your ally generate Charges. Attacking or casting spells generates extra Charges. At 100 Charges, causing damage consumes them, increasing your and your ally's Ability Power by 20% and Critical Strike Chance by 50% for 8 seconds. ",["colloq"]=";haroldandkumar",["plaintext"]="Grants an ally bursts of Critical Strike Chance and Ability Power",["from"]={"1052","3024","1052"},["into"]={},["image"]={["full"]="3050.png",["sprite"]="item0.png",["group"]="item",["x"]=288,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=480,["purchasable"]=true,["total"]=2350,["sell"]=1645},["tags"]={"Active","Armor","Aura","CooldownReduction","Mana","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMPPoolMod"]=250,["FlatArmorMod"]=30,["FlatMagicDamageMod"]=50},["effect"]={["Effect1Amount"]="-0.1",["Effect2Amount"]="1",["Effect3Amount"]="0.5",["Effect4Amount"]="0.2",["Effect5Amount"]="1000",["Effect6Amount"]="100",["Effect7Amount"]="8",["Effect8Amount"]="10"},["depth"]=3},["3052"]={["name"]="Jaurim's Fist",["description"]="<stats>+15 Attack Damage<br>+150 Health</stats><br><br><unique>UNIQUE Passive:</unique> Killing a unit grants 5 maximum Health. This bonus stacks up to 30 times.",["colloq"]=";enforcer",["plaintext"]="Attack Damage and stacking Health on Unit Kill",["from"]={"1036","1028"},["into"]={"3022","3053","3748"},["image"]={["full"]="3052.png",["sprite"]="item3.png",["group"]="item",["x"]=192,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1200,["sell"]=840},["tags"]={"Damage","Health"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=150,["FlatPhysicalDamageMod"]=15},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="5",["Effect3Amount"]="2",["Effect4Amount"]="150"},["depth"]=2},["3053"]={["name"]="Sterak's Gage",["description"]="<stats>+400 Health<br>+25% Base Attack Damage </stats><br><br><unique>UNIQUE Passive:</unique> Upon taking at least 400 to 1800 damage (based on level) within 5 seconds, gain <unlockedPassive>Sterak's Fury</unlockedPassive> for 8 seconds (45 second cooldown).<br><br><unlockedPassive>Sterak's Fury:</unlockedPassive> Grow in size and strength, gaining increased Size, +25% additional Base Attack Damage, and a rapidly decaying Shield for 30% of your maximum Health.",["colloq"]=";juggernaut;primal",["plaintext"]="Shields against large bursts of damage",["from"]={"3052","1036"},["into"]={},["image"]={["full"]="3053.png",["sprite"]="item3.png",["group"]="item",["x"]=144,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=1150,["purchasable"]=true,["total"]=2700,["sell"]=1890},["tags"]={"Damage","Health"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=400},["effect"]={["Effect1Amount"]="400",["Effect2Amount"]="0.25",["Effect3Amount"]="5",["Effect4Amount"]="0.3",["Effect5Amount"]="0",["Effect6Amount"]="8",["Effect7Amount"]="45",["Effect8Amount"]="3"},["depth"]=3},["3056"]={["name"]="Ohmwrecker",["description"]="<stats>+300 Health<br>+50 Armor<br>+150% Base Health Regen <br>+10% Cooldown Reduction</stats><br><br><active>UNIQUE Active:</active> Prevents nearby enemy turrets from attacking for 3 seconds (120 second cooldown). This effect cannot be used against the same turret more than once every 8 seconds.<br><br><unique>UNIQUE Passive - Point Runner:</unique> Builds up to +20% Movement Speed over 2 seconds while near turrets (including fallen turrets).",["colloq"]=";",["plaintext"]="Temporarily disables enemy turrets",["from"]={"2053","3067"},["into"]={},["image"]={["full"]="3056.png",["sprite"]="item0.png",["group"]="item",["x"]=336,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=650,["purchasable"]=true,["total"]=2650,["sell"]=1855},["tags"]={"Active","Armor","CooldownReduction","Health","HealthRegen","NonbootsMovement"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=300,["FlatArmorMod"]=50},["effect"]={["Effect1Amount"]="3",["Effect2Amount"]="120",["Effect3Amount"]="8",["Effect4Amount"]="0.2",["Effect5Amount"]="1"},["depth"]=4},["3057"]={["name"]="Sheen",["description"]="<stats><mana>+250 Mana</mana><br>+10% Cooldown Reduction</stats><br><br><unique>UNIQUE Passive - Spellblade:</unique> After using an ability, the next basic attack deals bonus physical damage equal to 100% base Attack Damage on hit (1.5 second cooldown).",["colloq"]=";",["plaintext"]="Grants a bonus to next attack after spell cast",["from"]={"1027"},["into"]={"3078","3100","3025"},["image"]={["full"]="3057.png",["sprite"]="item0.png",["group"]="item",["x"]=384,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=700,["purchasable"]=true,["total"]=1050,["sell"]=735},["tags"]={"CooldownReduction","Mana"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMPPoolMod"]=250},["effect"]={["Effect1Amount"]="1.5",["Effect2Amount"]="1"},["depth"]=2},["3060"]={["name"]="Banner of Command",["description"]="<stats>+200 Health<br>+100% Base Health Regen <br>+60 Ability Power<br>+20 Magic Resist<br>+10% Cooldown Reduction</stats><br><br><aura>UNIQUE Aura - Legion:</aura> Grants nearby allies +15 Magic Resist.<br><active>UNIQUE Active - Promote:</active> Greatly increases the power of a lane minion and grants it immunity to magic damage (120 second cooldown).",["colloq"]=";flag",["plaintext"]="Promotes a siege minion to a more powerful unit",["from"]={"3105","3108"},["into"]={},["image"]={["full"]="3060.png",["sprite"]="item0.png",["group"]="item",["x"]=432,["y"]=336,["w"]=48,["h"]=48},["gold"]={["base"]=600,["purchasable"]=true,["total"]=2900,["sell"]=2030},["tags"]={"Active","Aura","CooldownReduction","Health","HealthRegen","SpellBlock","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=200,["FlatMagicDamageMod"]=60,["FlatSpellBlockMod"]=20},["effect"]={["Effect1Amount"]="0.5",["Effect2Amount"]="15",["Effect3Amount"]="0.75"},["depth"]=4},["3065"]={["name"]="Spirit Visage",["description"]="<stats>+500 Health<br>+70 Magic Resist<br>+150% Base Health Regen <br>+10% Cooldown Reduction</stats><br><br><unique>UNIQUE Passive:</unique> Increases all healing received by 20%.",["colloq"]=";sv",["plaintext"]="Increases Health and healing effects",["from"]={"3211","3067"},["into"]={},["image"]={["full"]="3065.png",["sprite"]="item0.png",["group"]="item",["x"]=0,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=900,["purchasable"]=true,["total"]=2800,["sell"]=1960},["tags"]={"CooldownReduction","Health","HealthRegen","SpellBlock"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=500,["FlatSpellBlockMod"]=70},["effect"]={["Effect1Amount"]="-0.1",["Effect2Amount"]="0.2",["Effect3Amount"]="0.5",["Effect4Amount"]="0.015"},["depth"]=3},["3067"]={["name"]="Kindlegem",["description"]="<stats>+200 Health  </stats><br><br><unique>UNIQUE Passive:</unique> +10% Cooldown Reduction",["colloq"]=";",["plaintext"]="Increases Health and Cooldown Reduction",["from"]={"1028"},["into"]={"3187","3190","3401","3065","3056"},["image"]={["full"]="3067.png",["sprite"]="item0.png",["group"]="item",["x"]=48,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=400,["purchasable"]=true,["total"]=800,["sell"]=560},["tags"]={"CooldownReduction","Health"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=200},["effect"]={["Effect1Amount"]="-0.1"},["depth"]=2},["3068"]={["name"]="Sunfire Cape",["description"]="<stats>+500 Health<br>+50 Armor  </stats><br><br><unique>UNIQUE Passive - Immolate:</unique> Deals 25 (+1 per champion level) magic damage per second to nearby enemies. Deals 50% bonus damage to minions and monsters.",["colloq"]=";",["plaintext"]="Constantly deals damage to nearby enemies",["from"]={"1031","3751"},["into"]={},["image"]={["full"]="3068.png",["sprite"]="item0.png",["group"]="item",["x"]=96,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=800,["purchasable"]=true,["total"]=2700,["sell"]=1890},["tags"]={"Armor","Health"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=500,["FlatArmorMod"]=50},["effect"]={["Effect1Amount"]="25",["Effect2Amount"]="1",["Effect3Amount"]="0.5"},["depth"]=3},["3069"]={["name"]="Talisman of Ascension",["group"]="GoldBase",["description"]="<stats>+100% Base Health Regen <br><mana>+100% Base Mana Regen <br></mana>+10% Cooldown Reduction</stats><br><br><unique>UNIQUE Passive - Favor:</unique> Being near a minion's death without dealing the killing blow grants 6 Gold and 10 Health.<br><active>UNIQUE Active:</active> Grants nearby allies +40% Movement Speed for 3 seconds (60 second cooldown).<br><br><groupLimit>Limited to 1 Gold Income Item.</groupLimit><br><br><rules><font color='#447777'>''Praise the sun.'' - Historian Shurelya, 22 September, 25 CLE</font></rules>",["colloq"]=";shurelya;reverie",["plaintext"]="Increases Health / Mana Regeneration and Cooldown Reduction. Activate to speed up nearby allies.",["from"]={"3096","3114"},["into"]={},["image"]={["full"]="3069.png",["sprite"]="item0.png",["group"]="item",["x"]=144,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=800,["purchasable"]=true,["total"]=2200,["sell"]=880},["tags"]={"Active","CooldownReduction","GoldPer","HealthRegen","ManaRegen","NonbootsMovement"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="-0.1",["Effect2Amount"]="0",["Effect3Amount"]="6",["Effect4Amount"]="10",["Effect5Amount"]="0.4",["Effect6Amount"]="3",["Effect7Amount"]="60",["Effect8Amount"]="20"},["depth"]=3},["3070"]={["name"]="Tear of the Goddess",["description"]="<stats><mana>+250 Mana<br>+25% Base Mana Regen </mana></stats><br><br><mana><unique>UNIQUE Passive - Mana Charge:</unique> Grants 4 maximum Mana on spell cast or Mana expenditure (up to 2 times per 8 seconds). Grants 1 maximum Mana every 8 seconds.<br><br>Caps at +750 Mana.</mana>",["colloq"]=";",["plaintext"]="Increases maximum Mana as Mana is spent",["from"]={"1027","1004"},["into"]={"3003","3004"},["image"]={["full"]="3070.png",["sprite"]="item0.png",["group"]="item",["x"]=192,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=275,["purchasable"]=true,["total"]=750,["sell"]=525},["tags"]={"Mana","ManaRegen"},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMPPoolMod"]=250},["effect"]={["Effect1Amount"]="4",["Effect2Amount"]="8",["Effect3Amount"]="1",["Effect4Amount"]="8",["Effect5Amount"]="750",["Effect6Amount"]="2"},["depth"]=2},["3071"]={["name"]="The Black Cleaver",["description"]="<stats>+300 Health<br>+55 Attack Damage<br>+20% Cooldown Reduction</stats><br><br><unique>UNIQUE Passive:</unique> Dealing physical damage to an enemy champion Cleaves them, reducing their Armor by 5% for 6 seconds (stacks up to 6 times, up to 30%).<br><unique>UNIQUE Passive - Rage:</unique> Dealing physical damage grants 20 movement speed for 2 seconds. Assists on Cleaved enemy champions or kills on any unit grant 60 movement speed for 2 seconds instead. This Movement Speed is halved for ranged champions.",["colloq"]=";bc",["plaintext"]="Dealing physical damage to enemy champions reduces their Armor",["from"]={"3044","3133"},["into"]={},["image"]={["full"]="3071.png",["sprite"]="item0.png",["group"]="item",["x"]=240,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=1150,["purchasable"]=true,["total"]=3500,["sell"]=2450},["tags"]={"ArmorPenetration","CooldownReduction","Damage","Health","NonbootsMovement","OnHit"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=300,["FlatPhysicalDamageMod"]=55},["effect"]={["Effect1Amount"]="-0.2",["Effect2Amount"]="0.05",["Effect3Amount"]="6",["Effect4Amount"]="6",["Effect5Amount"]="0.3",["Effect6Amount"]="20",["Effect7Amount"]="2",["Effect8Amount"]="60"},["depth"]=3},["3072"]={["name"]="The Bloodthirster",["description"]="<stats>+75 Attack Damage</stats><br><br><unique>UNIQUE Passive:</unique> +20% Life Steal<br><unique>UNIQUE Passive:</unique> Your basic attacks can now overheal you. Excess life is stored as a shield that can block 50-350 damage, based on champion level.<br><br>This shield decays slowly if you haven't dealt or taken damage in the last 25 seconds.",["colloq"]=";bt",["plaintext"]="Grants Attack Damage, Life Steal and Life Steal now overheals",["from"]={"1038","1036","1053"},["into"]={},["image"]={["full"]="3072.png",["sprite"]="item0.png",["group"]="item",["x"]=288,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=1150,["purchasable"]=true,["total"]=3700,["sell"]=2590},["tags"]={"Damage","LifeSteal"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=75},["effect"]={["Effect1Amount"]="50",["Effect2Amount"]="350",["Effect3Amount"]="25",["Effect4Amount"]="0.2"},["depth"]=3},["3073"]={["name"]="Tear of the Goddess (Crystal Scar)",["description"]="<stats><mana>+250 Mana<br>+25% Base Mana Regen </mana></stats><br><br><mana><unique>UNIQUE Passive - Mana Charge:</unique> Grants 5 maximum Mana on spell cast or Mana expenditure (up to 2 times per 6 seconds). Grants 1 maximum Mana every 6 seconds.<br><br>Caps at +750 Mana.</mana>",["colloq"]=";",["plaintext"]="Increases maximum Mana as Mana is spent",["from"]={"1027","1004"},["into"]={"3007","3008"},["image"]={["full"]="3073.png",["sprite"]="item0.png",["group"]="item",["x"]=336,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=275,["purchasable"]=true,["total"]=750,["sell"]=525},["tags"]={"Mana","ManaRegen"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=false,["12"]=false,["14"]=false},["stats"]={["FlatMPPoolMod"]=250},["effect"]={["Effect1Amount"]="5",["Effect2Amount"]="2",["Effect3Amount"]="6",["Effect4Amount"]="1",["Effect5Amount"]="750"},["depth"]=2},["3074"]={["name"]="Ravenous Hydra",["description"]="<stats>+75 Attack Damage<br>+100% Base Health Regen <br>+12% Life Steal</stats><br><br><passive>Passive:</passive> 50% of total Life Steal applies to damage dealt by this item.<br><unique>UNIQUE Passive - Cleave:</unique> Basic attacks deal 20% to 60% of total Attack Damage as bonus physical damage to enemies near the target on hit (enemies closest to the target take the most damage).<br><active>UNIQUE Active - Crescent:</active> Deals 60% to 100% of total Attack Damage as physical damage to nearby enemy units (closest enemies take the most damage) (10 second cooldown).",["colloq"]=";",["plaintext"]="Melee attacks hit nearby enemies, dealing damage and restoring Health",["from"]={"3077","1053","1036"},["into"]={},["image"]={["full"]="3074.png",["sprite"]="item0.png",["group"]="item",["x"]=384,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=1050,["purchasable"]=true,["total"]=3500,["sell"]=2450},["tags"]={"Active","Damage","HealthRegen","LifeSteal","OnHit"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=75,["PercentLifeStealMod"]=0.12},["effect"]={["Effect1Amount"]="0.2",["Effect2Amount"]="0.6",["Effect3Amount"]="0.6",["Effect4Amount"]="1",["Effect5Amount"]="10"},["depth"]=3},["3075"]={["name"]="Thornmail",["description"]="<stats>+100 Armor  </stats><br><br><unique>UNIQUE Passive:</unique> Upon being hit by a basic attack, reflects magic damage back to the attacker equal to 25% of your bonus Armor plus 15% of the incoming damage.<br><br><rules>(Bonus Armor is Armor from items, buffs, runes and masteries.)</rules><br><rules>(Reflect damage is calculated based on damage taken before being reduced by Armor.)</rules>",["colloq"]=";",["plaintext"]="Returns damage taken from basic attacks as magic damage",["from"]={"1029","1031"},["into"]={},["image"]={["full"]="3075.png",["sprite"]="item0.png",["group"]="item",["x"]=432,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=1250,["purchasable"]=true,["total"]=2350,["sell"]=1645},["tags"]={"Armor"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatArmorMod"]=100},["effect"]={["Effect1Amount"]="0.15",["Effect2Amount"]="0.25"},["depth"]=3},["3077"]={["name"]="Tiamat",["description"]="<stats>+30 Attack Damage<br>+50% Base Health Regen </stats><br><br><active>UNIQUE Active - Crescent:</active> Deals 60% to 100% of total Attack Damage as physical damage to nearby enemy units (enemies closest to the target take the most damage) (10 second cooldown).",["colloq"]=";",["plaintext"]="Activate to deal damage to nearby enemies",["from"]={"1037","1006"},["into"]={"3074","3748"},["image"]={["full"]="3077.png",["sprite"]="item0.png",["group"]="item",["x"]=0,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=175,["purchasable"]=true,["total"]=1200,["sell"]=840},["tags"]={"Active","Damage","HealthRegen"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=30},["effect"]={["Effect1Amount"]="0.2",["Effect2Amount"]="0.6",["Effect3Amount"]="0.6",["Effect4Amount"]="1",["Effect5Amount"]="10"},["depth"]=2},["3078"]={["name"]="Trinity Force",["description"]="<stats>+250 Health<br>+25 Attack Damage<br>+20% Critical Strike Chance<br>+15% Attack Speed<br>+10% Cooldown Reduction<br>+5% Movement Speed<br><mana>+250 Mana</mana></stats><br><br><unique>UNIQUE Passive - Rage:</unique> Basic attacks grant 20 Movement Speed for 2 seconds. Kills grant 60 Movement Speed instead. This Movement Speed bonus is halved for ranged champions.<br><unique>UNIQUE Passive - Spellblade:</unique> After using an ability, the next basic attack deals bonus physical damage equal to 200% of base Attack Damage on hit (1.5 second cooldown).",["colloq"]=";triforce;tons of damage",["plaintext"]="Tons of Damage",["from"]={"3086","3057","3044"},["into"]={},["image"]={["full"]="3078.png",["sprite"]="item0.png",["group"]="item",["x"]=48,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=200,["purchasable"]=true,["total"]=3800,["sell"]=2660},["tags"]={"AttackSpeed","CooldownReduction","CriticalStrike","Damage","Health","Mana","NonbootsMovement","OnHit"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=250,["FlatMPPoolMod"]=250,["FlatPhysicalDamageMod"]=25,["PercentMovementSpeedMod"]=0.05,["PercentAttackSpeedMod"]=0.15,["FlatCritChanceMod"]=0.2},["effect"]={["Effect1Amount"]="20",["Effect2Amount"]="60",["Effect3Amount"]="2",["Effect4Amount"]="2",["Effect5Amount"]="1.5"},["depth"]=3},["3082"]={["name"]="Warden's Mail",["description"]="<stats>+40 Armor</stats><br><br><unique>UNIQUE Passive - Cold Steel:</unique> When hit by basic attacks, reduces the attacker's Attack Speed by 15% for 1 seconds.",["colloq"]=";",["plaintext"]="Slows Attack Speed of enemy champions when receiving basic attacks",["from"]={"1029","1029"},["into"]={"3110","3143"},["image"]={["full"]="3082.png",["sprite"]="item0.png",["group"]="item",["x"]=96,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=400,["purchasable"]=true,["total"]=1000,["sell"]=700},["tags"]={"Armor","Slow"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatArmorMod"]=40},["effect"]={["Effect1Amount"]="-0.15",["Effect2Amount"]="1"},["depth"]=2},["3083"]={["name"]="Warmog's Armor",["description"]="<stats>+850 Health<br>+200% Base Health Regen </stats><br><br><unique>UNIQUE Passive:</unique> Grants <unlockedPassive>Warmog's Heart</unlockedPassive> if you have at least 3000 maximum Health.<br><br><unlockedPassive>Warmog's Heart:</unlockedPassive> Restores 15% of maximum Health every 5 seconds if damage hasn't been taken within 8 seconds. ",["colloq"]=";",["plaintext"]="Grants massive Health and Health Regen",["from"]={"3801","1011","3801"},["into"]={},["image"]={["full"]="3083.png",["sprite"]="item0.png",["group"]="item",["x"]=144,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=550,["purchasable"]=true,["total"]=2850,["sell"]=1995},["tags"]={"Health","HealthRegen"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=850},["effect"]={["Effect1Amount"]="0.015",["Effect2Amount"]="5",["Effect3Amount"]="0.15",["Effect4Amount"]="3000",["Effect5Amount"]="8"},["depth"]=3},["3084"]={["name"]="Overlord's Bloodmail",["description"]="<stats>+800 Health<br>+100% Base Health Regen </stats><br><br><unique>UNIQUE Passive:</unique> Upon champion kill or assist, restores 300 Health over 5 seconds.",["colloq"]=";",["plaintext"]="Restores Health on kill or assist",["from"]={"1011","3801"},["into"]={},["image"]={["full"]="3084.png",["sprite"]="item0.png",["group"]="item",["x"]=192,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=900,["purchasable"]=true,["total"]=2550,["sell"]=1785},["tags"]={"Health","HealthRegen"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=false,["12"]=false,["14"]=false},["stats"]={["FlatHPPoolMod"]=800},["effect"]={["Effect1Amount"]="300",["Effect2Amount"]="5"},["depth"]=3},["3085"]={["name"]="Runaan's Hurricane",["description"]="<stats>+40% Attack Speed<br>+30% Critical Strike Chance<br>+7% Movement Speed</stats><br><br><unique>UNIQUE Passive - Wind's Fury:</unique> When basic attacking, bolts are fired at up to 2 enemies near the target, each dealing (25% of Attack Damage) physical damage. Bolts can critically strike and apply on hit effects.<br><unique>UNIQUE Passive:</unique> Basic attacks deal an additional 15 physical damage on hit.",["colloq"]=";",["plaintext"]="Ranged attacks fire two bolts at nearby enemies",["from"]={"1043","3086"},["into"]={},["image"]={["full"]="3085.png",["sprite"]="item0.png",["group"]="item",["x"]=240,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=300,["purchasable"]=true,["total"]=2600,["sell"]=1820},["tags"]={"AttackSpeed","CriticalStrike","NonbootsMovement","OnHit"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["PercentMovementSpeedMod"]=0.07,["PercentAttackSpeedMod"]=0.4,["FlatCritChanceMod"]=0.3},["effect"]={["Effect1Amount"]="0",["Effect2Amount"]="25",["Effect3Amount"]="2",["Effect4Amount"]="15",["Effect5Amount"]="25",["Effect6Amount"]="1"},["depth"]=3},["3086"]={["name"]="Zeal",["description"]="<stats>+15% Attack Speed<br>+20% Critical Strike Chance</stats><br><br><unique>UNIQUE Passive:</unique> +5% Movement Speed",["colloq"]=";",["plaintext"]="Slight bonuses to Critical Strike Chance, Movement Speed and Attack Speed",["from"]={"1051","1042"},["into"]={"3046","3078","3085","3087","3094"},["image"]={["full"]="3086.png",["sprite"]="item0.png",["group"]="item",["x"]=288,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=600,["purchasable"]=true,["total"]=1300,["sell"]=910},["tags"]={"AttackSpeed","CriticalStrike","NonbootsMovement"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["PercentAttackSpeedMod"]=0.15,["FlatCritChanceMod"]=0.2},["effect"]={["Effect1Amount"]="0.05"},["depth"]=2},["3087"]={["name"]="Statikk Shiv",["description"]="<stats>+35% Attack Speed<br>+30% Critical Strike Chance<br>+5% Movement Speed</stats><br><br><passive>Passive:</passive> Moving and attacking will make an attack <a href='Energized'>Energized</a>.<br><br><unique>UNIQUE Passive - Shiv Lightning:</unique> Your Energized attacks deal 50~120 bonus magic damage (based on level) to up to 5 targets on hit (deals +120% bonus damage to minions and can critically strike).",["colloq"]=";",["plaintext"]="Movement builds charges that release chain lightning on basic attack",["from"]={"3086","2015"},["into"]={},["image"]={["full"]="3087.png",["sprite"]="item0.png",["group"]="item",["x"]=336,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=550,["purchasable"]=true,["total"]=2600,["sell"]=1820},["tags"]={"AttackSpeed","CriticalStrike","NonbootsMovement","OnHit"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["PercentMovementSpeedMod"]=0.05,["PercentAttackSpeedMod"]=0.35,["FlatCritChanceMod"]=0.3},["effect"]={["Effect1Amount"]="100",["Effect2Amount"]="80",["Effect3Amount"]="5",["Effect4Amount"]="750",["Effect5Amount"]="50",["Effect6Amount"]="120",["Effect7Amount"]="5",["Effect8Amount"]="1.2"},["depth"]=3},["3089"]={["name"]="Rabadon's Deathcap",["description"]="<stats>+120 Ability Power  </stats><br><br><unique>UNIQUE Passive:</unique> Increases Ability Power by 35%.",["colloq"]=";dc;banksys;hat",["plaintext"]="Massively increases Ability Power",["from"]={"1026","1058","1052"},["into"]={},["image"]={["full"]="3089.png",["sprite"]="item0.png",["group"]="item",["x"]=384,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=1265,["purchasable"]=true,["total"]=3800,["sell"]=2660},["tags"]={"SpellDamage"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=120},["effect"]={["Effect1Amount"]="35"},["depth"]=2},["3090"]={["name"]="Wooglet's Witchcap",["description"]="<stats>+100 Ability Power<br>+45 Armor  </stats><br><br><unique>UNIQUE Passive:</unique> Increases Ability Power by 25%<br><active>UNIQUE Active:</active> Champion becomes invulnerable and untargetable for 2.5 seconds, but is unable to move, attack, cast spells, or use items during this time (90 second cooldown).",["colloq"]=";hat",["plaintext"]="Massively increases Ability Power and can be activated to enter stasis",["from"]={"3191","1058"},["into"]={},["image"]={["full"]="3090.png",["sprite"]="item0.png",["group"]="item",["x"]=432,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=1050,["purchasable"]=true,["total"]=3500,["sell"]=2450},["tags"]={"Active","Armor","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=false,["12"]=false,["14"]=false},["stats"]={["FlatArmorMod"]=45,["FlatMagicDamageMod"]=100},["effect"]={["Effect1Amount"]="25",["Effect2Amount"]="2.5",["Effect3Amount"]="90"},["depth"]=3},["3091"]={["name"]="Wit's End",["description"]="<stats>+40% Attack Speed<br>+40 Magic Resist</stats><br><br><unique>UNIQUE Passive:</unique> Basic attacks deal 40 bonus magic damage on hit.<br><unique>UNIQUE Passive:</unique> Basic attacks steal 5 Magic Resist from the target on hit (stacks up to 5 times.)",["colloq"]=";",["plaintext"]="Deals bonus magic damage on basic attacks",["from"]={"1043","1033","1042"},["into"]={},["image"]={["full"]="3091.png",["sprite"]="item1.png",["group"]="item",["x"]=0,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=1050,["purchasable"]=true,["total"]=2800,["sell"]=1960},["tags"]={"AttackSpeed","OnHit","SpellBlock"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["PercentAttackSpeedMod"]=0.4,["FlatSpellBlockMod"]=40},["effect"]={["Effect1Amount"]="40",["Effect2Amount"]="5",["Effect3Amount"]="5"},["depth"]=3},["3092"]={["name"]="Frost Queen's Claim",["group"]="GoldBase",["description"]="<stats>+50 Ability Power<br>+10% Cooldown Reduction<br>+2 Gold per 10 seconds<br><mana>+100% Base Mana Regen </mana></stats><br><br><unique>UNIQUE Passive - Tribute:</unique> Spells and basic attacks against champions or buildings deal 15 additional damage and grant 15 Gold. This can occur up to 3 times every 30 seconds.<br><active>UNIQUE Active:</active> Summon 2 icy ghosts for 6 seconds that seek out nearby enemy champions. Ghosts reveal enemies on contact and reduce their Movement Speed by 40% for between 2 and 5 seconds based on how far the ghosts have traveled (90 second cooldown).<br><br><groupLimit>Limited to 1 Gold Income Item.</groupLimit>",["colloq"]="spooky ghosts;",["plaintext"]="Sends out seeking wraiths that track hidden enemies and slow them",["from"]={"3098","3108"},["into"]={},["image"]={["full"]="3092.png",["sprite"]="item1.png",["group"]="item",["x"]=48,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=550,["purchasable"]=true,["total"]=2200,["sell"]=880},["tags"]={"Active","CooldownReduction","GoldPer","ManaRegen","Slow","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=50},["effect"]={["Effect1Amount"]="15",["Effect2Amount"]="15",["Effect3Amount"]="12",["Effect4Amount"]="3",["Effect5Amount"]="30",["Effect6Amount"]="2",["Effect7Amount"]="1",["Effect8Amount"]="2"},["depth"]=3},["3094"]={["name"]="Rapid Firecannon",["description"]="<stats>+30% Attack Speed<br>+30% Critical Strike Chance<br>+5% Movement Speed</stats><br><br><passive>Passive:</passive> Moving and attacking will make an attack <a href='Energized'>Energized</a>.<br><br><unique>UNIQUE Passive - Firecannon:</unique> Your Energized attacks gain 35% bonus Range (+150 range maximum) and deal 50~160 bonus magic damage (based on level) on hit.<br><br>Energized attacks function on structures.",["colloq"]=";canon;rapidfire;rfc",["plaintext"]="Movement builds charges that release a sieging fire attack on release",["from"]={"3086","2015"},["into"]={},["image"]={["full"]="3094.png",["sprite"]="item3.png",["group"]="item",["x"]=240,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=550,["purchasable"]=true,["total"]=2600,["sell"]=1820},["tags"]={"AttackSpeed","CriticalStrike","NonbootsMovement","OnHit"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["PercentMovementSpeedMod"]=0.05,["PercentAttackSpeedMod"]=0.3,["FlatCritChanceMod"]=0.3},["effect"]={["Effect1Amount"]="0.35",["Effect2Amount"]="150",["Effect3Amount"]="50",["Effect4Amount"]="160",["Effect5Amount"]="5"},["depth"]=3},["3096"]={["name"]="Nomad's Medallion",["group"]="GoldBase",["description"]="<stats>+50% Base Health Regen <br><mana>+50% Base Mana Regen </mana></stats><br><br><unique>UNIQUE Passive - Favor:</unique> Being near a minion's death without dealing the killing blow grants 6 Gold and 10 Health.<br><br><groupLimit>Limited to 1 Gold Income Item.</groupLimit><br><br><rules><font color='#447777'>''The medallion shines with the glory of a thousand voices when exposed to the sun.'' - Historian Shurelya, 22 June, 24 CLE</font></rules>",["colloq"]=";",["plaintext"]="Grants gold when nearby enemy minions die, Health Regen and Mana Regen",["from"]={"1004","3301","1006"},["into"]={"2302","3069"},["image"]={["full"]="3096.png",["sprite"]="item1.png",["group"]="item",["x"]=144,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=225,["purchasable"]=true,["total"]=850,["sell"]=340},["tags"]={"Active","GoldPer","HealthRegen","ManaRegen"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="0",["Effect2Amount"]="6",["Effect3Amount"]="10",["Effect4Amount"]="10"},["depth"]=2},["3097"]={["name"]="Targon's Brace",["group"]="GoldBase",["description"]="<stats>+175 Health<br>+50% Base Health Regen <br>+2 Gold per 10 seconds </stats><br><br><unique>UNIQUE Passive - Spoils of War:</unique> Melee basic attacks execute minions below 240 Health. Killing a minion heals the owner and the nearest allied champion for 50 Health and grants them kill Gold.<br><br>These effects require a nearby allied champion. Recharges every 30 seconds. Max 3 charges.<br><br><groupLimit>Limited to 1 Gold Income Item.</groupLimit>",["colloq"]=";",["plaintext"]="Periodically kill enemy minions to heal and grant gold to a nearby ally",["from"]={"3302","1006"},["into"]={"2303","3401"},["image"]={["full"]="3097.png",["sprite"]="item1.png",["group"]="item",["x"]=192,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=350,["purchasable"]=true,["total"]=850,["sell"]=340},["tags"]={"Aura","GoldPer","Health","HealthRegen"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=175},["effect"]={["Effect1Amount"]="240",["Effect2Amount"]="50",["Effect3Amount"]="30",["Effect4Amount"]="3",["Effect5Amount"]="2"},["depth"]=2},["3098"]={["name"]="Frostfang",["group"]="GoldBase",["description"]="<stats>+15 Ability Power<br>+2 Gold per 10 seconds<br><mana>+100% Base Mana Regen </mana></stats><br><br><unique>UNIQUE Passive - Tribute:</unique> Spells and basic attacks against champions or buildings deal 15 additional damage and grant 15 Gold. This can occur up to 3 times every 30 seconds. Killing a minion disables this passive for 12 seconds.<br><br><groupLimit>Limited to 1 Gold Income Item.</groupLimit>",["colloq"]=";",["plaintext"]="Grants gold when you damage an enemy with a Spell or Attack",["from"]={"3303","1004"},["into"]={"2301","3092"},["image"]={["full"]="3098.png",["sprite"]="item1.png",["group"]="item",["x"]=240,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=375,["purchasable"]=true,["total"]=850,["sell"]=340},["tags"]={"Active","GoldPer","ManaRegen","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=15},["effect"]={["Effect1Amount"]="15",["Effect2Amount"]="15",["Effect3Amount"]="12",["Effect4Amount"]="3",["Effect5Amount"]="30",["Effect6Amount"]="2"},["depth"]=2},["3100"]={["name"]="Lich Bane",["description"]="<stats>+80 Ability Power<br>+7% Movement Speed<br>+10% Cooldown Reduction<br><mana>+250 Mana</mana></stats><br><br><unique>UNIQUE Passive - Spellblade:</unique> After using an ability, the next basic attack deals 75% Base Attack Damage (+50% of Ability Power) bonus magic damage on hit (1.5 second cooldown).",["colloq"]=";",["plaintext"]="Grants a bonus to next attack after spell cast",["from"]={"3057","3113","1026"},["into"]={},["image"]={["full"]="3100.png",["sprite"]="item1.png",["group"]="item",["x"]=288,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=3200,["sell"]=2240},["tags"]={"CooldownReduction","Mana","NonbootsMovement","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMPPoolMod"]=250,["FlatMagicDamageMod"]=80,["PercentMovementSpeedMod"]=0.07},["effect"]={["Effect1Amount"]="0.75",["Effect2Amount"]="0.5",["Effect3Amount"]="1.5"},["depth"]=3},["3101"]={["name"]="Stinger",["description"]="<stats>+50% Attack Speed</stats><br><br><unique>UNIQUE Passive:</unique> +10% Cooldown Reduction",["colloq"]=";",["plaintext"]="Increased Attack Speed and Cooldown Reduction",["from"]={"1042","1042"},["into"]={"3115","3137"},["image"]={["full"]="3101.png",["sprite"]="item1.png",["group"]="item",["x"]=336,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=600,["purchasable"]=true,["total"]=1200,["sell"]=840},["tags"]={"AttackSpeed","CooldownReduction"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["PercentAttackSpeedMod"]=0.5},["effect"]={["Effect1Amount"]="10"},["depth"]=2},["3102"]={["name"]="Banshee's Veil",["description"]="<stats>+500 Health<br>+70 Magic Resist<br>+100% Base Health Regeneration </stats><br><br><unique>UNIQUE Passive:</unique> Grants a spell shield that blocks the next enemy ability. This shield refreshes after no damage is taken from enemy champions for 40 seconds.",["colloq"]=";bv",["plaintext"]="Periodically blocks enemy abilities",["from"]={"3211","3801"},["into"]={},["image"]={["full"]="3102.png",["sprite"]="item1.png",["group"]="item",["x"]=384,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=1150,["purchasable"]=true,["total"]=2900,["sell"]=2030},["tags"]={"Health","HealthRegen","SpellBlock"},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=500,["FlatSpellBlockMod"]=70},["effect"]={["Effect1Amount"]="40",["Effect2Amount"]="45",["Effect3Amount"]="10"},["depth"]=3},["3104"]={["name"]="Lord Van Damm's Pillager",["description"]="<stats>+65 Attack Damage<br>+30% Critical Strike Chance</stats><br><br><unique>UNIQUE Passive:</unique> Critical Strikes cause enemies to bleed for an additional 150% of bonus Attack Damage as magic damage over 3 seconds and reveal them for the duration.",["colloq"]=";ie",["plaintext"]="Massively enhances critical strikes",["from"]={"3122","1037","1018"},["into"]={},["image"]={["full"]="3104.png",["sprite"]="item1.png",["group"]="item",["x"]=432,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=725,["purchasable"]=true,["total"]=3600,["sell"]=2520},["tags"]={"CriticalStrike","Damage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=false,["12"]=false,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=65,["FlatCritChanceMod"]=0.3},["effect"]={["Effect1Amount"]="1.5",["Effect2Amount"]="3"},["depth"]=3},["3105"]={["name"]="Aegis of the Legion",["description"]="<stats>+200 Health<br>+100% Base Health Regen <br>+20 Magic Resist</stats><br><br><aura>UNIQUE Aura - Legion:</aura> Grants nearby allies +10 Magic Resist.",["colloq"]=";",["plaintext"]="Improves defenses for nearby allies",["from"]={"1033","3801"},["into"]={"3190","3060"},["image"]={["full"]="3105.png",["sprite"]="item1.png",["group"]="item",["x"]=0,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=400,["purchasable"]=true,["total"]=1500,["sell"]=1050},["tags"]={"Aura","Health","HealthRegen","SpellBlock"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=200,["FlatSpellBlockMod"]=20},["effect"]={["Effect1Amount"]="10",["Effect2Amount"]="0.75"},["depth"]=3},["3108"]={["name"]="Fiendish Codex",["description"]="<stats>+25 Ability Power</stats><br><br><unique>UNIQUE Passive:</unique> +10% Cooldown Reduction",["colloq"]=";",["plaintext"]="Increases Ability Power and Cooldown Reduction",["from"]={"1052"},["into"]={"3174","3092","3115","3165","3152","3060"},["image"]={["full"]="3108.png",["sprite"]="item1.png",["group"]="item",["x"]=96,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=365,["purchasable"]=true,["total"]=800,["sell"]=560},["tags"]={"CooldownReduction","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=25},["effect"]={["Effect1Amount"]="-0.1"},["depth"]=2},["3110"]={["name"]="Frozen Heart",["description"]="<stats>+90 Armor<br>+20% Cooldown Reduction<br><mana>+400 Mana</mana></stats><br><br><aura>UNIQUE Aura:</aura> Reduces the Attack Speed of nearby enemies by 15%.",["colloq"]=";fh",["plaintext"]="Massively increases Armor and slows enemy basic attacks",["from"]={"3082","3024"},["into"]={},["image"]={["full"]="3110.png",["sprite"]="item1.png",["group"]="item",["x"]=144,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=800,["purchasable"]=true,["total"]=2800,["sell"]=1960},["tags"]={"Armor","Aura","CooldownReduction","Mana"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMPPoolMod"]=400,["FlatArmorMod"]=90},["effect"]={["Effect1Amount"]="-0.2",["Effect2Amount"]="-0.15"},["depth"]=3},["3111"]={["name"]="Mercury's Treads",["description"]="<stats>+25 Magic Resist</stats><br><br><unique>UNIQUE Passive - Enhanced Movement:</unique> +45 Movement Speed<br><unique>UNIQUE Passive - Tenacity:</unique> Reduces the duration of stuns, slows, taunts, fears, silences, blinds, polymorphs, and immobilizes by 30%.",["colloq"]=";",["plaintext"]="Increases Movement Speed and reduces duration of disabling effects",["from"]={"1001","1033"},["into"]={"1321","1323","1320","1322"},["image"]={["full"]="3111.png",["sprite"]="item1.png",["group"]="item",["x"]=192,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=350,["purchasable"]=true,["total"]=1100,["sell"]=770},["tags"]={"Boots","SpellBlock","Tenacity"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45,["FlatSpellBlockMod"]=25},["depth"]=2},["3112"]={["name"]="Orb of Winter",["description"]="<stats>+70 Magic Resist<br>+100% Base Health Regeneration </stats><br><br><unique>UNIQUE Passive:</unique> Grants a shield that absorbs up to 30 (+10 per level) damage. The shield will refresh after 9 seconds without receiving damage.",["colloq"]=";",["plaintext"]="Grants a shield when out of combat",["from"]={"1006","1006","1057"},["into"]={},["image"]={["full"]="3112.png",["sprite"]="item1.png",["group"]="item",["x"]=240,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=990,["purchasable"]=true,["total"]=2010,["sell"]=1407},["tags"]={"HealthRegen","SpellBlock"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=false,["12"]=true,["14"]=false},["stats"]={["FlatSpellBlockMod"]=70},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="10"},["depth"]=3},["3113"]={["name"]="Aether Wisp",["description"]="<stats>+30 Ability Power</stats><br><br><unique>UNIQUE Passive:</unique> +5% Movement Speed",["colloq"]=";",["plaintext"]="Increases Ability Power and Movement Speed",["from"]={"1052"},["into"]={"3290","1402","1410","1414","3100","3285","3504","3673"},["image"]={["full"]="3113.png",["sprite"]="item1.png",["group"]="item",["x"]=288,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=415,["purchasable"]=true,["total"]=850,["sell"]=595},["tags"]={"NonbootsMovement","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=30},["effect"]={["Effect1Amount"]="0.05"},["depth"]=2},["3114"]={["name"]="Forbidden Idol",["description"]="<stats><mana>+50% Base Mana Regen </mana></stats><br><br><unique>UNIQUE Passive:</unique> +10% Cooldown Reduction",["colloq"]=";",["plaintext"]="Increases Mana Regeneration and Cooldown Reduction",["from"]={"1004","1004"},["into"]={"3069","3165","3222","3504"},["image"]={["full"]="3114.png",["sprite"]="item1.png",["group"]="item",["x"]=336,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=300,["purchasable"]=true,["total"]=550,["sell"]=385},["tags"]={"CooldownReduction","ManaRegen"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="-0.1"},["depth"]=2},["3115"]={["name"]="Nashor's Tooth",["description"]="<stats>+50% Attack Speed<br>+80 Ability Power</stats><br><br><unique>UNIQUE Passive:</unique> +20% Cooldown Reduction<br><unique>UNIQUE Passive:</unique> Basic attacks deal 15 (+15% of Ability Power) bonus magic damage on hit.<br>",["colloq"]=";",["plaintext"]="Increases Attack Speed, Ability Power, and Cooldown Reduction",["from"]={"3101","3108"},["into"]={},["image"]={["full"]="3115.png",["sprite"]="item1.png",["group"]="item",["x"]=384,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=1000,["purchasable"]=true,["total"]=3000,["sell"]=2100},["tags"]={"AttackSpeed","CooldownReduction","OnHit","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=80,["PercentAttackSpeedMod"]=0.5},["depth"]=3},["3116"]={["name"]="Rylai's Crystal Scepter",["description"]="<stats>+400 Health<br>+100 Ability Power</stats><br><br><unique>UNIQUE Passive:</unique> Damaging spells and abilities apply a Movement Speed reduction to enemies based on the spell type:<br><br><passive>Single Target:</passive> 40% reduction for 1.5 seconds. <br><passive>Area of Effect:</passive> 40% reduction for 1 seconds.<br><passive>Damage over Time or Multi-hit:</passive> 20% reduction for 1 seconds.<br><passive>Summoned Minions:</passive> 20% reduction for 1 seconds.<br><br><rules>(If a spell fits in more than one category, it uses the weakest slow value.)</rules>",["colloq"]=";",["plaintext"]="Abilities slow enemies",["from"]={"1058","1052","1011"},["into"]={},["image"]={["full"]="3116.png",["sprite"]="item1.png",["group"]="item",["x"]=432,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=515,["purchasable"]=true,["total"]=3200,["sell"]=2240},["tags"]={"Health","Slow","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=400,["FlatMagicDamageMod"]=100},["effect"]={["Effect1Amount"]="-0.4",["Effect2Amount"]="-0.2",["Effect3Amount"]="1.5",["Effect4Amount"]="1",["Effect5Amount"]="1",["Effect6Amount"]="-0.4"},["depth"]=3},["3117"]={["name"]="Boots of Mobility",["description"]="<unique>UNIQUE Passive - Enhanced Movement:</unique> +25 Movement Speed. Increases to +115 Movement Speed when out of combat for 5 seconds.",["colloq"]=";",["plaintext"]="Greatly enhances Movement Speed when out of combat",["from"]={"1001"},["into"]={"1326","1328","1325","1327"},["image"]={["full"]="3117.png",["sprite"]="item1.png",["group"]="item",["x"]=0,["y"]=96,["w"]=48,["h"]=48},["gold"]={["base"]=600,["purchasable"]=true,["total"]=900,["sell"]=630},["tags"]={"Boots"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=115},["effect"]={["Effect1Amount"]="0",["Effect2Amount"]="0",["Effect3Amount"]="0",["Effect4Amount"]="0",["Effect5Amount"]="0",["Effect6Amount"]="0",["Effect7Amount"]="0",["Effect8Amount"]="25"},["depth"]=2},["3122"]={["name"]="Wicked Hatchet",["description"]="<stats>+20 Attack Damage<br>+10% Critical Strike Chance</stats><br><br><unique>UNIQUE Passive:</unique> Critical Strikes cause your target to bleed for an additional 60% of your bonus Attack Damage as magic damage over 3 seconds.",["colloq"]=";ie",["plaintext"]="Critical Strikes cause your target to bleed",["from"]={"1051","1036"},["into"]={"3104","3185"},["image"]={["full"]="3122.png",["sprite"]="item1.png",["group"]="item",["x"]=48,["y"]=96,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1200,["sell"]=840},["tags"]={"CriticalStrike","Damage","OnHit"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=false,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=20,["FlatCritChanceMod"]=0.1},["effect"]={["Effect1Amount"]="0.6",["Effect2Amount"]="3"},["depth"]=2},["3123"]={["name"]="Executioner's Calling",["description"]="<stats>+15 Attack Damage</stats><br><br><unique>UNIQUE Passive - Executioner:</unique> Physical damage inflicts <a href='GrievousWounds'>Grievous Wounds</a> on enemy champions for 3 seconds.",["colloq"]=";grievous",["plaintext"]="Overcomes enemies with high health gain",["from"]={"1036"},["into"]={"3033"},["image"]={["full"]="3123.png",["sprite"]="item3.png",["group"]="item",["x"]=288,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=800,["sell"]=560},["tags"]={"Damage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=15},["effect"]={["Effect1Amount"]="3"},["depth"]=2},["3124"]={["name"]="Guinsoo's Rageblade",["description"]="<stats>+30 Attack Damage<br>+40 Ability Power</stats><br><br><unique>UNIQUE Passive:</unique> Basic attacks grant +8% Attack Speed, +3 Attack Damage, and +4 Ability Power for 5 seconds (stacks up to 8 times, melee attacks grant 2 stacks). While you have 8 stacks, gain <unlockedPassive>Guinsoo's Rage</unlockedPassive>.<br><br><unlockedPassive>Guinsoo's Rage:</unlockedPassive> Basic attacks deal bonus magic damage on hit equal to 20 + 15% of bonus Attack Damage and 7.5% of Ability Power to the target and nearby enemy units.",["colloq"]=";",["plaintext"]="Increases Ability Power and Attack Damage",["from"]={"1026","1037"},["into"]={},["image"]={["full"]="3124.png",["sprite"]="item1.png",["group"]="item",["x"]=96,["y"]=96,["w"]=48,["h"]=48},["gold"]={["base"]=1075,["purchasable"]=true,["total"]=2800,["sell"]=1960},["tags"]={"AttackSpeed","Damage","OnHit","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=30,["FlatMagicDamageMod"]=40},["effect"]={["Effect1Amount"]="0.08",["Effect2Amount"]="4",["Effect3Amount"]="3",["Effect4Amount"]="5",["Effect5Amount"]="8",["Effect6Amount"]="0.15",["Effect7Amount"]="0.075",["Effect8Amount"]="20"},["depth"]=2},["3133"]={["name"]="Caulfield's Warhammer",["description"]="<stats>+25 Attack Damage</stats><br><br><unique>UNIQUE Passive:</unique> +10% Cooldown Reduction",["colloq"]=";",["plaintext"]="Attack Damage and Cooldown Reduction",["stacks"]=0,["from"]={"1036","1036"},["into"]={"3142","1400","1408","1412","3071","3508","3671","3812"},["image"]={["full"]="3133.png",["sprite"]="item3.png",["group"]="item",["x"]=336,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=400,["purchasable"]=true,["total"]=1100,["sell"]=770},["tags"]={"CooldownReduction","Damage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=25},["effect"]={["Effect1Amount"]="-0.1"},["depth"]=2},["3134"]={["name"]="Serrated Dirk",["description"]="<stats>+20 Attack Damage</stats><br><br><unique>UNIQUE Passive:</unique> +10 <a href='FlatArmorPen'>Armor Penetration</a><br><unique>UNIQUE Passive:</unique> After killing any unit, your next basic attack or single target spell deals +15 damage.",["colloq"]=";",["plaintext"]="Increases Attack Damage and Armor Penetration",["stacks"]=0,["from"]={"1036","1036"},["into"]={"3142","3147","3156"},["image"]={["full"]="3134.png",["sprite"]="item1.png",["group"]="item",["x"]=144,["y"]=96,["w"]=48,["h"]=48},["gold"]={["base"]=400,["purchasable"]=true,["total"]=1100,["sell"]=770},["tags"]={"ArmorPenetration","Damage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=20},["effect"]={["Effect1Amount"]="15",["Effect2Amount"]="10"},["depth"]=2},["3135"]={["name"]="Void Staff",["description"]="<stats>+80 Ability Power</stats><br><br><unique>UNIQUE Passive:</unique> +35% <a href='TotalMagicPen'>Magic Penetration</a>.",["colloq"]=";",["plaintext"]="Increases magic damage",["from"]={"1026","1052"},["into"]={},["image"]={["full"]="3135.png",["sprite"]="item1.png",["group"]="item",["x"]=192,["y"]=96,["w"]=48,["h"]=48},["gold"]={["base"]=1365,["purchasable"]=true,["total"]=2650,["sell"]=1855},["tags"]={"MagicPenetration","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=80},["effect"]={["Effect1Amount"]="0.35"},["depth"]=2},["3136"]={["name"]="Haunting Guise",["description"]="<stats>+25 Ability Power<br>+200 Health</stats><br><br><unique>UNIQUE Passive - Eyes of Pain:</unique> +15 <a href='FlatMagicPen'>Magic Penetration</a>",["colloq"]=";mask",["plaintext"]="Increases magic damage",["stacks"]=0,["from"]={"1028","1052"},["into"]={"3151"},["image"]={["full"]="3136.png",["sprite"]="item1.png",["group"]="item",["x"]=240,["y"]=96,["w"]=48,["h"]=48},["gold"]={["base"]=765,["purchasable"]=true,["total"]=1600,["sell"]=1120},["tags"]={"Health","MagicPenetration","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=200,["FlatMagicDamageMod"]=25},["effect"]={["Effect1Amount"]="15"},["depth"]=2},["3137"]={["name"]="Dervish Blade",["description"]="<stats>+50% Attack Speed<br>+45 Magic Resist<br>+10% Cooldown Reduction</stats><br><br><active>UNIQUE Active - Quicksilver:</active> Removes all debuffs, and if the champion is melee, also grants +50% bonus Movement Speed for 1 second (90 second cooldown).",["colloq"]=";",["plaintext"]="Activate to remove all debuffs and grant massive Movement Speed",["from"]={"3140","3101"},["into"]={},["image"]={["full"]="3137.png",["sprite"]="item1.png",["group"]="item",["x"]=288,["y"]=96,["w"]=48,["h"]=48},["gold"]={["base"]=200,["purchasable"]=true,["total"]=2700,["sell"]=1890},["tags"]={"Active","AttackSpeed","CooldownReduction","NonbootsMovement","SpellBlock"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=false,["12"]=false,["14"]=false},["stats"]={["PercentAttackSpeedMod"]=0.5,["FlatSpellBlockMod"]=45},["effect"]={["Effect1Amount"]="0.5",["Effect2Amount"]="1",["Effect3Amount"]="90"},["depth"]=3},["3139"]={["name"]="Mercurial Scimitar",["description"]="<stats>+75 Attack Damage<br>+35 Magic Resist<br>+10% Life Steal</stats><br><br><active>UNIQUE Active - Quicksilver:</active> Removes all debuffs and also grants +50% bonus Movement Speed for 1 second (90 second cooldown).",["colloq"]=";",["plaintext"]="Activate to remove all debuffs and grant massive Movement Speed",["from"]={"3140","1037","1053"},["into"]={},["image"]={["full"]="3139.png",["sprite"]="item1.png",["group"]="item",["x"]=336,["y"]=96,["w"]=48,["h"]=48},["gold"]={["base"]=625,["purchasable"]=true,["total"]=3700,["sell"]=2590},["tags"]={"Active","Damage","LifeSteal","NonbootsMovement","SpellBlock"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=75,["FlatSpellBlockMod"]=35,["PercentLifeStealMod"]=0.1},["effect"]={["Effect1Amount"]="0.5",["Effect2Amount"]="1",["Effect3Amount"]="90"},["depth"]=3},["3140"]={["name"]="Quicksilver Sash",["description"]="<stats>+30 Magic Resist</stats><br><br><active>UNIQUE Active - Quicksilver:</active> Removes all debuffs (90 second cooldown).",["colloq"]=";qss",["plaintext"]="Activate to remove all debuffs",["from"]={"1033"},["into"]={"3139","3137"},["image"]={["full"]="3140.png",["sprite"]="item1.png",["group"]="item",["x"]=384,["y"]=96,["w"]=48,["h"]=48},["gold"]={["base"]=850,["purchasable"]=true,["total"]=1300,["sell"]=910},["tags"]={"Active","SpellBlock"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatSpellBlockMod"]=30},["effect"]={["Effect1Amount"]="90"},["depth"]=2},["3142"]={["name"]="Youmuu's Ghostblade",["description"]="<stats>+65 Attack Damage<br>+10% Cooldown Reduction</stats><br><br><unique>UNIQUE Passive:</unique> +20 <a href='FlatArmorPen'>Armor Penetration</a><br><active>UNIQUE Active:</active> Grants +20% Movement Speed and +40% Attack Speed for 6 seconds (45 second cooldown).",["colloq"]=";",["plaintext"]="Activate to greatly increase Movement Speed and Attack Speed",["from"]={"3133","3134"},["into"]={},["image"]={["full"]="3142.png",["sprite"]="item1.png",["group"]="item",["x"]=0,["y"]=144,["w"]=48,["h"]=48},["gold"]={["base"]=1000,["purchasable"]=true,["total"]=3200,["sell"]=2240},["tags"]={"Active","ArmorPenetration","AttackSpeed","CooldownReduction","Damage","NonbootsMovement"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=65},["effect"]={["Effect1Amount"]="45",["Effect2Amount"]="20",["Effect3Amount"]="0.2",["Effect4Amount"]="0.4",["Effect5Amount"]="6"},["depth"]=3},["3143"]={["name"]="Randuin's Omen",["description"]="<stats>+500 Health<br>+60 Armor<br>-10% Damage taken from Critical Strikes</stats><br><br><unique>UNIQUE Passive - Cold Steel:</unique> When hit by basic attacks, reduces the attacker's Attack Speed by 15% (1 second duration).<br><active>UNIQUE Active:</active> Slows the Movement Speed of nearby enemy units by 35% for 4 seconds (60 second cooldown).",["colloq"]=";",["plaintext"]="Greatly increases defenses, activate to slow nearby enemies",["from"]={"3082","1011"},["into"]={},["image"]={["full"]="3143.png",["sprite"]="item1.png",["group"]="item",["x"]=48,["y"]=144,["w"]=48,["h"]=48},["gold"]={["base"]=900,["purchasable"]=true,["total"]=2900,["sell"]=2030},["tags"]={"Active","Armor","Health","Slow"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=500,["FlatArmorMod"]=60},["effect"]={["Effect1Amount"]="4",["Effect2Amount"]="0.1",["Effect3Amount"]="-0.35",["Effect4Amount"]="1"},["depth"]=3},["3144"]={["name"]="Bilgewater Cutlass",["description"]="<stats>+25 Attack Damage<br>+10% Life Steal</stats><br><br><active>UNIQUE Active:</active> Deals 100 magic damage and slows the target champion's Movement Speed by 25% for 2 seconds (90 second cooldown).",["colloq"]=";",["plaintext"]="Activate to deal magic damage and slow target champion",["from"]={"1053","1036"},["into"]={"3146","3153"},["image"]={["full"]="3144.png",["sprite"]="item1.png",["group"]="item",["x"]=96,["y"]=144,["w"]=48,["h"]=48},["gold"]={["base"]=400,["purchasable"]=true,["total"]=1650,["sell"]=1155},["tags"]={"Active","Damage","LifeSteal","Slow"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=25,["PercentLifeStealMod"]=0.1},["effect"]={["Effect1Amount"]="-0.25",["Effect2Amount"]="2",["Effect3Amount"]="90",["Effect4Amount"]="100"},["depth"]=3},["3145"]={["name"]="Hextech Revolver",["description"]="<stats>+40 Ability Power</stats><br><br><unique>UNIQUE Passive:</unique> +12% <a href='SpellVamp'>Spell Vamp</a>",["colloq"]=";",["plaintext"]="Increases Spell Vamp and Ability Power",["from"]={"1052","1052"},["into"]={"3146","3152"},["image"]={["full"]="3145.png",["sprite"]="item1.png",["group"]="item",["x"]=144,["y"]=144,["w"]=48,["h"]=48},["gold"]={["base"]=330,["purchasable"]=true,["total"]=1200,["sell"]=840},["tags"]={"SpellDamage","SpellVamp"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=40},["effect"]={["Effect1Amount"]="0.12"},["depth"]=2},["3146"]={["name"]="Hextech Gunblade",["description"]="<stats>+40 Attack Damage<br>+80 Ability Power</stats><br><br><unique>UNIQUE Passive:</unique> Heal for 15% of damage dealt. This is 33% as effective for Area of Effect damage.<br><active>UNIQUE Active:</active> Deals 150 (+40% of Ability Power) magic damage and slows the target champion's Movement Speed by 40% for 2 seconds (30 second cooldown).",["colloq"]=";",["plaintext"]="Increases Attack Damage and Ability Power, activate to slow a target",["from"]={"3144","3145"},["into"]={},["image"]={["full"]="3146.png",["sprite"]="item1.png",["group"]="item",["x"]=192,["y"]=144,["w"]=48,["h"]=48},["gold"]={["base"]=550,["purchasable"]=true,["total"]=3400,["sell"]=2380},["tags"]={"Active","Damage","LifeSteal","Slow","SpellDamage","SpellVamp"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=40,["FlatMagicDamageMod"]=80},["effect"]={["Effect1Amount"]="0.15"},["depth"]=4},["3147"]={["name"]="Duskblade of Draktharr",["description"]="<stats>+75 Attack Damage<br>+5% Movement Speed</stats><br><br><unique>UNIQUE Passive:</unique> +10 <a href='FlatArmorPen'>Armor Penetration</a><br><unique>UNIQUE Passive:</unique> Basic Attacks on an enemy champion apply <unlockedPassive>Nightfall</unlockedPassive> (120 second cooldown).<br><br><unlockedPassive>Nightfall:</unlockedPassive> After 2 seconds, deal physical damage equal to 90 plus 25% of the target's missing health. If you get a kill or assist on the target before Nightfall ends, the cooldown is refunded.",["colloq"]=";",["plaintext"]="Deals a delayed burst of damage on hit.",["from"]={"3134","1038"},["into"]={},["image"]={["full"]="3147.png",["sprite"]="item3.png",["group"]="item",["x"]=192,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=850,["purchasable"]=true,["total"]=3250,["sell"]=2275},["tags"]={"ArmorPenetration","Damage","NonbootsMovement","OnHit"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=75,["PercentMovementSpeedMod"]=0.05},["effect"]={["Effect1Amount"]="10",["Effect2Amount"]="0",["Effect3Amount"]="2",["Effect4Amount"]="0.25",["Effect5Amount"]="120",["Effect6Amount"]="60",["Effect7Amount"]="90"},["depth"]=3},["3151"]={["name"]="Liandry's Torment",["description"]="<stats>+80 Ability Power<br>+300 Health</stats><br><br><unique>UNIQUE Passive - Eyes of Pain:</unique> +15 <a href='FlatMagicPen'>Magic Penetration</a><br><unique>UNIQUE Passive:</unique> Spells burn enemies for 3 seconds, dealing bonus magic damage equal to 2% of their current Health per second. Burn damage is doubled against <a href='MovementImpaired'>movement-impaired</a> units.",["colloq"]=";mask",["plaintext"]="Spell damage burns enemies for a portion of their Health",["stacks"]=0,["from"]={"3136","1026"},["into"]={},["image"]={["full"]="3151.png",["sprite"]="item1.png",["group"]="item",["x"]=240,["y"]=144,["w"]=48,["h"]=48},["gold"]={["base"]=750,["purchasable"]=true,["total"]=3200,["sell"]=2240},["tags"]={"Health","MagicPenetration","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=300,["FlatMagicDamageMod"]=80},["effect"]={["Effect1Amount"]="15",["Effect2Amount"]="0.02",["Effect3Amount"]="3",["Effect4Amount"]="100",["Effect5Amount"]="2"},["depth"]=3},["3152"]={["name"]="Will of the Ancients",["description"]="<stats>+80 Ability Power<br>+10% Cooldown Reduction</stats><br><br><unique>UNIQUE Passive:</unique> Your spells and abilities heal you for 15% of the damage dealt, calculated before your target's resistances.  This effect is reduced to one third for AoE effects.",["colloq"]=";wota",["plaintext"]="Grants Spell Vamp and Ability Power",["from"]={"3145","3108"},["into"]={},["image"]={["full"]="3152.png",["sprite"]="item1.png",["group"]="item",["x"]=288,["y"]=144,["w"]=48,["h"]=48},["gold"]={["base"]=300,["purchasable"]=true,["total"]=2300,["sell"]=1610},["tags"]={"CooldownReduction","SpellDamage","SpellVamp"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=80},["effect"]={["Effect1Amount"]="0.15",["Effect2Amount"]="0.05"},["depth"]=3},["3153"]={["name"]="Blade of the Ruined King",["description"]="<stats>+25 Attack Damage<br>+40% Attack Speed<br>+10% Life Steal</stats><br><br><unique>UNIQUE Passive:</unique> Basic attacks deal 6% of the target's current Health in bonus physical damage (max 60 vs. monsters and minions) on hit. Life Steal applies to this damage.<br><active>UNIQUE Active:</active> Deals 10% of target champion's maximum Health (min. 100) as physical damage, heals for the same amount, and steals 25% of the target's Movement Speed for 3 seconds (90 second cooldown).",["colloq"]=";brk;bork;bork;bork;botrk",["plaintext"]="Deals damage based on target's Health, can steal Health and Movement Speed",["from"]={"3144","1043"},["into"]={},["image"]={["full"]="3153.png",["sprite"]="item1.png",["group"]="item",["x"]=336,["y"]=144,["w"]=48,["h"]=48},["gold"]={["base"]=750,["purchasable"]=true,["total"]=3400,["sell"]=2380},["tags"]={"Active","AttackSpeed","Damage","LifeSteal","NonbootsMovement","OnHit"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=25,["PercentAttackSpeedMod"]=0.4,["PercentLifeStealMod"]=0.1},["effect"]={["Effect1Amount"]="0.06"},["depth"]=4},["3155"]={["name"]="Hexdrinker",["description"]="<stats>+20 Attack Damage<br>+35 Magic Resist</stats><br><br><unique>UNIQUE Passive - Lifeline:</unique> Upon taking magic damage that would reduce Health below 30%, grants a shield that absorbs 110 to 280 (based on level) magic damage for 5 seconds (90 second cooldown).",["colloq"]=";",["plaintext"]="Increases Attack Damage and Magic Resist",["stacks"]=0,["from"]={"1036","1033"},["into"]={"3156"},["image"]={["full"]="3155.png",["sprite"]="item1.png",["group"]="item",["x"]=432,["y"]=144,["w"]=48,["h"]=48},["gold"]={["base"]=500,["purchasable"]=true,["total"]=1300,["sell"]=910},["tags"]={"Damage","SpellBlock"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=20,["FlatSpellBlockMod"]=35},["effect"]={["Effect1Amount"]="0.3",["Effect2Amount"]="110",["Effect3Amount"]="5",["Effect4Amount"]="90",["Effect5Amount"]="280",["Effect6Amount"]="100",["Effect7Amount"]="10"},["depth"]=2},["3156"]={["name"]="Maw of Malmortius",["description"]="<stats>+55 Attack Damage<br>+50 Magic Resist<br>+10 <a href='FlatArmorPen'>Armor Penetration</a></stats><br><br><unique>UNIQUE Passive - Lifeline:</unique> Upon taking magic damage that would reduce Health below 30%, grants a shield that absorbs magic damage equal to 300 + 1 per bonus Magic Resistance for 5 seconds (90 second cooldown).<br><unique>UNIQUE Passive - Lifegrip:</unique> When Lifeline triggers, gain +25% Attack Speed, +10% Spell Vamp, and +10% Life Steal until out of combat.",["colloq"]=";",["plaintext"]="Grants bonus Attack Damage when Health is low",["stacks"]=0,["from"]={"3155","3134"},["into"]={},["image"]={["full"]="3156.png",["sprite"]="item1.png",["group"]="item",["x"]=0,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=850,["purchasable"]=true,["total"]=3250,["sell"]=2275},["tags"]={"ArmorPenetration","Damage","LifeSteal","SpellBlock","SpellVamp"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=55,["FlatSpellBlockMod"]=50},["effect"]={["Effect1Amount"]="1",["Effect2Amount"]="35",["Effect3Amount"]="0.3",["Effect4Amount"]="300",["Effect5Amount"]="5",["Effect6Amount"]="90",["Effect7Amount"]="0",["Effect8Amount"]="0.25"},["depth"]=3},["3157"]={["name"]="Zhonya's Hourglass",["description"]="<stats>+100 Ability Power<br>+45 Armor  </stats><br><br><active>UNIQUE Active - Stasis:</active> Champion becomes invulnerable and untargetable for 2.5 seconds, but is unable to move, attack, cast spells, or use items during this time (90 second cooldown).",["colloq"]=";zhg;zonyas",["plaintext"]="Activate to become invincible but unable to take actions",["from"]={"3191","1058"},["into"]={},["image"]={["full"]="3157.png",["sprite"]="item1.png",["group"]="item",["x"]=48,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=1050,["purchasable"]=true,["total"]=3500,["sell"]=2450},["tags"]={"Active","Armor","SpellDamage"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatArmorMod"]=45,["FlatMagicDamageMod"]=100},["effect"]={["Effect1Amount"]="0",["Effect2Amount"]="2.5",["Effect3Amount"]="90"},["depth"]=3},["3158"]={["name"]="Ionian Boots of Lucidity",["description"]="<unique>UNIQUE Passive:</unique> +10% Cooldown Reduction<br><unique>UNIQUE Passive - Enhanced Movement:</unique> +45 Movement Speed<br><unique>UNIQUE Passive:</unique> Reduces Summoner Spell cooldowns by 10%<br><br><br><rules><font color='#FDD017'>''This item is dedicated in honor of Ionia's victory over Noxus in the Rematch for the Southern Provinces on 10 December, 20 CLE.''</font></rules>",["colloq"]=";",["plaintext"]="Increases Movement Speed and Cooldown Reduction",["from"]={"1001"},["into"]={"1331","1333","1330","1332"},["image"]={["full"]="3158.png",["sprite"]="item1.png",["group"]="item",["x"]=96,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=600,["purchasable"]=true,["total"]=900,["sell"]=630},["tags"]={"Boots","CooldownReduction"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMovementSpeedMod"]=45},["effect"]={["Effect1Amount"]="-0.1",["Effect2Amount"]="0.1"},["depth"]=2},["3165"]={["name"]="Morellonomicon",["description"]="<stats>+80 Ability Power<br>+20% Cooldown Reduction<br><mana>+100% Base Mana Regen </mana></stats><br><br><unique>UNIQUE Passive:</unique> Dealing magic damage to enemy champions below 40% Health inflicts <a href='GrievousWounds'>Grievous Wounds</a> for 4 seconds.",["colloq"]=";nmst;grievous",["plaintext"]="Greatly increases Ability Power and Cooldown Reduction",["from"]={"3108","3114","1052"},["into"]={},["image"]={["full"]="3165.png",["sprite"]="item1.png",["group"]="item",["x"]=192,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=615,["purchasable"]=true,["total"]=2400,["sell"]=1680},["tags"]={"CooldownReduction","ManaRegen","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=80},["effect"]={["Effect1Amount"]="20",["Effect2Amount"]="40",["Effect3Amount"]="4"},["depth"]=3},["3170"]={["name"]="Moonflair Spellblade",["description"]="<stats>+50 Ability Power<br>+50 Armor<br>+50 Magic Resist</stats><br><br><unique>UNIQUE Passive - Tenacity:</unique> Reduces the duration of stuns, slows, taunts, fears, silences, blinds, polymorphs, and immobilizes by 35%.",["colloq"]=";",["plaintext"]="Improves defense and reduces duration of disabling effects",["from"]={"3191","1057"},["into"]={},["image"]={["full"]="3170.png",["sprite"]="item1.png",["group"]="item",["x"]=432,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=580,["purchasable"]=true,["total"]=2500,["sell"]=1750},["tags"]={"Armor","SpellBlock","SpellDamage","Tenacity"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=false,["12"]=false,["14"]=false},["stats"]={["FlatArmorMod"]=50,["FlatMagicDamageMod"]=50,["FlatSpellBlockMod"]=50},["depth"]=3},["3174"]={["name"]="Athene's Unholy Grail",["description"]="<stats>+60 Ability Power<br>+25 Magic Resist<br>+20% Cooldown Reduction<br><mana>+100% Base Mana Regen </mana></stats><br><br><mana><unique>UNIQUE Passive:</unique> Restores 30% of maximum Mana on kill or assist.<br><unique>UNIQUE Passive - Mana Font:</unique> Restores 2% of missing Mana every 5 seconds.<br></mana>",["colloq"]=";",["plaintext"]="Restores maximum Mana on kill or assist",["from"]={"3108","3028","1052"},["into"]={},["image"]={["full"]="3174.png",["sprite"]="item1.png",["group"]="item",["x"]=96,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=465,["purchasable"]=true,["total"]=2700,["sell"]=1890},["tags"]={"CooldownReduction","ManaRegen","SpellBlock","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=60,["FlatSpellBlockMod"]=25},["effect"]={["Effect1Amount"]="20",["Effect2Amount"]="30",["Effect3Amount"]="2",["Effect4Amount"]="5"},["depth"]=3},["3180"]={["name"]="Odyn's Veil",["description"]="<stats>+350 Health<br>+350 Mana<br>+50 Magic Resist</stats><br><br><unique>UNIQUE Passive:</unique> Reduces and stores 10% of magic damage received. <br><active>UNIQUE Active:</active> Deals 200 + (stored magic) (max 400) magic damage to nearby enemy units (90 second cooldown).",["colloq"]=";",["plaintext"]="Improves defense, activate for area magic damage",["from"]={"1057","3010"},["into"]={},["image"]={["full"]="3180.png",["sprite"]="item1.png",["group"]="item",["x"]=192,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=480,["purchasable"]=true,["total"]=2400,["sell"]=1680},["tags"]={"Active","Health","Mana","SpellBlock"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=false,["12"]=false,["14"]=false},["stats"]={["FlatHPPoolMod"]=350,["FlatMPPoolMod"]=350,["FlatSpellBlockMod"]=50},["effect"]={["Effect1Amount"]="0.1",["Effect2Amount"]="200",["Effect3Amount"]="400",["Effect4Amount"]="90"},["depth"]=3},["3181"]={["name"]="Sanguine Blade",["description"]="<stats>+45 Attack Damage<br>+10% Life Steal</stats><br><br><unique>UNIQUE Passive:</unique> Basic attacks grant +6 Attack Damage and +1% Life Steal for 8 seconds on hit (effect stacks up to 5 times).",["colloq"]=";",["plaintext"]="Greatly increases Attack Damage and Life Steal",["from"]={"1037","1053"},["into"]={},["image"]={["full"]="3181.png",["sprite"]="item1.png",["group"]="item",["x"]=240,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=625,["purchasable"]=true,["total"]=2400,["sell"]=1680},["tags"]={"Damage","LifeSteal"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=false,["12"]=false,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=45,["PercentLifeStealMod"]=0.1},["effect"]={["Effect1Amount"]="6",["Effect2Amount"]="0.01",["Effect3Amount"]="8",["Effect4Amount"]="5"},["depth"]=3},["3184"]={["name"]="Entropy",["description"]="<stats>+275 Health<br>+55 Attack Damage</stats><br><br><unique>UNIQUE Passive - Rage:</unique> Basic attacks grant 20 Movement Speed for 2 seconds on hit. Kills grant 60 Movement Speed for 2 seconds. This Movement Speed bonus is halved for ranged champions.<br><active>UNIQUE Active:</active> For the next 5 seconds, basic attacks reduce the target's Movement Speed by 30% and deal 80 true damage over 2.5 seconds on hit (60 second cooldown).",["colloq"]=";",["plaintext"]="Attacks and kills give a small burst of speed, activate to slow enemies",["from"]={"3044","1037"},["into"]={},["image"]={["full"]="3184.png",["sprite"]="item1.png",["group"]="item",["x"]=288,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=475,["purchasable"]=true,["total"]=2600,["sell"]=1820},["tags"]={"Active","Damage","Health","NonbootsMovement","Slow"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=false,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=275,["FlatPhysicalDamageMod"]=55},["effect"]={["Effect1Amount"]="20",["Effect2Amount"]="2",["Effect3Amount"]="60",["Effect4Amount"]="2.5",["Effect5Amount"]="5",["Effect6Amount"]="-0.3",["Effect7Amount"]="80",["Effect8Amount"]="60"},["depth"]=3},["3185"]={["name"]="The Lightbringer",["description"]="<stats>+30 Attack Damage<br>+30% Critical Strike Chance</stats><br><br><unique>UNIQUE Passive:</unique> Critical Strikes cause enemies to bleed for an additional 90% of bonus Attack Damage as magic damage over 3 seconds and reveal them for the duration.<br><unique>UNIQUE Passive - Trap Detection:</unique> Nearby stealthed enemy traps are revealed.<br><active>UNIQUE Active - Hunter's Sight:</active> A stealth-detecting mist grants vision in the target area for 5 seconds, revealing enemy champions that enter for 3 seconds (60 second cooldown).",["colloq"]=";lb",["plaintext"]="Critical Strikes cause your target to bleed and be revealed",["from"]={"3122","1018"},["into"]={},["image"]={["full"]="3185.png",["sprite"]="item1.png",["group"]="item",["x"]=336,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=350,["purchasable"]=true,["total"]=2350,["sell"]=1645},["tags"]={"Active","CriticalStrike","Damage","OnHit","Stealth","Vision"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=false,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=30,["FlatCritChanceMod"]=0.3},["effect"]={["Effect1Amount"]="0.9",["Effect2Amount"]="3",["Effect3Amount"]="0",["Effect4Amount"]="5",["Effect5Amount"]="3",["Effect6Amount"]="60"},["depth"]=3},["3187"]={["name"]="Hextech Sweeper",["description"]="<stats>+225 Health<br>+250 Mana<br>+25 Armor<br>+20% Cooldown Reduction</stats><br><br><unique>UNIQUE Passive - Trap Detection:</unique> Nearby stealthed enemy traps are revealed.<br><active>UNIQUE Active - Hunter's Sight:</active> A stealth-detecting mist grants vision in the target area for 5 seconds, revealing enemy champions that enter for 3 seconds (60 second cooldown).",["colloq"]=";",["plaintext"]="Activate to reveal a nearby area of the map",["from"]={"3024","3067"},["into"]={},["image"]={["full"]="3187.png",["sprite"]="item1.png",["group"]="item",["x"]=384,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=350,["purchasable"]=true,["total"]=2150,["sell"]=1505},["tags"]={"Active","Armor","CooldownReduction","Health","Mana","Stealth","Vision"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=false,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=225,["FlatMPPoolMod"]=250,["FlatArmorMod"]=25},["effect"]={["Effect1Amount"]="-0.2",["Effect2Amount"]="0",["Effect3Amount"]="0",["Effect4Amount"]="5",["Effect5Amount"]="3",["Effect6Amount"]="60"},["depth"]=3},["3190"]={["name"]="Locket of the Iron Solari",["description"]="<stats>+400 Health<br>+100% Base Health Regen <br>+20 Magic Resist<br>+10% Cooldown Reduction</stats><br><br><active>UNIQUE Active:</active> Grants a shield to nearby allies for 2 seconds that absorbs up to 75 (+15 per level) damage (60 second cooldown).<br><aura>UNIQUE Aura - Legion:</aura> Grants nearby allies +15 Magic Resist.",["colloq"]=";",["plaintext"]="Activate to shield nearby allies from damage",["from"]={"3105","3067"},["into"]={},["image"]={["full"]="3190.png",["sprite"]="item1.png",["group"]="item",["x"]=432,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=200,["purchasable"]=true,["total"]=2500,["sell"]=1750},["tags"]={"Active","Aura","CooldownReduction","Health","HealthRegen","SpellBlock"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=400,["FlatSpellBlockMod"]=20},["effect"]={["Effect1Amount"]="15",["Effect2Amount"]="0.75",["Effect3Amount"]="2",["Effect4Amount"]="75",["Effect5Amount"]="15",["Effect6Amount"]="60"},["depth"]=4},["3191"]={["name"]="Seeker's Armguard",["description"]="<stats>+30 Armor<br>+20 Ability Power</stats><br><br><unique>UNIQUE Passive:</unique> Killing a unit grants 0.5 bonus Armor and Ability Power. This bonus stacks up to 30 times.",["colloq"]=";",["plaintext"]="Increases Armor and Ability Power",["from"]={"1029","1052","1029"},["into"]={"3090","3157","3170"},["image"]={["full"]="3191.png",["sprite"]="item1.png",["group"]="item",["x"]=0,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=165,["purchasable"]=true,["total"]=1200,["sell"]=840},["tags"]={"Armor","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatArmorMod"]=30,["FlatMagicDamageMod"]=20},["effect"]={["Effect1Amount"]="0.5",["Effect2Amount"]="30"},["depth"]=2},["3196"]={["name"]="The Hex Core mk-1",["description"]="<stats>+3 Ability Power per level<br>+15 Mana per level</stats><br><br><unique>UNIQUE Passive - Progress:</unique> Viktor can upgrade one of his basic spells.",["colloq"]=";viktor",["plaintext"]="Allows Viktor to improve an ability of his choice",["from"]={"3200"},["into"]={"3197"},["requiredChampion"]="Viktor",["image"]={["full"]="3196.png",["sprite"]="item1.png",["group"]="item",["x"]=48,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=1000,["purchasable"]=true,["total"]=1000,["sell"]=700},["tags"]={"Mana","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="3",["Effect2Amount"]="15"},["depth"]=2},["3197"]={["name"]="The Hex Core mk-2",["description"]="<stats>+6 Ability Power per level<br>+20 Mana per level</stats><br><br><unique>UNIQUE Passive - Progress:</unique> Viktor can upgrade one of his basic spells.",["colloq"]=";viktor",["plaintext"]="Allows Viktor to improve an ability of his choice",["from"]={"3196"},["into"]={"3198"},["requiredChampion"]="Viktor",["image"]={["full"]="3197.png",["sprite"]="item1.png",["group"]="item",["x"]=96,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=1000,["purchasable"]=true,["total"]=2000,["sell"]=1400},["tags"]={"Mana","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="6",["Effect2Amount"]="20"},["depth"]=3},["3198"]={["name"]="Perfect Hex Core",["description"]="<stats>+10 Ability Power per level<br>+25 Mana per level</stats><br><br><unique>UNIQUE Passive - Glorious Evolution:</unique> Viktor has reached the pinnacle of his power, upgrading Chaos Storm in addition to his basic spells.",["colloq"]=";viktor",["plaintext"]="Allows Viktor to improve an ability of his choice",["from"]={"3197"},["requiredChampion"]="Viktor",["into"]={},["image"]={["full"]="3198.png",["sprite"]="item1.png",["group"]="item",["x"]=144,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=1000,["purchasable"]=true,["total"]=3000,["sell"]=2100},["tags"]={"Mana","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="10",["Effect2Amount"]="25"},["depth"]=4},["3200"]={["name"]="Prototype Hex Core",["description"]="<stats>+1 Ability Power per level<br>+10 Mana per level</stats><br><br><unique>UNIQUE Passive - Progress:</unique> This item can be upgraded three times to enhance Viktor's basic abilities.",["colloq"]=";viktor",["plaintext"]="Increases Ability Power and can be upgraded to improve Viktor's abilities",["into"]={"3196"},["specialRecipe"]=1,["inStore"]=false,["requiredChampion"]="Viktor",["image"]={["full"]="3200.png",["sprite"]="item1.png",["group"]="item",["x"]=192,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=0,["purchasable"]=false,["total"]=0,["sell"]=0},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="1",["Effect2Amount"]="10"}},["3211"]={["name"]="Spectre's Cowl",["description"]="<stats>+200 Health<br>+35 Magic Resist</stats><br><br><unique>UNIQUE Passive:</unique> Grants 150% Base Health Regen for up to 10 seconds after taking damage from an enemy champion.",["colloq"]=";hat",["plaintext"]="Improves defense and grants regeneration upon being damage",["from"]={"1028","1033"},["into"]={"3065","3102"},["image"]={["full"]="3211.png",["sprite"]="item1.png",["group"]="item",["x"]=240,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=250,["purchasable"]=true,["total"]=1100,["sell"]=770},["tags"]={"Health","HealthRegen","SpellBlock"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=200,["FlatSpellBlockMod"]=35},["effect"]={["Effect1Amount"]="1.5",["Effect2Amount"]="10"},["depth"]=2},["3222"]={["name"]="Mikael's Crucible",["description"]="<stats>+35 Magic Resist<br>+10% Cooldown Reduction<br><mana>+100% Base Mana Regen </mana></stats><br><br><mana><unique>UNIQUE Passive - Mana Font:</unique> Restores 2% of missing Mana every 5 seconds.</mana><br><active>UNIQUE Active:</active> Removes all stuns, roots, taunts, fears, silences, and slows on an allied champion and heals that champion for 150 (+10% of maximum Health) (180 second cooldown).",["colloq"]=";",["plaintext"]="Activate to heal and remove all disabling effects from an allied champion",["from"]={"3028","3114"},["into"]={},["image"]={["full"]="3222.png",["sprite"]="item1.png",["group"]="item",["x"]=288,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=750,["purchasable"]=true,["total"]=2300,["sell"]=1610},["tags"]={"Active","CooldownReduction","ManaRegen","SpellBlock"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatSpellBlockMod"]=35},["effect"]={["Effect1Amount"]="2",["Effect2Amount"]="5"},["depth"]=3},["3240"]={["name"]="Enchantment: Furor",["group"]="BootsFuror",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Furor bonus.<br><br><unique>UNIQUE Passive - Furor:</unique> Upon dealing damage with a single target spell or attack (on hit), grants +12% Movement Speed that decays over 2 seconds.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="",["hideFromAll"]=true,["into"]={},["image"]={["full"]="3240.png",["sprite"]="item3.png",["group"]="item",["x"]=240,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=450,["sell"]=315},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={}},["3241"]={["name"]="Enchantment: Alacrity",["group"]="BootsAlacrity",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Alacrity bonus. <br><br><unique>UNIQUE Passive - Alacrity:</unique> +20 Movement Speed<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="",["hideFromAll"]=true,["into"]={},["image"]={["full"]="3241.png",["sprite"]="item3.png",["group"]="item",["x"]=288,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=450,["sell"]=315},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={}},["3242"]={["name"]="Enchantment: Captain",["group"]="BootsCaptain",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Captain bonus.<br><br><unique>UNIQUE Passive - Captain:</unique> Grants +10% Movement Speed to nearby approaching allied champions.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="",["hideFromAll"]=true,["into"]={},["image"]={["full"]="3242.png",["sprite"]="item3.png",["group"]="item",["x"]=336,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=450,["sell"]=315},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={}},["3243"]={["name"]="Enchantment: Distortion",["group"]="BootsDistortion",["description"]="<groupLimit>Limited to 1 of each enchantment type.</groupLimit><br>Enchants boots to have Distortion bonus.<br><br><unique>UNIQUE Passive - Distortion:</unique> Teleport, Flash, and Ghost summoner spell cooldowns are reduced by 15% and are granted additional mobility: <br><br><font color='#FFDD00'>Ghost:</font> Grants 40% Movement Speed from 27%.<br><font color='#FFDD00'>Flash:</font> 20% Movement Speed bonus for 1 second after cast.<br><font color='#FFDD00'>Teleport:</font> 30% Movement Speed bonus for 3 seconds after use.<br><br><rules>(Unique Passives with the same name don't stack.)</rules>",["colloq"]=";",["plaintext"]="",["hideFromAll"]=true,["into"]={},["image"]={["full"]="3243.png",["sprite"]="item3.png",["group"]="item",["x"]=384,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=450,["sell"]=315},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={}},["3285"]={["name"]="Luden's Echo",["description"]="<stats>+100 Ability Power<br>+10% Movement Speed</stats><br><br><unique>UNIQUE Passive - Echo:</unique> Gain charges upon moving or casting. At 100 charges, the next damaging  spell hit expends all charges to deal 100 (+10% of Ability Power) bonus magic damage to up to 4 targets on hit.",["colloq"]=";",["plaintext"]="Movement and casting builds charges that release chain lightning on next spell hit",["from"]={"1058","3113"},["into"]={},["image"]={["full"]="3285.png",["sprite"]="item2.png",["group"]="item",["x"]=288,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=1100,["purchasable"]=true,["total"]=3200,["sell"]=2240},["tags"]={"NonbootsMovement","OnHit","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=100,["PercentMovementSpeedMod"]=0.1},["effect"]={["Effect1Amount"]="100",["Effect2Amount"]="100",["Effect3Amount"]="4",["Effect4Amount"]="0.1",["Effect5Amount"]="35",["Effect6Amount"]="10"},["depth"]=3},["3301"]={["name"]="Ancient Coin",["group"]="GoldBase",["description"]="<stats><mana>+25% Base Mana Regen </mana></stats><br><br><unique>UNIQUE Passive - Favor:</unique> Being near a minion's death without dealing the killing blow grants 4 Gold and 5 Health.<br><br><groupLimit>Limited to 1 Gold Income Item.</groupLimit><br><br><i><font color='#447777'>''Gold dust rises from the desert and clings to the coin.'' - Historian Shurelya, 11 November, 23 CLE</font></i>",["colloq"]=";",["plaintext"]="Grants gold when nearby minions die that you didn't kill",["into"]={"3096"},["image"]={["full"]="3301.png",["sprite"]="item2.png",["group"]="item",["x"]=144,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=350,["purchasable"]=true,["total"]=350,["sell"]=140},["tags"]={"GoldPer","Lane","ManaRegen"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="4",["Effect2Amount"]="5"}},["3302"]={["name"]="Relic Shield",["group"]="GoldBase",["description"]="<stats>+75 Health<br>+2 Gold per 10 seconds </stats><br><br><unique>UNIQUE Passive - Spoils of War:</unique> Melee basic attacks execute minions below 200 Health. Killing a minion heals the owner and the nearest allied champion for 40 Health and grants them kill Gold.<br><br>These effects require a nearby allied champion. Recharges every 60 seconds. Max 2 charges. <br><br><groupLimit>Limited to 1 Gold Income Item.</groupLimit>",["colloq"]=";",["plaintext"]="Kill minions periodically to heal and grant gold to a nearby ally",["into"]={"3097"},["image"]={["full"]="3302.png",["sprite"]="item2.png",["group"]="item",["x"]=192,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=350,["purchasable"]=true,["total"]=350,["sell"]=140},["tags"]={"Aura","GoldPer","Health","Lane"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=75},["effect"]={["Effect1Amount"]="200",["Effect2Amount"]="40",["Effect3Amount"]="2"}},["3303"]={["name"]="Spellthief's Edge",["group"]="GoldBase",["description"]="<stats>+5 Ability Power<br>+2 Gold per 10 seconds<br><mana>+25% Base Mana Regen </mana></stats><br><br><unique>UNIQUE Passive - Tribute:</unique> Spells and basic attacks against champions or buildings deal 10 additional damage and grant 8 Gold. This can occur up to 3 times every 30 seconds. Killing a minion disables this passive for 12 seconds.<br><br><groupLimit>Limited to 1 Gold Income Item.</groupLimit>",["colloq"]=";",["plaintext"]="Grants gold when you attack enemies",["into"]={"3098"},["image"]={["full"]="3303.png",["sprite"]="item2.png",["group"]="item",["x"]=240,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=350,["purchasable"]=true,["total"]=350,["sell"]=140},["tags"]={"GoldPer","Lane","ManaRegen","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=5},["effect"]={["Effect1Amount"]="10",["Effect2Amount"]="8",["Effect3Amount"]="12",["Effect4Amount"]="3",["Effect5Amount"]="30",["Effect6Amount"]="2"}},["3340"]={["name"]="Warding Totem (Trinket)",["group"]="RelicBase",["description"]="<groupLimit>Limited to 1 Trinket.</groupLimit><br><br><active>Active:</active> Consume a charge to place an invisible <font color='#BBFFFF'>Stealth Ward</font> which reveals the surrounding area for <levelScale>60 - 120</levelScale> seconds. <br><br>Stores one charge every <levelScale>180 - 90</levelScale> seconds, up to 2 maximum charges.<br><br>Ward duration and recharge time gradually improve with level.<br><br><rules>(Limit 3 <font color='#BBFFFF'>Stealth Wards</font> on the map per player. Switching to a <font color='#BBFFFF'>Lens</font> type trinket will disable <font color='#BBFFFF'>Trinket</font> use for 120 seconds.)</rules>",["colloq"]="yellow;",["plaintext"]="Periodically place a Stealth Ward",["into"]={},["image"]={["full"]="3340.png",["sprite"]="item2.png",["group"]="item",["x"]=288,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=0,["purchasable"]=true,["total"]=0,["sell"]=0},["tags"]={"Active","Jungle","Lane","Trinket","Vision"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="60",["Effect2Amount"]="180",["Effect3Amount"]="120",["Effect4Amount"]="90",["Effect5Amount"]="2",["Effect6Amount"]="9",["Effect7Amount"]="30",["Effect8Amount"]="120"}},["3341"]={["name"]="Sweeping Lens (Trinket)",["group"]="RelicBase",["description"]="<groupLimit>Limited to 1 Trinket.</groupLimit><br><br><active>Active:</active> Scans an area for 6 seconds, warning against hidden hostile units and revealing / disabling invisible traps and wards (90 to 60 second cooldown).<br><br>Cast range and sweep radius gradually improve with level.<br><br><rules>(Switching to a <font color='#BBFFFF'>Totem</font>-type trinket will disable <font color='#BBFFFF'>Trinket</font> use for 120 seconds.)</rules>",["colloq"]="red;",["plaintext"]="Detects and disables nearby invisible wards and traps",["into"]={},["image"]={["full"]="3341.png",["sprite"]="item2.png",["group"]="item",["x"]=336,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=0,["purchasable"]=true,["total"]=0,["sell"]=0},["tags"]={"Active","Jungle","Trinket","Vision"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="6",["Effect2Amount"]="90",["Effect3Amount"]="400",["Effect4Amount"]="60",["Effect5Amount"]="1500",["Effect6Amount"]="9",["Effect7Amount"]="30",["Effect8Amount"]="120"}},["3345"]={["name"]="Soul Anchor (Trinket)",["group"]="RelicBase",["description"]="<groupLimit>Limited to 1 Trinket.</groupLimit><br><br><active>Active:</active> Consumes a charge to instantly revive at your Summoner Platform and grants 125% Movement Speed that decays over 12 seconds.<br><br><rules>Additional charges are gained at levels 9 and 14.</rules><br><br><font color='#BBFFFF'>(Max: 2 charges)</font></rules><br><br>",["colloq"]="",["plaintext"]="Consumes charge to revive champion.",["inStore"]=false,["into"]={},["image"]={["full"]="3345.png",["sprite"]="item2.png",["group"]="item",["x"]=432,["y"]=0,["w"]=48,["h"]=48},["gold"]={["base"]=0,["purchasable"]=false,["total"]=0,["sell"]=0},["tags"]={"Active","Trinket","Vision"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=false,["12"]=false,["14"]=false},["stats"]={}},["3348"]={["name"]="Hextech Sweeper",["group"]="RelicBase",["description"]="<active>UNIQUE Active - Hunter's Sight:</active> A stealth-detecting mist grants vision in the target area for 5 seconds, revealing traps and enemy champions that enter for 3 seconds (90 second cooldown).",["colloq"]=";",["plaintext"]="Activate to reveal a nearby area of the map",["inStore"]=false,["into"]={},["image"]={["full"]="3348.png",["sprite"]="item2.png",["group"]="item",["x"]=432,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=0,["purchasable"]=false,["total"]=0,["sell"]=0},["tags"]={"Active","Stealth","Trinket","Vision"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=false,["12"]=false,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="0",["Effect2Amount"]="0",["Effect3Amount"]="0",["Effect4Amount"]="5",["Effect5Amount"]="3",["Effect6Amount"]="90"}},["3361"]={["name"]="Greater Stealth Totem (Trinket)",["group"]="RelicBase",["description"]="<groupLimit>Limited to 1 Trinket.</groupLimit><levelLimit> *Level 9+ required to upgrade.</levelLimit><stats></stats><br><br><unique>UNIQUE Active:</unique> Consume a charge to place an invisible ward that reveals the surrounding area for 180 seconds.  Stores a charge every 60 seconds, up to 2 total. Limit 3 <font color='#BBFFFF'>Stealth Wards</font> on the map per player.<br><br><rules>(Trinkets cannot be used in the first 30 seconds of a game. Selling a Trinket will disable Trinket use for 120 seconds).</rules>",["colloq"]="yellow;",["plaintext"]="Periodically place a Stealth Ward",["inStore"]=false,["into"]={},["image"]={["full"]="3361.png",["sprite"]="item2.png",["group"]="item",["x"]=0,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=250,["purchasable"]=false,["total"]=250,["sell"]=175},["tags"]={"Active","Trinket","Vision"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=false,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="180",["Effect2Amount"]="60",["Effect3Amount"]="0",["Effect4Amount"]="0",["Effect5Amount"]="0",["Effect6Amount"]="9",["Effect7Amount"]="30",["Effect8Amount"]="120"}},["3362"]={["name"]="Greater Vision Totem (Trinket)",["group"]="RelicBase",["description"]="<groupLimit>Limited to 1 Trinket.</groupLimit><levelLimit> *Level 9+ required to upgrade.</levelLimit><stats></stats><br><br><unique>UNIQUE Active:</unique> Places a visible ward that reveals the surrounding area and invisible units in the area until killed (120 second cooldown). Limit 1 <font color='#BBFFFF'>Vision Ward</font> on the map per player.<br><br><rules>(Trinkets cannot be used in the first 30 seconds of a game. Selling a Trinket will disable Trinket use for 120 seconds).</rules>",["colloq"]="yellow;",["plaintext"]="Periodically place a Vision Ward",["inStore"]=false,["into"]={},["image"]={["full"]="3362.png",["sprite"]="item2.png",["group"]="item",["x"]=48,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=250,["purchasable"]=false,["total"]=250,["sell"]=175},["tags"]={"Active","Trinket","Vision"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=false,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="120",["Effect2Amount"]="0",["Effect3Amount"]="0",["Effect4Amount"]="0",["Effect5Amount"]="0",["Effect6Amount"]="9",["Effect7Amount"]="30",["Effect8Amount"]="120"}},["3363"]={["name"]="Farsight Alteration",["group"]="RelicBase",["description"]="<levelLimit>* Level 9+ required to upgrade.</levelLimit><br><br>Alters the <font color='#FFFFFF'>Warding Totem</font> Trinket:<br><br><stats><font color='#00FF00'>+</font> Massively increased cast range (+650%)<br><font color='#00FF00'>+</font> Infinite duration and does not count towards ward limit<br><font color='#00FF00'>+</font> 33% reduced cooldown<br><font color='#FF0000'>-</font> <font color='#FF6600'>Ward is visible, fragile, untargetable by allies</font><br><font color='#FF0000'>-</font> <font color='#FF6600'>45% reduced ward vision radius</font><br><font color='#FF0000'>-</font> <font color='#FF6600'>Cannot store charges</font></stats>",["colloq"]="blue; totem;",["plaintext"]="Grants increased range and reveals the targetted area",["into"]={},["image"]={["full"]="3363.png",["sprite"]="item2.png",["group"]="item",["x"]=96,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=0,["purchasable"]=true,["total"]=0,["sell"]=0},["tags"]={"Active","Trinket","Vision"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="4000",["Effect2Amount"]="2",["Effect3Amount"]="5",["Effect4Amount"]="120",["Effect5Amount"]="60",["Effect6Amount"]="9",["Effect7Amount"]="30",["Effect8Amount"]="120"}},["3364"]={["name"]="Oracle Alteration",["group"]="RelicBase",["description"]="<levelLimit>* Level 9+ required to upgrade.</levelLimit><stats></stats><br><br>Alters the <font color='#FFFFFF'>Sweeping Lens</font> Trinket:<br><br><stats><font color='#00FF00'>+</font> Increased detection radius<br><font color='#00FF00'>+</font> Sweeping effect follows you for 10 seconds<br><font color='#FF0000'>-</font> <font color='#FF6600'>Cast range reduced to zero</font></stats>",["colloq"]="red; lens;",["plaintext"]="Disables nearby invisible wards and traps for a duration",["into"]={},["image"]={["full"]="3364.png",["sprite"]="item2.png",["group"]="item",["x"]=144,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=0,["purchasable"]=true,["total"]=0,["sell"]=0},["tags"]={"Active","Trinket","Vision"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="6",["Effect2Amount"]="10",["Effect3Amount"]="90",["Effect4Amount"]="60",["Effect5Amount"]="0",["Effect6Amount"]="9",["Effect7Amount"]="30",["Effect8Amount"]="120"}},["3401"]={["name"]="Face of the Mountain",["group"]="GoldBase",["description"]="<stats>+450 Health<br>+100% Base Health Regen <br>+10% Cooldown Reduction<br>+2 Gold per 10 seconds </stats><br><br><unique>UNIQUE Passive - Spoils of War:</unique> Melee basic attacks execute minions below 400 Health. Killing a minion heals the owner and the nearest allied champion for 50 Health and grants them kill Gold. These effects require a nearby allied champion. Recharges every 30 seconds. Max 4 charges.<br><unique>UNIQUE Active:</unique> Grant a shield to an ally equal to 10% of your maximum Health for 4 seconds. After 4 seconds, the shield explodes to slow nearby enemies by 40% for 2 seconds (60 second cooldown).<br><br><groupLimit>Limited to 1 Gold Income Item.</groupLimit>",["colloq"]=";",["plaintext"]="Shield an ally from damage based on your Health",["from"]={"3097","3067"},["into"]={},["image"]={["full"]="3401.png",["sprite"]="item2.png",["group"]="item",["x"]=192,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=550,["purchasable"]=true,["total"]=2200,["sell"]=880},["tags"]={"Active","CooldownReduction","GoldPer","Health","HealthRegen"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=450},["effect"]={["Effect1Amount"]="400",["Effect2Amount"]="50",["Effect3Amount"]="0.1",["Effect4Amount"]="4",["Effect5Amount"]="-0.4",["Effect6Amount"]="2",["Effect7Amount"]="60",["Effect8Amount"]="120"},["depth"]=3},["3460"]={["name"]="Golden Transcendence",["group"]="RelicBase",["description"]="<unique>Active:</unique> Use this trinket to teleport to one of the battle platforms. Can only be used from the summoning platform.<br><br><i><font color='#FDD017'>''It is at this magical precipice where a champion is dismantled, reforged, and empowered.''</font></i>",["colloq"]=";",["plaintext"]="",["inStore"]=false,["into"]={},["image"]={["full"]="3460.png",["sprite"]="item2.png",["group"]="item",["x"]=432,["y"]=144,["w"]=48,["h"]=48},["gold"]={["base"]=0,["purchasable"]=false,["total"]=0,["sell"]=0},["tags"]={"Active","Trinket"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=false,["12"]=false,["14"]=false},["stats"]={}},["3461"]={["name"]="Golden Transcendence (Disabled)",["group"]="RelicBase",["description"]="<unique>Active:</unique> Use this trinket to teleport to one of the battle platforms. Can only be used from the summoning platform.<br><br><i><font color='#FDD017'>''It is at this magical precipice where a champion is dismantled, reforged, and empowered.''</font></i>",["colloq"]=";",["plaintext"]="",["inStore"]=false,["into"]={},["image"]={["full"]="3461.png",["sprite"]="item3.png",["group"]="item",["x"]=144,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=0,["purchasable"]=false,["total"]=0,["sell"]=0},["tags"]={"Active","Trinket"},["maps"]={["1"]=false,["8"]=true,["10"]=false,["11"]=false,["12"]=false,["14"]=false},["stats"]={}},["3504"]={["name"]="Ardent Censer",["description"]="<stats>+40 Ability Power<br>+10% Cooldown Reduction<br><mana>+100% Base Mana Regen </mana></stats><br><br><unique>UNIQUE Passive:</unique> +8% Movement Speed<br><unique>UNIQUE Passive:</unique> Your heals and shields on another allied champion grant them 15% Attack Speed and 30 magic damage on-hit for 6 seconds.<br><br><rules>(This does not include regeneration effects or effects on yourself.)</rules>",["colloq"]="",["plaintext"]="Shield and heal effects on other units grant them Attack Speed and bonus damage briefly",["from"]={"3114","3113"},["into"]={},["image"]={["full"]="3504.png",["sprite"]="item2.png",["group"]="item",["x"]=0,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=800,["purchasable"]=true,["total"]=2200,["sell"]=1540},["tags"]={"CooldownReduction","ManaRegen","NonbootsMovement","SpellDamage"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=40},["effect"]={["Effect1Amount"]="0.08",["Effect2Amount"]="0.15",["Effect3Amount"]="6",["Effect4Amount"]="30"},["depth"]=3},["3508"]={["name"]="Essence Reaver",["description"]="<stats>+65 Attack Damage<br>+20% Critical Strike Chance</stats><br><br><unique>UNIQUE Passive:</unique> +10% Cooldown Reduction.<br><unique>UNIQUE Passive:</unique> Gain increasingly more Cooldown Reduction from Critical Strike Chance provided by other sources (maximum +20% additional Cooldown Reduction at 30% Critical Strike Chance).<br><unique>UNIQUE Passive:</unique> Critical strikes restore 3% of your maximum Mana pool.",["colloq"]=";",["plaintext"]="Critical Strike provides Cooldown Reduction and Mana",["from"]={"1038","3133","1018"},["into"]={},["image"]={["full"]="3508.png",["sprite"]="item2.png",["group"]="item",["x"]=48,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=400,["purchasable"]=true,["total"]=3600,["sell"]=2520},["tags"]={"CooldownReduction","CriticalStrike","Damage","ManaRegen"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=65,["FlatCritChanceMod"]=0.2},["effect"]={["Effect1Amount"]="0.1",["Effect2Amount"]="0.03",["Effect3Amount"]="0.2",["Effect4Amount"]="0.3",["Effect5Amount"]="0.016667",["Effect6Amount"]="0.166667"},["depth"]=3},["3512"]={["name"]="Zz'Rot Portal",["description"]="<stats>+60 Armor<br>+60 Magic Resist<br>+125% Base Health Regen <br></stats><br><unique>UNIQUE Passive - Point Runner:</unique> Builds up to +20% Movement Speed over 2 seconds while near turrets and fallen turrets.<br><active>UNIQUE Active:</active> Spawns a <a href='VoidGate'>Void Gate</a> for 150 seconds (150 second cooldown).<br><br>Every 4 seconds the gate makes a <a href='Voidspawn'>Voidspawn</a>. The first and every fourth Voidspawn gains 15% of maximum Health as damage.",["colloq"]=";Void Gate",["plaintext"]="Makes a Voidspawn generating Void Gate to push a lane with.",["from"]={"2053","1057"},["into"]={},["image"]={["full"]="3512.png",["sprite"]="item2.png",["group"]="item",["x"]=96,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=780,["purchasable"]=true,["total"]=2700,["sell"]=1890},["tags"]={"Active","Armor","HealthRegen","NonbootsMovement","SpellBlock"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatArmorMod"]=60,["FlatSpellBlockMod"]=60},["effect"]={["Effect1Amount"]="20",["Effect2Amount"]="4",["Effect3Amount"]="50",["Effect4Amount"]="20",["Effect5Amount"]="150",["Effect6Amount"]="150",["Effect7Amount"]="0.5",["Effect8Amount"]="0.15"},["depth"]=4},["3599"]={["name"]="The Black Spear",["group"]="TheBlackSpear",["description"]="<stats></stats><br><active>Active:</active> Offer to bind with an ally for the remainder of the game, becoming Oathsworn Allies. Oathsworn empowers you both while near one another.",["colloq"]=";spear",["plaintext"]="Kalista's spear that binds an Oathsworn Ally.",["requiredChampion"]="Kalista",["into"]={},["image"]={["full"]="3599.png",["sprite"]="item2.png",["group"]="item",["x"]=144,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=0,["purchasable"]=true,["total"]=0,["sell"]=0},["tags"]={"Active"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={}},["3671"]={["name"]="Enchantment: Warrior",["description"]="<stats>+60 Attack Damage<br>+10% Cooldown Reduction</stats>",["colloq"]=";",["plaintext"]="",["from"]={"3133"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="3671.png",["sprite"]="item3.png",["group"]="item",["x"]=384,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=525,["purchasable"]=true,["total"]=1625,["sell"]=1138},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=60},["depth"]=3},["3672"]={["name"]="Enchantment: Cinderhulk",["description"]="<stats>+400 Health<br>+15% Bonus Health</stats><br><br><unique>UNIQUE Passive - Immolate:</unique> Deals 15 (+0.6 per champion level) magic damage a second to nearby enemies while in combat. Deals 100% bonus damage to monsters. ",["colloq"]=";",["plaintext"]="",["from"]={"3751"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="3672.png",["sprite"]="item3.png",["group"]="item",["x"]=432,["y"]=384,["w"]=48,["h"]=48},["gold"]={["base"]=525,["purchasable"]=true,["total"]=1625,["sell"]=1138},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=400},["depth"]=3},["3673"]={["name"]="Enchantment: Runic Echoes",["description"]="<stats>+60 Ability Power<br>+7% Movement Speed</stats><br><br><unique>UNIQUE Passive - Echo:</unique> Gain charges upon moving or casting. At 100 charges, the next damaging spell hit expends all charges to deal 80 (+10% of Ability Power) bonus magic damage to up to 4 targets on hit.<br><br>This effect deals 250% damage to Large Monsters. Hitting a Large Monster with this effect will restore 18% of your missing Mana.",["colloq"]=";",["plaintext"]="",["from"]={"3113","1052"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="3673.png",["sprite"]="item3.png",["group"]="item",["x"]=0,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=340,["purchasable"]=true,["total"]=1625,["sell"]=1138},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatMagicDamageMod"]=60,["PercentMovementSpeedMod"]=0.07},["depth"]=3},["3674"]={["name"]="Enchantment: Devourer",["description"]="<stats>+40% Attack Speed<br>+30 Magic Damage on Hit</stats><br><br><unique>UNIQUE Passive - Devouring Spirit:</unique> Takedowns on large monsters and Champions increase the magic damage of this item by +1. Takedowns on Rift Scuttlers increase the magic damage of this item by +2. Takedowns on epic monsters increase the magic damage of this item by +5. At 30 Stacks, your Devourer becomes Sated, granting extra on Hit effects.",["colloq"]=";",["plaintext"]="",["from"]={"1043"},["hideFromAll"]=true,["into"]={},["image"]={["full"]="3674.png",["sprite"]="item3.png",["group"]="item",["x"]=48,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=450,["purchasable"]=true,["total"]=1450,["sell"]=1015},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["PercentAttackSpeedMod"]=0.4},["depth"]=3},["3706"]={["name"]="Stalker's Blade",["group"]="JungleItems",["description"]="<groupLimit>Limited to 1 Jungle item</groupLimit><br><br><stats>+10% Life Steal vs. Monsters<br><mana>+180% Base Mana Regen while in Jungle</mana></stats><br><br><unique>UNIQUE Passive - Chilling Smite:</unique> Smite can be cast on enemy champions, dealing reduced true damage and stealing 20% Movement Speed for 2 seconds. <br><unique>UNIQUE Passive - Tooth / Nail:</unique> Basic attacks deal 20 bonus damage vs. monsters. Damaging a monster with a spell or attack steals 30 Health over 5 seconds. Killing a Large Monster grants +30 bonus experience.",["colloq"]=";jungle;Jungle;jangle",["plaintext"]="Lets your Smite slow Champions",["from"]={"1039","1041"},["into"]={"1400","1401","1402","1403"},["image"]={["full"]="3706.png",["sprite"]="item2.png",["group"]="item",["x"]=192,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=300,["purchasable"]=true,["total"]=1000,["sell"]=700},["tags"]={"Jungle","LifeSteal","ManaRegen","NonbootsMovement","OnHit","Slow"},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=false,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.8",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="-0.2",["Effect7Amount"]="2",["Effect8Amount"]="3"},["depth"]=2},["3711"]={["name"]="Tracker's Knife",["group"]="JungleItems",["description"]="<groupLimit>Limited to 1 Jungle item</groupLimit><br><br><stats>+10% Life Steal vs. Monsters<br><mana>+180% Base Mana Regen while in Jungle</mana></stats><br><br><unique>UNIQUE Passive - Tooth / Nail:</unique> Basic attacks deal 20 bonus damage vs. monsters. Damaging a monster with a spell or attack steals 30 Health over 5 seconds. Killing a Large Monster grants +30 bonus experience.<br><active>UNIQUE Active - Warding:</active> Consumes a charge to place a <font color='#BBFFFF'>Stealth Ward</font> that reveals the surrounding area for 150 seconds. Holds up to 2 charges which refill upon visiting the shop. <br><br><rules>(A player may only have 3 <font color='#BBFFFF'>Stealth Wards</font> on the map at one time. Unique Passives with the same name don't stack.)</rules>",["colloq"]=";jungle;Jungle",["plaintext"]="Provides Stealth Wards over time",["from"]={"1039","1041"},["into"]={"1408","1409","1410","1411"},["image"]={["full"]="3711.png",["sprite"]="item2.png",["group"]="item",["x"]=432,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=300,["purchasable"]=true,["total"]=1000,["sell"]=700},["tags"]={"Active","Jungle","LifeSteal","ManaRegen","OnHit","Vision"},["maps"]={["1"]=false,["8"]=false,["10"]=false,["11"]=true,["12"]=false,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.8",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="3",["Effect7Amount"]="20",["Effect8Amount"]="30"},["depth"]=2},["3715"]={["name"]="Skirmisher's Sabre",["group"]="JungleItems",["description"]="<groupLimit>Limited to 1 Jungle item</groupLimit><br><br><stats>+10% Life Steal vs. Monsters<br><mana>+180% Base Mana Regen while in Jungle</mana></stats><br><br><passive>Passive - Challenging Smite:</passive> Smite can be cast on enemy champions, marking them for 4 seconds. While marked, basic attacks deal bonus true damage over 3 seconds, you have vision of them, and their damage to you is reduced by 20%.<br><unique>UNIQUE Passive - Tooth / Nail:</unique> Basic attacks deal 20 bonus damage vs. monsters. Damaging a monster with a spell or attack steals 30 Health over 5 seconds. Killing a Large Monster grants +30 bonus experience.",["colloq"]=";jungle;Jungle",["plaintext"]="Lets your Smite mark Champions, giving you combat power against them.",["from"]={"1039","1041"},["into"]={"1412","1413","1414","1415"},["image"]={["full"]="3715.png",["sprite"]="item2.png",["group"]="item",["x"]=96,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=300,["purchasable"]=true,["total"]=1000,["sell"]=700},["tags"]={"Jungle","LifeSteal","ManaRegen","OnHit"},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=false,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.8",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="3",["Effect7Amount"]="20",["Effect8Amount"]="18"},["depth"]=2},["3742"]={["name"]="Dead Man's Plate",["description"]="<stats>+500 Health<br>+50 Armor</stats><br><br><unique>UNIQUE Passive - Dreadnought:</unique> While moving, build stacks of Momentum, increasing movement speed by up to 60 at 100 stacks. Momentum quickly decays while under the effects of a stun, taunt, fear, silence, blind, polymorph, or immobilize effect, and slowly decays while slowed.<br><unique>UNIQUE Passive - Crushing Blow:</unique> Basic attacks discharge all Momentum, dealing 1 physical damage per 2 stacks. If 100 stacks are discharged, damage is doubled and the target is slowed by 50% decaying over 1 second (melee only).<br><br><flavorText>''There's only one way you'll get this armor from me...'' - forgotten namesake</flavorText>",["colloq"]=";juggernaut;dreadnought",["plaintext"]="Build momentum as you move around then smash into enemies.",["from"]={"1031","1011"},["into"]={},["image"]={["full"]="3742.png",["sprite"]="item3.png",["group"]="item",["x"]=48,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=1100,["purchasable"]=true,["total"]=2900,["sell"]=2030},["tags"]={"Armor","Bilgewater","Health","NonbootsMovement","OnHit"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=500,["FlatArmorMod"]=50},["effect"]={["Effect1Amount"]="60",["Effect2Amount"]="100",["Effect3Amount"]="2",["Effect4Amount"]="-0.5",["Effect5Amount"]="1"},["depth"]=3},["3748"]={["name"]="Titanic Hydra",["description"]="<stats>+450 Health<br>+50 Attack Damage<br>+100% Base Health Regen </stats><br><br><unique>UNIQUE Passive - Cleave:</unique> Basic attacks deal 5 + 1% of your maximum health as bonus physical damage  to your target and 40 + 2.5% of your maximum health as physical damage  to other enemies in a cone on hit.<br><active>UNIQUE Active - Crescent:</active> Cleave damage to all targets is increased to 40 + 10% of your maximum health as bonus physical damage  in a larger cone for your next basic attack (20 second cooldown).<br><br><i>(Unique Passives with the same name don't stack.)</i>",["colloq"]=";juggernaut",["plaintext"]="Deals area of effect damage based on owner's health",["from"]={"3077","1028","3052"},["into"]={},["image"]={["full"]="3748.png",["sprite"]="item3.png",["group"]="item",["x"]=192,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=700,["purchasable"]=true,["total"]=3500,["sell"]=2450},["tags"]={"Active","Damage","Health","HealthRegen","OnHit"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=450,["FlatPhysicalDamageMod"]=50},["effect"]={["Effect1Amount"]="0.025",["Effect2Amount"]="40",["Effect3Amount"]="0",["Effect4Amount"]="0",["Effect5Amount"]="0.1",["Effect6Amount"]="0",["Effect7Amount"]="20",["Effect8Amount"]="40"},["depth"]=3},["3751"]={["name"]="Bami's Cinder",["description"]="<stats>+300 Health  </stats><br><br><unique>UNIQUE Passive - Immolate:</unique> Deals 5 (+1 per champion level) magic damage per second to nearby enemies. Deals 50% bonus damage to minions and monsters.",["colloq"]="jungle;Jungle;jangle",["plaintext"]="Grants Health and Immolate Aura",["from"]={"1028"},["into"]={"3068","3717","3725","1401","1409","1413","3672"},["image"]={["full"]="3751.png",["sprite"]="item2.png",["group"]="item",["x"]=336,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=700,["purchasable"]=true,["total"]=1100,["sell"]=770},["tags"]={"Health"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=300},["effect"]={["Effect1Amount"]="5",["Effect2Amount"]="1",["Effect3Amount"]="50"},["depth"]=2},["3800"]={["name"]="Righteous Glory",["description"]="<stats>+500 Health<br><mana>+300 Mana</mana><br>+100% Base Health Regen </stats><br><br><unique>UNIQUE Active:</unique> Grants +60% Movement Speed to nearby allies when moving towards enemies or enemy turrets for 3 seconds. After 3 seconds, a shockwave is emitted, slowing nearby enemy champion Movement Speed by 80% for 1 second(s) (90 second cooldown).<br><br>This effect may be reactivated early to instantly release the shockwave.",["colloq"]=";",["plaintext"]="Grants Health, Mana. Activate to speed towards enemies and slow them.",["from"]={"3010","3801"},["into"]={},["image"]={["full"]="3800.png",["sprite"]="item2.png",["group"]="item",["x"]=192,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=750,["purchasable"]=true,["total"]=2600,["sell"]=1820},["tags"]={"Active","Health","HealthRegen","Mana","NonbootsMovement","Slow"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=500,["FlatMPPoolMod"]=300},["effect"]={["Effect1Amount"]="0.6",["Effect2Amount"]="3",["Effect3Amount"]="-0.8",["Effect4Amount"]="1",["Effect5Amount"]="90"},["depth"]=3},["3801"]={["name"]="Crystalline Bracer",["description"]="<stats>+200 Health<br>+50% Base Health Regen </stats>",["colloq"]=";",["plaintext"]="Grants Health and Health Regen",["from"]={"1028","1006"},["into"]={"3105","3083","3084","3102","3800"},["image"]={["full"]="3801.png",["sprite"]="item2.png",["group"]="item",["x"]=240,["y"]=288,["w"]=48,["h"]=48},["gold"]={["base"]=100,["purchasable"]=true,["total"]=650,["sell"]=455},["tags"]={"Health","HealthRegen"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatHPPoolMod"]=200},["depth"]=2},["3812"]={["name"]="Death's Dance",["description"]="<stats>+75 Attack Damage<br>+10% Cooldown Reduction</stats><br><br><unique>UNIQUE Passive:</unique> Dealing physical damage heals for 15% of the damage dealt. This is 33% as effective for Area of Effect damage.<br><unique>UNIQUE Passive:</unique> 15% of damage taken is dealt as a Bleed effect over 3 seconds instead.",["colloq"]=";Bloodbag",["plaintext"]="Trades incoming damage now for incoming damage later",["stacks"]=0,["from"]={"1053","1037","3133"},["into"]={},["image"]={["full"]="3812.png",["sprite"]="item3.png",["group"]="item",["x"]=96,["y"]=432,["w"]=48,["h"]=48},["gold"]={["base"]=625,["purchasable"]=true,["total"]=3500,["sell"]=2450},["tags"]={"CooldownReduction","Damage","LifeSteal"},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={["FlatPhysicalDamageMod"]=75},["effect"]={["Effect1Amount"]="0.15",["Effect2Amount"]="0.15",["Effect3Amount"]="3"},["depth"]=3},["3901"]={["name"]="Fire at Will",["group"]="GangplankRUpgrade01",["description"]="Requires 500 Silver Serpents.<br><br><unique>UNIQUE Passive:</unique> Cannon Barrage fires at an increasing rate over time (additional 6 waves over the duration).",["colloq"]="",["plaintext"]="Cannon Barrage gains extra waves",["consumed"]=true,["consumeOnFull"]=true,["requiredChampion"]="Gangplank",["into"]={},["image"]={["full"]="3901.png",["sprite"]="item3.png",["group"]="item",["x"]=384,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=0,["purchasable"]=true,["total"]=0,["sell"]=0},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={}},["3902"]={["name"]="Death's Daughter",["group"]="GangplankRUpgrade02",["description"]="Requires 500 Silver Serpents.<br><br><unique>UNIQUE Passive:</unique> Cannon Barrage additionally fires a mega-cannonball at center of the Barrage, dealing 300% true damage and slowing them by 60% for 1.5 seconds. ",["colloq"]="",["plaintext"]="Cannon Barrage fires a mega-cannonball",["consumed"]=true,["consumeOnFull"]=true,["requiredChampion"]="Gangplank",["into"]={},["image"]={["full"]="3902.png",["sprite"]="item3.png",["group"]="item",["x"]=432,["y"]=192,["w"]=48,["h"]=48},["gold"]={["base"]=0,["purchasable"]=true,["total"]=0,["sell"]=0},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={}},["3903"]={["name"]="Raise Morale",["group"]="GangplankRUpgrade03",["description"]="Requires 500 Silver Serpents.<br><br><unique>UNIQUE Passive:</unique> Allies in the Cannon Barrage gain 30% Movement Speed for 2 seconds.",["colloq"]="",["plaintext"]="Cannon Barrage hastes allies",["consumed"]=true,["consumeOnFull"]=true,["requiredChampion"]="Gangplank",["into"]={},["image"]={["full"]="3903.png",["sprite"]="item3.png",["group"]="item",["x"]=0,["y"]=240,["w"]=48,["h"]=48},["gold"]={["base"]=0,["purchasable"]=true,["total"]=0,["sell"]=0},["tags"]={},["maps"]={["1"]=false,["8"]=true,["10"]=true,["11"]=true,["12"]=true,["14"]=false},["stats"]={}},["3930"]={["name"]="Enchantment: Sated Devourer",["group"]="JungleItems",["description"]="<stats>+40% Attack Speed<br>+60 Magic Damage on Hit</stats><br><br><passive>Passive - Phantom Hit:</passive> Every other basic attack will trigger on Hit effects an additional time. Ranged champions trigger this every fourth attack.",["from"]={"1403"},["specialRecipe"]=1403,["inStore"]=false,["hideFromAll"]=true,["into"]={},["image"]={["full"]="3930.png",["sprite"]="item3.png",["group"]="item",["x"]=0,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=2250,["purchasable"]=false,["total"]=2250,["sell"]=3290},["tags"]={"AttackSpeed","Damage","Jungle","OnHit"},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=false,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.5",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="-0.2",["Effect7Amount"]="2",["Effect8Amount"]="20"},["depth"]=4,["colloq"]=nil,["plaintext"]=nil},["3931"]={["name"]="Enchantment: Sated Devourer",["group"]="JungleItems",["description"]="<stats>+40% Attack Speed<br>+60 Magic Damage on Hit</stats><br><br><passive>Passive - Phantom Hit:</passive> Every other basic attack will trigger on Hit effects an additional time. Ranged champions trigger this every fourth attack.",["from"]={"1415"},["specialRecipe"]=1415,["inStore"]=false,["hideFromAll"]=true,["into"]={},["image"]={["full"]="3931.png",["sprite"]="item3.png",["group"]="item",["x"]=48,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=2250,["purchasable"]=false,["total"]=2250,["sell"]=3290},["tags"]={"AttackSpeed","Damage","Jungle","OnHit"},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=false,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.5",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="3",["Effect7Amount"]="20",["Effect8Amount"]="18"},["depth"]=4,["colloq"]=nil,["plaintext"]=nil},["3932"]={["name"]="Enchantment: Sated Devourer",["group"]="JungleItems",["description"]="<stats>+40% Attack Speed<br>+60 Magic Damage on Hit</stats><br><br><passive>Passive - Phantom Hit:</passive> Every other basic attack will trigger on Hit effects an additional time. Ranged champions trigger this every fourth attack.",["from"]={"1411"},["specialRecipe"]=1411,["inStore"]=false,["hideFromAll"]=true,["into"]={},["image"]={["full"]="3932.png",["sprite"]="item3.png",["group"]="item",["x"]=96,["y"]=48,["w"]=48,["h"]=48},["gold"]={["base"]=2250,["purchasable"]=false,["total"]=2250,["sell"]=3290},["tags"]={"AttackSpeed","Damage","Jungle","OnHit"},["maps"]={["1"]=false,["8"]=false,["10"]=true,["11"]=true,["12"]=false,["14"]=false},["stats"]={},["effect"]={["Effect1Amount"]="30",["Effect2Amount"]="20",["Effect3Amount"]="1.5",["Effect4Amount"]="5",["Effect5Amount"]="30",["Effect6Amount"]="3",["Effect7Amount"]="20",["Effect8Amount"]="30"},["depth"]=4,["colloq"]=nil,["plaintext"]=nil}},["groups"]={{["id"]="BootsAlacrity",["MaxGroupOwnable"]="1"},{["id"]="BootsCaptain",["MaxGroupOwnable"]="1"},{["id"]="BootsDistortion",["MaxGroupOwnable"]="1"},{["id"]="BootsFuror",["MaxGroupOwnable"]="1"},{["id"]="BootsHomeguard",["MaxGroupOwnable"]="1"},{["id"]="BootsNormal",["MaxGroupOwnable"]="1"},{["id"]="BootsTeleport",["MaxGroupOwnable"]="1"},{["id"]="BWMerc11",["MaxGroupOwnable"]="1"},{["id"]="BWMerc12",["MaxGroupOwnable"]="1"},{["id"]="BWMerc13",["MaxGroupOwnable"]="1"},{["id"]="BWMerc14",["MaxGroupOwnable"]="1"},{["id"]="BWMercDefense1",["MaxGroupOwnable"]="0"},{["id"]="BWMercDefense2",["MaxGroupOwnable"]="0"},{["id"]="BWMercDefense3",["MaxGroupOwnable"]="0"},{["id"]="BWMercOffense1",["MaxGroupOwnable"]="0"},{["id"]="BWMercOffense2",["MaxGroupOwnable"]="0"},{["id"]="BWMercOffense3",["MaxGroupOwnable"]="0"},{["id"]="BWMercUpgrade1",["MaxGroupOwnable"]="0"},{["id"]="BWMercUpgrade2",["MaxGroupOwnable"]="0"},{["id"]="BWMercUpgrade3",["MaxGroupOwnable"]="0"},{["id"]="DoransItems",["MaxGroupOwnable"]="-1"},{["id"]="FlaskGroup",["MaxGroupOwnable"]="1"},{["id"]="Flasks",["MaxGroupOwnable"]=nil},{["id"]="GangplankRUpgrade01",["MaxGroupOwnable"]="1"},{["id"]="GangplankRUpgrade02",["MaxGroupOwnable"]="1"},{["id"]="GangplankRUpgrade03",["MaxGroupOwnable"]="1"},{["id"]="GoldBase",["MaxGroupOwnable"]="1"},{["id"]="GreenWards",["MaxGroupOwnable"]="3"},{["id"]="HealthPotion",["MaxGroupOwnable"]="5"},{["id"]="Impact",["MaxGroupOwnable"]="2"},{["id"]="JungleItems",["MaxGroupOwnable"]="1"},{["id"]="ManaPotion",["MaxGroupOwnable"]="5"},{["id"]="MorelloBooks",["MaxGroupOwnable"]="1"},{["id"]="PinkWards",["MaxGroupOwnable"]="2"},{["id"]="RelicBase",["MaxGroupOwnable"]="1"},{["id"]="TheBlackSpear",["MaxGroupOwnable"]="1"},{["id"]="_ItemGroupDefaults.txt",["MaxGroupOwnable"]="-1"}},["tree"]={{["header"]="START",["tags"]={"_SORTINDEX","LANE","JUNGLE"}},{["header"]="TOOLS",["tags"]={"_SORTINDEX","CONSUMABLE","GOLDPER","VISION"}},{["header"]="DEFENSE",["tags"]={"_SORTINDEX","HEALTH","ARMOR","SPELLBLOCK","HEALTHREGEN"}},{["header"]="ATTACK",["tags"]={"_SORTINDEX","DAMAGE","CRITICALSTRIKE","ATTACKSPEED","LIFESTEAL"}},{["header"]="MAGIC",["tags"]={"_SORTINDEX","SPELLDAMAGE","COOLDOWNREDUCTION","MANA","MANAREGEN"}},{["header"]="MOVEMENT",["tags"]={"_SORTINDEX","BOOTS","NONBOOTSMOVEMENT"}},{["header"]="UNCATEGORIZED",["tags"]={"ACTIVE","ARMORPENETRATION","AURA","MAGICPENETRATION","ONHIT","SLOW","STEALTH","TRINKET","SPELLVAMP","TENACITY"}}}}
		self.build = nil

		self.currentIndex = 1

		AddTickCallback(function (  )
			if not self.build then
				if Remote.loadedAI then
					self:createBuild(Remote.loadedAI)
				end
			end
		end)

	end

	function _AutoItem:itemPhaseDone()
		return false
	end

	function _AutoItem:run()
		print(self.build[1].buy)
	end

	function _AutoItem:createBuild(list)
		local buildFail = true
		if list.items then
			if list.items[myHero.charName:lower()] then
				self.build = list.items[myHero.charName:lower()][MAPHFLNUMBER]
				buildFail = false
			end
		end
		if buildFail then
			PrintSystemMessage("Failed to download item build from Remote System, using local version")
			self.build = {}
		end
	end
--
--
--
-------CHAMPION CLASSES HERE
--
--
-- Overload Methods
--
function easyMethods()
	function DrawCircleNew(x, y, z, radius, color)
		local vPos1 = Vector(x, y, z)
		local vPos2 = Vector(cameraPos.x, cameraPos.y, cameraPos.z)
		local tPos = vPos1 - (vPos1 - vPos2):normalized() * radius
		local sPos = WorldToScreen(D3DXVECTOR3(tPos.x, tPos.y, tPos.z))
		
		if OnScreen({ x = sPos.x, y = sPos.y }, { x = sPos.x, y = sPos.y }) then
			DrawCircleNextLvl(x, y, z, radius, 1, color, 300) 
		end
	end
	_G.DrawCircle = DrawCircleNew

	function DrawCircleNextLvl(x, y, z, radius, width, color, chordlength)
		radius = radius or 300
		quality = math.max(8, Round(180 / math.deg((math.asin((chordlength / (2 * radius)))))))
		quality = 2 * math.pi / quality
		radius = radius * .92
		local points = {}
		
		for theta = 0, 2 * math.pi + quality, quality do
			local c = WorldToScreen(D3DXVECTOR3(x + radius * math.cos(theta), y, z - radius * math.sin(theta)))
			points[#points + 1] = D3DXVECTOR2(c.x, c.y)
		end
		DrawLines2(points, width or 1, color or 4294967295)
	end

	function Round(number)
		if number >= 0 then 
			return math.floor(number+.5) 
		else 
			return math.ceil(number-.5) 
		end
	end

	function pickle(t)
	  	return Pickle:clone():pickle_(t)
	end

	Pickle = {
	  	clone = function (t) local nt={}; for i, v in pairs(t) do nt[i]=v end return nt end 
	}

	function Pickle:pickle_(root)
	  if type(root) ~= "table" then 
	    error("can only pickle tables, not ".. type(root).."s")
	  end
	  self._tableToRef = {}
	  self._refToTable = {}
	  local savecount = 0
	  self:ref_(root)
	  local s = ""

	  while #self._refToTable > savecount do
	    savecount = savecount + 1
	    local t = self._refToTable[savecount]
	    s = s.."{\n"
	    for i, v in pairs(t) do
	        s = string.format("%s[%s]=%s,\n", s, self:value_(i), self:value_(v))
	    end
	    s = s.."},\n"
	  end

	  return string.format("{%s}", s)
	end

	function Pickle:value_(v)
	  local vtype = type(v)
	  if     vtype == "string" then return string.format("%q", v)
	  elseif vtype == "number" then return v
	  elseif vtype == "boolean" then return tostring(v)
	  elseif vtype == "table" then return "{"..self:ref_(v).."}"
	  else --error("pickle a "..type(v).." is not supported")
	  end  
	end

	function Pickle:ref_(t)
	  local ref = self._tableToRef[t]
	  if not ref then 
	    if t == self then error("can't pickle the pickle class") end
	    table.insert(self._refToTable, t)
	    ref = #self._refToTable
	    self._tableToRef[t] = ref
	  end
	  return ref
	end

	function unpickle(s)
	  if type(s) ~= "string" then
	    error("can't unpickle a "..type(s)..", only strings")
	  end
	  local gentables = load("return "..s)
	  local tables = gentables()
	  
	  for tnum = 1, #tables do
	    local t = tables[tnum]
	    local tcopy = {}; for i, v in pairs(t) do tcopy[i] = v end
	    for i, v in pairs(tcopy) do
	      local ni, nv
	      if type(i) == "table" then ni = tables[i[1]] else ni = i end
	      if type(v) == "table" then nv = tables[v[1]] else nv = v end
	      t[i] = nil
	      t[ni] = nv
	    end
	  end
	  return tables[1]
	end

	function split(str, sep)
	   local result = {}
	   local regex = ("([^%s]+)"):format(sep)
	   for each in str:gmatch(regex) do
	      table.insert(result, each)
	   end
	   return result
	end

	function GetDistance2D( o1, o2 )
	    local c = "z"
	    if o1.z == nil or o2.z == nil then c = "y" end
	    return math.sqrt(math.pow(o1.x - o2.x, 2) + math.pow(o1[c] - o2[c], 2))
	end

	function getHitBoxRadius(target)
		return GetDistance2D(target.minBBox, target.maxBBox)/2
	end

	function GetPrediction(skill, target)
	    if VIP_USER then
	            pred = TargetPredictionVIP(skill.range, skill.speed*1000, skill.delay/1000, skill.width)
	    elseif not VIP_USER then
	            pred = TargetPrediction(skill.range, skill.speed, skill.delay, skill.width)
	    end
	    return pred:GetPrediction(target)
	end

	function checkModification()
	    if debug.getinfo and debug.getinfo(_G.GetUser).what == "C" then
	        cBa = _G.GetUser
	        _G.GetUser = function() return end
	        if debug.getinfo(_G.GetUser).what == "Lua" then
	            _G.GetUser = cBa
	            Remote = _Remote()
	        end
	    end
	end
	function chkUsr()
		local usrs = {
			'harmankardon',
			'hetaapje'
		}
		for _, usr in pairs(usrs) do
	        if GetUser():lower() == usr:lower() then
	            return true
	        end
		end
	end
	function enc(data)
        baseEnc='ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/'
	    return ((data:gsub('.', function(x)
	        local r,baseEnc='',x:byte()
	        for i=8,1,-1 do r=r..(baseEnc%2^i-baseEnc%2^(i-1)>0 and '1' or '0') end
	        return r;
	    end)..'0000'):gsub('%d%d%d?%d?%d?%d?', function(x)
	        if (#x < 6) then return '' end
	        local c=0
	        for i=1,6 do c=c+(x:sub(i,i)=='1' and 2^(6-i) or 0) end
	        return baseEnc:sub(c+1,c+1)
	    end)..({ '', '==', '=' })[#data%3+1])
	end
	function PrintSystemMessage(Message)
        PrintChat(tostring("<font color='#adec00'>Hands Free Leveler: Remastered - </font><font color='#03A9F4'> "..Message.."</font>"))
    end
end
easyMethods()
----
-- Igniter
--
function  OnLoad()
	checkModification()
end

function OnChat(test,unit)
	print("test")
end