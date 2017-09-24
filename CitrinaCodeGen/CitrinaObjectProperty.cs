namespace CitrinaCodeGen
{
    public class CitrinaObjectProperty
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string Type { get; set; }
        public bool NeedJsonAttribute { get; set; }
        public string Summary { get; set; }
    }
}
