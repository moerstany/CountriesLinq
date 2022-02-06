namespace CountriesLinQ
{
    public class WorldParts
    {
        public uint world_parts_id { get; set; }
        public string world_parts_name { get; set; }
        public uint id_country { get; set; }
        public string country_name { get; set; }
        public uint country_area { get; set; }
        public uint country_population { get; set; }
        public uint id_capital { get; set; }
        public string capital_name { get; set; }
    }
}