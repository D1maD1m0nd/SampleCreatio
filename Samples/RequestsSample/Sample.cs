using System.Runtime.Serialization;

namespace Samples;

[DataContract(Name = "CostVisa")]
public class Sample
{
    [DataMember(Name = "Trip")]
    public Trip TripObject {get; set;}
    public Service[] Services { get; set; }
    public string EventCode {get; set;}
    public int CompanyId {get; set;}
}

public class Trip
{
    public decimal JourneyNumber {get; set;}
    public decimal BusinessTripNumber {get; set;}
}


public class Service
{
    public int OrderNumber {get; set;}
    public int ServiceNumber {get; set;}
}

