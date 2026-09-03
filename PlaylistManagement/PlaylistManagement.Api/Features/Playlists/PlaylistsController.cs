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
        [HttpGet]
        public async Task<ActionResult<List<PlaylistResponse>>> GetMyPlaylists(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var response = await _playlistService.GetMyPlaylistsAsync(userId,cancellationToken);

            return Ok(response);
        }
        [HttpPut("{playlistId:guid}")]
        public async Task<IActionResult> Update(Guid playlistId,UpdatePlaylistRequest request,CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            await _playlistService.UpdateAsync(playlistId,request,userId,cancellationToken);

            return NoContent();
        }

        [HttpDelete("{playlistId:guid}")]
        public async Task<IActionResult> Delete(Guid playlistId,CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            await _playlistService.DeleteAsync(playlistId,userId,cancellationToken);

            return NoContent();
        }

        [HttpDelete("{playlistId:guid}/songs/{songId:guid}")]
        public async Task<IActionResult> RemoveSong(Guid playlistId,Guid songId,CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            await _playlistService.RemoveSongAsync(playlistId,songId,userId,cancellationToken);

            return NoContent();
        }

    }
}
