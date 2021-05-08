public class NPCPetOwner : NPCAchievementGiver
{
    public override bool MeetsConditions()
    {
        return NPCPet.PetTimes >= 10;
    }
}