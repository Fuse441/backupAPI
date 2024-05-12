namespace colab_api.Requests.user
{
    public class SpaceUpsertRequest
    {
       
        public string spaceName { get; set; }
        public string spaceDescription { get; set; }
        public string spaceType { get; set; }
        public bool status { get; set; }
        public string spaceDetails { get; set; }
        public int partnerId { get; set; }
        public string image { get; set; }
    }
}
