namespace colab_api.Requests.space
{
    public class SpaceRequest : InqueiryRequest
    {
        public int active { get; set; }
        public string[]? amenities { get; set; }
    }
}
