using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlaylistManagement.Api.Extensions;
using PlaylistManagement.Api.Features.Playlists.DTOs;

namespace PlaylistManagement.Api.Features.Playlists
{
    [ApiController]
    [Route("api/playlists")]
    [Authorize]
    public class PlaylistsController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;
        public PlaylistsController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }
        [HttpPost]
        public async Task<ActionResult<PlaylistResponse>> Create(CreatePlaylistRequest request,CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var response = await _playlistService.CreateAsync(request,userId,cancellationToken);

            return StatusCode(StatusCodes.Status201Created,response);
        }
    }
}
