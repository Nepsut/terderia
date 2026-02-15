public class PlayerData
{
    private Gender _gender;
    public Gender gender
    {
        get => _gender;
        set
        {
            _gender = value;
            GenderAsString = gender.ToString().Replace('_', '-');
        }
    }
    public Subclass _subclass;
    public Subclass subclass
    {
        get => _subclass;
        set
        {
            _subclass = value;
            _subclass.ToString().Replace('_', '-');
        }
    }
    public StartingWeapon _startingWeapon;
    public StartingWeapon startingWeapon
    {
        get => _startingWeapon;
        set
        {
            _startingWeapon = value;
            StartingWeaponAsString = _startingWeapon.ToString().Replace('_', '-');
        }
    }

    public string GenderAsString { get; private set; }
    public string SubclassAsString { get; private set; }
    public string StartingWeaponAsString { get; private set; }


    public enum Gender
    {
        female = 0,
        non_binary,
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
        glass_bottle = 0,
        dagger,
        frying_pan,
        staff
    }
}