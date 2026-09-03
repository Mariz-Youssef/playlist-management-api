using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlaylistManagement.Api.Extensions;
using PlaylistManagement.Api.Features.Playlists.DTOs;

namespace PlaylistManagement.Api.Features.Playlists
{
    /// <summary>
    /// Provides endpoints for managing playlists and their songs.
    /// </summary>
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
        /// <summary>
        /// Creates a new playlist for the authenticated user.
        /// </summary>
        /// <param name="request">
        /// The playlist information.
        /// </param>
        /// <param name="cancellationToken">
        /// Token used to cancel the request.
        /// </param>
        /// <returns>
        /// The newly created playlist.
        /// </returns>
        /// <response code="201">
        /// Playlist created successfully.
        /// </response>
        /// <response code="400">
        /// The request data is invalid.
        /// </response>
        /// <response code="401">
        /// The user is not authenticated.
        /// </response>
        [HttpPost]
        [ProducesResponseType(typeof(PlaylistResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PlaylistResponse>> Create(CreatePlaylistRequest request,CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var response = await _playlistService.CreateAsync(request,userId,cancellationToken);

            return StatusCode(StatusCodes.Status201Created,response);
        }

        /// <summary>
        /// Adds an existing song to one of the authenticated user's playlists.
        /// </summary>
        /// <param name="playlistId">
        /// The ID of the playlist.
        /// </param>
        /// <param name="request">
        /// The song to add.
        /// </param>
        /// <param name="cancellationToken">
        /// Token used to cancel the request.
        /// </param>
        /// <response code="204">
        /// Song added successfully.
        /// </response>
        /// <response code="400">
        /// The request data is invalid.
        /// </response>
        /// <response code="401">
        /// The user is not authenticated.
        /// </response>
        /// <response code="403">
        /// The playlist does not belong to the authenticated user.
        /// </response>
        /// <response code="404">
        /// The playlist or song was not found.
        /// </response>
        /// <response code="409">
        /// The song is already in the playlist.
        /// </response>
        [HttpPost("{playlistId:guid}/songs")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddSong(Guid playlistId,AddSongToPlaylistRequest request,CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            await _playlistService.AddSongAsync(playlistId,request,userId,cancellationToken);

            return NoContent();
        }
        /// <summary>
        /// Retrieves all playlists belonging to the authenticated user.
        /// </summary>
        /// <param name="cancellationToken">
        /// Token used to cancel the request.
        /// </param>
        /// <returns>
        /// The authenticated user's playlists and their songs.
        /// </returns>
        /// <response code="200">
        /// Playlists retrieved successfully.
        /// </response>
        /// <response code="401">
        /// The user is not authenticated.
        /// </response>
        [HttpGet]
        [ProducesResponseType(typeof(List<PlaylistResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<PlaylistResponse>>> GetMyPlaylists(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var response = await _playlistService.GetMyPlaylistsAsync(userId,cancellationToken);

            return Ok(response);
        }

        /// <summary>
        /// Updates the name of one of the authenticated user's playlists.
        /// </summary>
        /// <response code="204">
        /// Playlist updated successfully.
        /// </response>
        /// <response code="401">
        /// The user is not authenticated.
        /// </response>
        /// <response code="403">
        /// The playlist does not belong to the authenticated user.
        /// </response>
        /// <response code="404">
        /// The playlist was not found.
        /// </response>
        [HttpPut("{playlistId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid playlistId,UpdatePlaylistRequest request,CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            await _playlistService.UpdateAsync(playlistId,request,userId,cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Deletes one of the authenticated user's playlists.
        /// </summary>
        /// <response code="204">
        /// Playlist deleted successfully.
        /// </response>
        /// <response code="401">
        /// The user is not authenticated.
        /// </response>
        /// <response code="403">
        /// The playlist does not belong to the authenticated user.
        /// </response>
        /// <response code="404">
        /// The playlist was not found.
        /// </response>
        [HttpDelete("{playlistId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid playlistId,CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            await _playlistService.DeleteAsync(playlistId,userId,cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Removes a song from one of the authenticated user's playlists.
        /// </summary>
        /// <response code="204">
        /// Song removed successfully.
        /// </response>
        /// <response code="401">
        /// The user is not authenticated.
        /// </response>
        /// <response code="403">
        /// The playlist does not belong to the authenticated user.
        /// </response>
        /// <response code="404">
        /// The playlist or song relationship was not found.
        /// </response>
        [HttpDelete("{playlistId:guid}/songs/{songId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveSong(Guid playlistId,Guid songId,CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            await _playlistService.RemoveSongAsync(playlistId,songId,userId,cancellationToken);

            return NoContent();
        }

    }
}
