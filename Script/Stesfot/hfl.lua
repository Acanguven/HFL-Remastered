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
TEAMNUMBER = myHero.team
VERSION = "1.0"
CLIENTVERSION = ""
--
-- Class Objects
--
PACKETS = nil
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
Helper = nil
StateManager = nil
RandomPath = nil
--
--
require "VPrediction"
require "Collision"
VP = VPrediction()
--
-- Classes
--
class 'init'
	function init:__init()
		CLIENTVERSION = split(GetGameVersion()," ")[1]
		self.sprite = false
		self.sep = false
		if FileExist(LIB_PATH .. "/HfLib.lua") then
			self:load()
		end
		if (not _G.hflTasks or not _G.hflTasks[MAPNAME] or not _G.hflTasks[MAPNAME][TEAMNUMBER]) and not editMode then
			print("This map is not supported, please report the map name to law to make him update for this map")
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
					Helper = _Helper()
					PACKETS = packet()
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
		if (PACKETS:isUpdated()) then
			DrawText("Updated", 18, 180, 200, ARGB(255,0,255,0))
		else
			DrawText("Not Updated", 18, 200, 200, ARGB(255,255,0,0))
		end
		self.sep:Draw(0,230,500)
		self:drawAIinfo()
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

	function init:loadSprite()
		if FileExist(SPRITE_PATH .. "/hfl.png") then
			self.sprite = createSprite(SPRITE_PATH .. "/hfl.png")
			self.sprite:SetScale(0.5,0.5)

			self.sep = createSprite(SPRITE_PATH .. "/hflsep.png")
			self.sep:SetScale(0.5,0.5)
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
class 'packet'
	function packet:__init()
		self.disabledPacket = false
		self.version = split(GetGameVersion()," ")[1]
		self.idBytes = {}
		self.spellLevel = {}
		self.buyItem = {}

		self:initBytes()
		self:initFunctions()
		if not self.idBytes[self.version] then
			self.disabledPacket = true
		end
	end

	function packet:isUpdated()
		if not self.idBytes[self.version] then
			return false
		else
			return true
		end
	end

	function packet:buyItemId(id)
		self.buyItem[self.version](id)
	end

	function packet:spellUp(id)
		self.spellLevel[self.version](id)
	end

	function packet:initBytes()
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
	end

	function packet:initFunctions()
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
	end

	function packet:getTableByte(i)
		return self.idBytes[self.version][i]
	end
PRED_LAST_HIT = 0
PRED_TWO_HITS = 1
PRED_SKILL = 2
PRED_UNKILLABLE = 3
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
	    for i = 1, objManager.maxObjects do
	        local tower = objManager:getObject(i)
			if tower ~= nil and tower.valid and tower.type == "obj_AI_Turret" and tower.visible  then
				table.insert(self.towers, tower)
			end
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


class '_StateManager'
	function _StateManager:__init()
		self.State = LOADING_STATE
		self.stateList = {}

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

		AddTickCallback(function (  )
			self:runState()
		end)
	end

	function _StateManager:addState(object)
		table.insert(self.stateList, object)
	end

	function _StateManager:getActiveState()
		for i, state in pairs(self.stateList) do
			if state.valid then
				return state
			end
		end
	end

	function _StateManager:runState()
		self:getActiveState():run()
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
		MINIONS:LaneClear()
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
			if self:shopDone() and self:lifeFull() then
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

	function _Spawn:shopDone()
		return true
	end

	function _Spawn:run()

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

WAYPOINT = {
	x = 0,
	z = 0
}
STAGE_SHOOT = 0
STAGE_MOVE = 1
STAGE_SHOOTING = 2

RegisteredOnAttacked = {}
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
--
-- Overload Methods
--
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
--
-- Igniter
--
function  OnLoad()
	init()
end
