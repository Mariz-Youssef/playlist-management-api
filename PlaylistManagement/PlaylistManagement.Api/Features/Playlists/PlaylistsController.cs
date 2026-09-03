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

        [HttpPost("{playlistId:guid}/songs")]
        public async Task<IActionResult> AddSong(Guid playlistId,AddSongToPlaylistRequest request,CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            await _playlistService.AddSongAsync(playlistId,request,userId,cancellationToken);

            return NoContent();
        }
    }
}
