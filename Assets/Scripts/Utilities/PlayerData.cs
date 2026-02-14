

public class PlayerData
{
    public Gender gender;
    public Subclass subclass;
    public StartingWeapon startingWeapon;

    public enum Gender
    {
        female = 0,
        nonbinary,
        hat,
        male
    }
    
    public enum Subclass
    {
        alchemist = 0,
        trickster,
        chef,
        elementalist
    }

    public enum StartingWeapon
    {
        bottle = 0,
        dagger,
        fryingpan,
        staff
    }
}
