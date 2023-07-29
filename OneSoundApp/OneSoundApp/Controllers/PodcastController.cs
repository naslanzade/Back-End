using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Podcast;


namespace OneSoundApp.Controllers
{
    public class PodcastController : Controller
    {
        private readonly IAdvertService _advertService;
        private readonly IPodcastService _podcastService;



        public PodcastController(IAdvertService advertService,
                                 IPodcastService podcastService)
        {

            _advertService = advertService;
            _podcastService = podcastService;

        }
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Advert> adverts = await _advertService.GetAll();
            List<Podcast> paginatePodcasts = await _podcastService.GetPaginatedDatas(page, take);
            int pageCount = await GetPageCountAsync(take);
            Paginate<Podcast> paginatedDatas = new(paginatePodcasts, page, pageCount);


            PodcastVM model = new()
            {
                Adverts = adverts,
                PaginatedDatas = paginatedDatas


            };
            return View(model);
        }


        public async Task<IActionResult> Detail(int? id)
        {

            if (id is null) return BadRequest();

            Podcast podcast= await _podcastService.GetPodcastDetailAsync(id);

            if (podcast == null) return NotFound();

            PodcastDetailVM model = new()
            {
                Id=podcast.Id,
                Name=podcast.Name,
                Records=podcast.Records.ToList(),
                Images=podcast.Images.ToList(),
                Price=podcast.Price,
                AuthorName=podcast.Author.Name

            };

            return View(model);
        }


        private async Task<int> GetPageCountAsync(int take)
        {
            var blogCount = await _podcastService.GetCountAsync();

            return (int)Math.Ceiling((decimal)blogCount / take);
        }
    }
}
