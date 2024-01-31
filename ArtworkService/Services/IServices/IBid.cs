namespace ArtworkService.Services.IServices;

public class IBid
{
    Task<string> UpdateBidStatus(List<string> artIds);

}
