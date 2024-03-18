using FireSharp;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Repository.DTOs;
using Service.Interface;
using static Repository.DTOs.AuctionResultDto;

namespace Service.Implement
{
    public class FirebaseAuctionService : IFirebaseAuctionService
    {

        private readonly IFirebaseClient _client;

        public FirebaseAuctionService(IFirebaseConfig config)
        {
            _client = new FirebaseClient(config);
        }

        public async Task<AuctionData> GetAuctionDataAsync(int auctionId)
        {
            //IFirebaseConfig config = new FirebaseConfig
            //{
            //    AuthSecret = "BKzPJclM4rnmLoj8JXIgWKkjwP0aprY6NK266RL9",
            //    BasePath = "https://swd-reas-default-rtdb.asia-southeast1.firebasedatabase.app/"
            //};


            //IFirebaseClient client = new FirebaseClient(config);
            FirebaseResponse response = await _client.GetAsync("auctions");

            var result = JsonConvert.DeserializeObject<Dictionary<int, AuctionData>>(response.Body);

            return result[auctionId];
        }
    }
}
