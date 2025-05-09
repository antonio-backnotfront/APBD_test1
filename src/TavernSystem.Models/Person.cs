namespace Models;

public class Person
{ 
    static int count = 0;
    int Id = ++count;
    string FirstName { get; set; }
    string MiddleName { get; set; }
    string LastName { get; set; }
    bool HasBounty { get; set; }
    
}