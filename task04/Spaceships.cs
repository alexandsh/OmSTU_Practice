namespace task04;

public interface ISpaceship
{
    void MoveForward();   
    void Rotate(int angle); 
    void Fire();             
    int Speed { get; }       
    int FirePower { get; }   
}


public class Cruiser: ISpaceship
{
    public int Speed { get; } = 50; 

    public int FirePower { get; } = 100;
    
    public void MoveForward()
    {
        Console.WriteLine(Speed);
    }

    public void Rotate(int angle)
    {
        Console.WriteLine(angle);
    }

    public void Fire()
    {
        Console.WriteLine(FirePower);
    }
}

public class Fighter: ISpaceship
{
    public int Speed { get; } = 100;

    public int FirePower { get; } = 50;
    
    public void MoveForward()
    {
        Console.WriteLine(Speed);
    }

    public void Rotate(int angle)
    {
        Console.WriteLine(angle);
    }

    public void Fire()
    {
        Console.WriteLine(FirePower);
    }
}
