namespace Entities.RequestFeatures
{
    public class RequestParametersBase
    {

        public string OrderBy { get; set; }
        public uint MinPriority { get; set; }
        public uint MaxPriority { get; set; } = uint.MaxValue;
        public bool IsValidPriorityRange => MinPriority <= MaxPriority;
    }
}