using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GOAP;
using Sensors;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Memory))]
public class Action_TakeFoodToStore : GOAPAction
{

    Inventory inventory;
    Movement movement;
    Memory memory;
    SmartObject foodStore;

    public override float GetCost(){
        return 0.1f * worldState.GetFloatState("Fatigue");
    }
    public override void Setup(){
        base.Setup();
        movement = GetComponent<Movement>();
        inventory = GetComponent<Inventory>();
        memory = GetComponent<Memory>();
        foodStore = (SmartObject)memory.RememberNearest("FoodStore");
    }

    public override void OnActivate(){
        foodStore = (SmartObject)memory.RememberNearest("FoodStore");
        if (foodStore == null){
            StopAction();
            return;
        }
        movement.GoTo(foodStore);
    }

    public override void OnDeactivate(){
        worldState.RemoveBoolState("FoodHarvested");
        StopAction();
    }

    public override void OnTick()
    {
        movement.GoTo(foodStore);
        if (movement.AtTarget()){
            for(int i = inventory.items["Food"].Count -1; i >= 0; i--){
                foodStore.Add(inventory.items["Food"][i]);
                inventory.Remove(inventory.items["Food"][i]);
            }
            worldState.AddState("FoodHarvested", true);
        }
    }

    public override bool PreconditionsSatisfied(WorldState worldState)
    {
        bool result = base.PreconditionsSatisfied(worldState);
        if (!result){
            return result;
        }
        else{
            return (foodStore != null);
        }
    }

    protected override void SetupActionLayers(){
        base.SetupActionLayers();
        actionLayers.Add("Food");
    }

    protected override void SetupEffects(){
        base.SetupEffects();
        effects["FoodHarvested"] = true;
        effects["g_FoodAvailable"] = true;
    }

    protected override void SetupConditions(){
        base.SetupConditions();
        preconditions["HoldingFood"] = true;
        preconditions["FoodRemovedFromStore"] = false;
    }

}