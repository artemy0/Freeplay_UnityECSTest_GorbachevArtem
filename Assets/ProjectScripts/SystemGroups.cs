using ProjectScripts.Character.Player;
using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace ProjectScripts
{
    // ComponentSystemGroups are located in the calling order:

    #region InitializationSystemGroup

    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    [UpdateBefore(typeof(EndInitializationEntityCommandBufferSystem))]
    public partial class CustomInitializationSystemGroup : ComponentSystemGroup
    {
    }

    // EndInitializationEntityCommandBufferSystem

    #endregion


    #region PhysicsSystemGroup

    // PhysicsSimulationGroup

    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(PhysicsSimulationGroup))]
    [UpdateBefore(typeof(AfterPhysicsSystemGroup))]
    public partial class CustomPhysicsSystemGroup : ComponentSystemGroup
    {
    }

    // AfterPhysicsSystemGroup

    #endregion


    #region SimulationSystemGroup

    // BeginSimulationEntityCommandBufferSystem
    // FixedStepSimulationSystemGroup

    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial class CustomTranslationSystemGroup : ComponentSystemGroup
    {
    }

    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    [UpdateBefore(typeof(CustomInteractionSystemGroup))]
    public partial class CustomAttackSystemGroup : ComponentSystemGroup
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<PlayerTag>();
        }
    }

    // TransformSystemGroup

    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(CustomAttackSystemGroup))]
    [UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
    public partial class CustomInteractionSystemGroup : ComponentSystemGroup
    {
    }

    // LateSimulationSystemGroup

    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateBefore(typeof(CustomDestructionSystemGroup))]
    public partial class CustomEffectsSystemGroup : ComponentSystemGroup
    {
    }

    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateBefore(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial class CustomDestructionSystemGroup : ComponentSystemGroup
    {
    }

    // EndSimulationEntityCommandBufferSystem

    #endregion
}